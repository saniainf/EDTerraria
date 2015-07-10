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
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.Utilities;

namespace Terraria.IO
{
    public class PlayerFileData : FileData
    {
        private TimeSpan _playTime = TimeSpan.Zero;
        private Stopwatch _timer = new Stopwatch();
        private Player _player;
        private bool _isTimerActive;

        public Player Player
        {
            get { return _player; }
            set
            {
                _player = value;
                if (value == null)
                    return;

                Name = _player.name;
            }
        }

        public PlayerFileData()
            : base("Player") { }

        public PlayerFileData(string path)
            : base("Player", path) { }

        public static PlayerFileData CreateAndSave(Player player)
        {
            PlayerFileData playerFile = new PlayerFileData();
            playerFile.Metadata = FileMetadata.FromCurrentSettings(FileType.Player);
            playerFile.Player = player;
            playerFile._path = Main.GetPlayerPathFromName(player.name, false);
            Player.SavePlayer(playerFile, true);
            return playerFile;
        }

        public override void SetAsActive()
        {
            Main.ActivePlayerFileData = this;
            Main.player[Main.myPlayer] = Player;
        }

        public void UpdatePlayTimer()
        {
            if (Main.instance.IsActive && !Main.gamePaused && (Main.hasFocus && _isTimerActive))
                StartPlayTimer();
            else
                PausePlayTimer();
        }

        public void StartPlayTimer()
        {
            _isTimerActive = true;
            if (_timer.IsRunning)
                return;

            _timer.Start();
        }

        public void PausePlayTimer()
        {
            if (!_timer.IsRunning)
                return;

            _timer.Stop();
        }

        public TimeSpan GetPlayTime()
        {
            if (_timer.IsRunning)
                return _playTime + _timer.Elapsed;

            return _playTime;
        }

        public void StopPlayTimer()
        {
            _isTimerActive = false;
            if (!_timer.IsRunning)
                return;

            _playTime += _timer.Elapsed;
            _timer.Reset();
        }

        public void SetPlayTime(TimeSpan time)
        {
            _playTime = time;
        }
    }
}
