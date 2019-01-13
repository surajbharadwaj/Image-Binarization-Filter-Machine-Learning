using LearningFoundation;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;


namespace BinarizationLibrary
{
    /// <summary>
    /// This class contains helper for bitmap to 2d array,vice-versa convolution,gaussian helper's
    /// </summary>
    public class ConversionHelper
    {
        /// <summary>
        /// converts the Bitmap into a 2D array
        /// </summary>
        /// <param name="myBitmap">takes Bitmap as an input</param>
        /// <returns>2D double array</returns>
        public double[,] ConvertToArray(Bitmap myBitmap)
        {
            double[,] data = new double[myBitmap.Width, myBitmap.Height];

            for (int x = 0; x < myBitmap.Width; x++)
            {
                for (int y = 0; y < myBitmap.Height; y++)
                {
                    Color pixelColor = myBitmap.GetPixel(x, y);
                    data[x, y] = (int)(pixelColor.G * .7 + pixelColor.R * .2 + pixelColor.B * .1);
                }
            }
            return data;
        }

        /// <summary>
        /// Converts the image into Multidimensional array
        /// </summary>
        /// <param name="image">Bitmap image</param>
        /// <returns></returns>
        public int[,,] ConvertBitmapToArray(Bitmap image)
        {
            int[,,] greyImage = new int[image.Width, image.Height, 6];
            PixelObject pixelObject = new PixelObject();
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color color = image.GetPixel(i, j);
                    int alpha = color.A;
                    int red = color.R;
                    int green = color.G;
                    int blue = color.B;
                    int newColor = (int)(green * .7 + red * .2 + blue * .7);
                    pixelObject.red = red;
                    pixelObject.green = green;
                    pixelObject.blue = blue;
                    pixelObject.alpha = alpha;
                    pixelObject.newColor = newColor;
                    int newPixel = BinarizationHelper.ColorToRGB(pixelObject);
                    greyImage[i, j, 0] = alpha;
                    greyImage[i, j, 1] = red;
                    greyImage[i, j, 2] = green;
                    greyImage[i, j, 3] = blue;
                    greyImage[i, j, 4] = Color.FromArgb(newPixel).ToArgb();
                    greyImage[i, j, 5] = newPixel;
                }
            }
            return greyImage;
        }

        /// <summary>
        /// Converts a 2D double array into a Bitmap
        /// 
        /// </summary>
        /// <param name="data">2D array</param>
        /// <returns>Bitmap</returns>
        public Bitmap ConvertToBitmap(double[,] data)
        {
            Bitmap bitmap = new Bitmap(data.GetLength(0), data.GetLength(1));

            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    if (data[i, j].Equals(0))
                    {
                        bitmap.SetPixel(i, j, Color.Black);
                    }
                    else
                    {
                        bitmap.SetPixel(i, j, Color.White);
                    }
                }
            }
            return bitmap;
        }

        /// <summary>
        /// Converts 2D int array to Bitmap
        /// </summary>
        /// <param name="data">integer array</param>
        /// <returns>Bitmap</returns>
        public Bitmap ConvertToBitmap(int[,] data)
        {
            Bitmap bitmap = new Bitmap(data.GetLength(0), data.GetLength(1));

            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    if (data[i, j].Equals(0))
                    {
                        bitmap.SetPixel(i, j, Color.Black);
                    }
                    else
                    {
                        bitmap.SetPixel(i, j, Color.White);
                    }
                }
            }
            return bitmap;
        }

        /// <summary>
        /// Converts the multidimensional array with dynamic threshold into Bitmap
        /// </summary>
        /// <param name="data">multidimensional array</param>
        /// <param name="threshold">int value based on which conversion is done</param>
        /// <returns>Bitmap</returns>
        public Bitmap ConvertToBitmap(int[,,] data, int threshold)
        {
            Bitmap bitmap = new Bitmap(data.GetLength(0), data.GetLength(1));

            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    bitmap.SetPixel(i, j, Color.FromArgb(data[i, j, 4]));
                }
            }
            return bitmap;
        }

        /// <summary>
        /// Gaussian Filter helper for fixed thresholding binarization which locks the bitmapdata and applies the median filter .
        /// </summary>
        /// <param name="sourceBitmap"></param>
        /// <param name="sourceArray"></param>
        /// <returns>Bitmap</returns>
        public Bitmap GaussianFilter(Bitmap sourceBitmap, double[,] sourceArray)
        {

            var api = new LearningApi();

            BitmapData sourceData =
               sourceBitmap.LockBits(new Rectangle(0, 0,
               sourceBitmap.Width, sourceBitmap.Height),
               ImageLockMode.ReadOnly,
               PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride *
                                          sourceData.Height];


            byte[] resultBuffer = new byte[sourceData.Stride *
                                           sourceData.Height];


            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0,
                                       pixelBuffer.Length);


            sourceBitmap.UnlockBits(sourceData);

            api.UseActionModule<double[,], double[,]>((input, ctx) =>
            {
                return sourceArray;
            });

            api.AddModule(new Gaussian(sourceData.Stride, sourceData.Height, pixelBuffer, resultBuffer));
            resultBuffer = api.Run() as byte[];



            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width,
                                             sourceBitmap.Height);


            BitmapData resultData =
                       resultBitmap.LockBits(new Rectangle(0, 0,
                       resultBitmap.Width, resultBitmap.Height),
                       ImageLockMode.WriteOnly,
                       PixelFormat.Format32bppArgb);


            Marshal.Copy(resultBuffer, 0, resultData.Scan0,
                                       resultBuffer.Length);


            return resultBitmap;
        }

        /// <summary>
        /// Gaussian Filter helper for dynamic thresholding binarization which locks the bitmapdata and applies the median filter .
        /// </summary>
        /// <param name="sourceBitmap"></param>
        /// <param name="sourceArray"></param>
        /// <returns>Bitmap</returns>
        public Bitmap GaussianFilter(Bitmap sourceBitmap, int[,,] sourceArray)
        {

            var api = new LearningApi();
            BitmapData sourceData =
               sourceBitmap.LockBits(new Rectangle(0, 0,
               sourceBitmap.Width, sourceBitmap.Height),
               ImageLockMode.ReadOnly,
               PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride *
                                          sourceData.Height];


            byte[] resultBuffer = new byte[sourceData.Stride *
                                           sourceData.Height];


            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0,
                                       pixelBuffer.Length);


            sourceBitmap.UnlockBits(sourceData);

            api.UseActionModule<int[,,], int[,,]>((input, ctx) =>
            {
                return sourceArray;
            });

            api.AddModule(new DynamicGaussian(sourceData.Stride, sourceData.Height, pixelBuffer, resultBuffer));
            resultBuffer = api.Run() as byte[];



            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width,
                                             sourceBitmap.Height);


            BitmapData resultData =
                       resultBitmap.LockBits(new Rectangle(0, 0,
                       resultBitmap.Width, resultBitmap.Height),
                       ImageLockMode.WriteOnly,
                       PixelFormat.Format32bppArgb);

            Marshal.Copy(resultBuffer, 0, resultData.Scan0,
                                       resultBuffer.Length);

            return resultBitmap;
        }

        /// <summary>
        /// Convolution Filter helper for fixed thresholding binarization which locks the bitmapdata and applies the convolution filter.
        /// <param name="sourceBitmap"></param>
        /// <param name="sourceArray"></param>
        /// <param name="convolutionArray"></param>
        /// <returns>Bitmap</returns>
        public Bitmap ConvolutionHelper(Bitmap sourceBitmap, double[,] sourceArray, double[,] convolutionArray)
        {
            var api = new LearningApi();
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];


            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            api.UseActionModule<double[,], double[,]>((input, ctx) =>
            {
                return sourceArray;
            });

            api.AddModule(new Convolution(sourceData.Stride, sourceData.Height, pixelBuffer, resultBuffer, convolutionArray));
            resultBuffer = api.Run() as byte[];



            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width,
                                             sourceBitmap.Height);

            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                     resultBitmap.Width, resultBitmap.Height),
                                                      ImageLockMode.WriteOnly,
                                                 PixelFormat.Format32bppArgb);

            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        }

        /// <summary>
        /// Convolution Filter helper for Dynamic thresholding binarization which locks the bitmapdata and applies the convolution filter .
        /// </summary>
        /// <param name="sourceBitmap"></param>
        /// <param name="sourceArray"></param>
        /// <param name="convolutionArray"></param>
        /// <returns>Bitmap</returns>
        public Bitmap ConvolutionHelper(Bitmap sourceBitmap, int[,,] sourceArray, double[,] convolutionArray)
        {
            var api = new LearningApi();

            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];


            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            api.UseActionModule<int[,,], int[,,]>((input, ctx) =>
            {
                return sourceArray;
            });

            api.AddModule(new DynamicConvolution(sourceData.Stride, sourceData.Height, pixelBuffer, resultBuffer, convolutionArray));
            resultBuffer = api.Run() as byte[];



            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width,
                                             sourceBitmap.Height);

            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                     resultBitmap.Width, resultBitmap.Height),
                                                      ImageLockMode.WriteOnly,
                                                 PixelFormat.Format32bppArgb);


            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);
            return resultBitmap;
        }
    }
}
