using System.Drawing;

namespace BinarizationLibrary
{
    public class BinarizationFilterImpl : IBinarizationFilter
    {
        /// <summary>
        /// This is the main Class which contains the algorithm for calculating threshold dynamically and for performing binarization based on fixed threshold
        /// </summary>


        BinarizationHelper binarizationHelper = new BinarizationHelper();


        /// <summary>
        /// Converts the Image into GrayScale with Fixed threshold
        /// </summary>
        /// <param name="image">2D array of image</param>
        /// <param name="threshold">int value</param>
        /// <returns>2D array</returns>
        public double[,] ConvertIntoGreyScaleUsingFixedThreshold(double[,] image, int threshold)
        {
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    double newColor = image[i, j];
                    if (newColor < threshold)
                    {
                        image[i, j] = 0;
                    }
                    else
                    {
                        image[i, j] = 1;
                    }
                }
            }
            return image;
        }

        /// <summary>
        /// This method is used for converting bitmap to array and is used as a helper for calculating threshold dynamically
        /// </summary>
        /// <returns> pixel object</returns>
        public int[,,] TestHelperForDynamicThreshold()
        {
            ConversionHelper helper = new ConversionHelper();
            Bitmap image = new Bitmap("Lenna.png");
            int[,,] pixelObject = helper.ConvertBitmapToArray(image);
            return pixelObject;
        }


        /// <summary>
        /// Binarizes the image based on the threshold
        /// </summary>
        /// <param name="original">multidimensional array that represent original image</param>
        /// <param name="threshold">int value based on which binarization is performed</param>
        /// <returns> multidimensional int array</returns>
        public int[,,] BinarizeDynamic(int[,,] original)
        {

            int threshold = CalculateThreshold(TestHelperForDynamicThreshold());
            int red;
            int newPixel;

            int[,,] binarized = new int[original.GetLength(0), original.GetLength(1), 6];

            for (int i = 0; i < original.GetLength(0); i++)
            {
                for (int j = 0; j < original.GetLength(1); j++)
                {

                    // Get pixels
                    red = original[i, j, 1];
                    int alpha = original[i, j, 0];
                    if (red > threshold)
                    {
                        newPixel = 255;
                    }
                    else
                    {
                        newPixel = 0;
                    }
                    newPixel = binarizationHelper.ColorToRGB(alpha, newPixel, newPixel, newPixel);
                    binarized[i, j, 0] = original[i, j, 0];
                    binarized[i, j, 1] = original[i, j, 1];
                    binarized[i, j, 2] = original[i, j, 2];
                    binarized[i, j, 3] = original[i, j, 3];
                    binarized[i, j, 4] = newPixel;
                    binarized[i, j, 5] = threshold;


                }
            }

            return binarized;

        }

        /// <summary>
        /// Otsu thresholding Calculates the threshold dynamically based on the image. creates the image histogram.
        /// </summary>
        /// <param name="original">multidimensional representation of image</param>
        /// <returns>int threshold value</returns>
        public int CalculateThreshold(int[,,] original)
        {
            int[] histogram = binarizationHelper.ImageHistogram(original);
            int total = original.GetLength(1) * original.GetLength(0);

            float sum = 0;
            for (int i = 0; i < 256; i++) sum += i * histogram[i];

            float sumB = 0;
            int wB = 0;    // Weight Background
            int wF = 0;    // Weight Foreground

            float varMax = 0;
            int threshold = 0;

            for (int i = 0; i < 256; i++)
            {
                wB += histogram[i];
                if (wB == 0) continue;
                wF = total - wB;

                if (wF == 0) break;

                sumB += (float)(i * histogram[i]);
                // Mean Background
                float mB = sumB / wB;

                // Mean Foreground
                float mF = (sum - sumB) / wF;

                // Calculate Between Class Variance
                float varBetween = (float)wB * (float)wF * (mB - mF) * (mB - mF);

                // Check if new maximum found
                if (varBetween > varMax)
                {
                    varMax = varBetween;
                    threshold = i;
                }

            }
            return threshold;
        }

    }

}
