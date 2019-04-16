using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PCUK
{
    public partial class AnaGecisForm : Form
    {
        public AnaGecisForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        PCUK Form = new PCUK();
        bool BaglantiDurumu = false;
        private void AnaGecisForm_Load(object sender, EventArgs e)
        {
            usbHidPort1.ProductId = Int32.Parse("1818", System.Globalization.NumberStyles.HexNumber);
            this.usbHidPort1.VendorId = Int32.Parse("1818", System.Globalization.NumberStyles.HexNumber);
            //usbHidPort1.CheckDevicePresent();
            
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            progressBar1.Value = 15;
            System.Threading.Thread.Sleep(500);
            progressBar1.Value = 30;
            label1.Text = "Cihaza Bağlantı Kuruluyor...";

            if (!BaglantiDurumu) { label1.Text = "Lütfen Cihazınızın Bağlı Olduğundan Emin Olunuz !"; progressBar1.Value = 100; return; }
            
            System.Threading.Thread.Sleep(2000);
            progressBar1.Value = 45;
            System.Threading.Thread.Sleep(500);
            progressBar1.Value = 60;
            label1.Text = "Portlar Kapatılıyor...            ";
            System.Threading.Thread.Sleep(2000);
            progressBar1.Value = 75;
            System.Threading.Thread.Sleep(500);
            progressBar1.Value = 85;
            label1.Text = "Program Açılıyor...              ";
            System.Threading.Thread.Sleep(2000);
            progressBar1.Value = 100;
            System.Threading.Thread.Sleep(500);
            Hide();
           
            Form.ShowDialog();
            

        }

        private void usbHidPort1_OnSpecifiedDeviceArrived(object sender, EventArgs e)
        {
            BaglantiDurumu = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
        
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            usbHidPort1.RegisterHandle(Handle);
        }

        protected override void WndProc(ref Message m)
        {
            usbHidPort1.ParseMessages(ref m);
            base.WndProc(ref m);
        }
       
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void usbHidPort1_OnDeviceArrived(object sender, EventArgs e)
        {
            BaglantiDurumu = true;

        }
    }
}
