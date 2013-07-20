// The MIT License (MIT)
//
// Copyright (c) 2013 Gareth Higgins
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.ComponentModel;

namespace LittleMan {
    public enum ProgramType {
        Compiler,
        Computer
    }

    public class Memory : IBindingList {
        ushort[] _memory;

        public int Length { get{ return _memory.Length;}}

        public Memory() {
        }

        public Memory(int length) {
            _memory = new ushort[length];
        }

        public Memory(ushort[] arrayIn) {
            var handler = ListChanged;
            if (handler != null)
                ListChanged(this, new ListChangedEventArgs(ListChangedType.Reset,-1));
            _memory = arrayIn;
        }

        public void Set(ushort[] arrayIn) {
            var handler = ListChanged;
            if (handler != null)
                ListChanged(this, new ListChangedEventArgs(ListChangedType.Reset, -1));
            _memory = arrayIn;
        }

        public ushort[] Get() {
            return _memory;
        }
        unsafe public short[] GetShort() {
            short[] shortArray = new short[_memory.Length];
            fixed (ushort* sbytePointer = _memory) {
                fixed (short* bytePointer = shortArray) {
                    ushort* read = sbytePointer;
                    ushort* write = (ushort*)bytePointer;
                    for (int i = 0; i < _memory.Length; i++) {
                        *write++ = *read++;
                    }
                }
            }
            return shortArray;
        }
        
        public ushort this[int index] {
            get {
                var handler = ListChanged; 
                if (handler != null)
                    ListChanged(this, new ListChangedEventArgs(ListChangedType.ItemChanged, index));
                return _memory[index];
            }
            set {
                var handler = ListChanged;
                if (handler != null)
                    ListChanged(this, new ListChangedEventArgs(ListChangedType.ItemChanged, index));
                _memory[index] = value; }
        }

        #region IBindingList Members

        public void AddIndex(PropertyDescriptor property) {
            throw new NotSupportedException("Add Index");
        }

        public object AddNew() {
            throw new NotSupportedException("Add New");
        }

        public bool AllowEdit {
            get { return true; }
        }

        public bool AllowNew {
            get { throw new NotImplementedException(); }
        }

        public bool AllowRemove {
            get { return false; }
        }

        public void ApplySort(PropertyDescriptor property, ListSortDirection direction) {
            throw new NotSupportedException("Sorting, query");
        }

        public int Find(PropertyDescriptor property, object key) {
            throw new NotSupportedException("Find");
        }

        public bool IsSorted {
            get { return false; }
        }

        public event ListChangedEventHandler ListChanged;

        public void RemoveIndex(PropertyDescriptor property) {
            throw new NotSupportedException("RemoveIndex");
        }

        public void RemoveSort() {
            throw new NotSupportedException("RemoveSort");
        }

        public ListSortDirection SortDirection {
            get { throw new NotSupportedException("Sort Direction"); }
        }

        public PropertyDescriptor SortProperty {
            get { throw new NotSupportedException("Sort Property"); }
        }

        public bool SupportsChangeNotification {
            get { return true; }
        }

        public bool SupportsSearching {
            get { return false; }
        }

        public bool SupportsSorting {
            get { return false; }
        }

        #endregion

        #region IList Members

        public int Add(object value) {
            throw new NotSupportedException("Add");
        }

        public void Clear() {
            throw new NotSupportedException("Clear");
        }

        public bool Contains(object value) {
            throw new NotSupportedException("Contains");
        }

        public int IndexOf(object value) {
            return _memory[(int)value];
        }

        public void Insert(int index, object value) {
            _memory[index] = (ushort)value;
        }

        public bool IsFixedSize {
            get { return true; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public void Remove(object value) {
            throw new NotSupportedException("Remove");
        }

        public void RemoveAt(int index) {
            throw new NotSupportedException("RemoveAt");
        }

        object System.Collections.IList.this[int index] {
            get {
                var handler = ListChanged;
                if (handler != null)
                    ListChanged(this, new ListChangedEventArgs(ListChangedType.ItemChanged, index));
                return _memory[index];
            }
            set {
                var handler = ListChanged;
                if (handler != null)
                    ListChanged(this, new ListChangedEventArgs(ListChangedType.ItemChanged, index));
                _memory[index] = (ushort)value;
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index) {
            Array.Copy(_memory, 0, array, index, Length);
        }

        public int Count {
            get { return Length; }
        }

        public bool IsSynchronized {
            get { return false; }
        }

        public object SyncRoot {
            get { throw new NotSupportedException("Sync Root"); }
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator() {
            throw new NotImplementedException();
        }

        #endregion
    }
}
