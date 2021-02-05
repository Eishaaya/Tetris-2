using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class Toggler : Button
    {
        public Sprite ball;
        public Sprite bottomColor;
        public ScalableSprite MovingColor;
        public Label laby;
        public bool on;
        public bool done = true;
        Vector2 setOff;
        public Toggler(Texture2D image, Vector2 location, Color color, float rotation, SpriteEffects effect, Vector2 origin, float superscale, float depth, Color hovercolor, Color clickedcolor, Sprite Ball, Sprite Bottom, ScalableSprite Moving, SpriteFont font = null, string text = "", float stringH = 50, float offx = 0, float offy = 0, bool On = false)
            : base(image, location, color, rotation, effect, origin, superscale, depth, hovercolor, clickedcolor)
        {
            if (font != null)
            {
                laby = new Label(font, Color, new Vector2(location.X + image.Width / 2 - (int)font.MeasureString(text).X / 2, location.Y + stringH), text, TimeSpan.Zero);
            }
            ball = Ball;
            bottomColor = Bottom;
            MovingColor = Moving;
            on = On;
            if (on)
            {
                ball.Location = Location + new Vector2(Image.Width - ball.Image.Width, 0) - setOff;
            }
            else
            {
                ball.Location = Location + setOff;
            }
            setOff = new Vector2(offx, offy);
            ball.Location += setOff;
            MovingColor.scale = new Vector2((ball.Location.X - Location.X) / (Image.Width - ball.Image.Width), MovingColor.scale.Y);
        }
        public override bool check(Vector2 cursor, bool isclicked)
        {
            Move();
            var tempState = base.check(cursor, isclicked);
            if (!hold)
            {
                if (done)
                {
                    done = !tempState;
                    return !done;
                }
                if (tempState)
                {
                    on = !on;
                    return tempState;
                }
            }
            return false;
        }
        public void Move()
        {
            if (!done)
            {
                if (!on)
                {
                    ball.Location = Vector2.Lerp(ball.Location, Location + new Vector2(Image.Width - ball.Image.Width, 0) - setOff, .1f);
                    MovingColor.scale = new Vector2((ball.Location.X - Location.X) / (Image.Width - ball.Image.Width), MovingColor.scale.Y);
                    if (Vector2.Distance(ball.Location, Location + new Vector2(Image.Width - ball.Image.Width, 0) - setOff) <= .1f)
                    {
                        ball.Location = Location + new Vector2(Image.Width - ball.Image.Width, 0) - setOff;
                        on = !on;
                        done = true;
                    }
                }
                else
                {
                    ball.Location = Vector2.Lerp(ball.Location, Location + setOff, .1f);
                    MovingColor.scale = new Vector2((ball.Location.X - Location.X) / (Image.Width - ball.Image.Width), MovingColor.scale.Y);
                    if (Vector2.Distance(ball.Location, Location + setOff) <= .1f)
                    {
                        ball.Location = Location + setOff;
                        on = !on;
                        done = true;
                    }
                }
            }
        }
        public override void Draw(SpriteBatch batch)
        {
            bottomColor.Draw(batch);
            MovingColor.Draw(batch);
            base.Draw(batch);
            ball.Draw(batch);
            if (laby != null)
            {
                laby.write(batch);
            }
        }
    }
}
