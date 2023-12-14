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
    blocklist_item_text_1 = "k*ll"
    blocklist_item_text_2 = "h*te"

    blocklist_items = [TextBlocklistItem(text=blocklist_item_text_1), TextBlocklistItem(text=blocklist_item_text_2)]
    try:
        result = client.add_or_update_blocklist_items(
            blocklist_name=blocklist_name, options=AddOrUpdateTextBlocklistItemsOptions(blocklist_items=blocklist_items)
        )
        for blocklist_item in result.blocklist_items:
            print(
                f"BlocklistItemId: {blocklist_item.blocklist_item_id}, Text: {blocklist_item.text}, Description: {blocklist_item.description}"
            )
    except HttpResponseError as e:
        print("\nAdd blocklistItems failed: ")
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
                    f"BlocklistName: {match_result.blocklist_name}, BlocklistItemId: {match_result.blocklist_item_id}, "
                    f"BlocklistItemText: {match_result.blocklist_item_text}"
                )
    except HttpResponseError as e:
        print("\nAnalyze text failed: ")
        if e.error:
            print(f"Error code: {e.error.code}")
            print(f"Error message: {e.error.message}")
            raise
        print(e)
        raise


# Sample: List all blocklistItems in a blocklist
def list_blocklist_items():
    import os
    from azure.ai.contentsafety import BlocklistClient
    from azure.core.credentials import AzureKeyCredential
    from azure.core.exceptions import HttpResponseError

    key = os.environ["CONTENT_SAFETY_KEY"]
    endpoint = os.environ["CONTENT_SAFETY_ENDPOINT"]

    # Create a Blocklist client
    client = BlocklistClient(endpoint, AzureKeyCredential(key))

    blocklist_name = "TestBlocklist"

    try:
        blocklist_items = client.list_text_blocklist_items(blocklist_name=blocklist_name)
        if blocklist_items:
            print("\nList blocklist items: ")
            for blocklist_item in blocklist_items:
                print(
                    f"BlocklistItemId: {blocklist_item.blocklist_item_id}, Text: {blocklist_item.text}, "
                    f"Description: {blocklist_item.description}"
                )
    except HttpResponseError as e:
        print("\nList blocklist items failed: ")
        if e.error:
            print(f"Error code: {e.error.code}")
            print(f"Error message: {e.error.message}")
            raise
        print(e)
        raise


# Sample: List all blocklists
def list_text_blocklists():
    import os
    from azure.ai.contentsafety import BlocklistClient
    from azure.core.credentials import AzureKeyCredential
    from azure.core.exceptions import HttpResponseError

    key = os.environ["CONTENT_SAFETY_KEY"]
    endpoint = os.environ["CONTENT_SAFETY_ENDPOINT"]

    # Create a Blocklist client
    client = BlocklistClient(endpoint, AzureKeyCredential(key))

    try:
        blocklists = client.list_text_blocklists()
        if blocklists:
            print("\nList blocklists: ")
            for blocklist in blocklists:
                print(f"Name: {blocklist.blocklist_name}, Description: {blocklist.description}")
    except HttpResponseError as e:
        print("\nList text blocklists failed: ")
        if e.error:
            print(f"Error code: {e.error.code}")
            print(f"Error message: {e.error.message}")
            raise
        print(e)
        raise


# Sample: Get a blocklist by blocklistName
def get_text_blocklist():
    import os
    from azure.ai.contentsafety import BlocklistClient
    from azure.core.credentials import AzureKeyCredential
    from azure.core.exceptions import HttpResponseError

    key = os.environ["CONTENT_SAFETY_KEY"]
    endpoint = os.environ["CONTENT_SAFETY_ENDPOINT"]

    # Create a Blocklist client
    client = BlocklistClient(endpoint, AzureKeyCredential(key))

    blocklist_name = "TestBlocklist"

    try:
        blocklist = client.get_text_blocklist(blocklist_name=blocklist_name)
        if blocklist:
            print("\nGet blocklist: ")
            print(f"Name: {blocklist.blocklist_name}, Description: {blocklist.description}")
    except HttpResponseError as e:
        print("\nGet text blocklist failed: ")
        if e.error:
            print(f"Error code: {e.error.code}")
            print(f"Error message: {e.error.message}")
            raise
        print(e)
        raise


