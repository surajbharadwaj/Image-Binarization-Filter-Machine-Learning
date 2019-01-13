using LearningFoundation;

namespace BinarizationLibrary
{
    public class BinarizeDynamic : IPipelineModule<int[,,], int[,,]>
    {
        BinarizationFilterImpl binarizationFilter = new BinarizationFilterImpl();

        
        public BinarizeDynamic()
        {
            
        }

        public int[,,] Run(int[,,] data, IContext ctx)
        {

            return binarizationFilter.BinarizeDynamic(data);
        }

    }
}
