using Azure.Core;

namespace Azure.AI.ContentSafety.Dotnet.Sample
{
    class ContentSafetySampleManageBlocklist
    {
        public static void ManageBlocklist()
        {
            // Create Azure AI ContentSafety Client

            string endpoint = Environment.GetEnvironmentVariable("CONTENT_SAFETY_ENDPOINT");
            string key = Environment.GetEnvironmentVariable("CONTENT_SAFETY_KEY");

            BlocklistClient blocklistClient = new BlocklistClient(new Uri(endpoint), new AzureKeyCredential(key));
            ContentSafetyClient contentSafetyClient = new ContentSafetyClient(new Uri(endpoint), new AzureKeyCredential(key));

            // Sample: Create or modify a blocklist

            var blocklistName = "TestBlocklist";
            var blocklistDescription = "Test blocklist management";

            var data = new
            {
                description = blocklistDescription,
            };

            var createResponse = blocklistClient.CreateOrUpdateTextBlocklist(blocklistName, RequestContent.Create(data));
            if (createResponse.Status == 201)
            {
                Console.WriteLine("\nBlocklist {0} created.", blocklistName);
            }
            else if (createResponse.Status == 200)
            {
                Console.WriteLine("\nBlocklist {0} updated.", blocklistName);
            }

            // Sample: Add blocklistItems to the blocklist

            string blocklistItemText1 = "k*ll";
            string blocklistItemText2 = "h*te";

            var blocklistItems = new TextBlocklistItem[] { new TextBlocklistItem(blocklistItemText1), new TextBlocklistItem(blocklistItemText2) };
            var addedBlocklistItems = blocklistClient.AddOrUpdateBlocklistItems(blocklistName, new AddOrUpdateTextBlocklistItemsOptions(blocklistItems));

            if (addedBlocklistItems != null && addedBlocklistItems.Value != null)
            {
                Console.WriteLine("\nBlocklistItems added:");
                foreach (var addedBlocklistItem in addedBlocklistItems.Value.BlocklistItems)
                {
                    Console.WriteLine("BlocklistItemId: {0}, Text: {1}, Description: {2}", addedBlocklistItem.BlocklistItemId, addedBlocklistItem.Text, addedBlocklistItem.Description);
                }
            }

            // Sample: Analyze text with a blocklist

            // After you edit your blocklist, it usually takes effect in 5 minutes, please wait some time before analyzing with blocklist after editing.
            var request = new AnalyzeTextOptions("I h*te you and I want to k*ll you");
            request.BlocklistNames.Add(blocklistName);
            request.HaltOnBlocklistHit = true;

            Response<AnalyzeTextResult> response;
            try
            {
                response = contentSafetyClient.AnalyzeText(request);
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine("Analyze text failed.\nStatus code: {0}, Error code: {1}, Error message: {2}", ex.Status, ex.ErrorCode, ex.Message);
                throw;
            }

            if (response.Value.BlocklistsMatch != null)
            {
                Console.WriteLine("\nBlocklist match result:");
                foreach (var matchResult in response.Value.BlocklistsMatch)
                {
                    Console.WriteLine("BlocklistName: {0}, BlocklistItemId: {1}, BlocklistText: {2}, ", matchResult.BlocklistName, matchResult.BlocklistItemId, matchResult.BlocklistItemText);
                }
            }

            // Example: list blocklists

            var blocklists = blocklistClient.GetTextBlocklists();
            Console.WriteLine("\nList blocklists:");
            foreach (var blocklist in blocklists)
            {
                Console.WriteLine("BlocklistName: {0}, Description: {1}", blocklist.Name, blocklist.Description);
            }

            // Example: get blocklist

            var getBlocklist = blocklistClient.GetTextBlocklist(blocklistName);
            if (getBlocklist != null && getBlocklist.Value != null)
            {
                Console.WriteLine("\nGet blocklist:");
                Console.WriteLine("BlocklistName: {0}, Description: {1}", getBlocklist.Value.Name, getBlocklist.Value.Description);
            }

            // Example: list blocklistItems

            var allBlocklistitems = blocklistClient.GetTextBlocklistItems(blocklistName);
            Console.WriteLine("\nList BlocklistItems:");
            foreach (var blocklistItem in allBlocklistitems)
            {
                Console.WriteLine("BlocklistItemId: {0}, Text: {1}, Description: {2}", blocklistItem.BlocklistItemId, blocklistItem.Text, blocklistItem.Description);
            }

            // Example: get blocklistItem

            var getBlocklistItemId = addedBlocklistItems.Value.BlocklistItems[0].BlocklistItemId;
            var getBlocklistItem = blocklistClient.GetTextBlocklistItem(blocklistName, getBlocklistItemId);
            Console.WriteLine("\nGet BlocklistItem:");
            Console.WriteLine("BlocklistItemId: {0}, Text: {1}, Description: {2}", getBlocklistItem.Value.BlocklistItemId, getBlocklistItem.Value.Text, getBlocklistItem.Value.Description);

            // Example: remove blocklistItems

            var removeBlocklistItemId = addedBlocklistItems.Value.BlocklistItems[0].BlocklistItemId;
            var removeBlocklistItemIds = new List<string> { removeBlocklistItemId };
            var removeResult = blocklistClient.RemoveBlocklistItems(blocklistName, new RemoveTextBlocklistItemsOptions(removeBlocklistItemIds));

            if (removeResult != null && removeResult.Status == 204)
            {
                Console.WriteLine("\nBlocklistItem removed: {0}.", removeBlocklistItemId);
            }

            // Example: delete blocklist

            var deleteResult = blocklistClient.DeleteTextBlocklist(blocklistName);
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

