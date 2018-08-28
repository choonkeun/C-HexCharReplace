using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CLM_Segments
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CreateStatusBar();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtSourceFolder.Text = System.Configuration.ConfigurationManager.AppSettings["SourceFolder"];
            txtFilter.Text = System.Configuration.ConfigurationManager.AppSettings["Ext"];
            txtOutputFile.Text = System.Configuration.ConfigurationManager.AppSettings["OutputFile"];
        }

        List<string> sourceFiles = new List<string>();

        private List<string> GetFiles(string sourceFolder, string filters, System.IO.SearchOption searchOption)
        {
            filters = filters.Replace(" ", string.Empty);
            try
            {
                return filters.Split('|').SelectMany(filter => System.IO.Directory.GetFiles(txtSourceFolder.Text, filter, searchOption)).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        List<Task> TaskList = new List<Task>();

        private void btnProcess_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            if (rdoOverwrite.Checked)
                File.Delete(txtOutputFile.Text);

            sourceFiles = GetFiles(SourceFolder.Trim(), txtFilter.Text, System.IO.SearchOption.AllDirectories);
            if (sourceFiles != null && sourceFiles.Count > 0)
            {
                sourceFiles.Sort();

                //Way 1 - 10 Sec
                Parallel.ForEach(
                    sourceFiles,
                    new ParallelOptions { MaxDegreeOfParallelism = 50 },
                    item =>
                    {
                        Do_Parse(item);
                    }
                );

                //Way 2 - 9.4 Sec
                //foreach (var item in sourceFiles)
                //{
                //    var task = Task.Factory.StartNew(() =>
                //    {
                //        Do_Parse(item);
                //    });
                //    TaskList.Add(task);
                //}
                //Task.WaitAll(TaskList.ToArray());

                if (layouts.Count > 0)
                {
                    var layout = layouts.OrderBy(x => x.Filename).ThenBy(x => x.SubmitterId);
                    string outText = String.Join("\n", layout.ToArray());
                    File.AppendAllText(txtOutputFile.Text, outText);
                }
                stopwatch.Stop();
                statusPanel.Text = stopwatch.Elapsed.TotalSeconds.ToString() + " seconds";
                MessageBox.Show("Finished");
            }
            else
            {
                stopwatch.Stop();
                statusPanel.Text = string.Empty;
                MessageBox.Show("No file found");
            }

        }

        private void Do_Parse(string FullFileName)
        {
            try
            {
                int selectLength = 200;

                string readFile = Path.GetFileName(FullFileName);
                string readText = File.ReadAllText(FullFileName);
                //statusPanel.Text = readFile;

                do
                {
                    if (readText.Substring(0, 3) != "ISA") break;

                    string firstTag = readText.Substring(0, selectLength);
                    string seperateChar = firstTag.Split('~').First().Last().ToString();    // "<"
                    string[] clmMatches = { "B" + seperateChar + "6", "B" + seperateChar + "7" };

                    MatchCollection foundMatch = Regex.Matches(readText, @"CLM", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    string searchStr = string.Empty;
                    bool hasPattern = false;
                    int length = 0;

                    if (foundMatch.Count > 0)
                    {
                        for (int i = 0; i < foundMatch.Count; i++)
                        {
                            length = readText.Length - (foundMatch[i].Index + selectLength) > 0 ? selectLength : readText.Length - foundMatch[i].Index;
                            searchStr = readText.Substring(foundMatch[i].Index, length);
                            hasPattern = clmMatches.Any(s => searchStr.Contains(s));
                            if (hasPattern)
                            {
                                Layout837 l = new Layout837();
                                l.Filename = readFile;

                                string[] seg = searchStr.Split('~');
                                for (int t = 0; t < seg.Length - 1; t++)
                                {
                                    string[] e1 = seg[t].Split('*');
                                    switch (t)
                                    {
                                        case 0:
                                            l.SubmitterId = e1[1];
                                            l.FrequencyType = e1[5].Last().ToString();
                                            break;
                                        default:
                                            if (e1[0] == "REF" && e1[1] == "D9") l.ClaimNumber = e1[2];
                                            if (e1[0] == "REF" && e1[1] == "F8") l.ReferenceNumber = e1[2];
                                            break;
                                    }
                                }
                                layouts.Add(l);
                                //WriteToFileThreadSafe(txtOutputFile.Text, l.ToString());
                            }
                        }
                    }

                } while (false);

            }
            catch (Exception ex)
            {
                //
            }
        }

        //working but slow because threads are waiting to write
        private ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
        public void WriteToFileThreadSafe(string path, string text)
        {
            _readWriteLock.EnterWriteLock();
            try
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(text);
                    sw.Close();
                }
            }
            finally
            {
                _readWriteLock.ExitWriteLock();
            }
        }

        public struct Layout837
        {
            public string Filename { get; set; }
            public string SubmitterId { get; set; }
            public string FrequencyType { get; set; }
            public string ClaimNumber { get; set; }
            public string ReferenceNumber { get; set; }

            public override string ToString()
            {
                return Filename + "|" + SubmitterId + "|" + FrequencyType + "|" + ClaimNumber + "|" + ReferenceNumber;
            }
        }
        List<Layout837> layouts = new List<Layout837>();


        #region base

        string SourceFolder = string.Empty;
        string Ext = string.Empty;
        string OutputFile = string.Empty;

        protected StatusBar mainStatusBar = new StatusBar();
        protected StatusBarPanel statusPanel = new StatusBarPanel();
        protected StatusBarPanel fileSizePanel = new StatusBarPanel();
        protected StatusBarPanel datetimePanel = new StatusBarPanel();

        private void CreateStatusBar()
        {

            // Set first panel properties and add to StatusBar
            statusPanel.BorderStyle = StatusBarPanelBorderStyle.Sunken;
            statusPanel.ToolTipText = "";
            statusPanel.Text = "Ready";
            statusPanel.AutoSize = StatusBarPanelAutoSize.Spring;
            mainStatusBar.Panels.Add(statusPanel);

            // Set second panel properties and add to StatusBar
            fileSizePanel.BorderStyle = StatusBarPanelBorderStyle.Raised;
            fileSizePanel.ToolTipText = "";
            fileSizePanel.Text = "";
            fileSizePanel.AutoSize = StatusBarPanelAutoSize.Contents;
            mainStatusBar.Panels.Add(fileSizePanel);

            // Set third panel properties and add to StatusBar
            datetimePanel.BorderStyle = StatusBarPanelBorderStyle.Raised;
            datetimePanel.ToolTipText = "DateTime: " + System.DateTime.Today.ToString();
            datetimePanel.Text = System.DateTime.Today.ToLongDateString();
            datetimePanel.AutoSize = StatusBarPanelAutoSize.Contents;
            mainStatusBar.Panels.Add(datetimePanel);

            mainStatusBar.ShowPanels = true;

            // Add StatusBar to Form controls
            this.Controls.Add(mainStatusBar);
        }

        #endregion


    }
}
