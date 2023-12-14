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

// List all blocklistItems in a list
async function listBlockItems() {
  const blocklistName = "TestBlocklist";

  const result = await client
    .path("/text/blocklists/{blocklistName}/blocklistItems", blocklistName)
    .get();

  if (isUnexpected(result)) {
    throw result;
  }

  console.log("List block items: ");
  if (result.body.value) {
    for (const blockItem of result.body.value) {
      console.log(
        "BlockItemId: ",
        blockItem.blocklistItemId,
        ", Text: ",
        blockItem.text,
        ", Description: ",
        blockItem.description
      );
    }
  }
}

// List all blocklists
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

// Get a blocklist by blocklistName
async function getTextBlocklist() {
  const blocklistName = "TestBlocklist";

  const result = await client.path("/text/blocklists/{blocklistName}", blocklistName).get();

  if (isUnexpected(result)) {
    throw result;
  }

  console.log("Get blocklist: ");
  console.log("Name: ", result.body.blocklistName, ", Description: ", result.body.description);
}

// Get a blocklistItem by blocklistName and blocklistItemId
async function getBlockItem() {
  const blocklistName = "TestBlocklist";
  const blockItemText = "sample";

  // Add a blocklistItem and get its id
  const addOrUpdateBlocklistItemsParameters = {
    body: {
      blocklistItems: [
        {
          description: "Test block item 1",
          text: blockItemText,
        },
      ],
    },
  };
  const result = await client
    .path("/text/blocklists/{blocklistName}:addOrUpdateBlocklistItems", blocklistName)
    .post(addOrUpdateBlocklistItemsParameters);
  if (isUnexpected(result) || result.body.blocklistItems === undefined) {
    throw new Error("Block item not added.");
  }
  const blockItemId = result.body.blocklistItems[0].blocklistItemId;

  // Get this blocklistItem by blocklistItemId
  const blockItem = await client
    .path(
      "/text/blocklists/{blocklistName}/blocklistItems/{blocklistItemId}",
      blocklistName,
      blockItemId
    )
    .get();

  if (isUnexpected(blockItem)) {
    throw blockItem;
  }

  console.log("Get blockitem: ");
  console.log(
    "BlockItemId: ",
    blockItem.body.blocklistItemId,
    ", Text: ",
    blockItem.body.text,
    ", Description: ",
    blockItem.body.description
  );
}

// Remove blocklistItems from a blocklist
async function removeBlockItems() {
  const blocklistName = "TestBlocklist";
  const blockItemText = "sample";

  // Add a blocklistItem and get its id
  const addOrUpdateBlocklistItemsParameters = {
    body: {
      blocklistItems: [
        {
          description: "Test block item 1",
          text: blockItemText,
        },
      ],
    },
  };
  const result = await client
    .path("/text/blocklists/{blocklistName}:addOrUpdateBlocklistItems", blocklistName)
    .post(addOrUpdateBlocklistItemsParameters);
  if (isUnexpected(result) || result.body.blocklistItems === undefined) {
    throw new Error("Block item not added.");
  }
  const blockItemId = result.body.blocklistItems[0].blocklistItemId;

  // Remove this blocklistItem by blocklistItemId
  const removeBlocklistItemsParameters = {
    body: {
      blocklistItemIds: [blockItemId],
    },
  };
  const removeBlockItem = await client
    .path("/text/blocklists/{blocklistName}:removeBlocklistItems", blocklistName)
    .post(removeBlocklistItemsParameters);

  if (isUnexpected(removeBlockItem)) {
    throw removeBlockItem;
  }

  console.log("Removed blockItem: ", blockItemText);
}

// Delete a list and all of its contents
async function deleteBlocklist() {
  const blocklistName = "TestBlocklist";

  const result = await client.path("/text/blocklists/{blocklistName}", blocklistName).delete();

  if (isUnexpected(result)) {
    throw result;
  }

  console.log("Deleted blocklist: ", blocklistName);
}

(async () => {
  await listBlockItems();
  await listTextBlocklists();
  await getTextBlocklist();
  await getBlockItem();
  await removeBlockItems();
  await deleteBlocklist();
})().catch((err) => {
  console.error("The sample encountered an error:", err);
});
