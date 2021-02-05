using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class Particle : ScalableSprite
    {
        Vector2 speed;
        Timer fadeTime;
        public bool faded;        

        public Particle(Texture2D image, Vector2 location, Color color, float rotation, SpriteEffects effects, Vector2 origin, Vector2 Speed, int time, Vector2 scale, float depth = 1, float plebScale = 1)
            :base(image, location, color, rotation, effects, origin, scale, depth, plebScale)
        {
            speed = Speed;
            fadeTime = new Timer(time);
            Scale = Scale / 50;
        }
        public void Update(GameTime gameTime)
        {
            fadeTime.tick(gameTime);
            scale += new Vector2(Math.Abs(speed.X), Math.Abs(speed.Y)) * Scale;
            Location += speed;
            if (fadeTime.ready())
            {
                if (Fade())
                {
                    faded = true;
                }
                if (fadeTime.getMillies() < 0)
                {
                    for (int i = 0; i < -fadeTime.getMillies(); i++)
                    {
                        Fade();
                    }
                }
            }
        }
    }
}
