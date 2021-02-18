using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class Particle : ScalableSprite
    {
        public bool going;
        Vector2 speed;
        Timer fadeTime;
        Timer startTime;
        public bool faded;
        bool noScale;

        public Particle(Texture2D image, Vector2 location, Color color, float rotation, SpriteEffects effects, Vector2 origin, Vector2 Speed, int time, Vector2 scale, float depth = 1, float plebScale = 1, int timeTillStart = 0, bool willScale = true)
            : base(image, location, color, rotation, effects, origin, scale, depth, plebScale)
        {
            noScale = !willScale;
            speed = Speed;
            fadeTime = new Timer(time);
            startTime = new Timer(timeTillStart);
            if (timeTillStart == 0)
            {
                going = true;
            }
            else
            {
                going = false;
            }
            //if (!noScale)
            //{
                Scale = Scale / 50;
            //}
        }
        public void Update(GameTime gameTime)
        {
            if (!going)
            {
                startTime.tick(gameTime);
                if (startTime.ready())
                {
                    going = true;
                }
            }
            else
            {
                fadeTime.tick(gameTime);
                //if (!noScale)
                //{
                    scale += new Vector2(Math.Abs(speed.X), Math.Abs(speed.Y)) * Scale;
                //}
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
}
