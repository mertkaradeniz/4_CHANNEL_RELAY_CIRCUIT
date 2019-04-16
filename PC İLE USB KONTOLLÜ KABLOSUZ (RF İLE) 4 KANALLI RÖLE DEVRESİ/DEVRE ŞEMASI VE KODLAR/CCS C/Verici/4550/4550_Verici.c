#include <18f4550.h>
#fuses HSPLL, NOMCLR, PUT, BROWNOUT, BORV43, NOWDT, NOPROTECT, NOLVP
#fuses NODEBUG, USBDIV, PLL5, CPUDIV1, VREGEN, CCP2B3
#use delay(clock=48000000) //20 Mhz Kristal
#use rs232(baud=2400, parity=N, xmit=PIN_C6, rcv=PIN_C7, bits=8,ERRORS,BRGH1OK)
#include <stdio.h>  
#include <string.h>      
  
//Usb config
#define USB_HID_DEVICE     TRUE             
#define USB_EP1_TX_ENABLE  USB_ENABLE_INTERRUPT //Uçnokta1'de Kesme transferi aktif
#define USB_EP1_RX_ENABLE  USB_ENABLE_INTERRUPT    
#define USB_EP1_TX_SIZE    64  //Uçnokta1 için maksimum alýnacak ve gonderilecek
#define USB_EP1_RX_SIZE    64
#include <pic18_usb.h>     
#include <USB_Driver.h>  //USB konfigurasyon bilgileri bu dosyadadýr.
#include <usb.c>

//IO Belirlemesi 
#use fast_io(a)  
#use fast_io(b)
#use fast_io(c)
#use fast_io(d)
#use fast_io(e)

//Deðiþkenler
#define TransferLED  PIN_D4
#define UcNokta1 1 
byte ANAPAKET_H_R_G_B[256];
int i;

void giris_cikis()
{                         
   set_tris_a(0b00000000);  
   set_tris_b(0b00000000); 
   set_tris_c(0b00000000);
   set_tris_d(0b00000010);
   set_tris_e(0b00000000);  
}

void UsbTransferVeriOnayi()
{
   for(i=0;i<5;i++)
   {
      output_high(TransferLED);
      delay_ms(50);
      output_low(TransferLED);   
      delay_ms(50);  
   }
}

// RF cihzýný uyandýrmak için
void  Rf_Paket_H_Uyandir()
{
    putc(0x55);
    putc(0x00);
    putc(0xff);     
    return;
}

void main()
{

output_low(TransferLED);  
setup_adc_ports(NO_ANALOGS|VSS_VDD);
setup_adc(ADC_OFF);
setup_psp(PSP_DISABLED);
setup_spi(SPI_SS_DISABLED);
setup_wdt(WDT_OFF);
setup_timer_0(RTCC_INTERNAL);
setup_timer_3(T3_DISABLED|T3_DIV_BY_1);
setup_comparator(NC_NC_NC_NC);
setup_vref(FALSE); 
setup_timer_2(T2_DIV_BY_4,249,16);
setup_ccp1(CCP_PWM);
setup_ccp2(CCP_PWM);
giris_cikis();
enable_interrupts(GLOBAL);
setup_timer_1(T1_INTERNAL | T1_DIV_BY_8); 

//Usb Kontrol Noktasý  
usb_init();                      
usb_task();                    
usb_wait_for_enumeration();         //Cihaz, hazýr olana kadar bekle
          
while(true)
 { 
  while(usb_enumerated())
   {
     if (usb_kbhit(1))  //Paket Kontrol
      {                      
         usb_get_packet(UcNokta1, ANAPAKET_H_R_G_B, 64); //paketi oku

         if(ANAPAKET_H_R_G_B[0]==110)
         output_high(TransferLED);
         else
         output_low(TransferLED);
   
         //Harfler için Uyandýr
         Rf_Paket_H_Uyandir();
                  
         putc(ANAPAKET_H_R_G_B[0]);  
         putc(ANAPAKET_H_R_G_B[0]);  
      
       }
     }
  }  
 }  






