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

// Create or modify a blocklist
async function createOrUpdateTextBlocklist() {
  const blocklistName = "TestBlocklist";
  const blocklistDescription = "Test blocklist management.";
  const createOrUpdateTextBlocklistParameters = {
    contentType: "application/merge-patch+json",
    body: {
      description: blocklistDescription,
    },
  };

  const result = await client
    .path("/text/blocklists/{blocklistName}", blocklistName)
    .patch(createOrUpdateTextBlocklistParameters);

  if (isUnexpected(result)) {
    throw result;
  }

  console.log(
    "Blocklist created or updated: Name",
    result.body.blocklistName,
    ", Description: ",
    result.body.description
  );
}

// Add blocklistItems to the list
async function addBlockItems() {
  const blocklistName = "TestBlocklist";
  const blockItemText1 = "sample";
  const blockItemText2 = "text";
  const addOrUpdateBlocklistItemsParameters = {
    body: {
      blocklistItems: [
        {
          description: "Test block item 1",
          text: blockItemText1,
        },
        {
          description: "Test block item 2",
          text: blockItemText2,
        },
      ],
    },
  };

  const result = await client
    .path("/text/blocklists/{blocklistName}:addOrUpdateBlocklistItems", blocklistName)
    .post(addOrUpdateBlocklistItemsParameters);

  if (isUnexpected(result)) {
    throw result;
  }

  console.log("Block items added: ");
  if (result.body.blocklistItems) {
    for (const blockItem of result.body.blocklistItems) {
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

// Analyze text with a blocklist
async function analyzeTextWithBlocklists() {
  const blocklistName = "TestBlocklist";
  const inputText = "This is a sample to test text with blocklist.";
  const analyzeTextParameters = {
    body: {
      text: inputText,
      blocklistNames: [blocklistName],
      haltOnBlocklistHit: false,
    },
  };

  const result = await client.path("/text:analyze").post(analyzeTextParameters);

  if (isUnexpected(result)) {
    throw result;
  }

  console.log("Blocklist match results: ");
  if (result.body.blocklistsMatch) {
    for (const blocklistMatchResult of result.body.blocklistsMatch) {
      console.log(
        "BlocklistName: ",
        blocklistMatchResult.blocklistName,
        ", BlockItemId: ",
        blocklistMatchResult.blocklistItemId,
        ", BlockItemText: ",
        blocklistMatchResult.blocklistItemText
      );
    }
  }
}

(async () => {
  await createOrUpdateTextBlocklist();
  await addBlockItems();
  await analyzeTextWithBlocklists();
})().catch((err) => {
  console.error("The sample encountered an error:", err);
});
