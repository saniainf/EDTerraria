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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Terraria;

namespace Terraria.Graphics.Capture
{
    internal class CaptureCamera
    {
        private readonly object _captureLock = new object();
        private Queue<CaptureChunk> _renderQueue = new Queue<CaptureChunk>();
        public const int CHUNK_SIZE = 128;
        public const int FRAMEBUFFER_PIXEL_SIZE = 2048;
        public const int INNER_CHUNK_SIZE = 126;
        public const int MAX_IMAGE_SIZE = 4096;
        public const string CAPTURE_DIRECTORY = "Captures";
        private static bool CameraExists;
        private RenderTarget2D _frameBuffer;
        private RenderTarget2D _scaledFrameBuffer;
        private GraphicsDevice _graphics;
        private bool _isDisposed;
        private CaptureSettings _activeSettings;
        private SpriteBatch _spriteBatch;
        private byte[] _scaledFrameData;
        private byte[] _outputData;
        private Size _outputImageSize;
        private SamplerState _downscaleSampleState;
        private float _tilesProcessed;
        private float _totalTiles;

        public bool IsCapturing
        {
            get
            {
                Monitor.Enter(_captureLock);
                bool flag = _activeSettings != null;
                Monitor.Exit(_captureLock);
                return flag;
            }
        }

