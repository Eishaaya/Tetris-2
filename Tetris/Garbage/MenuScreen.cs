using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class MenuScreen : Screen
    {
        Button chrisIsAPoopyHead;
        Button Unlimited;
        public MenuScreen(Button poopMakerOfff, Button infinite)
            :base()
        {
            chrisIsAPoopyHead = poopMakerOfff;            
            Unlimited = infinite;      
        }
        public override void Update(GameTime time, Screenmanager manny)
        {
            base.Update(time, manny);
            if (heldMouse)
            {
                return;
            }
            if (Unlimited.check(mousy.Position.ToVector2(), nou))
            {
                manny.next(1, true);
                return;
            }
            else if (chrisIsAPoopyHead.check(mousy.Position.ToVector2(), nou))
            {
                manny.next(2, true);
                return;
            }
        }
        public override void Draw(SpriteBatch batch)
        {
            chrisIsAPoopyHead.Draw(batch);
            Unlimited.Draw(batch);
        }

    }
}
