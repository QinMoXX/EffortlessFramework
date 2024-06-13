using System;
using System.Collections.Generic;

namespace DataStructure
{
    public class PriorityQueue<TElement,TPriority> where TPriority :IComparable<TPriority>
    {
        private bool fix; //定容队列
        private int capacity;
        private ValueTuple<TElement, TPriority>[] _heap;
        private TElement[] _array;//顺序数组缓存

        private int _count;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="capacity">初始容积</param>
        /// <param name="fix">容积是否固定</param>
        public PriorityQueue(int capacity = 4,bool fix = false)
        {
            _heap = new ValueTuple<TElement, TPriority>[capacity];
            _count = 0;
            this.capacity = capacity;
            this.fix = fix;
            this._array = new TElement[_count];
        }

        public bool IsEmpty()
        {
            return _count == 0;
        }

        public int Count
        {
            get => _count;
        }

        public TElement[] List
        {
            get => this._array;
        }
        
        public void Clear()
        {
            _heap = new (TElement, TPriority)[capacity];
            _count = 0;
            this._array = Array.Empty<TElement>();
        }

        public TElement Dequeue()
        {
            var max = _heap[1];
            Exch(1,_count);
            _heap[_count--] = default;
            Sink(1);
            if (!fix && _count < _heap.Length / 4)
            {
                Array.Resize(ref _heap, _heap.Length / 2);
            }

            RefrashCache();
            return max.Item1;
        }

        //刷新数组缓存
        private void RefrashCache()
        {
            _array = new TElement[_count];

            for (int i = 0; i < _count; i++)
            {
                _array[i] = _heap[i].Item1;
            }
        }

        public void Enqueue(TElement item, TPriority priority)
        {
            if (!fix && _count >= _heap.Length - 1)
            {
                Array.Resize(ref _heap, _heap.Length * 2);
            }

            if (fix && _count >= _heap.Length - 1)
            {
                throw new Exception("Container is full");
            }

            _heap[++_count] = (item, priority);
            Swim(_count);
            RefrashCache();
        }

        /// <summary>
        /// 下潜
        /// </summary>
        /// <param name="k"></param>
        private void Sink(int k)
        {
            while (2*k<=_count)
            {
                int j = 2 * k;
                if (j<_count && Compare(j,j+1))
                {
                    j++;
                }
                if (!Compare(k, j))
                {
                    break;
                }
                Exch(k, j);
                k = j;
            }
        }
        
        /// <summary>
        /// 上浮
        /// </summary>
        /// <param name="k"></param>
        private void Swim(int k)
        {
            while (k>1 && Compare(k / 2, k))
            {
                Exch(k / 2, k);
                k /= 2;
            }
        }
        
        
        private bool Compare(int i,int j)
        {
            return _heap[i].Item2.CompareTo(_heap[j].Item2) > 0;
        }
        
        private void Exch(int v, int n)
        {
            (_heap[v], _heap[n]) = (_heap[n], _heap[v]);
        }
        
        public TElement Peek()
        {
            return _heap[1].Item1;
        }
    }
}