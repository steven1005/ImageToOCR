using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Patagames.Ocr;
using Patagames.Ocr.Enums;

namespace ImageToOCR
{
    public partial class Form1 : Form
    {
        private NotifyIcon notifyIcon;
        private ContextMenu contextMenu;
        private bool isLeftCtrlKeyPressed = false;

        HookFunction hf;

        public Form1()
        {
            InitializeComponent();
            
            hf = new HookFunction();
            HookFunction._isDrawRect = false;
            HookFunction._isRectBitmapCopyToClip = true;

            pbImage.AllowDrop = true;
            // 处理 KeyDown 事件
            this.KeyPreview = true;

            // 初始化托盘图标
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = new Icon(SystemIcons.Application, 40, 40);

            // 初始化托盘菜单
            contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add("Show", ShowForm);
            contextMenu.MenuItems.Add("Exit", ExitApplication);
            notifyIcon.ContextMenu = contextMenu;

            // 显示托盘图标
            notifyIcon.Visible = true;

            // 最小化窗体到托盘
            this.WindowState = FormWindowState.Minimized;
            
        }

        #region MouseHookEvent
        private void btnCaptureWindow_Click(object sender, EventArgs e)
        {
            
        }
        #endregion
        private void ShowForm(object sender, EventArgs e)
        {
            // 显示窗体
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            this.Activate();
        }

        private void ExitApplication(object sender, EventArgs e)
        {
            // 退出应用程序
            Application.Exit();
        }

        public string ExtractTextFromImage(string sImgPath)
        {
            string sRet = "";

            try
            {
                using (var api = OcrApi.Create())
                {
                //api.Init(Languages.ChineseTraditional | Languages.English);
                    api.Init(Languages.English);
                    sRet = api.GetTextFromImage(sImgPath);
                }
                UpdateStatus();
            }
            catch (Exception ex)
            {
                UpdateStatus("ERROR:" + ex.Message.ToString());
            }
            return sRet;
        }

        public string ExtractTextFromBitmap(string sImgPath)
        {
            // "D:\\test_image.png"
            string sRet = "";

            try
            {
                using (var api = OcrApi.Create())
                {
                    //api.Init(Languages.ChineseTraditional | Languages.English);
                    api.Init(Languages.English);
                    using (var bmp = Bitmap.FromFile(sImgPath) as Bitmap)
                    {
                        sRet = api.GetTextFromImage(bmp);
                    }
                    UpdateStatus();
                }
            }
            catch (Exception ex)
            {
                UpdateStatus("ERROR:" + ex.Message.ToString());
            }
            return sRet;
        }

        public string ExtractFromPictureBox(Bitmap bmp)
        {
            string sRet = "";

            try
            {
                using (var api = OcrApi.Create())
                {
                    //api.Init(Languages.ChineseTraditional | Languages.English);
                    api.Init(Languages.English);
                    sRet = api.GetTextFromImage(bmp);
                }
                UpdateStatus();
            }
            catch (Exception ex)
            {
                UpdateStatus("ERROR:" + ex.Message.ToString());
            }
            return sRet;
        }

        private void UpdateStatus(string sMsg = "")
        {
            statusBarLab.Text = sMsg;
        }

        private void btnBrowerImg_Click(object sender, EventArgs e)
        {
            string sRet = "";
            OpenFileDialog opf = new OpenFileDialog();
            if (opf.ShowDialog() == DialogResult.OK)
            {
                string sFileImg = opf.FileName;
                pbImage.Image = Image.FromFile(sFileImg);
                sRet = ExtractTextFromImage(sFileImg);
            }
            tbCaptureResult.Text = sRet;
        }

        private void pbImage_DragDrop(object sender, DragEventArgs e)
        {
            // 获取拖放的文件路径
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            // 只处理第一个文件
            if (files.Length > 0)
            {
                try
                {
                    // 从文件加载图像
                    Image image = Image.FromFile(files[0]);
                    // 在 PictureBox 中显示图像
                    pbImage.Image = image;
                    Bitmap bitmap = (Bitmap)pbImage.Image;
                    tbCaptureResult.Text = ExtractFromPictureBox(bitmap);
                }
                catch (Exception ex)
                {
                    UpdateStatus("ERROR：載入圖片錯誤 " + ex.Message.ToString());
                }
            }
        }

        private void pbImage_DragEnter(object sender, DragEventArgs e)
        {
            // 检查拖入的数据是否是文件
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                isLeftCtrlKeyPressed = true;
            }
            else if (isLeftCtrlKeyPressed && e.KeyCode == Keys.V)
            {
                // 如果剪贴板中有图像，直接粘贴到窗体中
                if (Clipboard.ContainsImage())
                {
                    // 获取剪贴板图像
                    Image clipboardImage = Clipboard.GetImage();
                    // 在窗体中粘贴图像
                    pbImage.Image = clipboardImage;
                    Bitmap bitmap = (Bitmap)pbImage.Image;
                    tbCaptureResult.Text = ExtractFromPictureBox(bitmap);
                }
                else
                {
                    // 否则显示窗体
                    this.WindowState = FormWindowState.Normal;
                    this.ShowInTaskbar = true;
                    this.Activate();
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                isLeftCtrlKeyPressed = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            hf = null;
        }

        private void ckbCopyRectBitmap_CheckedChanged(object sender, EventArgs e)
        {
            if (hf != null)
            {
                hf = null;
            }
            if (ckbCopyRectBitmap.Checked)
            {
                timerRectCapture.Start();
                hf = new HookFunction();
                HookFunction._isDrawRect = false;
                HookFunction._isRectBitmapCopyToClip = true;
            }
            else
            {
                timerRectCapture.Stop();
                hf = null;
                HookFunction._isDrawRect = false;
                HookFunction._isRectBitmapCopyToClip = false;
            }

        }

        private void timerRectCapture_Tick(object sender, EventArgs e)
        {
            if (hf != null)
            {
                if (HookFunction._isRectBitmapCopyToClip)
                {
                    Bitmap bitmap = HookFunction.bmpRect;
                    pbImage.Image = (Image)bitmap; // 顯示在picturebox上
                    tbCaptureResult.Text = ExtractFromPictureBox(bitmap);
                }
            } // end hf
        }
    }
}
