using Azure.Core;
using Azure.AI.ContentSafety;

namespace Azure.AI.ContentSafety.Dotnet.Sample
{
    class ContentSafetySampleManageBlocklist
    {
        public static void ManageBlocklist()
        {
            // Create Azure AI ContentSafety Client

            string endpoint = Environment.GetEnvironmentVariable("CONTENT_SAFETY_ENDPOINT");
            string key = Environment.GetEnvironmentVariable("CONTENT_SAFETY_KEY");

            ContentSafetyClient client = new ContentSafetyClient(new Uri(endpoint), new AzureKeyCredential(key));

            // Example: create new blocklist

            var blocklistName = "TestBlocklist";
            var blocklistDescription = "Test blocklist management";

            var data = new
            {
                description = blocklistDescription,
            };

            var createResponse = client.CreateOrUpdateTextBlocklist(blocklistName, RequestContent.Create(data));
            if (createResponse.Status == 201)
            {
                Console.WriteLine("\nBlocklist {0} created.", blocklistName);
            }
            else if (createResponse.Status == 200)
            {
                Console.WriteLine("\nBlocklist {0} updated.", blocklistName);
            }

            // Example: add blockItems

            string blockItemText1 = "k*ll";
            string blockItemText2 = "h*te";

            var blockItems = new TextBlockItemInfo[] { new TextBlockItemInfo(blockItemText1), new TextBlockItemInfo(blockItemText2) };
            var addedBlockItems = client.AddBlockItems(blocklistName, new AddBlockItemsOptions(blockItems));

            if (addedBlockItems != null && addedBlockItems.Value != null)
            {
                Console.WriteLine("\nBlockItems added:");
                foreach (var addedBlockItem in addedBlockItems.Value.Value)
                {
                    Console.WriteLine("BlockItemId: {0}, Text: {1}, Description: {2}", addedBlockItem.BlockItemId, addedBlockItem.Text, addedBlockItem.Description);
                }
            }

            // Example: analyze text with blocklist

            // After you edit your blocklist, it usually takes effect in 5 minutes, please wait some time before analyzing with blocklist after editing.
            var request = new AnalyzeTextOptions("I h*te you and I want to k*ll you");
            request.BlocklistNames.Add(blocklistName);
            request.BreakByBlocklists = true;

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

            if (response.Value.BlocklistsMatchResults != null)
            {
                Console.WriteLine("\nBlocklist match result:");
                foreach (var matchResult in response.Value.BlocklistsMatchResults)
                {
                    Console.WriteLine("Blockitem was hit in text: Offset: {0}, Length: {1}", matchResult.Offset, matchResult.Length);
                    Console.WriteLine("BlocklistName: {0}, BlockItemId: {1}, BlockItemText: {2}, ", matchResult.BlocklistName, matchResult.BlockItemId, matchResult.BlockItemText);
                }
            }

            // Example: list blocklists

            var blocklists = client.GetTextBlocklists();
            Console.WriteLine("\nList blocklists:");
            foreach (var blocklist in blocklists)
            {
                Console.WriteLine("BlocklistName: {0}, Description: {1}", blocklist.BlocklistName, blocklist.Description);
            }

            // Example: get blocklist

            var getBlocklist = client.GetTextBlocklist(blocklistName);
            if (getBlocklist != null && getBlocklist.Value != null)
            {
                Console.WriteLine("\nGet blocklist:");
                Console.WriteLine("BlocklistName: {0}, Description: {1}", getBlocklist.Value.BlocklistName, getBlocklist.Value.Description);
            }

            // Example: list blockItems

            var allBlockitems = client.GetTextBlocklistItems(blocklistName);
            Console.WriteLine("\nList BlockItems:");
            foreach (var blocklistItem in allBlockitems)
            {
                Console.WriteLine("BlockItemId: {0}, Text: {1}, Description: {2}", blocklistItem.BlockItemId, blocklistItem.Text, blocklistItem.Description);
            }

            // Example: get blockItem

            var getBlockItemId = addedBlockItems.Value.Value[0].BlockItemId;
            var getBlockItem = client.GetTextBlocklistItem(blocklistName, getBlockItemId);
            Console.WriteLine("\nGet BlockItem:");
            Console.WriteLine("BlockItemId: {0}, Text: {1}, Description: {2}", getBlockItem.Value.BlockItemId, getBlockItem.Value.Text, getBlockItem.Value.Description);

            // Example: remove blockItems

            var removeBlockItemId = addedBlockItems.Value.Value[0].BlockItemId;
            var removeBlockItemIds = new List<string> { removeBlockItemId };
            var removeResult = client.RemoveBlockItems(blocklistName, new RemoveBlockItemsOptions(removeBlockItemIds));

            if (removeResult != null && removeResult.Status == 204)
            {
                Console.WriteLine("\nBlockItem removed: {0}.", removeBlockItemId);
            }

            // Example: delete blocklist

            var deleteResult = client.DeleteTextBlocklist(blocklistName);
            if (deleteResult != null && deleteResult.Status == 204)
            {
                Console.WriteLine("\nDeleted blocklist.");
            }
        }

        static void Main()
        {             
            ManageBlocklist();
         }
    }
}

