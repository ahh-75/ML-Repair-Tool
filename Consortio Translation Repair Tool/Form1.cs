using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
//using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Consortio_Translation_Repair_Tool
{
    delegate void SetControlPropertyThreadSafeDelegate(
    Control control,
    string propertyName,
    object propertyValue);
    public partial class Form1 : Form
    {
        private string _folderPath;
        private string _folderCopyPath;
        private bool _findlastBracketsCaptions = false;
        private bool _findlastBracketsPageNames = false;
        private List<string> _lineTemp = new List<string>();
        private System.Timers.Timer _timer;
        private DateTime _startTime = DateTime.MinValue;
        private TimeSpan _currentElapsedTime = TimeSpan.Zero;
        private TimeSpan _totalElapsedTime = TimeSpan.Zero;
        private bool _timerRunning = false;


        public Form1()
        {
            InitializeComponent();
            _timer = new System.Timers.Timer();
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_tick);
            _timer.Interval = 1000;
           // this.Icon = Consortio_Translation_Repair_Tool.Properties.Resources.Logo;
        }

        // Timer event
        public void _timer_tick(object sender, EventArgs e)
        {
            var timeSinceStartTime = DateTime.Now - _startTime;
            timeSinceStartTime = new TimeSpan(timeSinceStartTime.Hours,
                                              timeSinceStartTime.Minutes,
                                              timeSinceStartTime.Seconds);
            // The current elapsed time is the time since the start 
            // was clicked, plus the total time elapsed since the last reset
            _currentElapsedTime = timeSinceStartTime + _totalElapsedTime;
         
            TotalElapsedTimeDisplayLbl.Invoke((MethodInvoker)(() => TotalElapsedTimeDisplayLbl.Text = _currentElapsedTime.ToString()));
        }

        private void OpenBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            // Show the FolderBrowserDialog.  
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                _folderPath = folderDlg.SelectedPath;
                pathLbl.Text = _folderPath;
                //Environment.SpecialFolder root = folderDlg.RootFolder;
            }
            folderDlg.Dispose();
        }

        private async void ExecuteBtn_Click(object sender, EventArgs e)
        {
          
            await Task.Run(() => Execute());
           

        }
        private void Execute()
        { 
            // Reset the elapsed time TimeSpan objects
            _totalElapsedTime = TimeSpan.Zero;
            _currentElapsedTime = TimeSpan.Zero;
            // If the timer isn't already running
            if (!_timerRunning)
            {
                // Set the start time to Now
                _startTime = DateTime.Now;

                // Store the total elapsed time so far
                _totalElapsedTime = _currentElapsedTime;

                _timer.Start();
                _timerRunning = true;
            }
            else // If the timer is already running
            {
                _timer.Stop();
                _timerRunning = false;
            }

            //Copy to Temp folder
            try
            {
                _folderCopyPath = _folderPath + "Copy";
                if (!Directory.Exists(_folderCopyPath))
                {
                    Directory.CreateDirectory(_folderCopyPath);
                }
                var files = Directory.GetFiles(_folderPath, "*.txt");

                foreach (string s in files)
                {
                    var fileName = System.IO.Path.GetFileName(s);
                    var destFile = System.IO.Path.Combine(_folderCopyPath, fileName);
                    System.IO.File.Copy(s, destFile, true);
                }
              
                copyInfoLbl.Invoke((MethodInvoker)(() => copyInfoLbl.Text = $"{_folderPath } is copied to new folder {_folderCopyPath}"));

            }
            catch (IOException ex)
            {
                Updateoutput(ex.InnerException.Message);
                MessageBox.Show(ex.InnerException.ToString());
            }
            finally
            {
                ExecuteFileTransformation();
            }

        }
        //Execute file transformation
        private void ExecuteFileTransformation()
        {
            string output;
            string backupFile;
            foreach (string filesInDir in Directory.GetFiles(_folderCopyPath, "*.txt"))
            {
                string dir = Path.GetDirectoryName(filesInDir);
                string fileName = Path.GetFileNameWithoutExtension(filesInDir);
                string fileExt = Path.GetExtension(filesInDir);
                output = Path.Combine(dir, fileName + "Copy" + fileExt);
                backupFile = Path.Combine(dir, fileName + "Copy" + fileExt+".bac");

                int CaptionMLCounter = 1;
                int TextConstCounter = 0;
                string line;
                string CaptionMLWithENU = "CaptionML=ENU=";
                string PageNamesML = "PageNamesML=";
                string TextConst = ": TextConst";
                string CaptionML = "CaptionML=";
                Encoding encoding = Encoding.GetEncoding(850);
                try
                {
                    using (System.IO.StreamReader fileReader = new System.IO.StreamReader(filesInDir, encoding))
                    {
                        using (System.IO.StreamWriter fileWriter = new System.IO.StreamWriter(output))
                        {

                            while ((line = fileReader.ReadLine()) != null)
                            {
                                if (line.Contains(CaptionML) && line.Contains('[') || _findlastBracketsCaptions)
                                {

                                    var newLine = AddDanFromEnuInArrayCaptionML(line);
                                    fileWriter.WriteLine(newLine);
                                    if (line.Contains(']'))
                                    {
                                        if (DisplayProgressOutputCb.Checked)
                                        {
                                            UpdateControl($"Line found: {CaptionMLCounter++} line content: {line} ");
                                            UpdateControl($"CaptionML Result: { newLine }");
                                        }
                                    }
                                }
                                else if (line.Contains(CaptionMLWithENU))
                                {
                                    var newLine = AddDanFromEnuOnCaptionML(line);
                                    fileWriter.WriteLine(newLine);
                                    if (DisplayProgressOutputCb.Checked)
                                    {
                                        UpdateControl($"Line found: {CaptionMLCounter++} line content: {line} ");
                                        UpdateControl($"CaptionML Result: { newLine }");
                                    }
                                }
                                else if (line.Contains(PageNamesML)&& line.Contains('}'))
                                {
                                    var newLine = AddDanFromEnuPagenamesML(line);
                                    fileWriter.WriteLine(newLine);
                                    if (DisplayProgressOutputCb.Checked)
                                    {
                                        UpdateControl($"Line found: { TextConstCounter++} line content: {line} ");
                                        UpdateControl($"TextConst Result: { newLine }");
                                    }
                                }
                                else if (line.Contains('[') && line.Contains(PageNamesML) || _findlastBracketsPageNames)
                                {
                                    var newLine = AddDanFromEnuInArrayPageNamesML(line);
                                    fileWriter.WriteLine(newLine);
                                    if (line.Contains(']'))
                                    {
                                        if (DisplayProgressOutputCb.Checked)
                                        {
                                            UpdateControl($"Line found: {CaptionMLCounter++} line content: {line} ");
                                            UpdateControl($"CaptionML Result: { newLine }");
                                        }
                                    }
                                }

                                else if (line.Contains(TextConst))
                                {
                                    var newLine = AddDanFromEnuOnTextConst(line);
                                    fileWriter.WriteLine(newLine);
                                    if (DisplayProgressOutputCb.Checked)
                                    {
                                        UpdateControl($"Line found: { TextConstCounter++} line content: {line} ");
                                        UpdateControl($"TextConst Result: { newLine }");
                                    }
                                }
                                else
                                    fileWriter.WriteLine(line);
                            }
                            fileWriter.Close();
                            fileReader.Close();
                        }
                    }
                    if (File.Exists(output))
                    {
                        File.Replace(output, filesInDir, backupFile);
                        File.Delete(backupFile);
                    }
                }
                catch(Exception ex)
                {
                    if (DisplayProgressOutputCb.Checked)
                        Updateoutput(ex.Message);
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    // Stop and reset the timer if it was running
                    _timer.Stop();
                    _timerRunning = false;
                    onComplete();
                }
                if (DisplayProgressOutputCb.Checked)
                    UpdateControl(filesInDir + $" Count : {CaptionMLCounter.ToString()}" + Environment.NewLine);
               
            }
        }

        // Output info to RichTextbox
        private void Updateoutput(string text)
        {
            try
            {
                richTextBox1.SuspendLayout();

                int len = text.Length;
                int start = richTextBox1.Text.Length;
                richTextBox1.Text += text + Environment.NewLine;
                richTextBox1.Select(start, len);
                richTextBox1.SelectionColor = Color.White;
                richTextBox1.Select(richTextBox1.Text.Length, 0);
                richTextBox1.ScrollToCaret();
            }
            finally
            {
                richTextBox1.ResumeLayout();
            }
        }

        private string AddDanFromEnuOnCaptionML(string line)
        {
            string newLine = line;
            if (!line.Contains("DAN") && line.Contains("ENU"))
            {
                int pos1 = line.IndexOf("ENU") + 2;
                int pos3 = line.IndexOf("CaptionML")-1;
                var semiColonBeforeCaption = line.Substring(pos3, 1);
                int pos2 = line.IndexOf('}');
                int pos4 = line.Length-1;
                string content;

                if (pos2 != -1 && pos2 > pos1)
                {
                    pos2 = pos2 - 1;
                    
                    if (semiColonBeforeCaption == ";")
                    {
                        content = line.Substring(0, pos3 + 1);
                        newLine = $"{content}CaptionML=[DAN{line.Substring(pos1 + 1, pos2 - pos1).Trim()};ENU{line.Substring(pos1 + 1, pos2 - pos1).Trim()}];" + " }";
                    }
                    else
                    {
                        newLine = $"CaptionML=[DAN{line.Substring(pos1 + 1, pos2 - pos1).Trim()};ENU{line.Substring(pos1 + 1, pos2 - pos1).Trim()}];" + " }";
                    }
                }
                else
                {
                    if (semiColonBeforeCaption == ";")
                    {
                        content = line.Substring(0, pos3 + 1);
                        newLine = $"{content}CaptionML=[DAN{line.Substring(pos1 + 1, pos4 - pos1).Trim()}ENU{line.Substring(pos1 + 1, (pos4 - pos1) - 1).Trim()}];";
                    }
                    else
                    {
                        pos2 = line.Length - 1;
                        var CaptionValue = line.Substring(pos1 + 1, (pos2 - pos1) - 1);
                        newLine = "CaptionML=[DAN" + CaptionValue.Trim() + ";ENU" + CaptionValue.Trim() + "];";
                    }
                }
            }
            return newLine;
        }

        private string AddDanFromEnuPagenamesML(string line)
        {
            string newLine = line;
            if (!line.Contains("DAN") && line.Contains("ENU"))
            {
                int pos1 = line.IndexOf("ENU") + 2;
                int pos2 = line.IndexOf('}')-1;
                if (pos2 != -1 && pos2 > pos1)
                    newLine = $"PageNamesML=[DAN{line.Substring(pos1 + 1, pos2 - pos1).Trim()};ENU{line.Substring(pos1 + 1, pos2 - pos1).Trim()}];" + " }";
            }
            else if(!line.Contains("DAN") && line.Contains("DEU"))
            {
                int pos1 = line.IndexOf("DEU") + 2;
                int pos2 = line.IndexOf('}')-1;
                if (pos2 != -1 && pos2 > pos1)
                    newLine = $"PageNamesML=[DAN{line.Substring(pos1 + 1, pos2 - pos1).Trim()};DEU{line.Substring(pos1 + 1, pos2 - pos1).Trim()}];" + " }";
            }
                return newLine;
        }

        private string AddDanFromEnuOnTextConst(string line)
        {
            string newLine = line;
            if (!line.Contains("DAN") && (!line.Contains("DEU") && line.Contains("ENU")))
            {
                int pos1 = line.IndexOf("ENU") + 2;
                int pos2 = line.IndexOf(';');
                var textConstStr = "TextConst";
                int pos3 = line.IndexOf(textConstStr) + textConstStr.Length;
                string textConstVar = line.Substring(0, pos3);
                string textConstValue = line.Substring(pos1 + 1, (pos2 - pos1)-2);
                newLine = $"{textConstVar} 'DAN{textConstValue};ENU{textConstValue}';";
            
            }
            else
            {
                int posDEU = 0;
                int posENUValue = 0;
                int posDEUValue = 0;
                string textConstValue = "";
                string currentValue = "";

                if (line.Contains("ENU") && line.Contains("DEU"))
                {
                    posDEU = line.IndexOf("DEU");
                    posENUValue = line.IndexOf("ENU") + 3;
                }
                else if(line.Contains("DEU") && !line.Contains("ENU"))
                {
                    posDEUValue = line.IndexOf("DEU") + 3;
                    posDEU = line.IndexOf("DEU");
                }
                    int pos4 = line.Length - 1;
                    var textConstStr = "TextConst '";
                    int pos3 = line.IndexOf(textConstStr) + textConstStr.Length;
                    string textConstVar = line.Substring(0, pos3);
                if (line.Contains("ENU") && line.Contains("DEU"))
                {
                    textConstValue = line.Substring(posENUValue, (pos4 - posENUValue) - 1);
                    currentValue = line.Substring(posDEU, (line.Length - posDEU));
                }
                else if (line.Contains("DEU") && !line.Contains("ENU"))
                {
                    textConstValue = line.Substring(posDEUValue, (pos4 - posDEUValue) - 1);
                    currentValue = line.Substring(posDEU, (line.Length - posDEU));
                }
                if(!string.IsNullOrEmpty(currentValue))
                    newLine = textConstVar + "DAN" + textConstValue + ";" + currentValue;
                
           
            }
            return newLine;
        }

        private string AddDanFromEnuInArrayCaptionML(string line)
        {
            string newLine = line;
            if(!line.Contains("DAN"))
            {
                if (!_findlastBracketsCaptions)
                {
                    _lineTemp.Add(line);
                    _findlastBracketsCaptions = true;
                }
                else if (line.Contains(']') && _findlastBracketsCaptions && line.Contains("ENU"))
                {
                    _lineTemp.Add(line.Trim());
                    _findlastBracketsCaptions = false;
                    int pos1 = line.IndexOf("ENU") + 2;
                    int pos2 = line.IndexOf(']');
                    int pos3 = line.IndexOf("ENU");
                    string captionValue = line.Substring(pos1 + 1, (pos2 - pos1) - 1);
                    _lineTemp.Add($"DAN{ captionValue};");
                    string newlineLength = new string(' ', line.Length);
                    newLine = newlineLength.Insert(pos3, _lineTemp[2] + _lineTemp[1]);
                    _lineTemp.Clear();
                }
                else if(_findlastBracketsCaptions && line.Contains("ENU"))
                {
                    _lineTemp.Add(line.Trim());
                }
                else if(line.Contains(']') && _findlastBracketsCaptions && line.Contains("VIT"))
                {
                    _findlastBracketsCaptions = false;
                    string ENUContent = _lineTemp[1];
                    int pos1 = ENUContent.IndexOf("ENU") + 2;
                    int pos2 = ENUContent.IndexOf(';');
                    string captionValue = ENUContent.Substring(pos1 + 1,(pos2 - pos1) - 1);
                    newLine = $"DAN{captionValue};{line}";
                    _lineTemp.Clear();

                }
            }
            return newLine;
        }

        private string AddDanFromEnuInArrayPageNamesML(string line)
        {
            string newLine = line;
            if (!line.Contains("DAN"))
            {
                if (!_findlastBracketsPageNames)
                {
                    _lineTemp.Add(line);
                    _findlastBracketsPageNames = true;
                }
                else if (line.Contains(']') && _findlastBracketsPageNames && line.Contains("ENU"))
                {
                    _lineTemp.Add(line.Trim());
                    _findlastBracketsPageNames = false;
                    int pos1 = line.IndexOf("ENU") + 2;
                    int pos2 = line.IndexOf(']');
                    int pos3 = line.IndexOf("ENU");
                    string captionValue = line.Substring(pos1 + 1, (pos2 - pos1) - 1);
                    _lineTemp.Add($"DAN{ captionValue};");
                    string newlineLength = new string(' ', line.Length);
                    newLine = newlineLength.Insert(pos3, _lineTemp[2] + _lineTemp[1]);
                    _lineTemp.Clear();
                }
            }

            return newLine;
        }


        //Update Label on UI thread
        public static void SetControlPropertyThreadSafe(Control control, string propertyName, object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new SetControlPropertyThreadSafeDelegate(SetControlPropertyThreadSafe), new object[] { control, propertyName, propertyValue });
            }
            else
            {
                control.GetType().InvokeMember(
                    propertyName,
                    BindingFlags.SetProperty,
                    null,
                    control,
                    new object[] { propertyValue });
            }
        }

        //Update RichTextBox on UI thread
        void UpdateControl(string text)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new Action<string>(Updateoutput), text);
                return;
            }
            else
            {
                //Code goes here to apply the update.  This will run on the UI thread, 
                //such as your call to update your RichTextBox:
                Updateoutput(text);
            }
        }

        Action onComplete = () => 
        {
            MessageBox.Show("COMPLETED!!");
        };
    }
}
