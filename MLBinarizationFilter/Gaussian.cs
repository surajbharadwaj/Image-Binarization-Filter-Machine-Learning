
using LearningFoundation;

namespace BinarizationLibrary
{

    public class Gaussian : IPipelineModule<double[,], byte[]>
    {
        private int stride;
        private int height;
        byte[] pixelBuffer;
        byte[] resultBuffer;
        BinarizationFilterImpl binarizationFilter = new BinarizationFilterImpl();
       
        public Gaussian(int stride,int height,byte[] pixelBuffer,byte[] resultBuffer)
        {
            this.stride = stride;
            this.height = height;
            this.pixelBuffer = pixelBuffer;
            this.resultBuffer = resultBuffer;
        }

        public byte[] Run(double[,] data, IContext ctx)
        {
            
            return GaussianFilter.MedianFilter(data, 3, pixelBuffer, resultBuffer, stride);
        }


    }
}
