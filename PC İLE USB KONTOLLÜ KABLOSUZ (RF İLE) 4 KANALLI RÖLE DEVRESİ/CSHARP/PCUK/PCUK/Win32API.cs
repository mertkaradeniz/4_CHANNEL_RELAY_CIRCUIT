using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace USBView
{
    // Ayg�t y�netimiyle ilgili baz� API fonksiyonlar�n� ve yap�lar�n�
    // sarmalayan s�n�f.
    public class Win32API
    {
        #region Constants

        //  USB ayg�tlar i�in sabit. Bkz. usbiodef.h
        internal const string GUID_DEVINTERFACE_USB_DEVICE =
            "A5DCBF10-6530-11D2-901F-00C04FB951ED";

        // winbase.h i�inden al�nan sabit.
        internal static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        // setupapi.h i�inden al�nan sabitler.
        internal const Int32 DIGCF_PRESENT = 0x00000002;
        internal const Int32 DIGCF_DEVICEINTERFACE = 0x00000010;

        // winerror.h i�inden al�nan sabitler.
        //internal const Int32 ERROR_INSUFFICIENT_BUFFER = 122;
        //internal const Int32 ERROR_NO_MORE_ITEMS = 259;
        //internal const Int32 ERROR_INVALID_USER_BUFFER = 1784;
       
        #endregion

        #region Structures

        // Ayg�t aray�z� tan�mlayan yap�. 
        [StructLayout(LayoutKind.Sequential)]
        internal class SP_DEVICE_INTERFACE_DATA
        {
            // Yap�n�n uzunlu�u, byte.
            public Int32 cbSize = Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DATA));
            // Ayg�t aray�z� s�n�f� GUID numaras�.
            public Guid InterfaceClassGuid = Guid.Empty;
            // Flags de�i�keniyle ayg�t hakk�nda bilgi verilir.
            public Int32 Flags = 0;
            public Int32 Reserved = 0;  // Kullan�lm�yor
        };

        // Bir ayg�t aray�z�n�n yol bilgisini g�steren yap�.
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        internal class SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            // Yap�n�n uzunlu�u, byte.
            public Int32 cbSize = Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DETAIL_DATA));
            // Ayg�t�n yol bilgisini (device path) i�eren
            // s�f�rla sonland�r�lm�� string'e i�aret eden pointer.
            public Int16 DevicePath;
        }
       
        #endregion

        #region P/Invokes Methods
        // SetupDiGetClassDevs fonksiyonu, belirtilen s�n�fa ait
        // ayg�tlar�n aray�z setini al�r.
        // Fonksiyon �a�r�s� ba�ar�l�ysa bir handle, ba�ar�s�z durumda ise
        // INVALID_HANDLE_VALUE de�erini �evirir.
        [DllImport("setupapi.dll", SetLastError = true, CharSet=CharSet.Auto)]
        internal static extern IntPtr SetupDiGetClassDevs(
            // Ayg�t s�n�f�n�n GUID numaras�.
            ref Guid ClassGuid,
            // DIGCF_DEVICEINTERFACE de�eriyle ilgili �zel bir kullan�m� var.
            // Kullan�lmayabilir, s�f�r veya null de�ere sahip olabilir.
            Int32 Enumerator,   
            // Ayg�t aray�z detaylar�yla ilgili bir handle. Kullanmayaca��z.
            IntPtr hwndParent,
            // Ayg�t kontrol de�erleri.
            Int32 Flags
            );

        // SetupDiDestroyDeviceInfoList fonksiyonu, ayg�t aray�z setini
        // sonland�r�r ve ayr�lan haf�zay� serbest b�rak�r.
        // Fonksiyon �a�r�s� ba�ar�l�ysa geriye s�f�r olmayan bir say�,
        // ba�ar�s�z durumda ise s�f�r de�erini �evirir.
        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern Boolean SetupDiDestroyDeviceInfoList(
            // Ayg�t aray�z�ne referans.
            IntPtr DeviceInfoSet);

        // SetupDiEnumDeviceInterfaces fonksiyonu, bir ayg�t i�in
        // ayg�t aray�z setinden, ayg�tla ilgili aray�z yap�s�n� al�r.
        // Fonksiyon �a�r�s� ba�ar�l�ysa geriye s�f�r olmayan bir say�,
        // ba�ar�s�z durumda ise s�f�r de�erini �evirir.
        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern Boolean SetupDiEnumDeviceInterfaces(
            // SetupDiGetClassDevs taraf�ndan d�nd�r�len, ayg�t aray�z grubuna ait handle 
            IntPtr DeviceInfoSet,
            // SP_DEVINFO_DATA yap�s�na ait g�sterici. Kullanmayaca��z.
            IntPtr DeviceInfoData,
            // Ayg�t s�n�f�n�n GUID numaras�.
            ref Guid InterfaceClassGuid,
            // Aray�z listesi dizini.
            Int32 MemberIndex,
            // SP_DEVICE_INTERFACE_DATA yap�s�.
            SP_DEVICE_INTERFACE_DATA DeviceInterfaceData
            );

        // SetupDiGetDeviceInterfaceDetail fonksiyonu, belirtilen ayg�t
        // aray�z�nden detay veya yol bilgisini al�r.
        // Fonksiyon �a�r�s� ba�ar�l�ysa geriye s�f�r olmayan bir say�,
        // ba�ar�s�z durumda ise s�f�r de�erini �evirir.
        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern Boolean SetupDiGetDeviceInterfaceDetail(
            // Ayg�t aray�z grubuna ait handle 
            IntPtr DeviceInfoSet,
            // SP_DEVICE_INTERFACE_DATA yap�s�.
            SP_DEVICE_INTERFACE_DATA DeviceInterfaceData,
            // SP_DEVICE_INTERFACE_DETAIL_DATA yap�s�na ait g�sterici.
            // Al�nacak veri (path gibi) bu yap�ya doldurulacak.
            IntPtr DeviceInterfaceDetailData,
            // SP_DEVICE_INTERFACE_DETAIL_DATA yap�s� + device path i�ine
            // alan tampon uzunlu�u.
            Int32 DeviceInterfaceDetailDataSize,
            // Yukar�daki tampon i�in gerekli olan uzunluk.
            ref Int32 RequiredSize,
            // SP_DEVINFO_DATA yap�s�na ait g�sterici. Kullanmayaca��z.
            IntPtr DeviceInfoData
            );

        #endregion

        #region Public methods
        // GetDevicesPath metodu USB ayg�tlar� bulup, ayg�t yollar�n� (device path)
        // bir ArrayList i�ine dolduracak.
        public static ArrayList GetDevicesPath()
        {
            IntPtr hDevInfo = IntPtr.Zero;  // Ayg�tlar i�in handle
            Int32 iSize = 0;                // Ayg�t yolu (device path) uzunlu�u
            Int32 iDevCount = 0;            // Ayg�t say�s�
            String sDevicePath = "";        // Ayg�t yolu (device path)
            
            // ArrayList i�ine bulunan ayg�t yollar�n� (device path) dolduraca��z.
            ArrayList arrList = new ArrayList();

            // USB ayg�tlar� aray�z GUID numaras�
            Guid myGuid = new Guid(GUID_DEVINTERFACE_USB_DEVICE);

            // Bilgisayar ba�l� durumda olan USB cihazlar�n ayg�t bilgisini
            // almak i�in SetupDiGetClassDevs fonksiyonunu �a��r�yoruz.
            // DIGCF_DEVICEINTERFACE: Verilen GUID'e g�re aray�z s�n�f�n� temsil eder.
            // DIGCF_PRESENT: Haz�r durumdaki USB ayg�tlar.
            hDevInfo = SetupDiGetClassDevs(ref myGuid, 0, IntPtr.Zero,
                DIGCF_DEVICEINTERFACE | DIGCF_PRESENT);
            
            // Fonksiyondan d�nen de�er ge�erli mi?
            if (hDevInfo == INVALID_HANDLE_VALUE)
            {
                // Ayg�t listesinin al�nmas� ba�ar�s�zl��a u�rad�.
                // Ayr�nt�l� hata bilgisi almak i�in GetLastWin32Error �a�r�l�r.
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            
            try
            {
                // SP_DEVICE_INTERFACE_DATA yap� nesnesi tan�ml�yoruz.
                SP_DEVICE_INTERFACE_DATA devInterfaceData = new
                        SP_DEVICE_INTERFACE_DATA();

                // SP_DEVICE_INTERFACE_DETAIL_DATA yap� nesnesi tan�ml�yoruz.
                SP_DEVICE_INTERFACE_DETAIL_DATA devInterfaceDetailData = new
                        SP_DEVICE_INTERFACE_DETAIL_DATA();

                // USB ayg�tlar� while tekrarl� yap�s�yla ar�yoruz.
                // SetupDiEnumDeviceInterfaces fonksiyonuyla ayg�t aray�z�n�
                // bulma �art� kontrol edilir.
                //
                // hDevInfo: SetupDiGetClassDevs fonksiyonundan gelen handle.
                // SP_DEVINFO_DATA yap�s� opsiyonel oldu�u i�in s�f�r de�erine e�itlenmi�.
                while (SetupDiEnumDeviceInterfaces (hDevInfo, IntPtr.Zero,
                        ref myGuid, iDevCount, devInterfaceData))
                {
                    // Ayg�t aray�z� hakk�ndaki detaylar� alabilmek i�in iki a�amal�
                    // bir yol izlenebilir.
                    // #1. Ad�m:
                    // SetupDiGetDeviceInterfaceDetail fonksiyonu,   
                    // SP_DEVICE_INTERFACE_DETAIL_DATA yap�s�na s�f�r veya null de�eri
                    // verilerek �a�r�l�r. Gerekli tampon uzunlu�u i�in s�f�r girilir.
                    // RequiredSize parametresinden gerekli olan uzunluk elde edilir.
                    // #2. Ad�m:
                    // Bellekte tampon i�in yeterli miktarda yer ay�r.
                    // Daha sonra SetupDiGetDeviceInterfaceDetail fonksiyonunu
                    // tekrar �a��r ve aray�z detay�n� al.
                    
                    if (!SetupDiGetDeviceInterfaceDetail (hDevInfo, devInterfaceData,
                         IntPtr.Zero, 0, ref iSize, IntPtr.Zero))
                    {
                        // Tampon i�in bellekte d�nen uzunluk de�eri kadar yer ay�r.
                        IntPtr buffer = Marshal.AllocHGlobal(iSize);

                        // StructureToPtr ile yap�daki de�erleri tampona kopyala.
                        // Burada y�netimli kod k�sm�ndan, y�netilmeyen haf�za
                        // blo�una veriler marshall ediliyor.
                        Marshal.StructureToPtr(devInterfaceDetailData, buffer, false);

                        try
                        {
                            // SetupDiGetDeviceInterfaceDetail fonksiyonunu tekrar �a��r.
                            // Tamponun ger�ek uzunlu�unu iSize parametresiyle ge�ir. 
                            if (SetupDiGetDeviceInterfaceDetail (hDevInfo,
                                devInterfaceData, buffer, iSize, ref iSize,
                                IntPtr.Zero))
                            {
                                // Tampon i�indeki ayg�t�n yol bilgisini (device path) alabilmek i�in
                                // tamponun adresini 4 byte �tele ve daha sonra
                                // pDevicePath g�stericisine aktar.
                                //
                                // Burada g�stericinin adres bile�eni 4 artt�r�l�yor.
                                IntPtr pDevicePath = (IntPtr)((Int32)buffer +
                                    Marshal.SizeOf(typeof(Int32)));
                                // Y�netilmeyen bellekte bulunan bir string i�eri�i
                                // y�netilen bir string i�ine kopyalan�yor.
                                sDevicePath = Marshal.PtrToStringAuto(pDevicePath);
                                // Bulunan ayg�t yolu ArrayList de�i�kenine ekleniyor.
                                arrList.Add(sDevicePath);
                            }
                            else
                            {
                                throw new Win32Exception(Marshal.GetLastWin32Error());
                            }//end if
                        }
                        finally
                        {
                            // AllocHGlobal ile bellekte ayr�lan yeri serbest b�rak.
                            Marshal.FreeHGlobal(buffer);
                        }
                        // Bir sonraki ayg�ta bakmak i�in sayac� artt�r.
                        iDevCount++;
                    }
                    else
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }//end if
                }//end while
            }
            finally
            {
                // Bellekteki ayg�t bilgi setini serbest b�rak.
                SetupDiDestroyDeviceInfoList(hDevInfo);
            }
            
            return arrList;
        }
        #endregion
    }
}
