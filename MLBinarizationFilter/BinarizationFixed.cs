using LearningFoundation;


namespace BinarizationLibrary
{
    public class BinarizationFixed : IPipelineModule<double[,], double[,]>
    {
        BinarizationFilterImpl binarizationFilter = new BinarizationFilterImpl();

        
        private int m_Threshold;

        public BinarizationFixed(int threshold)
        {
            this.m_Threshold = threshold;
        }

        public double[,] Run(double[,] data, IContext ctx)
        {

            return binarizationFilter.ConvertIntoGreyScaleUsingFixedThreshold(data, m_Threshold);

        }
    }
}
