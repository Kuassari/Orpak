using System;
using PPX_Pos;

namespace TestPPX
{
    public  static class POS_Process //Core process 
    {
      
        public static void Load(IPOS pos)
        {
            Console.WriteLine(pos.DisplayWelcomeScreen());
           
            Console.ReadLine();
        }
    }
}