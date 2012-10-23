using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace LogMonitor
{
    public partial class View : Form
    {
        string filePathToMonitor = String.Empty;
        FileToMonitor fileToMonitor = new FileToMonitor();
        LogStack logStack = new LogStack();
        LogStack historyStack = new LogStack();

        public View()
        {
            InitializeComponent();
        }

        private static void timer_Elapsed(FileToMonitor fileToMonitor,BackgroundWorker bgw1, LogStack logStack,LogStack historyStack)
        {
            object[] args = new object[3];
            args[0] = fileToMonitor;
            args[1] = logStack;
            args[2] = historyStack;
            if (!bgw1.IsBusy)
            {
                bgw1.RunWorkerAsync(args);
            }
        }

        private void View_Load(object sender, EventArgs e)
        {

        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdlg = new OpenFileDialog();
            ofdlg.ShowDialog();
            filePathToMonitor = ofdlg.FileName;
            btnFile.Text = filePathToMonitor;
        }

        private void btnMonitor_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(filePathToMonitor))
            {
                BeginMonitoring(false,File.GetLastWriteTime(filePathToMonitor));
            }
            else
            {
                MessageBox.Show("Please choose a file to monitor.");
            }
        }

        private void BeginMonitoring(bool hasBeenMonitored,DateTime accessedTimeStamp)
        {
            if(!String.IsNullOrEmpty(filePathToMonitor))
            {
                fileToMonitor.setPath(filePathToMonitor);
                fileToMonitor.setDateTime(File.GetLastWriteTime(fileToMonitor.getPath()));
            }

            if (DateTime.Compare(accessedTimeStamp, fileToMonitor.getTimeStamp()) > 0)
            {
                logStack = Monitor();
            }

            if (hasBeenMonitored.Equals(false) || DateTime.Compare(accessedTimeStamp, fileToMonitor.getTimeStamp())== 0)
            {
                logStack = Monitor();
            }

            var timer = new System.Timers.Timer(10000);
            timer.Elapsed += delegate { timer_Elapsed(fileToMonitor, backgroundWorker1,logStack,historyStack); };
            timer.AutoReset = true;
            timer.Start();
        }

        private LogStack Monitor()
        {
            StreamReader sr = new StreamReader(fileToMonitor.getPath());

            while (!sr.EndOfStream)
            {
                Node node = new Node();
                node.setText(sr.ReadLine());
                logStack.Push(node);
            }

            sr.Close();

            WriteValuesToScreen(logStack);
            return logStack;
        }

        private void WriteValuesToScreen(LogStack logStack)
        {
            string text = String.Empty;

            while (!logStack.GetCount().Equals(0))
            {
                Node node = logStack.Pop();

                text = text + Environment.NewLine + node.getText();
                historyStack.Push(node);
            }

            txtbxLogDisplay.Text = text;
        }

        static LogStack compareWithFile(FileToMonitor fileToMonitor, LogStack logStack,LogStack historyStack)
        {
            if (!fileToMonitor.getPath().Equals(null))
            {
                DateTime dateToCompare = File.GetLastWriteTime(fileToMonitor.getPath());

                if (DateTime.Compare(dateToCompare, fileToMonitor.getTimeStamp()) > 0)
                {
                    StreamReader sr = new StreamReader(fileToMonitor.getPath());

                    while (!sr.EndOfStream)
                    {
                        Node node = new Node();
                        string line = sr.ReadLine();

                        if(!line.Equals(historyStack.ViewHead()))
                        {
                            node.setText(line);
                            logStack.Push(node);
                        }
                    }

                    sr.Close();
                }
            }
            return historyStack;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            FileToMonitor ftm = (FileToMonitor)((object[])e.Argument)[0];
            LogStack ls = (LogStack)((object[])e.Argument)[1];
            LogStack hs = (LogStack)((object[])e.Argument)[2];
            e.Result = compareWithFile(ftm, ls,hs);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            historyStack = (LogStack)e.Result;
            WriteValuesToScreenFromThread(logStack,fileToMonitor);
        }

        private void WriteValuesToScreenFromThread(LogStack logStack, FileToMonitor fileToMonitor)
        {
            if (DateTime.Compare(File.GetLastWriteTime(fileToMonitor.getPath()), fileToMonitor.getTimeStamp()) > 0)
            {
                string text = String.Empty;

                while (!logStack.GetCount().Equals(0))
                {
                    Node node = logStack.Pop();

                    text = text + Environment.NewLine + node.getText();
                    historyStack.Push(node);
                }

                txtbxLogDisplay.Invoke((MethodInvoker)delegate { txtbxLogDisplay.Text = text; });
                fileToMonitor.setDateTime(File.GetLastWriteTime(fileToMonitor.getPath()));
            }
        }
    }
}
