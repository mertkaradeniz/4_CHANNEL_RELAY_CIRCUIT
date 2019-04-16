using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
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
            CheckForIllegalCrossThreadCalls = false;
        
        }
        string PID="1818", VID="1818";

        bool secildi = false;
        int maus_yukselik, maus_sol, fark_sol, fark_yukseklik;
       
        bool BaglantiDurumu = false;
        byte[] v = new byte[1000];
        bool ilkbaglanti = false;
        bool Durum = false;

        [DllImport("JMT.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public extern static int l();

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,  // Sol üst köşesi x-koordinatı 
            int nTopRect,  // y koordinatı sol üst köşesi 
            int nRightRect,  // x-koordinatı sağ alt köşesi 
            int nBottomRect,  // sağ alt köşe y-koordinatı 
            int nWidthEllipse,  // elips yüksekliği 
            int nHeightEllipse // Elips genişliği 
         );
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

        public void RoleKontrolFind()
        {
            ClsBagliAygitlar.BagliAygıtlariGetir();

            Durum = false;

            foreach (ClsDegiskenler bul in ClsBagliAygitlar.paket)
            {
                if ((bul.PID == PID) & (bul.VID == VID))
                {
                    Durum = true;
                }
            }
        }

        private void SaatTarih_Tick(object sender, EventArgs e)
        {
            label6.Text = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
        }

        private void PCUK_Load(object sender, EventArgs e)
        {
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            
            label6.Text = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();

            usbHidPort1.ProductId=Int32.Parse(PID, System.Globalization.NumberStyles.HexNumber);
            usbHidPort1.VendorId = Int32.Parse(VID, System.Globalization.NumberStyles.HexNumber);
            usbHidPort1.CheckDevicePresent();

            backgroundWorker1.RunWorkerAsync(); 
        }

        private void usbHidPort1_OnSpecifiedDeviceArrived(object sender, EventArgs e)
        {
            bglntxt.ForeColor = Color.Green;
            bglntxt.Text = "USB Bağlantısı Kuruldu";
            statusStrip1.Items[0].Image = ımageList1.Images[2];
            BaglantiDurumu = true;
            BaglantiDurumuAktif();

            if (ilkbaglanti)
            {
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
        }

        private void usbHidPort1_OnSpecifiedDeviceRemoved(object sender, EventArgs e)
        {
            RoleKontrolFind();
            if (Durum) return;
                  
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
            RoleKontrolFind();
            if (Durum) return;

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

            if (panel6.Visible)
            {
                backgroundWorker1.RunWorkerAsync();
            }
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
                for (int i = 0; i <10; i++)
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

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            progressBar1.Value = 15;
            System.Threading.Thread.Sleep(500);
            progressBar1.Value = 30;
            label7.Text = "Cihaza Bağlantı Kuruluyor...";

            if (!BaglantiDurumu) { label7.Text = "Lütfen Cihazınızın Bağlı Olduğundan Emin Olunuz !"; progressBar1.Value = 100; return; }

            RoleKontrolFind();
            if (!Durum) { label7.Text = "Lütfen Cihazınızın Bağlı Olduğundan Emin Olunuz !"; progressBar1.Value = 100; return; } 

            System.Threading.Thread.Sleep(2000);
            progressBar1.Value = 45;
            System.Threading.Thread.Sleep(500);
            progressBar1.Value = 60;
            label7.Text = "Portlar Kapatılıyor...            ";
            System.Threading.Thread.Sleep(1000);
            progressBar1.Value = 75;

            if (BaglantiDurumu)
            {
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

            progressBar1.Value = 85;
            
            if (!BaglantiDurumu) { label7.Text = "Lütfen Cihazınızın Bağlı Olduğundan Emin Olunuz !"; progressBar1.Value = 100; return; }

            label7.Text = "Program Açılıyor...              ";
            System.Threading.Thread.Sleep(2000);
            progressBar1.Value = 100;
          
            if (!BaglantiDurumu) { label7.Text = "Lütfen Cihazınızın Bağlı Olduğundan Emin Olunuz !"; progressBar1.Value = 100; return; }

            System.Threading.Thread.Sleep(500);

            //this.FormBorderStyle = FormBorderStyle.Fixed3D;
            panel6.Visible = false;
            ilkbaglanti = true;
         
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            new FrmIletisim().ShowDialog();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void panel6_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left))
            {
                maus_yukselik = e.Y;
                maus_sol = e.X;
                secildi = true;
            }
        }

        private void panel6_MouseUp(object sender, MouseEventArgs e)
        {
            secildi = false;
        }

        private void panel6_MouseMove(object sender, MouseEventArgs e)
        {
            if (secildi)
            {
                fark_yukseklik = maus_yukselik - e.Y;
                fark_sol = maus_sol - e.X;

                this.Top = this.Top - fark_yukseklik;
                this.Left = this.Left - fark_sol;
            }
        }

        private void usbHidPort1_OnDataSend(object sender, EventArgs e)
        {

        }
    }
}
