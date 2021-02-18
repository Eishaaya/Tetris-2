using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class AnimatingSprite : Sprite
    {
        public struct Animation
        {
            List<Rectangle> frames;

            public Animation(List<Rectangle> list)
            {
                frames = list;
            }
        }
        public List<Rectangle> Frames { get; set; }
        public TimeSpan frametime {get; set;}
        TimeSpan tick;
        public int currentframe;

        public AnimatingSprite (Texture2D image, Vector2 location, Color color, float rotation, SpriteEffects effects, Rectangle hitbox, Vector2 origin, float scale, float depth, List<Rectangle> frames, int time)
            :base(image, location, color, rotation, effects, origin, scale, depth)
        {
            Frames = frames;
            frametime = new TimeSpan(0, 0, 0, 0, time);
            tick = TimeSpan.Zero;
        }
        public void Animate(GameTime gametime)
        {
            tick += gametime.ElapsedGameTime;
            if(tick >= frametime)
            {
                currentframe++;
                tick = TimeSpan.Zero;
            }
            if(currentframe >= Frames.Count)
            {
                currentframe = 0;
            }
        }   
        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, Location, Frames[currentframe], Color, rotation, Origin, Scale, effect, Depth);
        }
    }
}