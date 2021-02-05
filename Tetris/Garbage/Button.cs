using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Button : Sprite
    {
        Color OriginalColor;
        public Color HoverColor;
        public Color ClickedColor;
        public bool hold;
        public bool held;
        public bool prevDown;
        public override Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)Location.X, (int)Location.Y, (int)(Image.Width * Scale), (int)(Image.Height * Scale));
            }
        }
        public Button(Texture2D image, Vector2 location, Color color, float rotation, SpriteEffects effect, Vector2 origin, float superscale, float depth, Color hovercolor, Color clickedcolor)
                : base(image, location, color, rotation, effect, origin, superscale, depth)
        {
            HoverColor = hovercolor;
            ClickedColor = clickedcolor;
            OriginalColor = color;
            hold = false;
            prevDown = false;
        }
        public virtual bool check(Vector2 cursor, bool isclicked)
        {
            if (Hitbox.Contains(cursor))
            {
                if (!isclicked)
                {
                    Color = HoverColor;
                    hold = false;
                    prevDown = false;
                }
                else
                {
                    Color = ClickedColor;
                    hold = prevDown;
                    prevDown = true;
                    return true;
                }
            }
            else
            {
                if (!held)
                {
                    Color = OriginalColor;
                }
                else
                {
                    Color = ClickedColor;
                }
            }
            return false;
        }
    }
}
