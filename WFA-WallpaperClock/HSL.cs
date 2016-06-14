using System;
using System.Drawing;
namespace WFA_WallpaperClock
{
    class HSL
    {
        public double H;
        public double S;
        public double L;


        /// <summary>
        /// Calculates the dominant color of an image/Bitmap.
        /// </summary>
        /// <param name="bmp">Bitmap to calculate.</param>
        /// <returns></returns>
        public static Color getDominantColor(Bitmap bmp)
        {
            //Used for tally
            long r = 0;
            long g = 0;
            long b = 0;

            long total = 0;

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color clr = bmp.GetPixel(x, y);
                    r += clr.R;
                    g += clr.G;
                    b += clr.B;
                    total++;
                }
            }

            //Calculate average
            r /= total;
            g /= total;
            b /= total;

            return Color.FromArgb(Convert.ToInt32(r), Convert.ToInt32(g), Convert.ToInt32(b));
        }
        /// <summary>
        /// Converts RGB to HSL.
        /// </summary>
        /// <param name="RGB">Color to calculate.</param>
        /// <returns></returns>
        public static HSL RGBToHSL(Color RGB)
        {
            HSL localHSL = new HSL();

            double varR = Convert.ToDouble(RGB.R) / 255.0d;
            double varG = Convert.ToDouble(RGB.G) / 255.0d;
            double varB = Convert.ToDouble(RGB.B) / 255.0d;

            double minVal = Math.Min(Math.Min(varR, varG), varB);//Min. value of RGB
            double maxVal = Math.Max(Math.Max(varR, varG), varB);//Max. value of RGB
            double maxDel = maxVal - minVal;//Delta RGB value

            localHSL.L = (maxVal + minVal) / 2.0d;

            if (maxDel == 0.0d)                     //This is a gray, no chroma...
            {
                localHSL.H = 0.0d;                  //HSL results from 0 to 1
                localHSL.S = 0.0d;
            }
            else                                    //Chromatic data...
            {
                if (localHSL.L < 0.5d)
                    localHSL.S = maxDel / (maxVal + minVal);
                else
                    localHSL.S = maxDel / (2.0d - maxVal - minVal);


                double delR = (((maxVal - varR) / 6.0d) + (maxDel / 2.0d)) / maxDel;
                double delG = (((maxVal - varG) / 6.0d) + (maxDel / 2.0d)) / maxDel;
                double delB = (((maxVal - varB) / 6.0d) + (maxDel / 2.0d)) / maxDel;

                if (varR == maxVal)
                    localHSL.H = delB - delG;
                else if (varG == maxVal)
                    localHSL.H = (1.0d / 3.0d) + delR - delB;
                else if (varB == maxVal)
                    localHSL.H = (2.0d / 3.0d) + delG - delR;


                if (localHSL.H < 0.0d)  //Getting Hue between 0 and 1.
                    localHSL.H += 1.0d;
                if (localHSL.H > 1.0d)
                    localHSL.H -= 1.0d;

            }


            return localHSL;
        }
        /// <summary>
        /// Takes normal HSL and rotates the Hue by 180 degrees and returns the new HSL value.
        /// </summary>
        /// <param name="normalHSL">HSL to calculate.</param>
        /// <returns></returns>
        public static HSL calculateTheOppositeHue(HSL normalHSL)
        {
            HSL newHue = normalHSL;

            newHue.H = (newHue.H + 0.333d) % 1.0d;

            return newHue;
        }
        /// <summary>
        /// Converts HSL to RGB.
        /// </summary>
        /// <param name="hsl">HSL that will be converted to RGB.</param>
        /// <returns></returns>
        public static Color HSLToRGB(HSL hsl)
        {
            double R;
            double B;
            double G;

            double var_1;
            double var_2;

            if (hsl.S == 0.0d)                       //HSL from 0 to 1
            {
                R = hsl.L * 255.0d;                      //RGB results from 0 to 255
                G = hsl.L * 255.0d;
                B = hsl.L * 255.0d;
            }
            else
            {
                if (hsl.L < 0.5d)
                    var_2 = hsl.L * (1.0d + hsl.S);
                else
                    var_2 = (hsl.L + hsl.S) - (hsl.S * hsl.L);

                var_1 = 2.0d * hsl.L - var_2;

                R = 255.0d * HueToRGB(var_1, var_2, hsl.H + (1.0d / 3.0d));
                G = 255.0d * HueToRGB(var_1, var_2, hsl.H);
                B = 255.0d * HueToRGB(var_1, var_2, hsl.H - (1.0d / 3.0d));
            }
            Color cl = Color.FromArgb(Convert.ToInt32(R), Convert.ToInt32(G), Convert.ToInt32(B));

            return cl;
        }
        /// <summary>
        /// Converts Hue to RGB.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="vH"></param>
        /// <returns></returns>
        public static double HueToRGB(double v1, double v2, double vH)
        {
            if (vH < 0.0d)
                vH += 1.0d;
            if (vH > 1.0d)
                vH -= 1.0d;   //Getting hue back in between 0 and 1.
            if ((6.0d * vH) < 1.0d)
                return (v1 + (v2 - v1) * 6.0d * vH);
            if ((2.0d * vH) < 1.0d)
                return (v2);
            if ((3.0d * vH) < 2.0d)
                return (v1 + (v2 - v1) * ((2.0d / 3.0d) - vH) * 6.0d);
            return (v1);
        }
        /// <summary>
        /// Returns the complementary color of the original color.
        /// </summary>
        /// <param name="_originalColor">Original color that is going to be calculated.</param>
        /// <returns></returns>
        public static Color GetComplementaryColor(Color _originalColor)
        {
            if (_originalColor.ToArgb() == Color.White.ToArgb())
                return Color.Black;

            if (_originalColor.ToArgb() == Color.Black.ToArgb())
                return Color.White;


            HSL newHSL = RGBToHSL(_originalColor);
            newHSL = calculateTheOppositeHue(newHSL);
            Color newColor = HSLToRGB(newHSL);
            return newColor;
        }

    }
}
