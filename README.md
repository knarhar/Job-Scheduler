# Job Scheduler

A small console app that simulates an event-driven job scheduler. Jobs are queued,
executed by pluggable delegate-based executors, and their lifecycle (started,
completed, failed) is broadcast as events to logging/monitoring/statistics subscribers.

## Project structure
```
Job_Schedule/

├── Job.cs                  # Job model (Id, Name, Status, Executor, ...)
├── JobStatus.cs            # Pending / Running / Completed / Failed
├── JobEventArgs.cs         # Event payload (Job, EventName, Error)
├── JobException.cs         # Custom exception carrying the failing Job
├── JobQueue.cs             # Array-backed queue with auto-resize on Enqueue
├── JobQueueEnumerator.cs   # Custom IEnumerator, yields only Pending jobs
├── Scheduler.cs            # Runs all pending jobs, raises JobStateChanged
├── Executors.cs            # FastExecutor / SafeExecutor / RetryExecutor
├── Program.cs              # Entry point — wires everything together
└── Services/
   ├── MonitoringService.cs    # Prints event + job status
   ├── LoggerService.cs        # Prints timestamped log lines
   └── StatisticsService.cs    # Tracks started/completed/failed counts
```

## How it works

1. Jobs are enqueued into a `JobQueue` (a custom `Job[]`-backed structure that
   doubles its capacity when full — no `List<Job>` used).
2. `Scheduler.ExecuteAll()` iterates only the *pending* jobs (via the custom
   enumerator), runs each job's `Executor` delegate, and updates its `Status`.
3. Each state change (`JobStarted`, `JobCompleted`, `JobFailed`) raises the
   `JobStateChanged` event, which `MonitoringService`, `LoggerService`, and
   `StatisticsService` all subscribe to independently.
4. Three executor strategies are provided:
   - `FastExecutor` — short simulated work, fails if the job name contains `fail-fast`.
   - `SafeExecutor` — full simulated work, fails if the name contains `fail-safe`,
     catches and rethrows as a wrapped `JobException`.
   - `RetryExecutor` — retries up to 3 times; fails every attempt for `fail-retry`
     jobs, or simulates a configurable number of transient failures via
     `Job.RetryFailuresBeforeSuccess` before succeeding.

## Results

`Program.cs` enqueues one job per scenario (normal success, fail-fast, fail-safe,
a retry that recovers, and a retry that never recovers) so you can see all the
event/log/stat output paths in a single run. Final counts are printed at the end
via `StatisticsService.Print()`.
