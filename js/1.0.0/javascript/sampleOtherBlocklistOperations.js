// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

/**
 * @summary Demonstrates how to manage blocklist.
 */

const ContentSafetyClient = require("@azure-rest/ai-content-safety").default,
  { isUnexpected } = require("@azure-rest/ai-content-safety");
const { AzureKeyCredential } = require("@azure/core-auth");

// Load the .env file if it exists
require("dotenv").config();

const endpoint = process.env["CONTENT_SAFETY_ENDPOINT"] || "<endpoint>";
const key = process.env["CONTENT_SAFETY_API_KEY"] || "<key>";

const credential = new AzureKeyCredential(key);
const client = ContentSafetyClient(endpoint, credential);

// Sample: List all blocklistItems in a list
async function listBlocklistItems() {
  const blocklistName = "TestBlocklist";

  const result = await client
    .path("/text/blocklists/{blocklistName}/blocklistItems", blocklistName)
    .get();

  if (isUnexpected(result)) {
    throw result;
  }

  console.log("List blocklist items: ");
  if (result.body.value) {
    for (const blocklistItem of result.body.value) {
      console.log(
        "BlocklistItemId: ",
        blocklistItem.blocklistItemId,
        ", Text: ",
        blocklistItem.text,
        ", Description: ",
        blocklistItem.description
      );
    }
  }
}

// Sample: List all blocklists
async function listTextBlocklists() {
  const result = await client.path("/text/blocklists").get();

  if (isUnexpected(result)) {
    throw result;
  }

  console.log("List blocklists: ");
  if (result.body.value) {
    for (const blocklist of result.body.value) {
      console.log(
        "BlocklistName: ",
        blocklist.blocklistName,
        ", Description: ",
        blocklist.description
      );
    }
  }
}

// Sample: Get a blocklist by blocklistName
async function getTextBlocklist() {
  const blocklistName = "TestBlocklist";

  const result = await client.path("/text/blocklists/{blocklistName}", blocklistName).get();

  if (isUnexpected(result)) {
    throw result;
  }

  console.log("Get blocklist: ");
  console.log("Name: ", result.body.blocklistName, ", Description: ", result.body.description);
}

// Sample: Get a blocklistItem by blocklistName and blocklistItemId
async function getBlocklistItem() {
  const blocklistName = "TestBlocklist";
  const blocklistItemText = "sample";

  // Add a blocklistItem and get its id
  const addOrUpdateBlocklistItemsParameters = {
    body: {
      blocklistItems: [
        {
          description: "Test blocklist item 1",
          text: blocklistItemText,
        },
      ],
    },
  };
  const result = await client
    .path("/text/blocklists/{blocklistName}:addOrUpdateBlocklistItems", blocklistName)
    .post(addOrUpdateBlocklistItemsParameters);
  if (isUnexpected(result) || result.body.blocklistItems === undefined) {
    throw new Error("Blocklist item not added.");
  }
  const blocklistItemId = result.body.blocklistItems[0].blocklistItemId;

  // Get this blocklistItem by blocklistItemId
  const blocklistItem = await client
    .path(
      "/text/blocklists/{blocklistName}/blocklistItems/{blocklistItemId}",
      blocklistName,
      blocklistItemId
    )
    .get();

  if (isUnexpected(blocklistItem)) {
    throw blocklistItem;
  }

  console.log("Get blocklistItem: ");
  console.log(
    "BlocklistItemId: ",
    blocklistItem.body.blocklistItemId,
    ", Text: ",
    blocklistItem.body.text,
    ", Description: ",
    blocklistItem.body.description
  );
}

// Sample: Remove blocklistItems from a blocklist
async function removeBlocklistItems() {
  const blocklistName = "TestBlocklist";
  const blocklistItemText = "sample";

  // Add a blocklistItem and get its id
  const addOrUpdateBlocklistItemsParameters = {
    body: {
      blocklistItems: [
        {
          description: "Test blocklist item 1",
          text: blocklistItemText,
        },
      ],
    },
  };
  const result = await client
    .path("/text/blocklists/{blocklistName}:addOrUpdateBlocklistItems", blocklistName)
    .post(addOrUpdateBlocklistItemsParameters);
  if (isUnexpected(result) || result.body.blocklistItems === undefined) {
    throw new Error("Blocklist item not added.");
  }
  const blocklistItemId = result.body.blocklistItems[0].blocklistItemId;

  // Remove this blocklistItem by blocklistItemId
  const removeBlocklistItemsParameters = {
    body: {
      blocklistItemIds: [blocklistItemId],
    },
  };
  const removeBlocklistItem = await client
    .path("/text/blocklists/{blocklistName}:removeBlocklistItems", blocklistName)
    .post(removeBlocklistItemsParameters);

  if (isUnexpected(removeBlocklistItem)) {
    throw removeBlocklistItem;
  }

  console.log("Removed blocklistItem: ", blocklistItemText);
}

// Sample: Delete a list and all of its contents
async function deleteBlocklist() {
  const blocklistName = "TestBlocklist";

  const result = await client.path("/text/blocklists/{blocklistName}", blocklistName).delete();

  if (isUnexpected(result)) {
    throw result;
  }

  console.log("Deleted blocklist: ", blocklistName);
}

(async () => {
  await listBlocklistItems();
  await listTextBlocklists();
  await getTextBlocklist();
  await getBlocklistItem();
  await removeBlocklistItems();
  await deleteBlocklist();
})().catch((err) => {
  console.error("The sample encountered an error:", err);
});
