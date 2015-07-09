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
    public class TileObjectPreviewData
    {
        public const int None = 0;
        public const int ValidSpot = 1;
        public const int InvalidSpot = 2;
        private ushort _type;
        private short _style;
        private int _alternate;
        private int _random;
        private bool _active;
        private Point16 _size;
        private Point16 _coordinates;
        private Point16 _objectStart;
        private int[,] _data;
        private Point16 _dataSize;
        private float _percentValid;
        public static TileObjectPreviewData placementCache;
        public static TileObjectPreviewData randomCache;

        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        public ushort Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public short Style
        {
            get { return _style; }
            set { _style = value; }
        }

        public int Alternate
        {
            get { return _alternate; }
            set { _alternate = value; }
        }

        public int Random
        {
            get { return _random; }
            set { _random = value; }
        }

        public Point16 Size
        {
            get { return _size; }
            set
            {
                if ((int)value.X <= 0 || (int)value.Y <= 0)
                    throw new FormatException("PlacementData.Size was set to a negative value.");
                if ((int)value.X > (int)_dataSize.X || (int)value.Y > (int)_dataSize.Y)
                {
                    int X = (int)value.X > (int)_dataSize.X ? (int)value.X : (int)_dataSize.X;
                    int Y = (int)value.Y > (int)_dataSize.Y ? (int)value.Y : (int)_dataSize.Y;
                    int[,] numArray = new int[X, Y];
                    if (_data != null)
                    {
                        for (int index1 = 0; index1 < (int)_dataSize.X; ++index1)
                        {
                            for (int index2 = 0; index2 < (int)_dataSize.Y; ++index2)
                                numArray[index1, index2] = _data[index1, index2];
                        }
                    }
                    _data = numArray;
                    _dataSize = new Point16(X, Y);
                }
                _size = value;
            }
        }

        public Point16 Coordinates
        {
            get { return _coordinates; }
            set { _coordinates = value; }
        }

        public Point16 ObjectStart
        {
            get { return _objectStart; }
            set { _objectStart = value; }
        }

        public int this[int x, int y]
        {
            get
            {
                if (x < 0 || y < 0 || (x >= (int)_size.X || y >= (int)_size.Y))
                    throw new IndexOutOfRangeException();
                return _data[x, y];
            }
            set
            {
                if (x < 0 || y < 0 || (x >= (int)_size.X || y >= (int)_size.Y))
                    throw new IndexOutOfRangeException();
                _data[x, y] = value;
            }
        }

        public void Reset()
        {
            _active = false;
            _size = Point16.Zero;
            _coordinates = Point16.Zero;
            _objectStart = Point16.Zero;
            _percentValid = 0.0f;
            _type = (ushort)0;
            _style = (short)0;
            _alternate = -1;
            _random = -1;
            if (_data != null)
                 Array.Clear((Array)_data, 0, (int)_dataSize.X * (int)_dataSize.Y);
        }

        public void CopyFrom(TileObjectPreviewData copy)
        {
            _type = copy._type;
            _style = copy._style;
            _alternate = copy._alternate;
            _random = copy._random;
            _active = copy._active;
            _size = copy._size;
            _coordinates = copy._coordinates;
            _objectStart = copy._objectStart;
            _percentValid = copy._percentValid;

            if (_data == null)
            {
                _data = new int[(int)copy._dataSize.X, (int)copy._dataSize.Y];
                _dataSize = copy._dataSize;
            }
            else
                Array.Clear(_data, 0, _data.Length);

            if ((int)_dataSize.X < (int)copy._dataSize.X || (int)_dataSize.Y < (int)copy._dataSize.Y)
            {
                int X = (int)copy._dataSize.X > (int)_dataSize.X ? (int)copy._dataSize.X : (int)_dataSize.X;
                int Y = (int)copy._dataSize.Y > (int)_dataSize.Y ? (int)copy._dataSize.Y : (int)_dataSize.Y;
                _data = new int[X, Y];
                _dataSize = new Point16(X, Y);
            }

            for (int index1 = 0; index1 < (int)copy._dataSize.X; ++index1)
            {
                for (int index2 = 0; index2 < (int)copy._dataSize.Y; ++index2)
                    _data[index1, index2] = copy._data[index1, index2];
            }
        }

        public void AllInvalid()
        {
            for (int index1 = 0; index1 < (int)_size.X; ++index1)
            {
                for (int index2 = 0; index2 < (int)_size.Y; ++index2)
                {
                    if (_data[index1, index2] != 0)
                        _data[index1, index2] = 2;
                }
            }
        }
    }
}
