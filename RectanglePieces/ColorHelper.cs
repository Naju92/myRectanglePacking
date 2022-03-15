using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectanglePieces
{
   public static class ColorHelper
   {
        static ColorHelper()
        {
            Alpha = 190;
            Red = 185;
            Green = 200;
            Blue=255;
        }

        public static void ResetColor()
        {
            if (Alpha == 0 || Alpha + 30 <= 255)
            {
                Alpha += 30;
            }
            else if (Alpha == 255 || Alpha - 30 >= 0)
            {
                Alpha -= 30;
            }
            else
            {
                Alpha = 200;
            }
            //###########Red
            if (Red == 0 || Red + 20 <= 255)
            {
                Red += 20;
            }
            else if (Red == 255 || Red - 20 >= 0)
            {
                Red -= 20;
            }
            else
            {
                Red = 150;
            }

            //###########Green
            if (Green == 0 || Green + 20 <= 255)
            {
                Green += 20;
            }
            else if (Green == 255 || Green - 20 >= 0)
            {
                Green -= 20;
            }
            else
            {
                Green = 200;
            }

            //###########Blue
            if (Blue == 0 || Blue + 60 <= 255)
            {
                Blue += 60;
            }
            else if (Blue == 255 || Blue - 120 >= 0)
            {
                Blue -= 120;
            }
            else
            {
                Blue = 60;
            }


        }

        public static int Alpha { get; set; }

        public static int Red { get; set; }

        public static int Green { get; set; }

        public static int Blue { get; set; }
   }
}
