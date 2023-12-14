namespace Azure.AI.ContentSafety.Dotnet.Sample
{
    class ContentSafetySampleOtherBlocklistOperations
    {
        public static void OtherBlocklistOperations()
        {
            // Create Azure AI ContentSafety Client

            string endpoint = Environment.GetEnvironmentVariable("CONTENT_SAFETY_ENDPOINT");
            string key = Environment.GetEnvironmentVariable("CONTENT_SAFETY_KEY");

            BlocklistClient blocklistClient = new BlocklistClient(new Uri(endpoint), new AzureKeyCredential(key));

            var blocklistName = "TestBlocklist";

            // Sample: List all blocklistItems in a blocklist

            var allBlocklistitems = blocklistClient.GetTextBlocklistItems(blocklistName);
            Console.WriteLine("\nList BlocklistItems:");
            foreach (var blocklistItem in allBlocklistitems)
            {
                Console.WriteLine("BlocklistItemId: {0}, Text: {1}, Description: {2}", blocklistItem.BlocklistItemId, blocklistItem.Text, blocklistItem.Description);
            }


            // Sample: List all blocklists

            var blocklists = blocklistClient.GetTextBlocklists();
            Console.WriteLine("\nList blocklists:");
            foreach (var blocklist in blocklists)
            {
                Console.WriteLine("BlocklistName: {0}, Description: {1}", blocklist.Name, blocklist.Description);
            }

            // Sample: Get a blocklist by blocklistName

            var getBlocklist = blocklistClient.GetTextBlocklist(blocklistName);
            if (getBlocklist != null && getBlocklist.Value != null)
            {
                Console.WriteLine("\nGet blocklist:");
                Console.WriteLine("BlocklistName: {0}, Description: {1}", getBlocklist.Value.Name, getBlocklist.Value.Description);
            }


            // Sample: Get a blocklistItem by blocklistName and blocklistItemId

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
            var getBlocklistItemId = addedBlocklistItems.Value.BlocklistItems[0].BlocklistItemId;
            var getBlocklistItem = blocklistClient.GetTextBlocklistItem(blocklistName, getBlocklistItemId);
            Console.WriteLine("\nGet BlocklistItem:");
            Console.WriteLine("BlocklistItemId: {0}, Text: {1}, Description: {2}", getBlocklistItem.Value.BlocklistItemId, getBlocklistItem.Value.Text, getBlocklistItem.Value.Description);

            // Sample: Remove blocklistItems from a blocklist

            var removeBlocklistItemId = addedBlocklistItems.Value.BlocklistItems[0].BlocklistItemId;
            var removeBlocklistItemIds = new List<string> { removeBlocklistItemId };
            var removeResult = blocklistClient.RemoveBlocklistItems(blocklistName, new RemoveTextBlocklistItemsOptions(removeBlocklistItemIds));

            if (removeResult != null && removeResult.Status == 204)
            {
                Console.WriteLine("\nBlocklistItem removed: {0}.", removeBlocklistItemId);
            }

            // Sample: Delete a list and all of its contents

            var deleteResult = blocklistClient.DeleteTextBlocklist(blocklistName);
            if (deleteResult != null && deleteResult.Status == 204)
            {
                Console.WriteLine("\nDeleted blocklist.");
            }
        }

        static void Main()
        {
            OtherBlocklistOperations();
        }
    }
}

