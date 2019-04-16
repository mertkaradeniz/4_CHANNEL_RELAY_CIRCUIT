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
        }

        private void usbHidPort1_OnDeviceArrived(object sender, EventArgs e)
        {
            BaglantiDurumu = true;

        }

        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {

        }
    }
}
