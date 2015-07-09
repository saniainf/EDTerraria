/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using System;

namespace Terraria.DataStructures
{
    public class DoubleStack<T1>
    {
        private T1[][] _segmentList;
        private readonly int _segmentSize;
        private int _segmentCount;
        private readonly int _segmentShiftPosition;
        private int _start;
        private int _end;
        private int _size;
        private int _last;

        public int Count
        {
            get { return _size; }
        }

        public DoubleStack(int segmentSize = 1024, int initialSize = 0)
        {
            if (segmentSize < 16)
                segmentSize = 16;
            _start = segmentSize / 2;
            _end = _start;
            _size = 0;
            _segmentShiftPosition = segmentSize + _start;
            initialSize += _start;
            int length = initialSize / segmentSize + 1;
            _segmentList = new T1[length][];
            for (int index = 0; index < length; ++index)
                _segmentList[index] = new T1[segmentSize];
            _segmentSize = segmentSize;
            _segmentCount = length;
            _last = _segmentSize * _segmentCount - 1;
        }

        public void PushFront(T1 front)
        {
            if (_start == 0)
            {
                T1[][] objArray = new T1[_segmentCount + 1][];
                for (int index = 0; index < _segmentCount; ++index)
                    objArray[index + 1] = _segmentList[index];
                objArray[0] = new T1[_segmentSize];
                _segmentList = objArray;
                ++_segmentCount;
                _start += _segmentSize;
                _end += _segmentSize;
                _last += _segmentSize;
            }
            --_start;
            _segmentList[_start / _segmentSize][_start % _segmentSize] = front;
            ++_size;
        }

        public T1 PopFront()
        {
            if (_size == 0)
                throw new InvalidOperationException("The DoubleStack is empty.");
            T1[] objArray1 = _segmentList[_start / _segmentSize];
            int index1 = _start % _segmentSize;
            T1 obj = objArray1[index1];
            objArray1[index1] = default(T1);
            ++_start;
            --_size;

            if (_start >= _segmentShiftPosition)
            {
                T1[] objArray2 = _segmentList[0];
                for (int index2 = 0; index2 < _segmentCount - 1; ++index2)
                    _segmentList[index2] = _segmentList[index2 + 1];
                _segmentList[_segmentCount - 1] = objArray2;
                _start -= _segmentSize;
                _end -= _segmentSize;
            }

            if (_size == 0)
            {
                _start = _segmentSize / 2;
                _end = _start;
            }

            return obj;
        }

        public T1 PeekFront()
        {
            if (_size == 0)
                throw new InvalidOperationException("The DoubleStack is empty.");
            return this._segmentList[_start / _segmentSize][_start % _segmentSize];
        }

        public void PushBack(T1 back)
        {
            if (_end == _last)
            {
                T1[][] objArray = new T1[_segmentCount + 1][];
                for (int index = 0; index < _segmentCount; ++index)
                    objArray[index] = _segmentList[index];
                objArray[_segmentCount] = new T1[_segmentSize];
                ++_segmentCount;
                _segmentList = objArray;
                _last += _segmentSize;
            }
            _segmentList[_end / _segmentSize][_end % _segmentSize] = back;
            ++_end;
            ++_size;
        }

        public T1 PopBack()
        {
            if (_size == 0)
                throw new InvalidOperationException("The DoubleStack is empty.");
            T1[] objArray = _segmentList[_end / _segmentSize];
            int index = _end % _segmentSize;
            T1 obj = objArray[index];
            objArray[index] = default(T1);
            --_end;
            --_size;

            if (_size == 0)
            {
                _start = _segmentSize / 2;
                _end = _start;
            }

            return obj;
        }

        public T1 PeekBack()
        {
            if (_size == 0)
                throw new InvalidOperationException("The DoubleStack is empty.");
            return this._segmentList[_end / _segmentSize][_end % _segmentSize];
        }

        public void Clear(bool quickClear = false)
        {
            if (!quickClear)
            {
                for (int index = 0; index < _segmentCount; ++index)
                    Array.Clear(_segmentList[index], 0, _segmentSize);
            }

            _start = _segmentSize / 2;
            _end = _start;
            _size = 0;
        }
    }
}
