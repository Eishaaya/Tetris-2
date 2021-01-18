using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class Screen
    {
        protected MouseState mousy;
        protected bool nou;
        public bool heldMouse;
        public Screen()
        {
            mousy = new MouseState();
            nou = false;
        }
        public virtual void Update(GameTime time, Screenmanager manny)
        {
            if (mousy.LeftButton == ButtonState.Pressed)
            {
                heldMouse = true;
            }
            mousy = Mouse.GetState();
            nou = false;
            if (mousy.LeftButton == ButtonState.Pressed)
            {
                nou = true;
            }
            else
            {
                heldMouse = false;
            }
        }

        public virtual void Transfer(int transfer)
        {

        }
        public virtual void Reset()
        {

        }
        public virtual void Draw(SpriteBatch batch)
        {

        }
    }
}
