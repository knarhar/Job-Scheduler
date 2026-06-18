using System.Collections;

namespace Job_Scheduler
{
    internal class JobQueue : IEnumerable
    {
        private Job[] _items;
        private int _count;

        public JobQueue(int capacity = 4)
        {
            if (capacity <= 0)
            {
                capacity = 4;
            }
            _items = new Job[capacity];
            _count = 0;
        }

        public int Count => _count;
        public Job[] Items => _items;
        public bool IsEmpty => _count == 0;
        public bool IsFull => _count == _items.Length;

        public void Enqueue(Job job)
        {
            if (job == null)
            {
                throw new ArgumentNullException(nameof(job));
            }
            if (IsFull)
            {
                Grow();
            }
            _items[_count] = job;
            _count++;
        }

        private void Grow()
        {
            int newCapacity = _items.Length * 2;
            Job[] newArray = new Job[newCapacity];
            Array.Copy(_items, newArray, _count);
            _items = newArray;
        }

        public IEnumerator GetEnumerator()
        {
            return new JobEnumerator(this);
        }
    }
}