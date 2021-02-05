using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class ScalableSprite : Sprite
    {
        public Vector2 scale;
        public override Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)Location.X, (int)Location.Y, (int)(Image.Width * scale.X), (int)(Image.Height * scale.Y));
            }
        }
        public ScalableSprite (Texture2D image, Vector2 location, Color color, float rotation, SpriteEffects effects, Vector2 origin, Vector2 Scale, float depth = 1, float plebScale = 1)
            :base (image, location, color, rotation, effects, origin, plebScale, depth)
        {
            scale = Scale;
        }
        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, Location, null, Color, Rotation, Origin, scale, Effects, Depth);
        }
    }
}
