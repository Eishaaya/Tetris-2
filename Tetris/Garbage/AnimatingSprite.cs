using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public abstract class AnimationFrame
    {
        public abstract void Draw(SpriteBatch batch, Texture2D Image, Vector2 Location, Color Color, float rotation, Vector2[] origins, Vector2 Origin, float Scale, SpriteEffects effect, float Depth, int currentframe);
    }

    class RectangleFrame : AnimationFrame
    {
        RectangleFrame(Rectangle rectangle)
        {
            rect = rectangle;
        }
        Rectangle rect;

        public static implicit operator Rectangle(RectangleFrame frame) => frame.rect;
        public static implicit operator RectangleFrame(Rectangle frame) => new RectangleFrame(frame);

        public override void Draw(SpriteBatch batch, Texture2D Image, Vector2 Location, Color Color, float rotation, Vector2[] origins, Vector2 Origin, float Scale, SpriteEffects effect, float Depth, int currentframe)
        {
            batch.Draw(Image, Location, this, Color, rotation, origins == null ? Origin : origins[currentframe], Scale, effect, Depth);
        }
    }

    class TextureFrame : AnimationFrame
    {
        TextureFrame(Texture2D txtr)
        {
            texture = txtr;
        }
        Texture2D texture;

        public static implicit operator Texture2D(TextureFrame frame) => frame.texture;
        public static implicit operator TextureFrame(Texture2D frame)  => new TextureFrame(frame);

        public override void Draw(SpriteBatch batch, Texture2D Image, Vector2 Location, Color Color, float rotation, Vector2[] origins, Vector2 Origin, float Scale, SpriteEffects effect, float Depth, int currentframe)
        {
            Image = this;
            batch.Draw(Image, Location, null, Color, rotation, origins == null ? Origin : origins[currentframe], Scale, effect, Depth);
        }
    }


    public class AnimatingSprite : Sprite
    {
        //public struct Animation
        //{
        //    List<Rectangle> frames;

        //    public Animation(List<Rectangle> list)
        //    {
        //        frames = list;
        //    }
        //}
        public AnimationFrame[] Frames { get; set; }
        Vector2[] origins;
        public Timer FrameTime { get; set; }
    //    TimeSpan tick;
        public int currentframe;

        public bool LastFrame 
        {
            get; private set;
        }

        public bool OnLastFrame
        {
            get => currentframe == Frames.Length - 1;
        }

        public AnimatingSprite (Texture2D image, Vector2 location, Color color, float rotation, SpriteEffects effects, Rectangle hitbox, Vector2 origin, float scale, float depth, AnimationFrame[] frames, int time, Vector2[] Origins = null)
            :base(image, location, color, rotation, effects, origin, scale, depth)
        {
            Frames = frames;
            FrameTime = new TimeSpan(0, 0, 0, 0, time);
            origins = Origins;
            currentframe = 0;


            if (origins != null && origins.Length < frames.Length)
            {
                origins = null;
            }

        }
        public void SetAnimatingSprite(Texture2D image, Vector2 location, Color color, float rotation, SpriteEffects effects, Rectangle hitbox, Vector2 origin, float scale, float depth, AnimationFrame[] frames, int time, Vector2[] Origins = null)
        {
            Frames = frames;
            FrameTime = new TimeSpan(0, 0, 0, 0, time);
            origins = Origins;
            currentframe = 0;


            if (origins != null && origins.Length < frames.Length)
            {
                origins = null;
            }

            Location = location;
            Color = color;
            Origin = origin;
            Scale = scale;
            Depth = depth;
            effect = effects;
            this.rotation = rotation;
            originalColor = color;
            oldScale = Scale;
            oldRotation = rotation;
            random = new Random();
            Image = image;

            offset = Vector2.Zero;
            moved = false;
            bigger = false;
            sizeSet = float.NaN;
            degreeSet = float.NaN;
            spotSet = new Vector2(float.NaN, float.NaN);

        }
        public void Animate(GameTime gametime)
        {
            LastFrame = false;
            FrameTime.Tick(gametime);
            if(FrameTime.Ready())
            {
                currentframe++;
            }
            if(currentframe >= Frames.Length)
            {
                currentframe = 0;
                LastFrame = true;
            }
        }   
        public override void Draw(SpriteBatch batch)
        {
            Frames[currentframe].Draw(batch, Image, Location, Color, rotation, origins, Origin, Scale, effect, Depth, currentframe);
        }
    }
}