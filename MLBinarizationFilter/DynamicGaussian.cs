using LearningFoundation;


namespace BinarizationLibrary
{
    public class DynamicGaussian : IPipelineModule<int[,,], byte[]>
    {
        private int stride;
        private int height;
        byte[] pixelBuffer;
        byte[] resultBuffer;
        BinarizationFilterImpl binarizationFilter = new BinarizationFilterImpl();

        public DynamicGaussian(int stride, int height, byte[] pixelBuffer, byte[] resultBuffer)
        {
            this.stride = stride;
            this.height = height;
            this.pixelBuffer = pixelBuffer;
            this.resultBuffer = resultBuffer;
        }

        public byte[] Run(int[,,] data, IContext ctx)
        {
           
            return GaussianFilter.MedianFilter2(data, 3, pixelBuffer, resultBuffer, stride);
        }
    }
}
