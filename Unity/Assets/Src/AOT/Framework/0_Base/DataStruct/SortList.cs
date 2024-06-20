using System;
using System.Collections;
using System.Collections.Generic;

namespace DataStructure
{
    public class SortList<TKey,TValue>where TKey:IComparable<TKey>
    {
        private (TKey,TValue)[] m_array;
        private int m_count;
        
        public SortList()
        {
            m_array = new ValueTuple<TKey,TValue>[4];
            m_count = 0;
        }

        public void Add(TKey key, TValue value)
        {
            ValueTuple<TKey, TValue> newTuple = new (key, value);
            //双指针
            Int32 beforPoint = 0;
            Int32 afterPoint = m_count;
            while (beforPoint < afterPoint && beforPoint<= m_count)
            {
                if (m_array[beforPoint].Item1.CompareTo(key) <= 0 )
                {
                    
                }

                beforPoint++;
            }
        }

        public void Remove(TValue value)
        {
            
        }
    }
}