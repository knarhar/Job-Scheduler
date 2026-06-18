namespace Job_Scheduler
{
    delegate void JobExecutor(Job job);

    internal class Job
    {
        public int Id { get; }
        public string Name { get; }
        public JobStatus Status { get; set; }
        public JobExecutor Executor { get; }

        public int Duration { get; }
        public int RetryFailuresBeforeSuccess { get; }

        public Job(
            int id,
            string name,
            JobExecutor executor,
            int duration = 200,
            int retryFailuresBeforeSuccess = 0)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Executor = executor ?? throw new ArgumentNullException(nameof(executor));
            Duration = duration;
            RetryFailuresBeforeSuccess = retryFailuresBeforeSuccess;
            Status = JobStatus.Pending;
        }
    }
}