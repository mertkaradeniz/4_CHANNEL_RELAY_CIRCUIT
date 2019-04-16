#include <16F628.H>
#fuses  INTRC_IO,NOMCLR,NOWDT,NOPROTECT,NOBROWNOUT,NOLVP,NOPUT,NOCPD
#use delay(clock=4000000)
#use rs232(baud=2400,parity=N, rcv=pin_B1, bits=8,ERRORS,BRGH1OK)

#define VeriTransferLed pin_a3
#define DevrePowerLedLed pin_a2  
int i=0;   
  
void UsbTransferVeriOnayi()  
{
   for(i=0;i<5;i++)
   {
      output_high(VeriTransferLed);
      delay_ms(50);
      output_low(VeriTransferLed);   
      delay_ms(50);  
   }
}

void main(){    
   
   clear_interrupt(INT_RDA);
   clear_interrupt(INT_EXT);
   Disable_interrupts(GLOBAL);
   port_b_pullups(TRUE);
   setup_timer_0(RTCC_INTERNAL);
   setup_timer_1(T1_DISABLED);
   setup_timer_2(T2_DISABLED,0,1);
   setup_comparator(NC_NC_NC_NC);
   setup_vref(FALSE);
   enable_interrupts(INT_RDA);
   enable_interrupts(GLOBAL);
   
   
   Disable_interrupts(GLOBAL);
   setup_CCP1(CCP_OFF);            // CCP1 birimi devre d���

   set_tris_b(01000000); 
   set_tris_a(0x00);   
  
   output_low(pin_b3);
   output_low(pin_b7);
   output_low(pin_b6);
   output_low(pin_b5);
   output_low(VeriTransferLed);
   output_High(DevrePowerLedLed);

   while(1)  
   {         
      if(getc()==110){ output_high(pin_b4); }  //1 R�le A�
      if(getc()==120){ output_low(pin_b4);  }   //1 R�le Kapat
      if(getc()==130){ output_high(pin_b7); }  //2 R�le A�
      if(getc()==140){ output_low(pin_b7); }   //2 R�le Kapat
      if(getc()==190){ output_high(pin_b6); }  //3 R�le A�
      if(getc()==200){ output_low(pin_b6); }   //3 R�le Kapat
      if(getc()==170){ output_high(pin_b5); }  //4 R�le A�
      if(getc()==180){ output_low(pin_b5); }   //4 R�le Kapat
   
   }
}
 
 
