using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using UsbLibrary;

namespace PCUK
{
    public partial class PCUK : Form
    {
        public PCUK()
        {
            InitializeComponent();
        }
      
        bool BaglantiDurumu = false;
        byte[] v = new byte[1000];

        void BaglantiDurumuAktif()
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
        }

        void BaglantiDurumuPasif()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
        }

        private void SaatTarih_Tick(object sender, EventArgs e)
        {
            label6.Text = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
        }

        private void PCUK_Load(object sender, EventArgs e)
        {
            label6.Text = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();

            usbHidPort1.ProductId=Int32.Parse("1818", System.Globalization.NumberStyles.HexNumber);
            usbHidPort1.VendorId = Int32.Parse("1818", System.Globalization.NumberStyles.HexNumber);
            usbHidPort1.CheckDevicePresent();

            if (BaglantiDurumu)
            {
                bglntxt.ForeColor = Color.Green;
                bglntxt.Text = "USB Bağlantısı Kuruldu.";
                statusStrip1.Items[0].Image = ımageList1.Images[2];
                BaglantiDurumu = true;
                BaglantiDurumuAktif();

               
            }
            else
            {
                bglntxt.ForeColor = Color.Red;
                bglntxt.Text = "USB Bağlantısı Bekleniyor...";
                BaglantiDurumu = true;
                statusStrip1.Items[0].Image = ımageList1.Images[1];
                BaglantiDurumuPasif();
            }        
        }

        private void usbHidPort1_OnSpecifiedDeviceArrived(object sender, EventArgs e)
        {
            bglntxt.ForeColor = Color.Green;
            bglntxt.Text = "USB Bağlantısı Kuruldu";
            statusStrip1.Items[0].Image = ımageList1.Images[2];
            BaglantiDurumu = true;
            BaglantiDurumuAktif();

            for (int i = 0; i < 10; i++)
            {
                v[1] = 120;
                usbHidPort1.SpecifiedDevice.SendData(v);
            }
            for (int i = 0; i < 10; i++)
            {
                v[1] = 140;
                usbHidPort1.SpecifiedDevice.SendData(v);
            } for (int i = 0; i < 10; i++)
            {
                v[1] = 200;
                usbHidPort1.SpecifiedDevice.SendData(v);
            } for (int i = 0; i < 10; i++)
            {
                v[1] = 180;
                usbHidPort1.SpecifiedDevice.SendData(v);
            }
        
        }

        private void usbHidPort1_OnSpecifiedDeviceRemoved(object sender, EventArgs e)
        {
            bglntxt.ForeColor = Color.Red;
            bglntxt.Text = "USB Bağlantısı kesildi";
            statusStrip1.Items[0].Image = ımageList1.Images[0];
            BaglantiDurumu = false;
            BaglantiDurumuPasif();

            button1.Text = "Aç";
            button2.Text = "Aç";
            button3.Text = "Aç";
            button4.Text = "Aç";

            panel2.BackColor = Color.Red;
            panel3.BackColor = Color.Red;
            panel4.BackColor = Color.Red;
            panel5.BackColor = Color.Red; 
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

        private void usbHidPort1_OnDeviceRemoved(object sender, EventArgs e)
        {
            bglntxt.ForeColor = Color.Red;
            bglntxt.Text = "USB Bağlantısı kesildi";
            statusStrip1.Items[0].Image = ımageList1.Images[0];
            BaglantiDurumu = false;
            BaglantiDurumuPasif();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Aç")
            {
                v[1] =110;
                for (int i = 0; i <10; i++)
                    usbHidPort1.SpecifiedDevice.SendData(v);
                panel2.BackColor = Color.Lime;
                button1.Text = "Kapat";
            }
            else
            {
                v[1] = 120; 
                for (int i = 0; i < 10; i++)
                    usbHidPort1.SpecifiedDevice.SendData(v);
                panel2.BackColor = Color.Red;
                button1.Text = "Aç";
            }
        }

        private void usbHidPort1_OnDeviceArrived(object sender, EventArgs e)
        {
            bglntxt.ForeColor = Color.Green;
            bglntxt.Text = "USB Bağlantısı Kuruldu";
            statusStrip1.Items[0].Image = ımageList1.Images[2];
            BaglantiDurumu = true;
            BaglantiDurumuAktif();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "Aç")
            {
                v[1] = 130;
                for (int i = 0; i <10; i++)                
                usbHidPort1.SpecifiedDevice.SendData(v);
                panel3.BackColor = Color.Lime;
                button2.Text = "Kapat";
            }
            else
            {
                v[1] = 140;
                for (int i = 0; i < 10; i++)
                usbHidPort1.SpecifiedDevice.SendData(v);
                panel3.BackColor = Color.Red;
                button2.Text = "Aç";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (button4.Text == "Aç")
            {
                v[1] = 190;
                for (int i = 0; i < 10; i++)
                   usbHidPort1.SpecifiedDevice.SendData(v);
                panel5.BackColor = Color.Lime;
                button4.Text = "Kapat";
            }
            else
            {
                v[1] = 200;
                for (int i = 0; i < 10; i++)
                    usbHidPort1.SpecifiedDevice.SendData(v);
                panel5.BackColor = Color.Red;
                button4.Text = "Aç";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "Aç")
            {
                v[1] = 170;
                for (int i = 0; i < 10; i++)
                    usbHidPort1.SpecifiedDevice.SendData(v);
                panel4.BackColor = Color.Lime;
                button3.Text = "Kapat";
            }
            else
            {
                v[1] =180;
                for (int i = 0; i < 10; i++)
                    usbHidPort1.SpecifiedDevice.SendData(v);
                panel4.BackColor = Color.Red;
                button3.Text = "Aç";
            }
        }
    }
}
