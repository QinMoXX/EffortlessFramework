using System;

namespace AOT.Framework
{
    public class SortList<TKey,TValue>where TKey:IComparable<TKey>
    {
        private (TKey,TValue)[] m_array;
        private int m_count;
        
        public SortList()
        {
            m_array = new ValueTuple<TKey,TValue>[0];
            m_count = 0;
        }

        public (TKey,TValue)[] List
        {
            get => m_array;
        }

        public int Count
        {
            get => m_count;
        }

        public (TKey, TValue) this[int i]
        {
            get => m_array[i];
        }

        public void Add(TKey key, TValue value)
        {
            ValueTuple<TKey, TValue> newTuple = new (key, value);
            //双指针
            Int32 beforPoint = 0;
            Int32 afterPoint = m_count;
            //扩容
            (TKey, TValue)[] newArray = new (TKey, TValue)[m_count+1];
            Array.Copy(m_array,newArray,m_count);
            while (beforPoint < afterPoint)
            {
                if (newArray[beforPoint].Item1.CompareTo(key) > 0 )
                {
                    break;
                }
                beforPoint++;
            }

            //数组后移
            while (afterPoint>beforPoint)
            {
                newArray[afterPoint] = newArray[afterPoint - 1];
                afterPoint--;
            }

            newArray[beforPoint] = newTuple;
            m_array = newArray;
            m_count++;
        }
        
        

        public void Remove(TValue value)
        {
            int index = 0;
            while (index < m_count)
            {
                if (m_array[index].Item2.Equals(value))
                {
                    break;
                }

                index++;
            }

            if (index == m_count)
            {
                throw new GameFrameworkException(Utility.String.Format("list is not have {0} type date.", value.ToString()));
            }
            (TKey, TValue)[] newArray = new (TKey, TValue)[--m_count];
            Array.Copy(m_array,newArray,index+1);
            while (index < m_count)
            {
                newArray[index] = m_array[index + 1]; 
                index++;
            }
            m_array = newArray;
        }

        public void Clear()
        {
            m_array = new ValueTuple<TKey,TValue>[0];
            m_count = 0;
        }
    }
}