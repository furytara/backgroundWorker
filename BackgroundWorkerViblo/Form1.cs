using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackgroundWorkerViblo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            myBackgroundWorker.DoWork += myBackgroundWorker_DoWork;
            myBackgroundWorker.ProgressChanged += myBackgroundWorker_ProgressChanged;
            myBackgroundWorker.RunWorkerCompleted += myBackgroundWorker_RunWorkerCompleted;

            myBackgroundWorker.WorkerReportsProgress = true;
            myBackgroundWorker.WorkerSupportsCancellation = true;

            btStart.Enabled = true;
            btStop.Enabled = false;
        }

        void myBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                lbStatus.Text = "Task Cancelled.";
            }

            else if (e.Error != null)
            {
                lbStatus.Text = "Error while performing background operation.";
            }
            else
            {
                // Everything completed normally.
                lbStatus.Text = "Task Completed...";
            }

            //Change the status of the buttons on the UI accordingly
            btStart.Enabled = true;
            btStop.Enabled = false;
        }

        void myBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            lbStatus.Text = "Processing......" + progressBar1.Value.ToString() + "%";
        }

        void myBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(100);

                myBackgroundWorker.ReportProgress(i);

                if (myBackgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    myBackgroundWorker.ReportProgress(0);
                    return;
                }
            }
            
            myBackgroundWorker.ReportProgress(100);
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            btStart.Enabled = false;
            btStop.Enabled = true;

            myBackgroundWorker.RunWorkerAsync();
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            if (myBackgroundWorker.IsBusy)
            {
                myBackgroundWorker.CancelAsync();
            }
        }
    }
}
