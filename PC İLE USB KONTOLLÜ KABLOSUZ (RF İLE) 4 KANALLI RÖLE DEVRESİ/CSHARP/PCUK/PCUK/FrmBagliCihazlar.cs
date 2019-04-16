using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Query;
using System.Xml.XLinq;
using System.Data.DLinq;
using System.Runtime.InteropServices;
using USBView;
using System.Collections;

namespace PCUK
{
    public partial class FrmBagliCihazlar : Form
    {
        public FrmBagliCihazlar()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClsBagliAygitlar.BagliAygıtlariGetir();

            listView1.Items.Clear();
          
            foreach(ClsDegiskenler bul in ClsBagliAygitlar.paket)
            {
                ListViewItem ekle = new ListViewItem(bul.VID);
                ekle.SubItems.Add(bul.PID);
                listView1.Items.Add(ekle);
                
            }
        
        }

        
       
    }
}
