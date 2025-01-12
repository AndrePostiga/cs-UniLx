package com.uff.cs_unilx.UniLx.CreateAdvertisementsConsumerAndScheduler.Bus.Services.CreateAdvertisement

import com.fasterxml.jackson.module.kotlin.jacksonObjectMapper
import com.uff.cs_unilx.UniLx.CreateAdvertisementsConsumerAndScheduler.Bus.ConsumerClient
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext

class CreateAdvertisementConsumerService(bootstrapServers: String, groupId: String)
    : ConsumerClient<CreateAdvertisementMessage>(
    topic = "CreateAdvertisementTopic",
    bootstrapServers = bootstrapServers,
    groupId = groupId
) {

    private val objectMapper = jacksonObjectMapper()

    override suspend fun processMessage(message: CreateAdvertisementMessage) {
        try {
            withContext(Dispatchers.IO) {
                println("Processed advertisement: ${message.content.advertisementId}")
            }
        } catch (ex: Exception) {
            println("Error processing CreateAdvertisementMessage: $message ex:${ex.message}")
        }
    }

    suspend fun startConsuming() {
        pollAndProcess()
    }
}