using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace USBView
{
    // Aygýt yönetimiyle ilgili bazý API fonksiyonlarýný ve yapýlarýný
    // sarmalayan sýnýf.
    public class Win32API
    {
        #region Constants

        //  USB aygýtlar için sabit. Bkz. usbiodef.h
        internal const string GUID_DEVINTERFACE_USB_DEVICE =
            "A5DCBF10-6530-11D2-901F-00C04FB951ED";

        // winbase.h içinden alýnan sabit.
        internal static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        // setupapi.h içinden alýnan sabitler.
        internal const Int32 DIGCF_PRESENT = 0x00000002;
        internal const Int32 DIGCF_DEVICEINTERFACE = 0x00000010;

        // winerror.h içinden alýnan sabitler.
        //internal const Int32 ERROR_INSUFFICIENT_BUFFER = 122;
        //internal const Int32 ERROR_NO_MORE_ITEMS = 259;
        //internal const Int32 ERROR_INVALID_USER_BUFFER = 1784;
       
        #endregion

        #region Structures

        // Aygýt arayüzü tanýmlayan yapý. 
        [StructLayout(LayoutKind.Sequential)]
        internal class SP_DEVICE_INTERFACE_DATA
        {
            // Yapýnýn uzunluðu, byte.
            public Int32 cbSize = Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DATA));
            // Aygýt arayüzü sýnýfý GUID numarasý.
            public Guid InterfaceClassGuid = Guid.Empty;
            // Flags deðiþkeniyle aygýt hakkýnda bilgi verilir.
            public Int32 Flags = 0;
            public Int32 Reserved = 0;  // Kullanýlmýyor
        };

        // Bir aygýt arayüzünün yol bilgisini gösteren yapý.
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        internal class SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            // Yapýnýn uzunluðu, byte.
            public Int32 cbSize = Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DETAIL_DATA));
            // Aygýtýn yol bilgisini (device path) içeren
            // sýfýrla sonlandýrýlmýþ string'e iþaret eden pointer.
            public Int16 DevicePath;
        }
       
        #endregion

        #region P/Invokes Methods
        // SetupDiGetClassDevs fonksiyonu, belirtilen sýnýfa ait
        // aygýtlarýn arayüz setini alýr.
        // Fonksiyon çaðrýsý baþarýlýysa bir handle, baþarýsýz durumda ise
        // INVALID_HANDLE_VALUE deðerini çevirir.
        [DllImport("setupapi.dll", SetLastError = true, CharSet=CharSet.Auto)]
        internal static extern IntPtr SetupDiGetClassDevs(
            // Aygýt sýnýfýnýn GUID numarasý.
            ref Guid ClassGuid,
            // DIGCF_DEVICEINTERFACE deðeriyle ilgili özel bir kullanýmý var.
            // Kullanýlmayabilir, sýfýr veya null deðere sahip olabilir.
            Int32 Enumerator,   
            // Aygýt arayüz detaylarýyla ilgili bir handle. Kullanmayacaðýz.
            IntPtr hwndParent,
            // Aygýt kontrol deðerleri.
            Int32 Flags
            );

        // SetupDiDestroyDeviceInfoList fonksiyonu, aygýt arayüz setini
        // sonlandýrýr ve ayrýlan hafýzayý serbest býrakýr.
        // Fonksiyon çaðrýsý baþarýlýysa geriye sýfýr olmayan bir sayý,
        // baþarýsýz durumda ise sýfýr deðerini çevirir.
        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern Boolean SetupDiDestroyDeviceInfoList(
            // Aygýt arayüzüne referans.
            IntPtr DeviceInfoSet);

        // SetupDiEnumDeviceInterfaces fonksiyonu, bir aygýt için
        // aygýt arayüz setinden, aygýtla ilgili arayüz yapýsýný alýr.
        // Fonksiyon çaðrýsý baþarýlýysa geriye sýfýr olmayan bir sayý,
        // baþarýsýz durumda ise sýfýr deðerini çevirir.
        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern Boolean SetupDiEnumDeviceInterfaces(
            // SetupDiGetClassDevs tarafýndan döndürülen, aygýt arayüz grubuna ait handle 
            IntPtr DeviceInfoSet,
            // SP_DEVINFO_DATA yapýsýna ait gösterici. Kullanmayacaðýz.
            IntPtr DeviceInfoData,
            // Aygýt sýnýfýnýn GUID numarasý.
            ref Guid InterfaceClassGuid,
            // Arayüz listesi dizini.
            Int32 MemberIndex,
            // SP_DEVICE_INTERFACE_DATA yapýsý.
            SP_DEVICE_INTERFACE_DATA DeviceInterfaceData
            );

        // SetupDiGetDeviceInterfaceDetail fonksiyonu, belirtilen aygýt
        // arayüzünden detay veya yol bilgisini alýr.
        // Fonksiyon çaðrýsý baþarýlýysa geriye sýfýr olmayan bir sayý,
        // baþarýsýz durumda ise sýfýr deðerini çevirir.
        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern Boolean SetupDiGetDeviceInterfaceDetail(
            // Aygýt arayüz grubuna ait handle 
            IntPtr DeviceInfoSet,
            // SP_DEVICE_INTERFACE_DATA yapýsý.
            SP_DEVICE_INTERFACE_DATA DeviceInterfaceData,
            // SP_DEVICE_INTERFACE_DETAIL_DATA yapýsýna ait gösterici.
            // Alýnacak veri (path gibi) bu yapýya doldurulacak.
            IntPtr DeviceInterfaceDetailData,
            // SP_DEVICE_INTERFACE_DETAIL_DATA yapýsý + device path içine
            // alan tampon uzunluðu.
            Int32 DeviceInterfaceDetailDataSize,
            // Yukarýdaki tampon için gerekli olan uzunluk.
            ref Int32 RequiredSize,
            // SP_DEVINFO_DATA yapýsýna ait gösterici. Kullanmayacaðýz.
            IntPtr DeviceInfoData
            );

        #endregion

        #region Public methods
        // GetDevicesPath metodu USB aygýtlarý bulup, aygýt yollarýný (device path)
        // bir ArrayList içine dolduracak.
        public static ArrayList GetDevicesPath()
        {
            IntPtr hDevInfo = IntPtr.Zero;  // Aygýtlar için handle
            Int32 iSize = 0;                // Aygýt yolu (device path) uzunluðu
            Int32 iDevCount = 0;            // Aygýt sayýsý
            String sDevicePath = "";        // Aygýt yolu (device path)
            
            // ArrayList içine bulunan aygýt yollarýný (device path) dolduracaðýz.
            ArrayList arrList = new ArrayList();

            // USB aygýtlarý arayüz GUID numarasý
            Guid myGuid = new Guid(GUID_DEVINTERFACE_USB_DEVICE);

            // Bilgisayar baðlý durumda olan USB cihazlarýn aygýt bilgisini
            // almak için SetupDiGetClassDevs fonksiyonunu çaðýrýyoruz.
            // DIGCF_DEVICEINTERFACE: Verilen GUID'e göre arayüz sýnýfýný temsil eder.
            // DIGCF_PRESENT: Hazýr durumdaki USB aygýtlar.
            hDevInfo = SetupDiGetClassDevs(ref myGuid, 0, IntPtr.Zero,
                DIGCF_DEVICEINTERFACE | DIGCF_PRESENT);
            
            // Fonksiyondan dönen deðer geçerli mi?
            if (hDevInfo == INVALID_HANDLE_VALUE)
            {
                // Aygýt listesinin alýnmasý baþarýsýzlýða uðradý.
                // Ayrýntýlý hata bilgisi almak için GetLastWin32Error çaðrýlýr.
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            
            try
            {
                // SP_DEVICE_INTERFACE_DATA yapý nesnesi tanýmlýyoruz.
                SP_DEVICE_INTERFACE_DATA devInterfaceData = new
                        SP_DEVICE_INTERFACE_DATA();

                // SP_DEVICE_INTERFACE_DETAIL_DATA yapý nesnesi tanýmlýyoruz.
                SP_DEVICE_INTERFACE_DETAIL_DATA devInterfaceDetailData = new
                        SP_DEVICE_INTERFACE_DETAIL_DATA();

                // USB aygýtlarý while tekrarlý yapýsýyla arýyoruz.
                // SetupDiEnumDeviceInterfaces fonksiyonuyla aygýt arayüzünü
                // bulma þartý kontrol edilir.
                //
                // hDevInfo: SetupDiGetClassDevs fonksiyonundan gelen handle.
                // SP_DEVINFO_DATA yapýsý opsiyonel olduðu için sýfýr deðerine eþitlenmiþ.
                while (SetupDiEnumDeviceInterfaces (hDevInfo, IntPtr.Zero,
                        ref myGuid, iDevCount, devInterfaceData))
                {
                    // Aygýt arayüzü hakkýndaki detaylarý alabilmek için iki aþamalý
                    // bir yol izlenebilir.
                    // #1. Adým:
                    // SetupDiGetDeviceInterfaceDetail fonksiyonu,   
                    // SP_DEVICE_INTERFACE_DETAIL_DATA yapýsýna sýfýr veya null deðeri
                    // verilerek çaðrýlýr. Gerekli tampon uzunluðu için sýfýr girilir.
                    // RequiredSize parametresinden gerekli olan uzunluk elde edilir.
                    // #2. Adým:
                    // Bellekte tampon için yeterli miktarda yer ayýr.
                    // Daha sonra SetupDiGetDeviceInterfaceDetail fonksiyonunu
                    // tekrar çaðýr ve arayüz detayýný al.
                    
                    if (!SetupDiGetDeviceInterfaceDetail (hDevInfo, devInterfaceData,
                         IntPtr.Zero, 0, ref iSize, IntPtr.Zero))
                    {
                        // Tampon için bellekte dönen uzunluk deðeri kadar yer ayýr.
                        IntPtr buffer = Marshal.AllocHGlobal(iSize);

                        // StructureToPtr ile yapýdaki deðerleri tampona kopyala.
                        // Burada yönetimli kod kýsmýndan, yönetilmeyen hafýza
                        // bloðuna veriler marshall ediliyor.
                        Marshal.StructureToPtr(devInterfaceDetailData, buffer, false);

                        try
                        {
                            // SetupDiGetDeviceInterfaceDetail fonksiyonunu tekrar çaðýr.
                            // Tamponun gerçek uzunluðunu iSize parametresiyle geçir. 
                            if (SetupDiGetDeviceInterfaceDetail (hDevInfo,
                                devInterfaceData, buffer, iSize, ref iSize,
                                IntPtr.Zero))
                            {
                                // Tampon içindeki aygýtýn yol bilgisini (device path) alabilmek için
                                // tamponun adresini 4 byte ötele ve daha sonra
                                // pDevicePath göstericisine aktar.
                                //
                                // Burada göstericinin adres bileþeni 4 arttýrýlýyor.
                                IntPtr pDevicePath = (IntPtr)((Int32)buffer +
                                    Marshal.SizeOf(typeof(Int32)));
                                // Yönetilmeyen bellekte bulunan bir string içeriði
                                // yönetilen bir string içine kopyalanýyor.
                                sDevicePath = Marshal.PtrToStringAuto(pDevicePath);
                                // Bulunan aygýt yolu ArrayList deðiþkenine ekleniyor.
                                arrList.Add(sDevicePath);
                            }
                            else
                            {
                                throw new Win32Exception(Marshal.GetLastWin32Error());
                            }//end if
                        }
                        finally
                        {
                            // AllocHGlobal ile bellekte ayrýlan yeri serbest býrak.
                            Marshal.FreeHGlobal(buffer);
                        }
                        // Bir sonraki aygýta bakmak için sayacý arttýr.
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
                // Bellekteki aygýt bilgi setini serbest býrak.
                SetupDiDestroyDeviceInfoList(hDevInfo);
            }
            
            return arrList;
        }
        #endregion
    }
}
