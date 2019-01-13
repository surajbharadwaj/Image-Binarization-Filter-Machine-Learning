using LearningFoundation;



namespace BinarizationLibrary
{
    public class Convolution : IPipelineModule<double[,], byte[]>
    {

        private int stride;
        private int height;
        byte[] pixelBuffer;
        byte[] resultBuffer;
        double[,] convolutionArray = new double[3, 3];
        

        public Convolution(int stride, int height, byte[] pixelBuffer, byte[] resultBuffer,double[,]convolutionArray)
        {
            this.stride = stride;
            this.height = height;
            this.pixelBuffer = pixelBuffer;
            this.resultBuffer = resultBuffer;
            this.convolutionArray = convolutionArray;
        }

        public byte[] Run(double[,] data, IContext ctx)
        {
            
            return ConvolutionFilter.convolutionFilter(data, convolutionArray, pixelBuffer, resultBuffer, stride);
        }
    }
}
