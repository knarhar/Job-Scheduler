namespace Job_Scheduler
{
    internal class Scheduler
    {
        private readonly JobQueue _queue;

        public event EventHandler<JobEventArgs>? JobStateChanged;

        public Scheduler(JobQueue queue)
        {
            _queue = queue;
        }

        public void ExecuteAll()
        {
            foreach (Job job in _queue)
            {
                try
                {
                    job.Status = JobStatus.Running;
                    JobStateChanged?.Invoke(this, new JobEventArgs(job, "JobStarted"));

                    job.Executor(job);

                    job.Status = JobStatus.Completed;
                    JobStateChanged?.Invoke(this, new JobEventArgs(job, "JobCompleted"));
                }
                catch (Exception ex)
                {
                    job.Status = JobStatus.Failed;
                    JobStateChanged?.Invoke(this, new JobEventArgs(job, "JobFailed", ex));
                }

                Console.WriteLine($"Scheduler finished processing job {job.Id}");
            }
        }
    }
}