using LearningFoundation;
using System.Drawing;
using Xunit;
using BinarizationLibrary;
using System.IO;

namespace Test
{
    public class Test
    {

        [Fact]
        public void TestMedianFilterForFixedThreshold()
        {
            ConversionHelper helper = new ConversionHelper();
            var api = new LearningApi();
            api.UseActionModule<double[,], double[,]>((input, ctx) =>
            {
                return TestHelperForFixedThreshold();
            });
            api.UseBinarizarion1(127);
            double[,] grey = api.Run() as double[,];
            Bitmap greyImage = helper.ConvertToBitmap(grey);
            string savePath = Directory.GetCurrentDirectory() + @"\Fixed_Threshold.jpg";
            greyImage.Save($"{savePath}");
            Bitmap medFilter = helper.GaussianFilter(greyImage, grey);
            string savePath1 = Directory.GetCurrentDirectory() + @"\Fixed_Threshold_Median_Filter.jpg";
            medFilter.Save($"{savePath1}");
        }

        [Fact]
        public void TestConvolutionFilterForFixedThreshold()
        {
            ConversionHelper helper = new ConversionHelper();
            var api = new LearningApi();
            api.UseActionModule<double[,], double[,]>((input, ctx) =>
            {
                return TestHelperForFixedThreshold();
            });
            api.UseBinarizarion1(127);
            double[,] grey = api.Run() as double[,];
            Bitmap greyImage = helper.ConvertToBitmap(grey);
            
            Bitmap convolutionResult = helper.ConvolutionHelper(greyImage, grey, TestHelperConvolutionArray());
            string savePath2 = Directory.GetCurrentDirectory() + @"\Fixed_Threshold_Convolution_Filter.jpg";
            convolutionResult.Save($"{savePath2}");
        }

        [Fact]
        public void TestMedianFilterForDynamicThreshold()
        {
            ConversionHelper helper = new ConversionHelper();
            BinarizationFilterImpl binarizationFilter = new BinarizationFilterImpl();
            var api = new LearningApi();
            
            api.UseActionModule<int[,,], int[,,]>((input, ctx) =>
            {
                return TestHelperForDynamicThreshold();
            });
            api.UseBinarizarion2();
            int[,,] binarized = api.Run() as int[,,];
            int binarizedLength = binarized.GetLength(2);
            Bitmap binarizedImage = helper.ConvertToBitmap(binarized, binarized[0,0, binarizedLength - 1]);
            string savePath3 = Directory.GetCurrentDirectory() + @"\Dynamic_Threshold.jpg";
            binarizedImage.Save($"{savePath3}");
            Bitmap medianFilter = helper.GaussianFilter(binarizedImage, binarized);
            string savePath4 = Directory.GetCurrentDirectory() + @"\Dynamic_Threshold_Median_Filter.jpg";
            medianFilter.Save($"{savePath4}");

        }

        [Fact]
        public void TestConvolutionFilterforDynamicThreshold()
        {
            ConversionHelper helper = new ConversionHelper();
            BinarizationFilterImpl binarizationFilter = new BinarizationFilterImpl();
            var api = new LearningApi();
            api.UseActionModule<int[,,], int[,,]>((input, ctx) =>
            {
                return TestHelperForDynamicThreshold();
            });
            api.UseBinarizarion2();
            int[,,] binarized = api.Run() as int[,,];
            int binarizedLength = binarized.GetLength(2);
            Bitmap binarizedImage = helper.ConvertToBitmap(binarized, binarized[0, 0, binarizedLength - 1]);
            Bitmap dynamicConvolutionResult = helper.ConvolutionHelper(binarizedImage, binarized, TestHelperConvolutionArray());
            string savePath5 = Directory.GetCurrentDirectory() + @"\Dynamic_Threshold_Convolution_Filter.jpg";
            dynamicConvolutionResult.Save($"{savePath5}");

        }

        public double[,] TestHelperForFixedThreshold()
        {
            ConversionHelper helper = new ConversionHelper();
            Bitmap image = new Bitmap("Lenna.png");

            double[,] convertedImage = helper.ConvertToArray(image);
            return convertedImage;
        }

        public int[,,] TestHelperForDynamicThreshold()
        {
            ConversionHelper helper = new ConversionHelper();
            Bitmap image = new Bitmap("Lenna.png");
            int[,,] pixelObject = helper.ConvertBitmapToArray(image);
            return pixelObject;
        }

        public double[,] TestHelperConvolutionArray()
        {
            double[,] convolutionArray = new double[3, 3];
            convolutionArray[0, 0] = 2;
            convolutionArray[0, 1] = 0;
            convolutionArray[0, 2] = 0;
            convolutionArray[1, 0] = 0;
            convolutionArray[1, 1] = -1;
            convolutionArray[1, 2] = 0;
            convolutionArray[2, 0] = 0;
            convolutionArray[2, 1] = 0;
            convolutionArray[2, 2] = -1;
            return convolutionArray;
        }
    }
}
