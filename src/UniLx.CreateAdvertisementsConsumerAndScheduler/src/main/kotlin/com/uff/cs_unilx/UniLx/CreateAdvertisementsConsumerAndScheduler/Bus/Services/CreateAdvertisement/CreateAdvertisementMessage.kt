package com.uff.cs_unilx.UniLx.CreateAdvertisementsConsumerAndScheduler.Bus.Services.CreateAdvertisement

import com.fasterxml.jackson.annotation.JsonProperty
import java.time.ZonedDateTime

data class CreateAdvertisementMessage(
    @JsonProperty("content")
    val content: AdvertisementContent,

    @JsonProperty("message_id")
    val messageId: String,

    @JsonProperty("timestamp")
    val timestamp: ZonedDateTime,

    @JsonProperty("type")
    val type: String
)

data class AdvertisementContent(
    @JsonProperty("advertisement_id")
    val advertisementId: String,

    @JsonProperty("owner_id")
    val ownerId: String,

    @JsonProperty("owner_name")
    val ownerName: String,

    @JsonProperty("category_id")
    val categoryId: String,

    @JsonProperty("category_name")
    val categoryName: String,

    @JsonProperty("status")
    val status: String,

    @JsonProperty("type")
    val type: String,

    @JsonProperty("expires_at")
    val expiresAt: ZonedDateTime
)