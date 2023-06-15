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
using System.IO;


namespace Dev_ADPlayer
{
    public partial class mainForm : Form
    {        
        private BackgroundWorker bw;   // 背景執行(顯示時間計數)   
        Thread videoThread;            // 影片執行             

        public mainForm()
        {
            InitializeComponent();
            initBackgroundWorker();                     
        }


        private void mainForm_Load(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = true;

            // 啟動 videoForm 開始
            // 建立 ThreadStart，它是用來表示在執行緒上執行的方法(啟動videoForm)
            ThreadStart myRun = new ThreadStart(threadVideoForm);

            // 建立 Thread 類別，它是用來建立和控制執行緒，設定執行緒的優先權，並取得它的狀態
            videoThread = new Thread(myRun);      
            
            // 建立並輸入執行緒
            videoThread.SetApartmentState(ApartmentState.STA);

            videoThread.Start();

            if (videoThread.IsAlive)
            {               
                btnStart.Enabled = false;                
            }            
           
            // 執行時間計數
            if (bw.IsBusy != true)
            {
                bw.RunWorkerAsync();                             
            }
        }


        public void threadVideoForm()
        {
            videoForm videoForm = new videoForm();
            videoForm.ShowDialog();    
        }


        // 背景執行初始化
        private void initBackgroundWorker()
        {
            // 建立一個 BackgroundWorker 執行緒
            bw = new BackgroundWorker();  
            
            // 建立一個 DoWork 事件，指定 bw_DoWork 方法去做事
            bw.DoWork += new DoWorkEventHandler(bwDoWork);

            bw.WorkerReportsProgress = true;          
            bw.WorkerSupportsCancellation = true;
            bw.ProgressChanged += new ProgressChangedEventHandler(bwProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwRunWorkerCompleted);
        }


        // 背景執行
        private void bwDoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= 10000; i++)
            {
                // 使用 sleep 模擬運算時的停頓
                Thread.Sleep(100);
                bw.ReportProgress((i * 1));

                if ((bw.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
            }
        }        


        // 處理進度 顯示在畫面上
        private void bwProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.lblText.Text = $"處理中... " + e.ProgressPercentage.ToString();
        }


        // 執行完成
        private void bwRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.lblText.Text = "完成!";
            btnStart.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = true;
           
            videoThread.Abort();
            bw.CancelAsync();
        }

        public void callButton()
        {
            this.btnStart.Enabled = true;            
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {           
            Application.Exit();
        }

        
    }

}






