using System;

namespace Azure.AI.ContentSafety.Dotnet.Sample
{
    class Program
    {
        static void Main()
        {
            ContentSafetySampleAnalyzeText.AnalyzeText();

            ContentSafetySampleAnalyzeImage.AnalyzeImage();

            ContentSafetySampleManageBlocklist.ManageBlocklist();

            Console.ReadLine();
        }
    }
}
