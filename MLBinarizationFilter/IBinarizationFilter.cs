

namespace BinarizationLibrary
{
    public interface IBinarizationFilter
    {
        
        double[,] ConvertIntoGreyScaleUsingFixedThreshold(double[,] image, int threshold);

        int[,,] BinarizeDynamic(int[,,] original);

        int CalculateThreshold(int[,,] original);
    }
}
