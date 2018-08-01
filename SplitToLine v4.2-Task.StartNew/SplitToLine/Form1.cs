using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Threading.Tasks;


namespace SplitToLine
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CreateStatusBar();
        }

        private int matchFound = 0;

        private void btnSource_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK)
            {
                txtSource.Text = openFileDialog1.FileName;
                try
                {
                    //string text = File.ReadAllText(txtSource.Text);
                    //size = text.Length;
                }
                catch (IOException)
                {
                }
            }
        }

        private void btnTarget_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog2.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK)
            {
                txtTarget.Text = openFileDialog2.FileName;
                try
                {
                    //string text = File.ReadAllText(txtTarget);
                    //size = text.Length;
                }
                catch (IOException)
                {
                }
            }
        }

        public byte[] SearchBytes;
        public byte[] ReplaceBytes;
        
        private void btnConvert_Click(object sender, EventArgs e)
        {
            bool action = true;

            //check Source FilePath
            if (!File.Exists(txtSource.Text))
            {
                MessageBox.Show("File not found");
                action = false;
            }

            //check Target FilePath
            if (action && !Directory.Exists(Path.GetDirectoryName(txtTarget.Text)))
            {
                MessageBox.Show("Path not found");
                action = false;
            }

            if (txtSearchHEX.Text.Trim() == txtReplaceHEX.Text.Trim())
            {
                MessageBox.Show("search and replace pattern is same");
                action = false;
            }

            //check Search string as HEX
            if (txtSearchHEX.Text.Trim().Length > 0)
            {
                txtSearchHEX.Text = txtSearchHEX.Text.Trim();
                SearchBytes = txtSearchHEX.Text.Split(' ').Select(s => Convert.ToByte(s.Substring(0,2), 16)).ToArray();
            }
            else
            {
                MessageBox.Show("search value not found");
                action = false;
            }

            //check target string as HEX
            if (txtReplaceHEX.Text.Trim().Length > 0)
            {
                txtReplaceHEX.Text = txtReplaceHEX.Text.Trim();
                ReplaceBytes = txtReplaceHEX.Text.Split(' ').Select(s => Convert.ToByte(s.Substring(0, 2), 16)).ToArray();
            }
            else
            {
                MessageBox.Show("replace value not found");
                action = false;
            }

            //start replace
            if (action)
            {
                if (rdoBuffer.Checked) Process_Buffer();
                if (rdoBypass.Checked) Process_Bypass();
                if (rdoMMF.Checked) Process_MMF();
            }
        }

        private void Process_Buffer()
        {
            matchFound = 0;

            //Open Source File
            FileStream inFS = new FileStream(txtSource.Text, FileMode.Open, FileAccess.Read);
            byte[] sourceBytes = new byte[inFS.Length];
            int sourceFileLength = (int)inFS.Length;

            fileSizePanel.Text = "FileSize: " + sourceFileLength.ToString("##,###,###,###");
            statusPanel.Text = "Processing";

            //Open Target File
            FileStream outFS = new FileStream(txtTarget.Text, FileMode.Create);    //overwrite

            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                int readCount = 0;
                while (sourceFileLength > 0)
                {
                    //Read Byte Array from Source File
                    readCount = inFS.Read(sourceBytes, 0, sourceFileLength);
                    if (readCount == 0)
                        break;
                    sourceFileLength -= readCount;

                    List<int> found1 = PatternAt1(sourceBytes, SearchBytes);
                    matchFound += found1.Count;

                    int cursor = 0;
                    int count = 0;
                    for (int i = 0; i <= found1.Count; i++)
                    {
                        count = i == found1.Count ? readCount - cursor : found1[i] - cursor;
                        outFS.Write(sourceBytes, cursor, count);
                        if (i < found1.Count)
                        {
                            outFS.Write(ReplaceBytes, 0, ReplaceBytes.Length);
                            cursor = found1[i] + SearchBytes.Length;
                        }

                    }
                }
                System.Threading.Thread.Sleep(500);
                stopwatch.Stop();
                statusPanel.Text = stopwatch.Elapsed.TotalSeconds.ToString() + " seconds - " + matchFound.ToString() + " match(s) found.";
                MessageBox.Show("File is Converted.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                inFS.Close();
                outFS.Close();
            }
            //statusPanel.Text = "Ready";
            GC.Collect();
            System.Threading.Thread.Sleep(1000);     //give disk hardware time to recover
        }

        private void Process_Bypass()
        {
            matchFound = 0;

            FileStream inFS = new FileStream(txtSource.Text, FileMode.Open, FileAccess.Read);
            int sourceFileLength = (int)inFS.Length;

            fileSizePanel.Text = "FileSize: " + sourceFileLength.ToString("##,###,###,###");
            statusPanel.Text = "Processing";

            int bufferLength = 1024;        //512: 31 seconds, 1024: 41 seconds
            byte[] bufferBytes = new byte[bufferLength];

            //Open Target File
            FileStream outFS = new FileStream(txtTarget.Text, FileMode.Create);    //overwrite

            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                long cursorPoint = 0;
                int readCount = 0;
                while ((readCount = inFS.Read(bufferBytes, 0, bufferLength)) != 0)
                {
                    List<int> found2 = PatternAt2(bufferBytes, SearchBytes).ToList();
                    matchFound += found2.Count;

                    int cursor = 0;
                    int count = 0;
                    for (int i = 0; i <= found2.Count; i++)
                    {
                        count = i == found2.Count ? readCount - cursor : found2[i] - cursor;
                        outFS.Write(bufferBytes, cursor, count);
                       if (i < found2.Count)
                        {
                            outFS.Write(ReplaceBytes, 0, ReplaceBytes.Length);
                            cursor = found2[i] + SearchBytes.Length;
                        }
                        cursorPoint += count;
                    }
                    Array.Clear(bufferBytes, 0, bufferLength);
                    if (cursorPoint % 100 == 0 && cbProgress.Checked)
                        statusPanel.Text = cursorPoint.ToString("##,###,###,###");
                }

                System.Threading.Thread.Sleep(500);
                stopwatch.Stop();
                statusPanel.Text = stopwatch.Elapsed.TotalSeconds.ToString() + " seconds - " + matchFound.ToString() + " match(s) found.";
                MessageBox.Show("File is Converted.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                inFS.Close();
                outFS.Close();
            }
            //statusPanel.Text = "Ready";
            GC.Collect();
            System.Threading.Thread.Sleep(1000);     //give disk hardware time to recover
        }

        // --- 25 seconds with 100 Task.Factory.StartNew: the most fast
        // --- 28 seconds with 100 thread
        // Step:
        // 1. Calculate Loop Count
        // 2. Create Thread and put process result into chunk of List
        // 3. Thread Join and Write to File

        public List<Task> TaskList = new List<Task>();

        public class ThreadStream
        {
            public int id;              //chunk id
            public int matchCount;
            public int readSize;
            public MemoryMappedViewAccessor accessor;
            public Stream chunkStream;  //converted chunk
        }
        public List<ThreadStream> mmfChunkList = new List<ThreadStream>();    //Converted MMF Chunk

        private int maxThreads = 100;

        public void Process_MMF()
        {
            matchFound = 0;
            FileStream inFS = new FileStream(txtSource.Text, FileMode.Open, FileAccess.Read);
            var sourceFileLength = inFS.Length;
            inFS.Close();

            fileSizePanel.Text = "FileSize: " + sourceFileLength.ToString("##,###,###,###");
            statusPanel.Text = "Processing";

            //Open Target File
            FileStream outFS = new FileStream(txtTarget.Text, FileMode.Create);    //overwrite

            //measure loop count to build MMF bundle
            int unitSize = 1048576;                         //1 Mega Bytes: 1,048,576 bytes
            int loopCount = (int)(sourceFileLength / unitSize);
            if (sourceFileLength % unitSize > 0)
                loopCount++;

            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                long filePoint = 0;
                int readSize = 0;
                byte[] bufferBytes = new byte[unitSize];

                using (var mmf = MemoryMappedFile.CreateFromFile(txtSource.Text, FileMode.Open, "Map1"))
                {
                    int t = 0;
                    while (t < loopCount)
                    {
                        TaskList.Clear();
                        mmfChunkList.Clear();

                        for (int i = 0; i < maxThreads && t < loopCount; i++)
                        {
                            readSize = sourceFileLength - filePoint > unitSize ? unitSize : (int)(sourceFileLength - filePoint);
                            MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor(filePoint, readSize);
                            var obj = new ThreadStream
                            {
                                id = i,
                                readSize = readSize,
                                accessor = accessor
                            };
                            mmfChunkList.Add(obj);

                            start_thread(i);    //create task

                            filePoint += readSize;
                            t++;
                        }

                        Task.WaitAll(TaskList.ToArray());

                        //Write into File from ChunkList
                        for (int idx = 0; idx < mmfChunkList.Count; idx++)
                        {
                            mmfChunkList[idx].chunkStream.Seek(0, SeekOrigin.Begin);
                            mmfChunkList[idx].chunkStream.CopyTo(outFS);

                            matchFound += mmfChunkList[idx].matchCount;
                        }

                        if (cbProgress.Checked)
                            statusPanel.Text = string.Format("{0:0,0}", filePoint);

                    }
                }

                System.Threading.Thread.Sleep(500);
                stopwatch.Stop();
                statusPanel.Text = stopwatch.Elapsed.TotalSeconds.ToString() + " seconds - " + matchFound.ToString() + " match(s) found.";
                MessageBox.Show("File is Converted.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                outFS.Close();
            }
            GC.Collect();
            System.Threading.Thread.Sleep(1000);     //give disk hardware time to recover
        }

        public void start_thread(int id)
        {
            var task = Task.Factory.StartNew(() =>
            {
                Do_Chunk_Process(mmfChunkList[id]);
            });
            TaskList.Add(task);
        }

        public int Do_Chunk_Process(Object hostObject)
        {
            var obj = hostObject as ThreadStream;   //call by reference

            byte[] bufferBytes = new byte[obj.readSize];
            int readCount = obj.accessor.ReadArray(0, bufferBytes, 0, obj.readSize);
            List<int> searchList = PatternAt1(bufferBytes, SearchBytes).ToList();
            obj.matchCount = searchList.Count;

            MemoryStream stream = new MemoryStream();

            int cursor = 0;
            int count = 0;
            for (int i = 0; i <= searchList.Count; i++)
            {
                count = i == searchList.Count ? readCount - cursor : searchList[i] - cursor;
                stream.Write(bufferBytes, cursor, count);
                if (i < searchList.Count)
                {
                    stream.Write(ReplaceBytes, 0, ReplaceBytes.Length);
                    cursor = searchList[i] + SearchBytes.Length;
                }
            }
            stream.Seek(0, SeekOrigin.Begin);
            obj.chunkStream = stream;
            obj.accessor.Flush();
            obj.accessor.Dispose();
            return readCount;
        }

        
        
        //WORKING: Byte Compare : 61 seconds for 2.3 GB file
        static public List<int> PatternAt1(byte[] source, byte[] pattern)
        {
            List<int> positions = new List<int>();

            int totalLength = source.Length - pattern.Length + 1;
            int j;
            for (int i = 0; i < totalLength; i++)
            {
                if (source[i] == pattern[0])
                {
                    for (j = pattern.Length - 1; j >= 1 && source[i + j] == pattern[j]; j--) 
                        ;
                    if (j == 0)
                    {
                        positions.Add(i);
                        //i += patternLength - 1;   //move pointer
                    }
                }
            }
            return positions;
        }

        //WORKING: RegEx : 129 seconds for 2.3 GB file
        public List<int> PatternAt2(byte[] source, byte[] pattern)
        {
            List<int> positions = new List<int>();

            string inString = System.Text.Encoding.UTF8.GetString(source);
            //CultureInfo ci = new CultureInfo("en-us");
            string looks = string.Empty;
            foreach (var item in pattern)
            {
                looks += string.Format(@"\x{0}", item.ToString("x2", new CultureInfo("en-us")));
            }
            Regex regex = new Regex(looks);
            var matches = regex.Matches(inString);
            if (matches.Count > 0)
            {
                for (int i = 0; i < matches.Count; i++)
                {
                    positions.Add(matches[i].Index);
                }
            }
            return positions;
        }

        //WORKING: LINQ - Bad performance
        public IEnumerable<int> PatternAt3(byte[] source, byte[] pattern)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (source.Skip(i).Take(pattern.Length).SequenceEqual(pattern))
                {
                    yield return i;
                }
            }
        }

        //WORKING: RegEx
        public Regex regEx = new Regex(@"([a-fA-F0-9]{2})");        //@"^([a-fA-F0-9]{2}\s+)"

        private void txtSearchHEX_Leave(object sender, EventArgs e)
        {
            //string txtSearchHEX.Text = "AA 11 22 33 44 55 66 77 88 99 AA BB CC DD EE FF";
            //check Search string as HEX
            if (txtSearchHEX.Text.Trim().Length > 0)
            {
                //"[^a-fA-F0-9 ]" --- a-f, A-F, 0-9 and space
                //"[^a-fA-F0-9]"  --- a-f, A-F, 0-9
                txtSearchHEX.Text = Regex.Replace(txtSearchHEX.Text, "[^a-fA-F0-9]", "");
                MatchCollection match = Regex.Matches(txtSearchHEX.Text, @"([a-fA-F0-9]{2})");
                if (match.Count > 0)
                {
                    string[] array = SplitByLength(txtSearchHEX.Text, 2).ToArray();
                    txtSearchHEX.Text = string.Join(" ", array);
                }
                else
                    txtSearchHEX.Text = string.Empty;
            }

        }

        private void txtReplaceHEX_Leave(object sender, EventArgs e)
        {
            if (txtReplaceHEX.Text.Trim().Length > 0)
            {
                txtReplaceHEX.Text = Regex.Replace(txtReplaceHEX.Text, "[^a-fA-F0-9]", "");
                MatchCollection match = Regex.Matches(txtReplaceHEX.Text, @"([a-fA-F0-9]{2})");
                if (match.Count > 0)
                {
                    string[] array = SplitByLength(txtReplaceHEX.Text, 2).ToArray();
                    txtReplaceHEX.Text = string.Join(" ", array);
                }
                else
                    txtReplaceHEX.Text = string.Empty;
            }
        }

        static IEnumerable<string> SplitByLength(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }

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

    }
}
