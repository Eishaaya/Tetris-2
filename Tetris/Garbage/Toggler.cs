using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class Toggler : Button
    {
        Sprite ball;
        Sprite bottomColor;
        ScalableSprite MovingColor;
        bool on;
        bool done = true;
        public Toggler(Texture2D image, Vector2 location, Color color, float rotation, SpriteEffects effect, Vector2 origin, float superscale, float depth, Color hovercolor, Color clickedcolor, Sprite Ball, Sprite Bottom, ScalableSprite Moving, bool On = false)
            :base(image, location, color, rotation, effect, origin, superscale, depth, hovercolor, clickedcolor)
        {
            ball = Ball;
            bottomColor = Bottom;
            MovingColor = Moving;
            on = On;
        }
        public override bool check(Vector2 cursor, bool isclicked)
        {
            done = !base.check(cursor, isclicked);
            if (!done)
            {
                on = !on;
            }
            return !done;
        }
        public void Move ()
        {
            if (done)
            {
                return;
            }
        }
        public override void Draw(SpriteBatch batch)
        {
            bottomColor.Draw(batch);
            MovingColor.Draw(batch);
            base.Draw(batch);
            ball.Draw(batch);
        }
    }
}
