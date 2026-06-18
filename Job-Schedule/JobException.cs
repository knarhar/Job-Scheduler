namespace Job_Scheduler
{
    internal class JobException : Exception
    {
        public Job Job { get; }

        public JobException(string message, Job job)
            : base(message)
        {
            Job = job;
        }

        public JobException(string message, Job job, Exception inner)
            : base(message, inner)
        {
            Job = job;
        }
    }
}