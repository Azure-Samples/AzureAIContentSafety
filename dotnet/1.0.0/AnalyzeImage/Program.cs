namespace Azure.AI.ContentSafety.Dotnet.Sample
{
    class ContentSafetySampleAnalyzeImage
    {
        // Sample: Analyze image
        public static void AnalyzeImage()
        {
            // Create Azure AI ContentSafety Client

            string endpoint = Environment.GetEnvironmentVariable("CONTENT_SAFETY_ENDPOINT");
            string key = Environment.GetEnvironmentVariable("CONTENT_SAFETY_KEY");

            ContentSafetyClient client = new ContentSafetyClient(new Uri(endpoint), new AzureKeyCredential(key));

            string imagePath = @"sample_data\image.jpg";
            ContentSafetyImageData image = new ContentSafetyImageData(BinaryData.FromBytes(File.ReadAllBytes(imagePath)));

            var request = new AnalyzeImageOptions(image);

            Response<AnalyzeImageResult> response;
            try
            {
                response = client.AnalyzeImage(request);
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine("Analyze image failed.\nStatus code: {0}, Error code: {1}, Error message: {2}", ex.Status, ex.ErrorCode, ex.Message);
                throw;
            }

            Console.WriteLine("Hate severity: {0}", response.Value.CategoriesAnalysis.FirstOrDefault(a => a.Category == ImageCategory.Hate)?.Severity ?? 0);
            Console.WriteLine("SelfHarm severity: {0}", response.Value.CategoriesAnalysis.FirstOrDefault(a => a.Category == ImageCategory.SelfHarm)?.Severity ?? 0);
            Console.WriteLine("Sexual severity: {0}", response.Value.CategoriesAnalysis.FirstOrDefault(a => a.Category == ImageCategory.Sexual)?.Severity ?? 0);
            Console.WriteLine("Violence severity: {0}", response.Value.CategoriesAnalysis.FirstOrDefault(a => a.Category == ImageCategory.Violence)?.Severity ?? 0);
        }

        static void Main()
        {
            AnalyzeImage();
        }
    }
}
