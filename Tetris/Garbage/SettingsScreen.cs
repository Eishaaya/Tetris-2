using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class SettingsScreen : Screen
    {
        Button defaltButt;
        Button arrowButt;
        List<Button> bindButtons;
        List<Label> bindLabels;
        List<Keys> binds;
        List<Keys> defaults;
        List<Keys> arrows;
        List<string> keyTypes;

        public SettingsScreen(Button d, Button a, Texture2D b, List<Keys> dk, List<Keys> ak, List<string> kt, SpriteFont font)
        {
            defaults = dk;
            arrows = ak;
            binds = defaults;
            defaltButt = d;
            arrowButt = a;
            keyTypes = kt;

            for (int i = 0; i < defaults.Count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    bindButtons.Add(new Button(b, new Vector2(i * 50 + 100, j * 250 + 50), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1, Color.DarkGray, Color.Gray));
                    bindLabels.Add(new Label(font, Color.White, new Vector2(i * 55 + 100, j * 250 + 55), $"{keyTypes[i + j * binds.Count]} : {binds[i + j * binds.Count]}", TimeSpan.Zero));
                }
            }
        }
        public override void Update(GameTime time, Screenmanager manny)
        {
            base.Update(time, manny);
        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
            defaltButt.Draw(batch);
            arrowButt.Draw(batch);
            for (int i = 0; i < binds.Count; i++)
            {
                bindButtons[i].Draw(batch);
                bindLabels[i].write(batch);
            }
        }
    }
}
