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

            var allBlockitems = blocklistClient.GetTextBlocklistItems(blocklistName);
            Console.WriteLine("\nList BlockItems:");
            foreach (var blocklistItem in allBlockitems)
            {
                Console.WriteLine("BlockItemId: {0}, Text: {1}, Description: {2}", blocklistItem.BlocklistItemId, blocklistItem.Text, blocklistItem.Description);
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
                Console.WriteLine("\nBlockItems added:");
                foreach (var addedBlocklistItem in addedBlocklistItems.Value.BlocklistItems)
                {
                    Console.WriteLine("BlockItemId: {0}, Text: {1}, Description: {2}", addedBlocklistItem.BlocklistItemId, addedBlocklistItem.Text, addedBlocklistItem.Description);
                }
            }
            var getBlockItemId = addedBlocklistItems.Value.BlocklistItems[0].BlocklistItemId;
            var getBlockItem = blocklistClient.GetTextBlocklistItem(blocklistName, getBlockItemId);
            Console.WriteLine("\nGet BlockItem:");
            Console.WriteLine("BlockItemId: {0}, Text: {1}, Description: {2}", getBlockItem.Value.BlocklistItemId, getBlockItem.Value.Text, getBlockItem.Value.Description);

            // Sample: Remove blocklistItems from a blocklist

            var removeBlockItemId = addedBlocklistItems.Value.BlocklistItems[0].BlocklistItemId;
            var removeBlockItemIds = new List<string> { removeBlockItemId };
            var removeResult = blocklistClient.RemoveBlocklistItems(blocklistName, new RemoveTextBlocklistItemsOptions(removeBlockItemIds));

            if (removeResult != null && removeResult.Status == 204)
            {
                Console.WriteLine("\nBlockItem removed: {0}.", removeBlockItemId);
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

