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
        public Color NormalColor { get; set; }
        public Color HoverColor { get; set; }
        public Color ClickedColor { get; set; }
        public bool Hold { get; set; }
        public bool Held { get; set; }
        public bool PrevDown { get; set; }
        public override Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)(Location.X - Origin.X), (int)(Location.Y - Origin.Y), (int)(Image.Width * Scale), (int)(Image.Height * Scale));
            }
        }

        public Button(Texture2D image, Vector2 location)
            : this(image, location, Color.White, Vector2.Zero, Color.DarkGray, Color.Gray) { }
        public Button(Texture2D image, Vector2 location, Color color, Vector2 origin, Color hoverColor, Color clickedColor)
            : this(image, location, color, 0, SpriteEffects.None, origin, 1, 1, hoverColor, clickedColor) { }
        public Button(Texture2D image, Vector2 location, Color color, float rotation, SpriteEffects effect, Vector2 origin, float superscale, float depth, Color hovercolor, Color clickedcolor)
            : base(image, location, color, rotation, effect, origin, superscale, depth)
        {
            HoverColor = hovercolor;
            ClickedColor = clickedcolor;
            NormalColor = color;
            Hold = false;
            PrevDown = false;
        }

        #region clone

        public new Button Clone()
        {
            var copy = new Button(Image, Location, Color, rotation, effect, Origin, Scale, Depth, HoverColor, ClickedColor);
            CloneLogic(copy);

            return copy;
        }
        protected new void CloneLogic<T>(T copy) where T : Button
        {
            base.CloneLogic(copy);
            copy.NormalColor = NormalColor;
            copy.Hold = Hold;
            copy.Held = Held;
            copy.PrevDown = PrevDown;
        }

        #endregion

        public virtual bool check(Vector2 cursor, bool isclicked)
        {
            if (Hitbox.Contains(cursor))
            {
                if (!isclicked)
                {
                    Color = HoverColor;
                    Hold = false;
                    PrevDown = false;
                }
                else
                {
                    Color = ClickedColor;
                    Hold = PrevDown;
                    PrevDown = true;
                    return true;
                }
            }
            else
            {
                if (!Held)
                {
                    Color = NormalColor;
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
