package com.uff.cs_unilx.UniLx.CreateAdvertisementsConsumerAndScheduler.BackgroundJobs

import org.springframework.boot.context.properties.ConfigurationProperties
import org.springframework.boot.context.properties.bind.ConstructorBinding

@ConfigurationProperties(prefix = "worker")
data class WorkerConfig @ConstructorBinding constructor(
    val delay: Long,
    val maxThreads: Int
)