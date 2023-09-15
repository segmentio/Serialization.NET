using System.Collections;
using System.Collections.Generic;

namespace Segment.Serialization
{
    public class ConcurrentList<T> : IList<T>
    {
        protected readonly IList<T> InternalList;

        public IEnumerator<T> GetEnumerator()
        {
            return SafeCopy().GetEnumerator();
        }

        protected static readonly object s_lock = new object();

        public ConcurrentList()
        {
            InternalList = new List<T>();
        }

        public ConcurrentList(IList<T> content)
        {
            InternalList = content;
        }

        public List<T> SafeCopy()
        {
            List<T> newList;

            lock (s_lock)
            {
                newList = new List<T>(InternalList);
            }

            return newList;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(T item) => InternalList.Add(item);

        public void Clear() => InternalList.Clear();

        public bool Contains(T item) => InternalList.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => InternalList.CopyTo(array, arrayIndex);

        public bool Remove(T item) => InternalList.Remove(item);

        public int Count => InternalList.Count;

        public bool IsReadOnly => false;

        public int IndexOf(T item) => InternalList.IndexOf(item);

        public void Insert(int index, T item) => InternalList.Insert(index, item);

        public void RemoveAt(int index) => InternalList.RemoveAt(index);

        public T this[int index]
        {
            get => InternalList[index];
            set => InternalList[index] = value;
        }
    }
}
