package com.uff.cs_unilx.UniLx.CreateAdvertisementsConsumerAndScheduler.Bus.Config

import org.springframework.boot.context.properties.ConfigurationProperties
import org.springframework.boot.context.properties.bind.ConstructorBinding

@ConfigurationProperties(prefix = "kafka")
data class KafkaConfig @ConstructorBinding constructor(
    val bootstrapServers: String,
    val createAdvertisementConsumer: ConsumerProperties,
    val expireAdvertisementConsumer: ConsumerProperties
) {
    data class ConsumerProperties(
        val groupId: String,
        val autoOffsetReset: String,
        val maxPollRecords: Int
    )
}
