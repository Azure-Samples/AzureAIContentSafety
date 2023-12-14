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

// Sample: Create or modify a blocklist
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
    "Blocklist created or updated. Name: ",
    result.body.blocklistName,
    ", Description: ",
    result.body.description
  );
}

// Sample: Add blocklistItems to the list
async function addBlocklistItems() {
  const blocklistName = "TestBlocklist";
  const blocklistItemText1 = "sample";
  const blocklistItemText2 = "text";
  const addOrUpdateBlocklistItemsParameters = {
    body: {
      blocklistItems: [
        {
          description: "Test blocklist item 1",
          text: blocklistItemText1,
        },
        {
          description: "Test blocklist item 2",
          text: blocklistItemText2,
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

  console.log("Blocklist items added: ");
  if (result.body.blocklistItems) {
    for (const blocklistItem of result.body.blocklistItems) {
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

// Sample: Analyze text with a blocklist
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
        ", BlocklistItemId: ",
        blocklistMatchResult.blocklistItemId,
        ", BlocklistItemText: ",
        blocklistMatchResult.blocklistItemText
      );
    }
  }
}

(async () => {
  await createOrUpdateTextBlocklist();
  await addBlocklistItems();
  await analyzeTextWithBlocklists();
})().catch((err) => {
  console.error("The sample encountered an error:", err);
});
