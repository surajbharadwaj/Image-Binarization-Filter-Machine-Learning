using LearningFoundation;

namespace BinarizationLibrary
{
    /// <summary>
    /// Class ExtensionClass contains the MLBinarizationFilter extension methods to invoke the BinarizeDynamic object and BinarizationFixed object from the unit test project
    /// </summary>
    public static class ExtensionClass
    {
        /// <summary>
        /// This method is invoked from the unit test project to Apply a fixed threshold on the image
        /// </summary>
        /// <param name="api">Learning api</param>
        /// <param name="threshold"></param>
        /// <returns>LearningApi</returns>
        public static LearningApi UseBinarizarion1(this LearningApi api, int threshold)
        {
            var alg = new BinarizationFixed(threshold);
            api.AddModule(alg, "BinarizationFixed");
            return api;
        }

        /// <summary>
        ///This method is invoked from the unit test project to Apply a calculated threshold on the image
        /// </summary>
        /// <param name="api"></param>
        /// <returns></returns>
        public static LearningApi UseBinarizarion2(this LearningApi api)
        {
            var alg = new BinarizeDynamic();
            api.AddModule(alg, "BinarizeDynamic");
            return api;
        }

    }
}


