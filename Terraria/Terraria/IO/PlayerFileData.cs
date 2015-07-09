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
            get
            {
                return this._player;
            }
            set
            {
                this._player = value;
                if (value == null)
                    return;
                this.Name = this._player.name;
            }
        }

        public PlayerFileData()
            : base("Player")
        {
        }

        public PlayerFileData(string path, bool cloudSave)
            : base("Player", path, cloudSave)
        {
        }

        public static PlayerFileData CreateAndSave(Player player)
        {
            PlayerFileData playerFile = new PlayerFileData();
            playerFile.Metadata = FileMetadata.FromCurrentSettings(FileType.Player);
            playerFile.Player = player;
            playerFile._isCloudSave = false;
            playerFile._path = Main.GetPlayerPathFromName(player.name, playerFile.IsCloudSave);
            (playerFile.IsCloudSave ? Main.CloudFavoritesData : Main.LocalFavoriteData).ClearEntry((FileData)playerFile);
            Player.SavePlayer(playerFile, true);
            return playerFile;
        }

        public override void SetAsActive()
        {
            Main.ActivePlayerFileData = this;
            Main.player[Main.myPlayer] = this.Player;
        }

        public void UpdatePlayTimer()
        {
            if (Main.instance.IsActive && !Main.gamePaused && (Main.hasFocus && this._isTimerActive))
                this.StartPlayTimer();
            else
                this.PausePlayTimer();
        }

        public void StartPlayTimer()
        {
            this._isTimerActive = true;
            if (this._timer.IsRunning)
                return;
            this._timer.Start();
        }

        public void PausePlayTimer()
        {
            if (!this._timer.IsRunning)
                return;
            this._timer.Stop();
        }

        public TimeSpan GetPlayTime()
        {
            if (this._timer.IsRunning)
                return this._playTime + this._timer.Elapsed;
            return this._playTime;
        }

        public void StopPlayTimer()
        {
            this._isTimerActive = false;
            if (!this._timer.IsRunning)
                return;
            this._playTime += this._timer.Elapsed;
            this._timer.Reset();
        }

        public void SetPlayTime(TimeSpan time)
        {
            this._playTime = time;
        }
    }
}
