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
        //Texture2D menu
        Button menu;
        Button back;
        Button restart;
        Button setting;

        Sprite tint;
        Keys exit;
        public PauseScreen(Sprite dark, Button menuButt, Button ReturnButt, Button restartButt, Button settingButt, int number, Keys Exit = Keys.Escape)
            : base(number)
        {
            exit = Exit;
            tint = dark;
            menu = menuButt;
            restart = restartButt;
            back = ReturnButt;
            setting = settingButt;
        }

        public override void Update(GameTime time, Screenmanager manny, bool isActiveWindow)
        {           
            base.Update(time, manny, isActiveWindow);

            if (!isActiveWindow) return;

            var mousePos = mousy.Position.ToVector2();

            if (heldMouse || keysDown)
            {
                return;
            }
            if (menu.check(mousePos, isMouseClicked))
            {
                manny.next(0, true);
                manny.previousScreens.Pop();
                manny.previousScreens.Pop().Reset();
                manny.clearMemory();
                return;
            }
            else if (restart.check(mousePos, isMouseClicked))
            {
                manny.back();
                manny.peek().Reset();
                return;
            }
            else if (setting.check(mousePos, isMouseClicked))
            {
                manny.next(5, false);
                return;
            }
            if (back.check(mousePos, isMouseClicked) || Maryland.IsKeyDown(exit) || isMouseClicked)
            {
                manny.back();
                return;
            }

        }

        public override void Draw(SpriteBatch batch)
        {
            tint.Draw(batch);
            back.Draw(batch);
            menu.Draw(batch);
            restart.Draw(batch);
            setting.Draw(batch);
        }
    }
}