using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class MenuScreen : Screen
    {
        Button classic;
        Button unlimited;
        Button setting;
        public MenuScreen(Button poopMakerOfff, Button infinite, SoundEffect music, Button sett, int number)
            :base(music, number)
        {
            classic = poopMakerOfff;            
            unlimited = infinite;
            setting = sett;
        }
        public override void Update(GameTime time, Screenmanager manny, bool isActiveWindow)
        {
            base.Update(time, manny, isActiveWindow);

            if (!isActiveWindow) return;

            Play(time);
            if (heldMouse)
            {
                return;
            }
            if (unlimited.check(mousy.Position.ToVector2(), isMouseClicked))
            {
                manny.next(1, true);
                return;
            }
            else if (classic.check(mousy.Position.ToVector2(), isMouseClicked))
            {
                manny.next(2, true);
                return;
            }
            else if (setting.check(mousy.Position.ToVector2(), isMouseClicked))
            {
                manny.next(5, true);                
                return;
            }
        }
        public override void Draw(SpriteBatch batch)
        {
            classic.Draw(batch);
            unlimited.Draw(batch);
            setting.Draw(batch);
        }

    }
}
