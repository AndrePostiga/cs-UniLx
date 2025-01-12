package com.uff.cs_unilx.UniLx.CreateAdvertisementsConsumerAndScheduler.BackgroundJobs

import com.uff.cs_unilx.UniLx.CreateAdvertisementsConsumerAndScheduler.Bus.Services.CreateAdvertisement.CreateAdvertisementConsumerService
import com.uff.cs_unilx.UniLx.CreateAdvertisementsConsumerAndScheduler.Bus.Services.ExpireAdvertisementConsumerService
import kotlinx.coroutines.*
import org.springframework.stereotype.Component

@Component
class Worker(
    private val workerConfig: WorkerConfig
) {
    private val scope = CoroutineScope(Dispatchers.Default + SupervisorJob())
    private lateinit var createConsumer: CreateAdvertisementConsumerService
    private lateinit var expireConsumer: ExpireAdvertisementConsumerService

    fun start() {
        println("Starting worker...")

        createConsumer = CreateAdvertisementConsumerService(
            topic = "CreateAdvertisementTopic",
            bootstrapServers = kafkaConfig.bootstrapServers,
            groupId = kafkaConfig.createAdvertisementConsumer.groupId
        )

        scope.launch { createConsumer.pollAndProcess { rawMessage -> deserializeCreateAdvertisementMessage(rawMessage) } }
    }

    fun stop() {
        println("Stopping worker...")
        createConsumer.close()
        scope.cancel()
    }

    suspend fun processAsync() {
        try {
            while (scope.isActive) {
                println("Running background job at: ${System.currentTimeMillis()}")
                delay(workerConfig.delay) // Simulate work with a delay
            }
        } finally {
            println("Cleaning up background job resources.")
        }
    }
}

