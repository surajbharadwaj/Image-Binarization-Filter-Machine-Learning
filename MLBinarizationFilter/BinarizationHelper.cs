namespace BinarizationLibrary
{
    public class BinarizationHelper
    {
        /// <summary>
        ///  shifts the bits of expression1 left by the number of bits specified in expression2. 
        ///  The data type of expression1 determines the data type returned by this operator.
        ///  Converts the Color to RGB
        /// </summary>
        /// <param name="alpha">int</param>
        /// <param name="red">int</param>
        /// <param name="green">int</param>
        /// <param name="blue">int</param>
        /// <returns>int</returns>
        public int ColorToRGB(int alpha, int red, int green, int blue)
        {

            int newPixel = 0;
            newPixel += alpha;
            newPixel = newPixel << 8;
            newPixel += red;
            newPixel = newPixel << 8;
            newPixel += green;
            newPixel = newPixel << 8;
            newPixel += blue;

            return newPixel;

        }

        /// <summary>
        /// shifts the bits of expression1 left by the number of bits specified in expression2. 
        ///  The data type of expression1 determines the data type returned by this operator.
        ///  Converts the Color to RGB
        /// </summary>
        /// <param name="pixelObject">getter/setter class (Object class)</param>
        /// <returns>int</returns>
        public static int ColorToRGB(PixelObject pixelObject)
        {
            int newPixel = 0;
            newPixel += pixelObject.alpha;
            newPixel = newPixel << 8;
            newPixel += pixelObject.red;
            newPixel = newPixel << 8;
            newPixel += pixelObject.green;
            newPixel = newPixel << 8;
            newPixel += pixelObject.blue;

            return newPixel;
        }


        /// <summary>
        /// Calculates the image histogram for threshold calculation
        /// </summary>
        /// <param name="input">image source array</param>
        /// <returns>int[]</returns>
        public int[] ImageHistogram(int[,,] input)
        {
            int[] histogram = new int[256];

            for (int i = 0; i < histogram.Length; i++) histogram[i] = 0;

            for (int i = 0; i < input.GetLength(0); i++)
            {
                for (int j = 0; j < input.GetLength(1); j++)
                {
                    int red = input[i, j, 1];
                    histogram[red]++;
                }
            }

            return histogram;
        }

    }
}
