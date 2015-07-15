/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using System.Collections.Generic;
using Terraria.DataStructures;

namespace Terraria
{
    public class Animation
    {
        private static List<Animation> _animations;
        private static Dictionary<Point16, Animation> _temporaryAnimations;
        private static List<Point16> _awaitingRemoval;
        private static List<Animation> _awaitingAddition;
        private bool _temporary;
        private Point16 _coordinates;
        private ushort _tileType;
        private int _frame;
        private int _frameMax;
        private int _frameCounter;
        private int _frameCounterMax;
        private int[] _frameData;

        public static void Initialize()
        {
            _animations = new List<Animation>();
            _temporaryAnimations = new Dictionary<Point16, Animation>();
            _awaitingRemoval = new List<Point16>();
            _awaitingAddition = new List<Animation>();
        }

        private void SetDefaults(int type)
        {
            _tileType = 0;
            _frame = 0;
            _frameMax = 0;
            _frameCounter = 0;
            _frameCounterMax = 0;
            _temporary = false;
            switch (type)
            {
                case 0:
                    _frameMax = 5;
                    _frameCounterMax = 12;
                    _frameData = new int[_frameMax];
                    for (int index = 0; index < _frameMax; ++index)
                        _frameData[index] = index + 1;
                    break;
                case 1:
                    _frameMax = 5;
                    _frameCounterMax = 12;
                    _frameData = new int[_frameMax];
                    for (int index = 0; index < _frameMax; ++index)
                        _frameData[index] = 5 - index;
                    break;
            }
        }

        public static void NewTemporaryAnimation(int type, ushort tileType, int x, int y)
        {
            Point16 point16 = new Point16(x, y);
            if (x < 0 || x >= Main.maxTilesX || (y < 0 || y >= Main.maxTilesY))
                return;
            Animation animation = new Animation();
            animation.SetDefaults(type);
            animation._tileType = tileType;
            animation._coordinates = point16;
            animation._temporary = true;
            _awaitingAddition.Add(animation);
            if (Main.netMode != 2)
                return;
            NetMessage.SendTemporaryAnimation(-1, type, tileType, x, y);
        }

        private static void RemoveTemporaryAnimation(short x, short y)
        {
            Point16 key = new Point16(x, y);
            if (!_temporaryAnimations.ContainsKey(key))
                return;
            _awaitingRemoval.Add(key);
        }

        public static void UpdateAll()
        {
            for (int index = 0; index < _animations.Count; ++index)
                _animations[index].Update();
            if (_awaitingAddition.Count > 0)
            {
                for (int index = 0; index < _awaitingAddition.Count; ++index)
                {
                    Animation animation = _awaitingAddition[index];
                    _temporaryAnimations[animation._coordinates] = animation;
                }
                _awaitingAddition.Clear();
            }
            foreach (KeyValuePair<Point16, Animation> keyValuePair in _temporaryAnimations)
                keyValuePair.Value.Update();
            if (_awaitingRemoval.Count <= 0)
                return;
            for (int index = 0; index < _awaitingRemoval.Count; ++index)
                _temporaryAnimations.Remove(_awaitingRemoval[index]);
            _awaitingRemoval.Clear();
        }

        public void Update()
        {
            if (_temporary)
            {
                Tile tile = Main.tile[_coordinates.X, _coordinates.Y];
                if (tile != null && tile.type != _tileType)
                {
                    RemoveTemporaryAnimation(_coordinates.X, _coordinates.Y);
                    return;
                }
            }
            ++_frameCounter;
            if (_frameCounter < _frameCounterMax)
                return;
            _frameCounter = 0;
            ++_frame;
            if (_frame < _frameMax)
                return;
            _frame = 0;
            if (!_temporary)
                return;
            RemoveTemporaryAnimation(_coordinates.X, _coordinates.Y);
        }

        public static bool GetTemporaryFrame(int x, int y, out int frameData)
        {
            Point16 key = new Point16(x, y);
            Animation animation;
            if (!_temporaryAnimations.TryGetValue(key, out animation))
            {
                frameData = 0;
                return false;
            }
            frameData = animation._frameData[animation._frame];
            return true;
        }
    }
}
