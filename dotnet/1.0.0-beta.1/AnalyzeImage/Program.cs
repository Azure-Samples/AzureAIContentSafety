using System;
using Azure.AI.ContentSafety;

namespace Azure.AI.ContentSafety.Dotnet.Sample
{
    class ContentSafetySampleAnalyzeImage
    {
        public static void AnalyzeImage()
        {
            // Create Azure AI ContentSafety Client

            string endpoint = Environment.GetEnvironmentVariable("CONTENT_SAFETY_ENDPOINT");
            string key = Environment.GetEnvironmentVariable("CONTENT_SAFETY_KEY") ;

            ContentSafetyClient client = new ContentSafetyClient(new Uri(endpoint), new AzureKeyCredential(key));

            // Example: analyze image

            string imagePath = @"sample_data\image.jpg";
            ImageData image = new ImageData() { Content = BinaryData.FromBytes(File.ReadAllBytes(imagePath)) };

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

            Console.WriteLine("\nAnalyze image succeeded:");
            Console.WriteLine("Hate severity: {0}", response.Value.HateResult?.Severity ?? 0);
            Console.WriteLine("SelfHarm severity: {0}", response.Value.SelfHarmResult?.Severity ?? 0);
            Console.WriteLine("Sexual severity: {0}", response.Value.SexualResult?.Severity ?? 0);
            Console.WriteLine("Violence severity: {0}", response.Value.ViolenceResult?.Severity ?? 0);
        }

        static void Main()
        {
            AnalyzeImage();
        }
    }
}
