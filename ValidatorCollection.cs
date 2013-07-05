using System.Collections.ObjectModel;

namespace DevWinformValidation
{

    public class ValidatorCollection : Collection<BaseValidator>
    {
    }

    /*/// <summary>
    ///        A strongly-typed collection of <see cref="BaseValidator"/> objects.
    /// </summary>
    [Serializable]
    public class ValidatorCollectionBak : IList, ICloneable
    {
        /* #region Interfaces
         /// <summary>
         ///        Supports type-safe iteration over a <see cref="ValidatorCollectionBak"/>.
         /// </summary>
         public interface BaseValidatorCollectionEnumerator
         {
             /// <summary>
             ///        Gets the current element in the collection.
             /// </summary>
             BaseValidator Current { get; }

             /// <summary>
             ///        Advances the enumerator to the next element in the collection.
             /// </summary>
             /// <exception cref="InvalidOperationException">
             ///        The collection was modified after the enumerator was created.
             /// </exception>
             /// <returns>
             ///        <c>true</c> if the enumerator was successfully advanced to the next element; 
             ///        <c>false</c> if the enumerator has passed the end of the collection.
             /// </returns>
             bool MoveNext();

             /// <summary>
             ///        Sets the enumerator to its initial position, before the first element in the collection.
             /// </summary>
             void Reset();
         }
         #endregion#1#

        private const int DEFAULT_CAPACITY = 16;

        #region Implementation (data)
        private BaseValidator[] m_array;
        private int m_count = 0;
        [NonSerialized]
        private int m_version = 0;
        #endregion

        #region Static Wrappers
        /// <summary>
        ///        Creates a synchronized (thread-safe) wrapper for a 
        ///     <c>ValidatorCollectionBak</c> instance.
        /// </summary>
        /// <returns>
        ///     An <c>ValidatorCollectionBak</c> wrapper that is synchronized (thread-safe).
        /// </returns>
        public static ValidatorCollectionBak Synchronized(ValidatorCollectionBak list)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            return new SyncValidatorCollection(list);
        }

        /// <summary>
        ///        Creates a read-only wrapper for a 
        ///     <c>ValidatorCollectionBak</c> instance.
        /// </summary>
        /// <returns>
        ///     An <c>ValidatorCollectionBak</c> wrapper that is read-only.
        /// </returns>
        public static ValidatorCollectionBak ReadOnly(ValidatorCollectionBak list)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            return new ReadOnlyValidatorCollection(list);
        }
        #endregion

        #region Construction
        /// <summary>
        ///        Initializes a new instance of the <c>ValidatorCollectionBak</c> class
        ///        that is empty and has the default initial capacity.
        /// </summary>
        public ValidatorCollectionBak()
        {
            m_array = new BaseValidator[DEFAULT_CAPACITY];
        }

        /// <summary>
        ///        Initializes a new instance of the <c>ValidatorCollectionBak</c> class
        ///        that has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">
        ///        The number of elements that the new <c>ValidatorCollectionBak</c> is initially capable of storing.
        ///    </param>
        public ValidatorCollectionBak(int capacity)
        {
            m_array = new BaseValidator[capacity];
        }

        /// <summary>
        ///        Initializes a new instance of the <c>ValidatorCollectionBak</c> class
        ///        that contains elements copied from the specified <c>ValidatorCollectionBak</c>.
        /// </summary>
        /// <param name="c">The <c>ValidatorCollectionBak</c> whose elements are copied to the new collection.</param>
        public ValidatorCollectionBak(ValidatorCollectionBak c)
        {
            m_array = new BaseValidator[c.Count];
            AddRange(c);
        }

        /// <summary>
        ///        Initializes a new instance of the <c>ValidatorCollectionBak</c> class
        ///        that contains elements copied from the specified <see cref="BaseValidator"/> array.
        /// </summary>
        /// <param name="a">The <see cref="BaseValidator"/> array whose elements are copied to the new list.</param>
        public ValidatorCollectionBak(BaseValidator[] a)
        {
            m_array = new BaseValidator[a.Length];
            AddRange(a);
        }
        #endregion

        #region Operations (type-safe ICollection)
        /// <summary>
        ///        Gets the number of elements actually contained in the <c>ValidatorCollectionBak</c>.
        /// </summary>
        public virtual int Count
        {
            get { return m_count; }
        }

        /// <summary>
        ///        Copies the entire <c>ValidatorCollectionBak</c> to a one-dimensional
        ///        string array.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="BaseValidator"/> array to copy to.</param>
        public virtual void CopyTo(BaseValidator[] array)
        {
            this.CopyTo(array, 0);
        }

        /// <summary>
        ///        Copies the entire <c>ValidatorCollectionBak</c> to a one-dimensional
        ///        <see cref="BaseValidator"/> array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="BaseValidator"/> array to copy to.</param>
        /// <param name="start">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        public virtual void CopyTo(BaseValidator[] array, int start)
        {
            if (m_count > array.GetUpperBound(0) + 1 - start)
                throw new System.ArgumentException("Destination array was not long enough.");

            Array.Copy(m_array, 0, array, start, m_count);
        }

        /// <summary>
        ///        Gets a value indicating whether access to the collection is synchronized (thread-safe).
        /// </summary>
        /// <returns>true if access to the ICollection is synchronized (thread-safe); otherwise, false.</returns>
        public virtual bool IsSynchronized
        {
            get { return m_array.IsSynchronized; }
        }

        /// <summary>
        ///        Gets an object that can be used to synchronize access to the collection.
        /// </summary>
        public virtual object SyncRoot
        {
            get { return m_array.SyncRoot; }
        }
        #endregion

        #region Operations (type-safe IList)
        /// <summary>
        ///        Gets or sets the <see cref="BaseValidator"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///        <para><paramref name="index"/> is less than zero</para>
        ///        <para>-or-</para>
        ///        <para><paramref name="index"/> is equal to or greater than <see cref="ValidatorCollectionBak.Count"/>.</para>
        /// </exception>
        public virtual BaseValidator this[int index]
        {
            get
            {
                ValidateIndex(index); // throws
                return m_array[index];
            }
            set
            {
                ValidateIndex(index); // throws
                ++m_version;
                m_array[index] = value;
            }
        }

        /// <summary>
        ///        Adds a <see cref="BaseValidator"/> to the end of the <c>ValidatorCollectionBak</c>.
        /// </summary>
        /// <param name="item">The <see cref="BaseValidator"/> to be added to the end of the <c>ValidatorCollectionBak</c>.</param>
        /// <returns>The index at which the value has been added.</returns>
        public virtual int Add(BaseValidator item)
        {
            if (m_count == m_array.Length)
                EnsureCapacity(m_count + 1);

            m_array[m_count] = item;
            m_version++;

            return m_count++;
        }

        /// <summary>
        ///        Removes all elements from the <c>ValidatorCollectionBak</c>.
        /// </summary>
        public virtual void Clear()
        {
            ++m_version;
            m_array = new BaseValidator[DEFAULT_CAPACITY];
            m_count = 0;
        }

        /// <summary>
        ///        Creates a shallow copy of the <see cref="ValidatorCollectionBak"/>.
        /// </summary>
        public virtual object Clone()
        {
            ValidatorCollectionBak newColl = new ValidatorCollectionBak(m_count);
            Array.Copy(m_array, 0, newColl.m_array, 0, m_count);
            newColl.m_count = m_count;
            newColl.m_version = m_version;

            return newColl;
        }

        /// <summary>
        ///        Determines whether a given <see cref="BaseValidator"/> is in the <c>ValidatorCollectionBak</c>.
        /// </summary>
        /// <param name="item">The <see cref="BaseValidator"/> to check for.</param>
        /// <returns><c>true</c> if <paramref name="item"/> is found in the <c>ValidatorCollectionBak</c>; otherwise, <c>false</c>.</returns>
        public virtual bool Contains(BaseValidator item)
        {
            for (int i = 0; i != m_count; ++i)
                if (m_array[i].Equals(item))
                    return true;
            return false;
        }

        /// <summary>
        ///        Returns the zero-based index of the first occurrence of a <see cref="BaseValidator"/>
        ///        in the <c>ValidatorCollectionBak</c>.
        /// </summary>
        /// <param name="item">The <see cref="BaseValidator"/> to locate in the <c>ValidatorCollectionBak</c>.</param>
        /// <returns>
        ///        The zero-based index of the first occurrence of <paramref name="item"/> 
        ///        in the entire <c>ValidatorCollectionBak</c>, if found; otherwise, -1.
        ///    </returns>
        public virtual int IndexOf(BaseValidator item)
        {
            for (int i = 0; i != m_count; ++i)
                if (m_array[i].Equals(item))
                    return i;
            return -1;
        }

        /// <summary>
        ///        Inserts an element into the <c>ValidatorCollectionBak</c> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The <see cref="BaseValidator"/> to insert.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///        <para><paramref name="index"/> is less than zero</para>
        ///        <para>-or-</para>
        ///        <para><paramref name="index"/> is equal to or greater than <see cref="ValidatorCollectionBak.Count"/>.</para>
        /// </exception>
        public virtual void Insert(int index, BaseValidator item)
        {
            ValidateIndex(index, true); // throws

            if (m_count == m_array.Length)
                EnsureCapacity(m_count + 1);

            if (index < m_count)
            {
                Array.Copy(m_array, index, m_array, index + 1, m_count - index);
            }

            m_array[index] = item;
            m_count++;
            m_version++;
        }

        /// <summary>
        ///        Removes the first occurrence of a specific <see cref="BaseValidator"/> from the <c>ValidatorCollectionBak</c>.
        /// </summary>
        /// <param name="item">The <see cref="BaseValidator"/> to remove from the <c>ValidatorCollectionBak</c>.</param>
        /// <exception cref="ArgumentException">
        ///        The specified <see cref="BaseValidator"/> was not found in the <c>ValidatorCollectionBak</c>.
        /// </exception>
        public virtual void Remove(BaseValidator item)
        {
            int i = IndexOf(item);
            if (i < 0)
                throw new System.ArgumentException("Cannot remove the specified item because it was not found in the specified Collection.");

            ++m_version;
            RemoveAt(i);
        }

        /// <summary>
        ///        Removes the element at the specified index of the <c>ValidatorCollectionBak</c>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///        <para><paramref name="index"/> is less than zero</para>
        ///        <para>-or-</para>
        ///        <para><paramref name="index"/> is equal to or greater than <see cref="ValidatorCollectionBak.Count"/>.</para>
        /// </exception>
        public virtual void RemoveAt(int index)
        {
            ValidateIndex(index); // throws

            m_count--;

            if (index < m_count)
            {
                Array.Copy(m_array, index + 1, m_array, index, m_count - index);
            }

            // We can't set the deleted entry equal to null, because it might be a value type.
            // Instead, we'll create an empty single-element array of the right type and copy it 
            // over the entry we want to erase.
            BaseValidator[] temp = new BaseValidator[1];
            Array.Copy(temp, 0, m_array, m_count, 1);
            m_version++;
        }

        /// <summary>
        ///        Gets a value indicating whether the collection has a fixed size.
        /// </summary>
        /// <value>true if the collection has a fixed size; otherwise, false. The default is false</value>
        public virtual bool IsFixedSize
        {
            get { return false; }
        }

        /// <summary>
        ///        gets a value indicating whether the IList is read-only.
        /// </summary>
        /// <value>true if the collection is read-only; otherwise, false. The default is false</value>
        public virtual bool IsReadOnly
        {
            get { return false; }
        }
        #endregion

        #region Operations (type-safe IEnumerable)

        /// <summary>
        ///        Returns an enumerator that can iterate through the <c>ValidatorCollectionBak</c>.
        /// </summary>
        /// <returns>An <see cref=""/> for the entire <c>ValidatorCollectionBak</c>.</returns>
        public virtual IEnumerator GetEnumerator()
        {
            //return new Enumerator(this);
            int i = 0;
            while (true)
            {
                if (i == Count)
                    yield break;
                yield return m_array[i++];

            }
        }
        #endregion

        #region Public helpers (just to mimic some nice features of ArrayList)

        /// <summary>
        ///        Gets or sets the number of elements the <c>ValidatorCollectionBak</c> can contain.
        /// </summary>
        public virtual int Capacity
        {
            get { return m_array.Length; }

            set
            {
                if (value < m_count)
                    value = m_count;

                if (value != m_array.Length)
                {
                    if (value > 0)
                    {
                        BaseValidator[] temp = new BaseValidator[value];
                        Array.Copy(m_array, temp, m_count);
                        m_array = temp;
                    }
                    else
                    {
                        m_array = new BaseValidator[DEFAULT_CAPACITY];
                    }
                }
            }
        }

        /// <summary>
        ///        Adds the elements of another <c>ValidatorCollectionBak</c> to the current <c>ValidatorCollectionBak</c>.
        /// </summary>
        /// <param name="x">The <c>ValidatorCollectionBak</c> whose elements should be added to the end of the current <c>ValidatorCollectionBak</c>.</param>
        /// <returns>The new <see cref="ValidatorCollectionBak.Count"/> of the <c>ValidatorCollectionBak</c>.</returns>
        public virtual int AddRange(ValidatorCollectionBak x)
        {
            if (m_count + x.Count >= m_array.Length)
                EnsureCapacity(m_count + x.Count);

            Array.Copy(x.m_array, 0, m_array, m_count, x.Count);
            m_count += x.Count;
            m_version++;

            return m_count;
        }

        /// <summary>
        ///        Adds the elements of a <see cref="BaseValidator"/> array to the current <c>ValidatorCollectionBak</c>.
        /// </summary>
        /// <param name="x">The <see cref="BaseValidator"/> array whose elements should be added to the end of the <c>ValidatorCollectionBak</c>.</param>
        /// <returns>The new <see cref="ValidatorCollectionBak.Count"/> of the <c>ValidatorCollectionBak</c>.</returns>
        public virtual int AddRange(BaseValidator[] x)
        {
            if (m_count + x.Length >= m_array.Length)
                EnsureCapacity(m_count + x.Length);

            Array.Copy(x, 0, m_array, m_count, x.Length);
            m_count += x.Length;
            m_version++;

            return m_count;
        }

        /// <summary>
        ///        Sets the capacity to the actual number of elements.
        /// </summary>
        public virtual void TrimToSize()
        {
            this.Capacity = m_count;
        }

        #endregion

        #region Implementation (helpers)

        /// <exception cref="ArgumentOutOfRangeException">
        ///        <para><paramref name="index"/> is less than zero</para>
        ///        <para>-or-</para>
        ///        <para><paramref name="index"/> is equal to or greater than <see cref="ValidatorCollectionBak.Count"/>.</para>
        /// </exception>
        private void ValidateIndex(int i)
        {
            ValidateIndex(i, false);
        }

        /// <exception cref="ArgumentOutOfRangeException">
        ///        <para><paramref name="index"/> is less than zero</para>
        ///        <para>-or-</para>
        ///        <para><paramref name="index"/> is equal to or greater than <see cref="ValidatorCollectionBak.Count"/>.</para>
        /// </exception>
        private void ValidateIndex(int i, bool allowEqualEnd)
        {
            int max = (allowEqualEnd) ? (m_count) : (m_count - 1);
            if (i < 0 || i > max)
                throw new System.ArgumentOutOfRangeException("Index was out of range.  Must be non-negative and less than the size of the collection.", (object)i, "Specified argument was out of the range of valid values.");
        }

        private void EnsureCapacity(int min)
        {
            int newCapacity = ((m_array.Length == 0) ? DEFAULT_CAPACITY : m_array.Length * 2);
            if (newCapacity < min)
                newCapacity = min;

            this.Capacity = newCapacity;
        }

        #endregion

        #region Implementation (ICollection)

        void ICollection.CopyTo(Array array, int start)
        {
            this.CopyTo((BaseValidator[])array, start);
        }

        #endregion

        #region Implementation (IList)

        object IList.this[int i]
        {
            get { return (object)this[i]; }
            set { this[i] = (BaseValidator)value; }
        }

        int IList.Add(object x)
        {
            return this.Add((BaseValidator)x);
        }

        bool IList.Contains(object x)
        {
            return this.Contains((BaseValidator)x);
        }

        int IList.IndexOf(object x)
        {
            return this.IndexOf((BaseValidator)x);
        }

        void IList.Insert(int pos, object x)
        {
            this.Insert(pos, (BaseValidator)x);
        }

        void IList.Remove(object x)
        {
            this.Remove((BaseValidator)x);
        }

        void IList.RemoveAt(int pos)
        {
            this.RemoveAt(pos);
        }

        #endregion

        #region Implementation (IEnumerable)

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region Nested enumerator class
        /// <summary>
        ///        Supports simple iteration over a <see cref="ValidatorCollectionBak"/>.
        /// </summary>
        /*private class Enumerator : IEnumerator<BaseValidator>, BaseValidatorCollectionEnumerator
        {
            #region Implementation (data)

            private ValidatorCollectionBak m_collection;
            private int m_index;
            private int m_version;

            #endregion

            #region Construction

            /// <summary>
            ///        Initializes a new instance of the <c>Enumerator</c> class.
            /// </summary>
            /// <param name="tc"></param>
            internal Enumerator(ValidatorCollectionBak tc)
            {
                m_collection = tc;
                m_index = -1;
                m_version = tc.m_version;
            }

            #endregion

            #region Operations (type-safe IEnumerator)

            /// <summary>
            ///        Gets the current element in the collection.
            /// </summary>
            public BaseValidator Current
            {
                get { return m_collection[m_index]; }
            }

            /// <summary>
            ///        Advances the enumerator to the next element in the collection.
            /// </summary>
            /// <exception cref="InvalidOperationException">
            ///        The collection was modified after the enumerator was created.
            /// </exception>
            /// <returns>
            ///        <c>true</c> if the enumerator was successfully advanced to the next element; 
            ///        <c>false</c> if the enumerator has passed the end of the collection.
            /// </returns>
            public bool MoveNext()
            {
                if (m_version != m_collection.m_version)
                    throw new System.InvalidOperationException("Collection was modified; enumeration operation may not execute.");

                ++m_index;
                return (m_index < m_collection.Count) ? true : false;
            }

            /// <summary>
            ///        Sets the enumerator to its initial position, before the first element in the collection.
            /// </summary>
            public void Reset()
            {
                m_index = -1;
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            #endregion

            #region Implementation (IEnumerator)

           

            #endregion

            public void Dispose()
            {
                m_collection = null;
            }
        }#1#
        #endregion

        #region Nested Syncronized Wrapper class
        private class SyncValidatorCollection : ValidatorCollectionBak
        {
            #region Implementation (data)
            private ValidatorCollectionBak m_collection;
            private object m_root;
            #endregion

            #region Construction
            internal SyncValidatorCollection(ValidatorCollectionBak list)
            {
                m_root = list.SyncRoot;
                m_collection = list;
            }
            #endregion

            #region Type-safe ICollection
            public override void CopyTo(BaseValidator[] array)
            {
                lock (this.m_root)
                    m_collection.CopyTo(array);
            }

            public override void CopyTo(BaseValidator[] array, int start)
            {
                lock (this.m_root)
                    m_collection.CopyTo(array, start);
            }
            public override int Count
            {
                get
                {
                    lock (this.m_root)
                        return m_collection.Count;
                }
            }

            public override bool IsSynchronized
            {
                get { return true; }
            }

            public override object SyncRoot
            {
                get { return this.m_root; }
            }
            #endregion

            #region Type-safe IList
            public override BaseValidator this[int i]
            {
                get
                {
                    lock (this.m_root)
                        return m_collection[i];
                }
                set
                {
                    lock (this.m_root)
                        m_collection[i] = value;
                }
            }

            public override int Add(BaseValidator x)
            {
                lock (this.m_root)
                    return m_collection.Add(x);
            }

            public override void Clear()
            {
                lock (this.m_root)
                    m_collection.Clear();
            }

            public override bool Contains(BaseValidator x)
            {
                lock (this.m_root)
                    return m_collection.Contains(x);
            }

            public override int IndexOf(BaseValidator x)
            {
                lock (this.m_root)
                    return m_collection.IndexOf(x);
            }

            public override void Insert(int pos, BaseValidator x)
            {
                lock (this.m_root)
                    m_collection.Insert(pos, x);
            }

            public override void Remove(BaseValidator x)
            {
                lock (this.m_root)
                    m_collection.Remove(x);
            }

            public override void RemoveAt(int pos)
            {
                lock (this.m_root)
                    m_collection.RemoveAt(pos);
            }

            public override bool IsFixedSize
            {
                get { return m_collection.IsFixedSize; }
            }

            public override bool IsReadOnly
            {
                get { return m_collection.IsReadOnly; }
            }
            #endregion

            #region Type-safe IEnumerable
            public override IEnumerator GetEnumerator()
            {
                lock (m_root)
                    return m_collection.GetEnumerator();
            }
            #endregion

            #region Public Helpers
            // (just to mimic some nice features of ArrayList)
            public override int Capacity
            {
                get
                {
                    lock (this.m_root)
                        return m_collection.Capacity;
                }

                set
                {
                    lock (this.m_root)
                        m_collection.Capacity = value;
                }
            }

            public override int AddRange(ValidatorCollectionBak x)
            {
                lock (this.m_root)
                    return m_collection.AddRange(x);
            }

            public override int AddRange(BaseValidator[] x)
            {
                lock (this.m_root)
                    return m_collection.AddRange(x);
            }
            #endregion
        }
        #endregion

        #region Nested Read Only Wrapper class
        private class ReadOnlyValidatorCollection : ValidatorCollectionBak
        {
            #region Implementation (data)
            private ValidatorCollectionBak m_collection;
            #endregion

            #region Construction
            internal ReadOnlyValidatorCollection(ValidatorCollectionBak list)
            {
                m_collection = list;
            }
            #endregion

            #region Type-safe ICollection
            public override void CopyTo(BaseValidator[] array)
            {
                m_collection.CopyTo(array);
            }

            public override void CopyTo(BaseValidator[] array, int start)
            {
                m_collection.CopyTo(array, start);
            }
            public override int Count
            {
                get { return m_collection.Count; }
            }

            public override bool IsSynchronized
            {
                get { return m_collection.IsSynchronized; }
            }

            public override object SyncRoot
            {
                get { return this.m_collection.SyncRoot; }
            }
            #endregion

            #region Type-safe IList
            public override BaseValidator this[int i]
            {
                get { return m_collection[i]; }
                set { throw new NotSupportedException("This is a Read Only Collection and can not be modified"); }
            }

            public override int Add(BaseValidator x)
            {
                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
            }

            public override void Clear()
            {
                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
            }

            public override bool Contains(BaseValidator x)
            {
                return m_collection.Contains(x);
            }

            public override int IndexOf(BaseValidator x)
            {
                return m_collection.IndexOf(x);
            }

            public override void Insert(int pos, BaseValidator x)
            {
                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
            }

            public override void Remove(BaseValidator x)
            {
                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
            }

            public override void RemoveAt(int pos)
            {
                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
            }

            public override bool IsFixedSize
            {
                get { return true; }
            }

            public override bool IsReadOnly
            {
                get { return true; }
            }
            #endregion

            #region Type-safe IEnumerable
            public override IEnumerator GetEnumerator()
            {
                return m_collection.GetEnumerator();
            }
            #endregion

            #region Public Helpers
            // (just to mimic some nice features of ArrayList)
            public override int Capacity
            {
                get { return m_collection.Capacity; }

                set { throw new NotSupportedException("This is a Read Only Collection and can not be modified"); }
            }

            public override int AddRange(ValidatorCollectionBak x)
            {
                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
            }

            public override int AddRange(BaseValidator[] x)
            {
                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
            }
            #endregion
        }
        #endregion
    }*/
}