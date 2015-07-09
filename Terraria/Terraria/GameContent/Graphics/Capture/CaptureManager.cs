/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Terraria.Graphics.Capture
{
    internal class CaptureManager
    {
        public static CaptureManager Instance = new CaptureManager();
        private CaptureInterface _interface;
        private CaptureCamera _camera;

        public bool IsCapturing
        {
            get
            {
                return this._camera.IsCapturing;
            }
        }

        public bool Active
        {
            get
            {
                return this._interface.Active;
            }
            set
            {
                if (Main.CaptureModeDisabled || this._interface.Active == value)
                    return;
                this._interface.ToggleCamera(value);
            }
        }

        public bool UsingMap
        {
            get
            {
                if (!this.Active)
                    return false;
                return this._interface.UsingMap();
            }
        }

        public CaptureManager()
        {
            this._interface = new CaptureInterface();
            this._camera = new CaptureCamera(Main.instance.GraphicsDevice);
        }

        public void Scrolling()
        {
            this._interface.Scrolling();
        }

        public void Update()
        {
            this._interface.Update();
        }

        public void Draw(SpriteBatch sb)
        {
            this._interface.Draw(sb);
        }

        public float GetProgress()
        {
            return this._camera.GetProgress();
        }

        public void Capture()
        {
            this.Capture(new CaptureSettings()
            {
                Area = new Rectangle(2660, 100, 1000, 1000),
                UseScaling = false
            });
        }

        public void Capture(CaptureSettings settings)
        {
            this._camera.Capture(settings);
        }

        public void DrawTick()
        {
            this._camera.DrawTick();
        }
    }
}
