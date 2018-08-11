using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constant
{
   public class Identity
    {
        public const int FARMER = 0;//农民
        public const int LANDLORD = 1;//地主

        public static string GetString(int identity)
        {
            if(identity==0)
            {
                return "农民";
            }else
            {
                return "地主";
            }
        }
    }
}
