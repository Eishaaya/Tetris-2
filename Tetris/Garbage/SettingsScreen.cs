using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        Button applyButt;
        Button menuButt;
        List<Button> bindButtons;
        List<Label> bindLabels;
        List<Keys> defaults;
        List<Keys> arrows;
        List<string> keyTypes;
        List<Keys> oldBinds;
        int index;

        public SettingsScreen(Button d, Button a, Button ap, Button menuButton, Texture2D b, List<Keys> dk, List<Keys> ak, List<string> kt, SpriteFont font, SoundEffect effect)
            : base(effect)
        {
            bindButtons = new List<Button>();
            bindLabels = new List<Label>();
            keyTypes = new List<string>();
            applyButt = ap;
            defaults = dk;
            arrows = ak;
            menuButt = menuButton;
            binds = defaults;
            defaltButt = d;
            arrowButt = a;
            keyTypes = kt;
            oldBinds = binds;
            index = -1;

            for (int i = 0; i < defaults.Count / 2; i++)
            {
                bindLabels.Add(new Label(font, Color.White, new Vector2(150, i * 50 + 250), $"{keyTypes[i]} : {binds[i]}", TimeSpan.Zero));
                bindButtons.Add(new Button(b, new Vector2(150, i * 50 + 250), Color.Black, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1, Color.DarkGray, Color.Gray));
            }
            for (int i = defaults.Count / 2; i < defaults.Count; i++)
            {
                bindLabels.Add(new Label(font, Color.White, new Vector2(350, (i - defaults.Count / 2) * 50 + 250), $"{keyTypes[i]} : {binds[i]}", TimeSpan.Zero));
                bindButtons.Add(new Button(b, new Vector2(350, (i - defaults.Count / 2) * 50 + 250), Color.Black, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1, Color.DarkGray, Color.Gray));
            }
        }
        public override void Start()
        {
            base.Start();
            oldBinds = binds;
        }
        public override void Update(GameTime time, Screenmanager manny)
        {
            base.Update(time, manny);
            if (heldMouse)
            {
                return;
            }
            if (index < 0)
            {
                if (defaltButt.check(mousy.Position.ToVector2(), nou))
                {
                    binds = defaults;
                    for (int i = 0; i < binds.Count; i++)
                    {
                        bindLabels[i].Text = $"{keyTypes[i]} : {binds[i]}";
                    }
                }
                else if (arrowButt.check(mousy.Position.ToVector2(), nou))
                {
                    binds = arrows;
                    for (int i = 0; i < binds.Count; i++)
                    {
                        bindLabels[i].Text = $"{keyTypes[i]} : {binds[i]}";
                    }
                }
                else if (applyButt.check(mousy.Position.ToVector2(), nou))
                {
                    manny.bindsChanged = true;
                    manny.back();
                    return;
                }
                else if (menuButt.check(mousy.Position.ToVector2(), nou))
                {
                    binds = oldBinds;
                    manny.back();
                    return;
                }

                for (int i = 0; i < binds.Count; i++)
                {
                    if (bindButtons[i].check(mousy.Position.ToVector2(), nou))
                    {
                        bindLabels[i].Text = $"{keyTypes[i]} : ";
                        index = i;
                    }
                }
            }
            else
            {
                if (!bindButtons[index].check(mousy.Position.ToVector2(), nou) && nou)
                {
                    bindLabels[index].Text = $"{keyTypes[index]} : {binds[index]}";
                    index = -1;
                    return;
                }
                if (Maryland.GetPressedKeyCount() > 0)
                {
                    binds[index] = Maryland.GetPressedKeys()[0];
                    bindLabels[index].Text = $"{keyTypes[index]} : {binds[index]}";
                    index = -1;
                    return;
                }
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
            defaltButt.Draw(batch);
            arrowButt.Draw(batch);
            menuButt.Draw(batch);
            applyButt.Draw(batch);
            for (int i = 0; i < binds.Count; i++)
            {
                bindButtons[i].Draw(batch);
                bindLabels[i].write(batch);
            }
        }
    }
}
