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
    class ClsBagliAygitlar
    {
        public static ClsDegiskenler degisken = new ClsDegiskenler();
        public static List<ClsDegiskenler> paket = new List<ClsDegiskenler>();



       public static void BagliAygıtlariGetir()
       {

           string strVID;          // Üretici ID
           string strVendorName;   // Firma Adı
           string strPID;          // Aygıt ID
           string strProductName;  // Aygıt Adı
           string strDevicePnPID;  // Aygıt PnPID
           // Aygıt yollarını tutacak bir ArrayList nesnesi oluştur.
           ArrayList myAL = new ArrayList();

           // ListView kontrolümüzün içeriğini temizliyoruz.
          paket.Clear();

           try
           {
               // Win32API içinden GetDevicesPath metodunu çağır.
               // USB aygıt yollarını ArrayList içine doldurur. 
               myAL = Win32API.GetDevicesPath();
               // ArrayList içindeki eleman sayısı sıfırsan büyük mü? 
               if (myAL.Count > 0)
               {
                   // Aygıt yolları arasında gezinmek için foreach döngüsünü kullanıyoruz.
                   foreach (String str in myAL)
                   {
                       // GetDevicePnPID metodunu çağırarak USB donanım ID'si alınıyor.
                       strDevicePnPID = GetDevicePnPID(str);
                       // USB donanım ID'si içinden "vid_" kelimesini yakala ve
                       // üretici ID'sini strVID değişkenine kopyala.
                       strVID = strDevicePnPID.Substring(strDevicePnPID.IndexOf("vid_") + 4, 4);
                       // USB donanım ID'si içinden "pid_" kelimesini yakala ve
                       // aygıt ID'sini strPID değişkenine kopyala.
                       strPID = strDevicePnPID.Substring(strDevicePnPID.IndexOf("pid_") + 4, 4);
                       // Firma adını Xml içerikten sorgula.
                       strVendorName = XmlQueryByVID(strVID);
                       // Aygıt adını Xml içerikten sorgula.
                       //strProductName = XmlQueryByVIDnPID(strVID, strPID);

                       degisken.PID = strPID;
                       degisken.VID = strVID;
                       paket.Add(degisken);

                   }//end foreach
               }
               else
               {
                   MessageBox.Show("Bağlı USB aygıt bulunamadı.");
               }//end if
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.Message);
           }

       }

        private static string GetDevicePnPID(string sPath)
        {
            // Aygıt yolu VID & PID desenleri
            Regex rgx1 = new Regex(@"vid_[0-9a-fA-F]{4}&pid_[0-9a-fA-F]{4}",
                RegexOptions.IgnoreCase);

            Regex rgx2 = new Regex(@"VID_[0-9a-fA-F]{4}&PID_[0-9a-fA-F]{4}");

            // Desenlere göre karşılaştırma yap.
            if (rgx1.Match(sPath.ToLower()).Success)
                return (rgx1.Match(sPath).Value).ToLower();
            else if (rgx2.Match(sPath).Success)
                return (rgx2.Match(sPath).Value).Replace("VID", "vid").Replace("PID", "pid");
            else
                return String.Empty;
        }

        private static string XmlQueryByVID(string sVID)
        {
            // XElement sınıfından Load metoduyla usbdev.xml isimli dosya
            // belleğe yüklenir.
            XElement usbids = XElement.Load("usbdev.xml");

            try
            {
                // vendorName nesnesi üzerinden LINQ sorgusunu çalıştır.
                var vendorName =
                    (from
                        vend in usbids.Elements("vendor")
                     where
                         (string)vend.Attribute("vid") == sVID
                     select
                         (string)vend.Attribute("name")).First();

                return vendorName;
            }
            catch { }
            return "";
        }
        
    }
}
