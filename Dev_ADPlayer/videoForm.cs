using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dev_ADPlayer
{
    public partial class videoForm : Form
    {
        string AppPath = Application.StartupPath;
        string AppName = "Player";

        public videoForm()
        {
            InitializeComponent();         
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                // 自動依照 Video 目錄內的檔案建立播放清單
                FileInfo info;
                string[] mediaFile;
                string fileName;

                // 使用 playList 節省效能
                WMPLib.IWMPPlaylist playList = axWMP.playlistCollection.newPlaylist("myPlayList");
                WMPLib.IWMPMedia media;

                mediaFile = Directory.GetFiles(AppPath + "\\ADmedia1\\", "*.*");
                foreach (string item in mediaFile)
                {
                    info = new FileInfo(item);
                    fileName = info.Name.ToLower();
                    if (fileName.EndsWith(".mp4") || fileName.EndsWith(".jpg") || fileName.EndsWith(".png"))
                    {
                        fileName = info.FullName.ToLower();
                        media = axWMP.newMedia(fileName);
                        playList.appendItem(media);
                    }
                }

                // 媒體播放器設定
                axWMP.windowlessVideo = true;
                axWMP.stretchToFit = true;
                axWMP.settings.mute = false;
                axWMP.settings.volume = 30;
                axWMP.settings.autoStart = true;

                // 循環
                axWMP.settings.setMode("loop", true);
                axWMP.currentPlaylist = playList;
                axWMP.Ctlcontrols.play();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, AppName);
            }
        }      


        private void videoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm mainForm = new mainForm();           
            mainForm.callButton();
        }
        
    }
}
