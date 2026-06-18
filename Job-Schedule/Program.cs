using Job_Scheduler.Services;

namespace Job_Scheduler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var queue = new JobQueue();

            queue.Enqueue(new Job(1, "send-welcome-email", Executors.FastExecutor, duration: 100));
            queue.Enqueue(new Job(2, "fail-fast-job", Executors.FastExecutor, duration: 100));
            queue.Enqueue(new Job(3, "generate-report", Executors.SafeExecutor, duration: 200));
            queue.Enqueue(new Job(4, "fail-safe-job", Executors.SafeExecutor, duration: 200));
            queue.Enqueue(new Job(5, "sync-inventory", Executors.RetryExecutor, duration: 90, retryFailuresBeforeSuccess: 2));
            queue.Enqueue(new Job(6, "fail-retry-job", Executors.RetryExecutor, duration: 90));

            var scheduler = new Scheduler(queue);

            var monitoring = new MonitoringService();
            var logger = new LoggerService();
            var statistics = new StatisticsService();

            scheduler.JobStateChanged += monitoring.Handle;
            scheduler.JobStateChanged += logger.Handle;
            scheduler.JobStateChanged += statistics.Handle;

            scheduler.ExecuteAll();

            Console.WriteLine();
            Console.WriteLine("=== Final Statistics ===");
            statistics.Print();
        }
    }
}

