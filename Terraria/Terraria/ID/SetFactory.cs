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
using System.Collections.Generic;

namespace Terraria.ID
{
    public class SetFactory
    {
        protected int _size;
        private Queue<int[]> _intBufferCache = new Queue<int[]>();
        private Queue<bool[]> _boolBufferCache = new Queue<bool[]>();
        private object _queueLock = new object();
        public SetFactory(int size)
        {
            this._size = size;
        }
        protected bool[] GetBoolBuffer()
        {
            bool[] result;
            lock (this._queueLock)
            {
                if (this._boolBufferCache.Count == 0)
                {
                    result = new bool[this._size];
                }
                else
                {
                    result = this._boolBufferCache.Dequeue();
                }
            }
            return result;
        }
        protected int[] GetIntBuffer()
        {
            int[] result;
            lock (this._queueLock)
            {
                if (this._boolBufferCache.Count == 0)
                {
                    result = new int[this._size];
                }
                else
                {
                    result = this._intBufferCache.Dequeue();
                }
            }
            return result;
        }
        public void Recycle<T>(T[] buffer)
        {
            lock (this._queueLock)
            {
                if (typeof(T).Equals(typeof(bool)))
                {
                    this._boolBufferCache.Enqueue((bool[])(object)buffer);
                }
                else if (typeof(T).Equals(typeof(int)))
                {
                    this._intBufferCache.Enqueue((int[])(object)buffer);
                }
            }
        }
        public bool[] CreateBoolSet(params int[] types)
        {
            return this.CreateBoolSet(false, types);
        }
        public bool[] CreateBoolSet(bool defaultState, params int[] types)
        {
            bool[] boolBuffer = this.GetBoolBuffer();
            for (int i = 0; i < boolBuffer.Length; i++)
            {
                boolBuffer[i] = defaultState;
            }
            for (int j = 0; j < types.Length; j++)
            {
                boolBuffer[types[j]] = !defaultState;
            }
            return boolBuffer;
        }
        public int[] CreateIntSet(int defaultState, params int[] inputs)
        {
            if (inputs.Length % 2 != 0)
            {
                throw new Exception("You have a bad length for inputs on CreateArraySet");
            }
            int[] intBuffer = this.GetIntBuffer();
            for (int i = 0; i < intBuffer.Length; i++)
            {
                intBuffer[i] = defaultState;
            }
            for (int j = 0; j < inputs.Length; j += 2)
            {
                intBuffer[inputs[j]] = inputs[j + 1];
            }
            return intBuffer;
        }
        public T[] CreateCustomSet<T>(T defaultState, params object[] inputs)
        {
            if (inputs.Length % 2 != 0)
            {
                throw new Exception("You have a bad length for inputs on CreateCustomSet");
            }

            T[] array = new T[this._size];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = defaultState;
            }

            for (int j = 0; j < inputs.Length; j += 2)
            {
                array[(int)(inputs[j])] = (T)(inputs[j + 1]);
            }
            return array;
        }
    }
}