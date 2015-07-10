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
            get { return _camera.IsCapturing; }
        }

        public bool Active
        {
            get { return _interface.Active; }
            set
            {
                if (Main.CaptureModeDisabled || _interface.Active == value)
                    return;

                _interface.ToggleCamera(value);
            }
        }

        public bool UsingMap
        {
            get
            {
                if (!Active)
                    return false;

                return _interface.UsingMap();
            }
        }

        public CaptureManager()
        {
            _interface = new CaptureInterface();
            _camera = new CaptureCamera(Main.instance.GraphicsDevice);
        }

        public void Scrolling()
        {
            _interface.Scrolling();
        }

        public void Update()
        {
            _interface.Update();
        }

        public void Draw(SpriteBatch sb)
        {
            _interface.Draw(sb);
        }

        public float GetProgress()
        {
            return _camera.GetProgress();
        }

        public void Capture()
        {
            Capture(new CaptureSettings()
            {
                Area = new Rectangle(2660, 100, 1000, 1000),
                UseScaling = false
            });
        }

        public void Capture(CaptureSettings settings)
        {
            _camera.Capture(settings);
        }

        public void DrawTick()
        {
            _camera.DrawTick();
        }
    }
}
