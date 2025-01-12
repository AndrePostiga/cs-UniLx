package com.uff.cs_unilx.UniLx.CreateAdvertisementsConsumerAndScheduler

import com.uff.cs_unilx.UniLx.CreateAdvertisementsConsumerAndScheduler.BackgroundJobs.Worker
import org.springframework.boot.autoconfigure.SpringBootApplication
import org.springframework.boot.runApplication
import org.springframework.context.ApplicationContext


@SpringBootApplication
class Application

fun main(args: Array<String>) {
	val context: ApplicationContext = runApplication<Application>(*args)

	val worker : Worker = context.getBean(Worker::class.java)
	worker.start()

	Runtime.getRuntime().addShutdownHook(Thread {
		println("Graceful shutdown initiated...")
		worker.stop()
		println("Application has shut down gracefully.")
	})
}