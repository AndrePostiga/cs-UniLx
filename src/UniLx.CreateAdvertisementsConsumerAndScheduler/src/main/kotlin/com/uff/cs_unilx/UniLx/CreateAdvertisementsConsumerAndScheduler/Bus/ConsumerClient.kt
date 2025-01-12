package com.uff.cs_unilx.UniLx.CreateAdvertisementsConsumerAndScheduler.Bus

import org.apache.kafka.clients.consumer.Consumer
import org.apache.kafka.clients.consumer.ConsumerConfig
import org.apache.kafka.clients.consumer.ConsumerRecords
import org.apache.kafka.clients.consumer.KafkaConsumer
import java.time.Duration
import java.util.*

abstract class ConsumerClient<T> (
    private val topic: String,
    private val bootstrapServers: String,
    private val groupId: String,
    private val autoOffsetReset: String = "earliest"
){

    private val kafkaConsumer: Consumer<String, T>

    init {
        val props = Properties()
        props[ConsumerConfig.BOOTSTRAP_SERVERS_CONFIG] = bootstrapServers
        props[ConsumerConfig.GROUP_ID_CONFIG] = groupId
        props[ConsumerConfig.KEY_DESERIALIZER_CLASS_CONFIG] = "org.apache.kafka.common.serialization.StringDeserializer"
        props[ConsumerConfig.VALUE_DESERIALIZER_CLASS_CONFIG] = "org.apache.kafka.common.serialization.StringDeserializer"
        props[ConsumerConfig.AUTO_OFFSET_RESET_CONFIG] = autoOffsetReset

        kafkaConsumer = KafkaConsumer(props)
        kafkaConsumer.subscribe(listOf(topic))
        println("Kafka consumer initialized and subscribed to topic: $topic")
    }

    abstract suspend fun processMessage(message: T)

    suspend fun pollAndProcess(duration: Duration = Duration.ofMillis(1000)) {
        try {
            val records: ConsumerRecords<String, T> = kafkaConsumer.poll(duration)
            for (record in records) {
                try {
                    processMessage(record.value())
                } catch (ex: Exception) {
                    println("Failed to process message: ${record.value()} ex: ${ex.message}")
                }
            }
        } catch (ex: Exception) {
            println("Error during Kafka polling: ${ex.message}")
        }
    }

    fun close() {
        try {
            kafkaConsumer.close()
        } catch (ex: Exception) {
            println("Error closing Kafka consumer for topic [$topic]: ${ex.message}")
        }
    }

}