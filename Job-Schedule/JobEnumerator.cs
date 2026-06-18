using System.Collections;

namespace Job_Scheduler
{
    internal class JobEnumerator : IEnumerator
    {
        private readonly JobQueue _queue;
        private int _index = -1;
        private bool _onValidItem = false;

        public JobEnumerator(JobQueue queue)
        {
            _queue = queue;
        }

        public object Current
        {
            get
            {
                if (!_onValidItem)
                {
                    throw new InvalidOperationException(
                        "Enumerator is not positioned on a valid element.");
                }
                return _queue.Items[_index];
            }
        }

        public bool MoveNext()
        {
            while (++_index < _queue.Count)
            {
                if (_queue.Items[_index].Status == JobStatus.Pending)
                {
                    _onValidItem = true;
                    return true;
                }
            }
            _onValidItem = false;
            return false;
        }

        public void Reset()
        {
            _index = -1;
            _onValidItem = false;
        }
    }
}