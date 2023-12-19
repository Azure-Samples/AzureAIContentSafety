package org.example;

import com.azure.ai.contentsafety.BlocklistClient;
import com.azure.ai.contentsafety.BlocklistClientBuilder;
import com.azure.ai.contentsafety.ContentSafetyClient;
import com.azure.ai.contentsafety.ContentSafetyClientBuilder;
import com.azure.core.credential.KeyCredential;
import com.azure.core.util.Configuration;
import com.azure.identity.DefaultAzureCredentialBuilder;

public class CreateClient {
    public static void main(String[] args) {
        // Create client with credential
        String endpoint = Configuration.getGlobalConfiguration().get("CONTENT_SAFETY_ENDPOINT");
        String key = Configuration.getGlobalConfiguration().get("CONTENT_SAFETY_KEY");
        ContentSafetyClient contentSafetyClient = new ContentSafetyClientBuilder()
                .credential(new KeyCredential(key))
                .endpoint(endpoint).buildClient();
        BlocklistClient blocklistClient = new BlocklistClientBuilder()
                .credential(new KeyCredential(key))
                .endpoint(endpoint).buildClient();

        // Create client with Microsoft Entra ID
        // Set the values of the client ID, tenant ID, and client secret of the Microsoft Entra ID application as
        // environment variables: AZURE_CLIENT_ID, AZURE_TENANT_ID, AZURE_CLIENT_SECRET.
        ContentSafetyClient contentSafetyClientOauth = new ContentSafetyClientBuilder()
                .credential(new DefaultAzureCredentialBuilder().build())
                .endpoint(endpoint).buildClient();
        BlocklistClient blocklistClientOauth = new BlocklistClientBuilder()
                .credential(new DefaultAzureCredentialBuilder().build())
                .endpoint(endpoint).buildClient();
    }
}
