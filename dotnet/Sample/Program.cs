using System;

namespace Azure.AI.ContentSafety.Dotnet.Sample
{
    class Program
    {
        static void Main()
        {
            ContentSafetySampleAnalyzeText sampleAnalyzeText = new ContentSafetySampleAnalyzeText();
            sampleAnalyzeText.AnalyzeText();

            ContentSafetySampleAnalyzeImage sampleAnalyzeImage = new ContentSafetySampleAnalyzeImage();
            sampleAnalyzeImage.AnalyzeImage();

            ContentSafetySampleManageBlocklist sampleManageBlocklist = new ContentSafetySampleManageBlocklist();
            sampleManageBlocklist.ManageBlocklist();

            Console.ReadLine();
        }
    }
}
