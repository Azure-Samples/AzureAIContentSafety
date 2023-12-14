# coding: utf-8

# -------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# --------------------------------------------------------------------------

# Sample: Create or modify a blocklist
def create_or_update_text_blocklist():
    # [START create_or_update_text_blocklist]

    import os
    from azure.ai.contentsafety import BlocklistClient
    from azure.ai.contentsafety.models import TextBlocklist
    from azure.core.credentials import AzureKeyCredential
    from azure.core.exceptions import HttpResponseError

    key = os.environ["CONTENT_SAFETY_KEY"]
    endpoint = os.environ["CONTENT_SAFETY_ENDPOINT"]

    # Create a Blocklist client
    client = BlocklistClient(endpoint, AzureKeyCredential(key))

    blocklist_name = "TestBlocklist"
    blocklist_description = "Test blocklist management."

    try:
        blocklist = client.create_or_update_text_blocklist(
            blocklist_name=blocklist_name,
            options=TextBlocklist(blocklist_name=blocklist_name, description=blocklist_description),
        )
        if blocklist:
            print("\nBlocklist created or updated: ")
            print(f"Name: {blocklist.blocklist_name}, Description: {blocklist.description}")
    except HttpResponseError as e:
        print("\nCreate or update text blocklist failed: ")
        if e.error:
            print(f"Error code: {e.error.code}")
            print(f"Error message: {e.error.message}")
            raise
        print(e)
        raise

    # [END create_or_update_text_blocklist]


# Sample: Add blocklistItems to the list
def add_blocklist_items():
    import os
    from azure.ai.contentsafety import BlocklistClient
    from azure.ai.contentsafety.models import AddOrUpdateTextBlocklistItemsOptions, TextBlocklistItem
    from azure.core.credentials import AzureKeyCredential
    from azure.core.exceptions import HttpResponseError

    key = os.environ["CONTENT_SAFETY_KEY"]
    endpoint = os.environ["CONTENT_SAFETY_ENDPOINT"]

    # Create a Blocklist client
    client = BlocklistClient(endpoint, AzureKeyCredential(key))

    blocklist_name = "TestBlocklist"
    block_item_text_1 = "k*ll"
    block_item_text_2 = "h*te"

    block_items = [TextBlocklistItem(text=block_item_text_1), TextBlocklistItem(text=block_item_text_2)]
    try:
        result = client.add_or_update_blocklist_items(
            blocklist_name=blocklist_name, options=AddOrUpdateTextBlocklistItemsOptions(blocklist_items=block_items)
        )
        for block_item in result.blocklist_items:
            print(
                f"BlockItemId: {block_item.blocklist_item_id}, Text: {block_item.text}, Description: {block_item.description}"
            )
    except HttpResponseError as e:
        print("\nAdd block items failed: ")
        if e.error:
            print(f"Error code: {e.error.code}")
            print(f"Error message: {e.error.message}")
            raise
        print(e)
        raise


# Sample: Analyze text with a blocklist
def analyze_text_with_blocklists():
    import os
    from azure.ai.contentsafety import ContentSafetyClient
    from azure.core.credentials import AzureKeyCredential
    from azure.ai.contentsafety.models import AnalyzeTextOptions
    from azure.core.exceptions import HttpResponseError

    key = os.environ["CONTENT_SAFETY_KEY"]
    endpoint = os.environ["CONTENT_SAFETY_ENDPOINT"]

    # Create a Content Safety client
    client = ContentSafetyClient(endpoint, AzureKeyCredential(key))

    blocklist_name = "TestBlocklist"
    input_text = "I h*te you and I want to k*ll you."

    try:
        # After you edit your blocklist, it usually takes effect in 5 minutes, please wait some time before analyzing
        # with blocklist after editing.
        analysis_result = client.analyze_text(
            AnalyzeTextOptions(text=input_text, blocklist_names=[blocklist_name], halt_on_blocklist_hit=False)
        )
        if analysis_result and analysis_result.blocklists_match:
            print("\nBlocklist match results: ")
            for match_result in analysis_result.blocklists_match:
                print(
                    f"BlocklistName: {match_result.blocklist_name}, BlockItemId: {match_result.blocklist_item_id}, "
                    f"BlockItemText: {match_result.blocklist_item_text}"
                )
    except HttpResponseError as e:
        print("\nAnalyze text failed: ")
        if e.error:
            print(f"Error code: {e.error.code}")
            print(f"Error message: {e.error.message}")
            raise
        print(e)
        raise


if __name__ == "__main__":
    create_or_update_text_blocklist()
    add_blocklist_items()
    analyze_text_with_blocklists()