# Sample: Get a blocklistItem by blocklistName and blocklistItemId
def get_blocklist_item():
    import os
    from azure.ai.contentsafety import BlocklistClient
    from azure.core.credentials import AzureKeyCredential
    from azure.ai.contentsafety.models import TextBlocklistItem, AddOrUpdateTextBlocklistItemsOptions
    from azure.core.exceptions import HttpResponseError

    key = os.environ["CONTENT_SAFETY_KEY"]
    endpoint = os.environ["CONTENT_SAFETY_ENDPOINT"]

    # Create a Blocklist client
    client = BlocklistClient(endpoint, AzureKeyCredential(key))

    blocklist_name = "TestBlocklist"
    block_item_text_1 = "k*ll"

    try:
        # Add a blocklistItem
        add_result = client.add_or_update_blocklist_items(
            blocklist_name=blocklist_name,
            options=AddOrUpdateTextBlocklistItemsOptions(blocklist_items=[TextBlocklistItem(text=block_item_text_1)]),
        )
        if not add_result or not add_result.blocklist_items or len(add_result.blocklist_items) <= 0:
            raise RuntimeError("BlocklistItem not created.")
        blocklist_item_id = add_result.blocklist_items[0].blocklist_item_id

        # Get this blocklistItem by blocklistItemId
        blocklist_item = client.get_text_blocklist_item(blocklist_name=blocklist_name, blocklist_item_id=blocklist_item_id)
        print("\nGet blocklistItem: ")
        print(
            f"BlocklistItemId: {blocklist_item.blocklist_item_id}, Text: {blocklist_item.text}, Description: {blocklist_item.description}"
        )
    except HttpResponseError as e:
        print("\nGet blocklist item failed: ")
        if e.error:
            print(f"Error code: {e.error.code}")
            print(f"Error message: {e.error.message}")
            raise
        print(e)
        raise


# Sample: Remove blocklistItems from a blocklist
def remove_blocklist_items():
    import os
    from azure.ai.contentsafety import BlocklistClient
    from azure.core.credentials import AzureKeyCredential
    from azure.ai.contentsafety.models import (
        TextBlocklistItem,
        AddOrUpdateTextBlocklistItemsOptions,
        RemoveTextBlocklistItemsOptions,
    )
    from azure.core.exceptions import HttpResponseError

    key = os.environ["CONTENT_SAFETY_KEY"]
    endpoint = os.environ["CONTENT_SAFETY_ENDPOINT"]

    # Create a Blocklist client
    client = BlocklistClient(endpoint, AzureKeyCredential(key))

    blocklist_name = "TestBlocklist"
    block_item_text_1 = "k*ll"

    try:
        # Add a blocklistItem
        add_result = client.add_or_update_blocklist_items(
            blocklist_name=blocklist_name,
            options=AddOrUpdateTextBlocklistItemsOptions(blocklist_items=[TextBlocklistItem(text=block_item_text_1)]),
        )
        if not add_result or not add_result.blocklist_items or len(add_result.blocklist_items) <= 0:
            raise RuntimeError("BlocklistItem not created.")
        block_item_id = add_result.blocklist_items[0].blocklist_item_id

        # Remove this blocklistItem by blocklistItemId
        client.remove_blocklist_items(
            blocklist_name=blocklist_name, options=RemoveTextBlocklistItemsOptions(blocklist_item_ids=[block_item_id])
        )
        print(f"\nRemoved blocklistItem: {add_result.blocklist_items[0].blocklist_item_id}")
    except HttpResponseError as e:
        print("\nRemove blocklist item failed: ")
        if e.error:
            print(f"Error code: {e.error.code}")
            print(f"Error message: {e.error.message}")
            raise
        print(e)
        raise


# Sample: Delete a list and all of its contents
def delete_blocklist():
    import os
    from azure.ai.contentsafety import BlocklistClient
    from azure.core.credentials import AzureKeyCredential
    from azure.core.exceptions import HttpResponseError

    key = os.environ["CONTENT_SAFETY_KEY"]
    endpoint = os.environ["CONTENT_SAFETY_ENDPOINT"]

    # Create a Blocklist client
    client = BlocklistClient(endpoint, AzureKeyCredential(key))

    blocklist_name = "TestBlocklist"

    try:
        client.delete_text_blocklist(blocklist_name=blocklist_name)
        print(f"\nDeleted blocklist: {blocklist_name}")
    except HttpResponseError as e:
        print("\nDelete blocklist failed:")
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
    list_blocklist_items()
    list_text_blocklists()
    get_text_blocklist()
    get_blocklist_item()
    remove_blocklist_items()
    delete_blocklist()