        public CaptureCamera(GraphicsDevice graphics)
        {
            CameraExists = true;
            _graphics = graphics;
            _spriteBatch = new SpriteBatch(graphics);

            try
            {
                _frameBuffer = new RenderTarget2D(graphics, 2048, 2048, false, graphics.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            }
            catch
            {
                Main.CaptureModeDisabled = true;
                return;
            }

            _downscaleSampleState = SamplerState.AnisotropicClamp;
        }

        ~CaptureCamera()
        {
            Dispose();
        }

        public void Capture(CaptureSettings settings)
        {
            Main.GlobalTimerPaused = true;
            Monitor.Enter(_captureLock);
            if (_activeSettings != null)
                throw new InvalidOperationException("Capture called while another capture was already active.");
            _activeSettings = settings;
            Microsoft.Xna.Framework.Rectangle rectangle = settings.Area;

            float num1 = 1f;
            if (settings.UseScaling)
            {
                if (rectangle.Width << 4 > 4096)
                    num1 = 4096f / (float)(rectangle.Width << 4);
                if (rectangle.Height << 4 > 4096)
                    num1 = Math.Min(num1, 4096f / (float)(rectangle.Height << 4));
                num1 = Math.Min(1f, num1);
                _outputImageSize = new Size((int)MathHelper.Clamp((float)(num1 * (rectangle.Width << 4)), 1f, 4096f), (int)MathHelper.Clamp((float)(num1 * (rectangle.Height << 4)), 1f, 4096f));
                _outputData = new byte[4 * _outputImageSize.Width * _outputImageSize.Height];
                int num2 = (int)Math.Floor(num1 * 2048.0);
                _scaledFrameData = new byte[4 * num2 * num2];
                _scaledFrameBuffer = new RenderTarget2D(_graphics, num2, num2, false, _graphics.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            }
            else
                _outputData = new byte[16777216];

            _tilesProcessed = 0.0f;
            _totalTiles = (float)(rectangle.Width * rectangle.Height);
            int x1 = rectangle.X;
            while (x1 < rectangle.X + rectangle.Width)
            {
                int y1 = rectangle.Y;
                while (y1 < rectangle.Y + rectangle.Height)
                {
                    int width1 = Math.Min(128, rectangle.X + rectangle.Width - x1);
                    int height1 = Math.Min(128, rectangle.Y + rectangle.Height - y1);
                    int width2 = (int)Math.Floor(num1 * (width1 << 4));
                    int height2 = (int)Math.Floor(num1 * (height1 << 4));
                    int x2 = (int)Math.Floor(num1 * (x1 - rectangle.X << 4));
                    int y2 = (int)Math.Floor(num1 * (y1 - rectangle.Y << 4));
                    _renderQueue.Enqueue(new CaptureChunk(new Microsoft.Xna.Framework.Rectangle(x1, y1, width1, height1), new Microsoft.Xna.Framework.Rectangle(x2, y2, width2, height2)));
                    y1 += 126;
                }
                x1 += 126;
            }
            Monitor.Exit(_captureLock);
        }

        public void DrawTick()
        {
            Monitor.Enter(_captureLock);
            if (_activeSettings == null)
                return;

            CaptureCamera.CaptureChunk captureChunk = _renderQueue.Dequeue();
            _graphics.SetRenderTarget(_frameBuffer);
            _graphics.Clear(Microsoft.Xna.Framework.Color.Transparent);
            Main.instance.DrawCapture(captureChunk.Area, _activeSettings);
            if (_activeSettings.UseScaling)
            {
                _graphics.SetRenderTarget(_scaledFrameBuffer);
                _graphics.Clear(Microsoft.Xna.Framework.Color.Transparent);
                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, _downscaleSampleState, DepthStencilState.Default, RasterizerState.CullNone);
                _spriteBatch.Draw((Texture2D)_frameBuffer, new Microsoft.Xna.Framework.Rectangle(0, 0, _scaledFrameBuffer.Width, _scaledFrameBuffer.Height), Microsoft.Xna.Framework.Color.White);
                _spriteBatch.End();
                _graphics.SetRenderTarget((RenderTarget2D)null);
                _scaledFrameBuffer.GetData<byte>(_scaledFrameData, 0, _scaledFrameBuffer.Width * _scaledFrameBuffer.Height * 4);
                DrawBytesToBuffer(_scaledFrameData, _outputData, _scaledFrameBuffer.Width, _outputImageSize.Width, captureChunk.ScaledArea);
            }
            else
            {
                _graphics.SetRenderTarget((RenderTarget2D)null);
                SaveImage((Texture2D)_frameBuffer, captureChunk.ScaledArea.Width, captureChunk.ScaledArea.Height, ImageFormat.Png, _activeSettings.OutputName,
                    string.Concat(new object[4] { captureChunk.Area.X, "-", captureChunk.Area.Y, ".png" }));
            }

            _tilesProcessed += (float)(captureChunk.Area.Width * captureChunk.Area.Height);
            if (_renderQueue.Count == 0)
                FinishCapture();
            Monitor.Exit(_captureLock);
        }

        private unsafe void DrawBytesToBuffer(byte[] sourceBuffer, byte[] destinationBuffer, int sourceBufferWidth, int destinationBufferWidth, Microsoft.Xna.Framework.Rectangle area)
        {
            fixed (byte* ptr = &destinationBuffer[0])
            {
                byte* ptr2 = ptr;
                fixed (byte* ptr3 = &sourceBuffer[0])
                {
                    byte* ptr4 = ptr3;
                    ptr2 += (destinationBufferWidth * area.Y + area.X << 2) / 1;
                    for (int i = 0; i < area.Height; i++)
                    {
                        for (int j = 0; j < area.Width; j++)
                        {
                            ptr2[2] = *ptr4;
                            ptr2[1] = *(ptr4 + 1);
                            *ptr2 = ptr4[2];
                            ptr2[3] = *(ptr4 + 3);
                            ptr4 += 4;
                            ptr2 += 4;
                        }

                        ptr4 += (sourceBufferWidth - area.Width << 2) / 1;
                        ptr2 += (destinationBufferWidth - area.Width << 2) / 1;
                    }
                }
            }
        }

        public float GetProgress()
        {
            return _tilesProcessed / _totalTiles;
        }

        private bool SaveImage(int width, int height, ImageFormat imageFormat, string filename)
        {
            try
            {
                Directory.CreateDirectory(string.Concat(new object[4] { Main.SavePath, Path.DirectorySeparatorChar, "Captures", Path.DirectorySeparatorChar }));
                using (Bitmap bitmap = new Bitmap(width, height))
                {
                    System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, width, height);
                    BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                    Marshal.Copy(_outputData, 0, bitmapdata.Scan0, width * height * 4);
                    bitmap.UnlockBits(bitmapdata);
                    bitmap.Save(filename, imageFormat);
                    bitmap.Dispose();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void SaveImage(Texture2D texture, int width, int height, ImageFormat imageFormat, string foldername, string filename)
        {
            Directory.CreateDirectory(Main.SavePath + Path.DirectorySeparatorChar + "Captures" + Path.DirectorySeparatorChar + foldername);
            using (Bitmap bitmap = new Bitmap(width, height))
            {
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, width, height);
                int elementCount = texture.Width * texture.Height * 4;
                texture.GetData<byte>(_outputData, 0, elementCount);
                int index1 = 0;
                int index2 = 0;
                for (int index3 = 0; index3 < height; ++index3)
                {
                    for (int index4 = 0; index4 < width; ++index4)
                    {
                        byte num = _outputData[index1 + 2];
                        _outputData[index2 + 2] = _outputData[index1];
                        _outputData[index2] = num;
                        _outputData[index2 + 1] = _outputData[index1 + 1];
                        _outputData[index2 + 3] = _outputData[index1 + 3];
                        index1 += 4;
                        index2 += 4;
                    }
                    index1 += texture.Width - width << 2;
                }

                BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                Marshal.Copy(_outputData, 0, bitmapdata.Scan0, width * height * 4);
                bitmap.UnlockBits(bitmapdata);
                bitmap.Save(Main.SavePath + Path.DirectorySeparatorChar + "Captures" + Path.DirectorySeparatorChar + foldername + Path.DirectorySeparatorChar + filename, imageFormat);
            }
        }

        private void FinishCapture()
        {
            if (_activeSettings.UseScaling)
            {
                int num = 0;
                do
                {
                    if (!SaveImage(_outputImageSize.Width, _outputImageSize.Height, ImageFormat.Png, Main.SavePath + Path.DirectorySeparatorChar + "Captures" + Path.DirectorySeparatorChar +
                        _activeSettings.OutputName + ".png"))
                    {
                        GC.Collect();
                        Thread.Sleep(5);
                        ++num;
                        Console.WriteLine("An error occured while saving the capture. Attempting again...");
                    }
                } while (num <= 5);

                Console.WriteLine("Unable to capture.");
                _outputData = null;
                _scaledFrameData = null;
                Main.GlobalTimerPaused = false;
                CaptureInterface.EndCamera();
                if (_scaledFrameBuffer != null)
                {
                    _scaledFrameBuffer.Dispose();
                    _scaledFrameBuffer = null;
                }
                _activeSettings = null;
            }
        }

        public void Dispose()
        {
            Monitor.Enter(_captureLock);
            if (_isDisposed)
                return;

            _frameBuffer.Dispose();
            if (_scaledFrameBuffer != null)
            {
                _scaledFrameBuffer.Dispose();
                _scaledFrameBuffer = null;
            }

            CameraExists = false;
            _isDisposed = true;
            Monitor.Exit(_captureLock);
        }

        private class CaptureChunk
        {
            public readonly Microsoft.Xna.Framework.Rectangle Area;
            public readonly Microsoft.Xna.Framework.Rectangle ScaledArea;

            public CaptureChunk(Microsoft.Xna.Framework.Rectangle area, Microsoft.Xna.Framework.Rectangle scaledArea)
            {
                Area = area;
                ScaledArea = scaledArea;
            }
        }
    }
}
