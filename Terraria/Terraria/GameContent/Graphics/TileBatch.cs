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

namespace Terraria.Graphics
{
    public class TileBatch
    {
        private struct SpriteData
        {
            public Vector4 Source;
            public Vector4 Destination;
            public Vector2 Origin;
            public SpriteEffects Effects;
            public VertexColors Colors;
        }
        private static float[] xCornerOffsets;
        private static float[] yCornerOffsets;
        private GraphicsDevice graphicsDevice;
        private TileBatch.SpriteData[] spriteDataQueue = new TileBatch.SpriteData[2048];
        private Texture2D[] spriteTextures;
        private int queuedSpriteCount;
        private SpriteBatch _spriteBatch;
        private static Vector2 vector2Zero;
        private static Rectangle? nullRectangle;
        private DynamicVertexBuffer vertexBuffer;
        private DynamicIndexBuffer indexBuffer;
        private short[] fallbackIndexData;
        private VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[8192];
        private int vertexBufferPosition;
        public TileBatch(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            this._spriteBatch = new SpriteBatch(graphicsDevice);
            this.Allocate();
        }
        private void Allocate()
        {
            if (this.vertexBuffer == null || this.vertexBuffer.IsDisposed)
            {
                this.vertexBuffer = new DynamicVertexBuffer(this.graphicsDevice, typeof(VertexPositionColorTexture), 8192, BufferUsage.WriteOnly);
                this.vertexBufferPosition = 0;
                this.vertexBuffer.ContentLost += delegate(object sender, EventArgs e)
                {
                    this.vertexBufferPosition = 0;
                };
            }
            if (this.indexBuffer == null || this.indexBuffer.IsDisposed)
            {
                if (this.fallbackIndexData == null)
                {
                    this.fallbackIndexData = new short[12288];
                    for (int i = 0; i < 2048; i++)
                    {
                        this.fallbackIndexData[i * 6] = (short)(i * 4);
                        this.fallbackIndexData[i * 6 + 1] = (short)(i * 4 + 1);
                        this.fallbackIndexData[i * 6 + 2] = (short)(i * 4 + 2);
                        this.fallbackIndexData[i * 6 + 3] = (short)(i * 4);
                        this.fallbackIndexData[i * 6 + 4] = (short)(i * 4 + 2);
                        this.fallbackIndexData[i * 6 + 5] = (short)(i * 4 + 3);
                    }
                }
                this.indexBuffer = new DynamicIndexBuffer(this.graphicsDevice, typeof(short), 12288, BufferUsage.WriteOnly);
                this.indexBuffer.SetData<short>(this.fallbackIndexData);
                this.indexBuffer.ContentLost += delegate(object sender, EventArgs e)
                {
                    this.indexBuffer.SetData<short>(this.fallbackIndexData);
                };
            }
        }
        private void FlushRenderState()
        {
            this.Allocate();
            this.graphicsDevice.SetVertexBuffer(this.vertexBuffer);
            this.graphicsDevice.Indices = this.indexBuffer;
            this.graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        }
        public void Dispose()
        {
            if (this.vertexBuffer != null)
            {
                this.vertexBuffer.Dispose();
            }
            if (this.indexBuffer != null)
            {
                this.indexBuffer.Dispose();
            }
        }
        public void Begin()
        {
            this._spriteBatch.Begin();
            this._spriteBatch.End();
        }
        public void Draw(Texture2D texture, Vector2 position, VertexColors colors)
        {
            Vector4 vector = default(Vector4);
            vector.X = position.X;
            vector.Y = position.Y;
            vector.Z = 1f;
            vector.W = 1f;
            this.InternalDraw(texture, ref vector, true, ref TileBatch.nullRectangle, ref colors, ref TileBatch.vector2Zero, SpriteEffects.None);
        }
        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, VertexColors colors, Vector2 origin, float scale, SpriteEffects effects)
        {
            Vector4 vector = default(Vector4);
            vector.X = position.X;
            vector.Y = position.Y;
            vector.Z = scale;
            vector.W = scale;
            this.InternalDraw(texture, ref vector, true, ref sourceRectangle, ref colors, ref origin, effects);
        }
        public void Draw(Texture2D texture, Vector4 destination, VertexColors colors)
        {
            this.InternalDraw(texture, ref destination, false, ref TileBatch.nullRectangle, ref colors, ref TileBatch.vector2Zero, SpriteEffects.None);
        }
        public void Draw(Texture2D texture, Vector2 position, VertexColors colors, Vector2 scale)
        {
            Vector4 vector = default(Vector4);
            vector.X = position.X;
            vector.Y = position.Y;
            vector.Z = scale.X;
            vector.W = scale.Y;
            this.InternalDraw(texture, ref vector, true, ref TileBatch.nullRectangle, ref colors, ref TileBatch.vector2Zero, SpriteEffects.None);
        }
        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, VertexColors colors)
        {
            Vector4 vector = default(Vector4);
            vector.X = (float)destinationRectangle.X;
            vector.Y = (float)destinationRectangle.Y;
            vector.Z = (float)destinationRectangle.Width;
            vector.W = (float)destinationRectangle.Height;
            this.InternalDraw(texture, ref vector, false, ref sourceRectangle, ref colors, ref TileBatch.vector2Zero, SpriteEffects.None);
        }
        private static short[] CreateIndexData()
        {
            short[] array = new short[12288];
            for (int i = 0; i < 2048; i++)
            {
                array[i * 6] = (short)(i * 4);
                array[i * 6 + 1] = (short)(i * 4 + 1);
                array[i * 6 + 2] = (short)(i * 4 + 2);
                array[i * 6 + 3] = (short)(i * 4);
                array[i * 6 + 4] = (short)(i * 4 + 2);
                array[i * 6 + 5] = (short)(i * 4 + 3);
            }
            return array;
        }
        private unsafe void InternalDraw(Texture2D texture, ref Vector4 destination, bool scaleDestination, ref Rectangle? sourceRectangle, ref VertexColors colors, ref Vector2 origin, SpriteEffects effects)
        {
            if (this.queuedSpriteCount >= this.spriteDataQueue.Length)
            {
                Array.Resize<TileBatch.SpriteData>(ref this.spriteDataQueue, this.spriteDataQueue.Length << 1);
            }
            fixed (TileBatch.SpriteData* ptr = &this.spriteDataQueue[this.queuedSpriteCount])
            {
                float num = destination.Z;
                float num2 = destination.W;
                if (sourceRectangle.HasValue)
                {
                    Rectangle value = sourceRectangle.Value;
                    ptr->Source.X = (float)value.X;
                    ptr->Source.Y = (float)value.Y;
                    ptr->Source.Z = (float)value.Width;
                    ptr->Source.W = (float)value.Height;
                    if (scaleDestination)
                    {
                        num *= (float)value.Width;
                        num2 *= (float)value.Height;
                    }
                }
                else
                {
                    float num3 = (float)texture.Width;
                    float num4 = (float)texture.Height;
                    ptr->Source.X = 0f;
                    ptr->Source.Y = 0f;
                    ptr->Source.Z = num3;
                    ptr->Source.W = num4;
                    if (scaleDestination)
                    {
                        num *= num3;
                        num2 *= num4;
                    }
                }
                ptr->Destination.X = destination.X;
                ptr->Destination.Y = destination.Y;
                ptr->Destination.Z = num;
                ptr->Destination.W = num2;
                ptr->Origin.X = origin.X;
                ptr->Origin.Y = origin.Y;
                ptr->Effects = effects;
                ptr->Colors = colors;
            }
            if (this.spriteTextures == null || this.spriteTextures.Length != this.spriteDataQueue.Length)
            {
                Array.Resize<Texture2D>(ref this.spriteTextures, this.spriteDataQueue.Length);
            }
            this.spriteTextures[this.queuedSpriteCount++] = texture;
        }
        public void End()
        {
            if (this.queuedSpriteCount == 0)
            {
                return;
            }
            this.FlushRenderState();
            this.Flush();
        }
        private void Flush()
        {
            Texture2D texture2D = null;
            int num = 0;
            for (int i = 0; i < this.queuedSpriteCount; i++)
            {
                if (this.spriteTextures[i] != texture2D)
                {
                    if (i > num)
                    {
                        this.RenderBatch(texture2D, this.spriteDataQueue, num, i - num);
                    }
                    num = i;
                    texture2D = this.spriteTextures[i];
                }
            }
            this.RenderBatch(texture2D, this.spriteDataQueue, num, this.queuedSpriteCount - num);
            Array.Clear(this.spriteTextures, 0, this.queuedSpriteCount);
            this.queuedSpriteCount = 0;
        }
        private unsafe void RenderBatch(Texture2D texture, TileBatch.SpriteData[] sprites, int offset, int count)
        {
            this.graphicsDevice.Textures[0] = texture;
            float num = 1f / (float)texture.Width;
            float num2 = 1f / (float)texture.Height;
            while (count > 0)
            {
                SetDataOptions options = SetDataOptions.NoOverwrite;
                int num3 = count;
                if (num3 > 2048 - this.vertexBufferPosition)
                {
                    num3 = 2048 - this.vertexBufferPosition;
                    if (num3 < 256)
                    {
                        this.vertexBufferPosition = 0;
                        options = SetDataOptions.Discard;
                        num3 = count;
                        if (num3 > 2048)
                        {
                            num3 = 2048;
                        }
                    }
                }
                fixed (TileBatch.SpriteData* ptr = &sprites[offset])
                {
                    fixed (VertexPositionColorTexture* ptr2 = &this.vertices[0])
                    {
                        TileBatch.SpriteData* ptr3 = ptr;
                        VertexPositionColorTexture* ptr4 = ptr2;
                        for (int i = 0; i < num3; i++)
                        {
                            float num4 = ptr3->Origin.X / ptr3->Source.Z;
                            float num5 = ptr3->Origin.Y / ptr3->Source.W;
                            ptr4->Color = (*ptr3).Colors.TopLeftColor;
                            ptr4[1].Color = ptr3->Colors.TopRightColor;
                            ptr4[2].Color = ptr3->Colors.BottomRightColor;
                            ptr4[3].Color = ptr3->Colors.BottomLeftColor;
                            for (int j = 0; j < 4; j++)
                            {
                                float num6 = TileBatch.xCornerOffsets[j];
                                float num7 = TileBatch.yCornerOffsets[j];
                                float num8 = (num6 - num4) * ptr3->Destination.Z;
                                float num9 = (num7 - num5) * ptr3->Destination.W;
                                float x = ptr3->Destination.X + num8;
                                float y = ptr3->Destination.Y + num9;
                                if ((ptr3->Effects & SpriteEffects.FlipVertically) != SpriteEffects.None)
                                {
                                    num6 = 1f - num6;
                                }
                                if ((ptr3->Effects & SpriteEffects.FlipHorizontally) != SpriteEffects.None)
                                {
                                    num7 = 1f - num7;
                                }
                                ptr4->Position.X = x;
                                ptr4->Position.Y = y;
                                ptr4->Position.Z = 0f;
                                ptr4->TextureCoordinate.X = (ptr3->Source.X + num6 * ptr3->Source.Z) * num;
                                ptr4->TextureCoordinate.Y = (ptr3->Source.Y + num7 * ptr3->Source.W) * num2;
                                ptr4++;
                            }
                            ptr3++;
                        }
                    }
                }
                int offsetInBytes = this.vertexBufferPosition * sizeof(VertexPositionColorTexture) * 4;
                this.vertexBuffer.SetData<VertexPositionColorTexture>(offsetInBytes, this.vertices, 0, num3 * 4, sizeof(VertexPositionColorTexture), options);
                int minVertexIndex = this.vertexBufferPosition * 4;
                int numVertices = num3 * 4;
                int startIndex = this.vertexBufferPosition * 6;
                int primitiveCount = num3 * 2;
                this.graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, minVertexIndex, numVertices, startIndex, primitiveCount);
                this.vertexBufferPosition += num3;
                offset += num3;
                count -= num3;
            }
        }
        static TileBatch()
        {
            // Note: this type is marked as 'beforefieldinit'.
            float[] array = new float[4];
            array[1] = 1f;
            array[2] = 1f;
            TileBatch.xCornerOffsets = array;
            TileBatch.yCornerOffsets = new float[]
			{
				0f,
				0f,
				1f,
				1f
			};
        }
    }
}