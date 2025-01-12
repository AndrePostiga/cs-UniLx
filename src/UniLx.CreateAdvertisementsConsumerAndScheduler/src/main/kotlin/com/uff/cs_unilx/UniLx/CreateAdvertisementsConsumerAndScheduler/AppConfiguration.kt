package com.uff.cs_unilx.UniLx.CreateAdvertisementsConsumerAndScheduler

import com.uff.cs_unilx.UniLx.CreateAdvertisementsConsumerAndScheduler.BackgroundJobs.WorkerConfig
import com.uff.cs_unilx.UniLx.CreateAdvertisementsConsumerAndScheduler.Bus.Config.KafkaConfig
import org.springframework.boot.context.properties.EnableConfigurationProperties
import org.springframework.context.annotation.Configuration

@Configuration
@EnableConfigurationProperties(
    WorkerConfig::class,
    KafkaConfig::class
)
class AppConfig