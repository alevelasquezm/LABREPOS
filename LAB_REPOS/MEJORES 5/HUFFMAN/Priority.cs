using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAB_REPOS.MEJORES_5.HUFFMAN
{
    public class Priority<T>
    {
        #region Definiciones
        IComparer<T> comparator;
        T[] heap;
        public int counter { get; private set; }
        public Priority() : this(null) { }
        public Priority(int capacity) : this(capacity, null) { }
        public Priority(IComparer<T> comparer) : this(16, comparer) { }
        #endregion
        public Priority(int capacity, IComparer<T> comparer)
        {
            this.comparator = (comparer == null) ? Comparer<T>.Default : comparer;
            this.heap = new T[capacity];
        }
        void up(int n)
        {
            var v = heap[n];
            for (var n2 = n / 2; n > 0 && comparator.Compare(v, heap[n2]) > 0; n = n2, n2 /= 2) heap[n] = heap[n2];
            heap[n] = v;
        }
        public void push(T v)
        {
            if (counter >= heap.Length) Array.Resize(ref heap, counter * 2);
            heap[counter] = v;
            up(counter++);
        }
        void down(int n)
        {
            var v = heap[n];
            for (var n2 = n * 2; n2 < counter; n = n2, n2 *= 2)
            {
                if (n2 + 1 < counter && comparator.Compare(heap[n2 + 1], heap[n2]) > 0) n2++;
                if (comparator.Compare(v, heap[n2]) >= 0) break;
                heap[n] = heap[n2];
            }
            heap[n] = v;
        }
        public T pop()
        {
            var v = top();
            heap[0] = heap[--counter];
            if (counter > 0) down(0);
            return v;
        }
        public T top()
        {
            if (counter > 0) return heap[0];
            throw new InvalidOperationException("No existen elementos en el Heap");
        }
    }
}
