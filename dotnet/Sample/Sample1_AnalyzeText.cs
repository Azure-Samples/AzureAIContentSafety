using Azure.AI.ContentSafety;

namespace Azure.AI.ContentSafety.Dotnet.Sample
{
    class ContentSafetySampleAnalyzeText
    {
        public void AnalyzeText()
        {
            #region Snippet:Azure_AI_ContentSafety_CreateClient

            string endpoint = "<endpoint>";
            string key = "<apiKey>";

            ContentSafetyClient client = new ContentSafetyClient(new Uri(endpoint), new AzureKeyCredential(key));

            #endregion Snippet:Azure_AI_ContentSafety_CreateClient

            #region Snippet:Azure_AI_ContentSafety_AnalyzeText

            string text = "You are an idiot";

            var request = new AnalyzeTextOptions(text);

            Response<AnalyzeTextResult> response;
            try
            {
                response = client.AnalyzeText(request);
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine("Analyze text failed.\nStatus code: {0}, Error code: {1}, Error message: {2}", ex.Status, ex.ErrorCode, ex.Message);
                throw;
            }

            Console.WriteLine("\nAnalyze text succeeded:");
            Console.WriteLine("Hate severity: {0}", response.Value.HateResult?.Severity ?? 0);
            Console.WriteLine("SelfHarm severity: {0}", response.Value.SelfHarmResult?.Severity ?? 0);
            Console.WriteLine("Sexual severity: {0}", response.Value.SexualResult?.Severity ?? 0);
            Console.WriteLine("Violence severity: {0}", response.Value.ViolenceResult?.Severity ?? 0);

            #endregion Snippet:Azure_AI_ContentSafety_AnalyzeText
        }
    }
}
