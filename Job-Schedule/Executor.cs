namespace Job_Scheduler
{
    internal static class Executors
    {
        public static void FastExecutor(Job job)
        {
            Console.WriteLine($"[Fast] Running '{job.Name}'...");

            if (Contains(job.Name, "fail-fast"))
            {
                throw new JobException($"'{job.Name}' is configured to fail fast.", job);
            }

            Thread.Sleep(job.Duration / 2);
        }

        public static void SafeExecutor(Job job)
        {
            try
            {
                Console.WriteLine($"[Safe] Running '{job.Name}'...");

                if (Contains(job.Name, "fail-safe"))
                {
                    throw new JobException($"'{job.Name}' is configured to fail safely.", job);
                }

                Thread.Sleep(job.Duration);
            }
            catch (Exception ex)
            {
                throw new JobException($"SafeExecutor failed for '{job.Name}': {ex.Message}", job, ex);
            }
        }

        public static void RetryExecutor(Job job)
        {
            Exception? lastError = null;

            for (int attempt = 1; attempt <= 3; attempt++)
            {
                try
                {
                    Console.WriteLine($"[Retry] Attempt {attempt}/3 for '{job.Name}'...");

                    if (Contains(job.Name, "fail-retry"))
                    {
                        throw new JobException($"'{job.Name}' is configured to always fail.", job);
                    }

                    if (attempt <= job.RetryFailuresBeforeSuccess)
                    {
                        throw new JobException(
                            $"Transient failure on attempt {attempt} for '{job.Name}'.", job);
                    }

                    Thread.Sleep(job.Duration / 3);
                    return;
                }
                catch (Exception ex)
                {
                    lastError = ex;
                    Console.WriteLine($"[Retry] Attempt {attempt} failed: {ex.Message}");
                }
            }

            throw lastError!;
        }

        private static bool Contains(string name, string token)
        {
            return name.Contains(token, StringComparison.OrdinalIgnoreCase);
        }
    }
}