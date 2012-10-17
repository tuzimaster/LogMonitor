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
                Monitor();
            }

            if (hasBeenMonitored.Equals(false) || DateTime.Compare(accessedTimeStamp, fileToMonitor.getTimeStamp())== 0)
            {
                Monitor();
            }
        }

        private void Monitor()
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
    }
}
