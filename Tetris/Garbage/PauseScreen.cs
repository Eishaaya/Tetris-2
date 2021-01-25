using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class PauseScreen : Screen
    {
        Button menu;
        Button back;
        Sprite tint;
        Keys exit;
        public PauseScreen(Sprite dark, Button menuButt, Button ReturnButt, Keys Exit = Keys.Escape)
            : base()
        {
            exit = Exit;
            tint = dark;
            menu = menuButt;
            back = ReturnButt;
        }

        public override void Update(GameTime time, Screenmanager manny)
        {
            base.Update(time, manny);
            if (heldMouse || keysDown)
            {
                return;
            }
            if (menu.check(mousy.Position.ToVector2(), nou))
            {
                manny.next(0, true);
                manny.previousScreens.Pop();
                manny.previousScreens.Pop().Reset();
                manny.clearMemory();
                return;
            }
            if (back.check(mousy.Position.ToVector2(), nou) || Maryland.IsKeyDown(exit) || nou)
            {
                manny.back();
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            tint.Draw(batch);
            back.Draw(batch);
            menu.Draw(batch);
        }
    }
}
