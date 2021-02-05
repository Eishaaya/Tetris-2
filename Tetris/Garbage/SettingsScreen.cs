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
        public List<Toggler> toggles;
        List<bool> toggOns;

        public SettingsScreen(Button d, Button a, Button ap, Button menuButton, Texture2D b, List<Keys> dk, List<Keys> ak, List<string> kt, List<bool> togs, Toggler template, SpriteFont font, SoundEffect effect)
            : base(effect)
        {
            bindButtons = new List<Button>();
            toggles = new List<Toggler>();
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
            toggOns = togs;

            for (int i = 0; i < defaults.Count / 2; i++)
            {
                bindLabels.Add(new Label(font, Color.White, new Vector2(150, i * 50 + 250), $"{keyTypes[i]} : {binds[i]}", TimeSpan.Zero));
                bindButtons.Add(new Button(b, new Vector2(150, i * 50 + 250), Color.Black, 0, SpriteEffects.None, new Vector2(0, 0), 1, .1f, Color.DarkGray, Color.Gray));
            }
            for (int i = defaults.Count / 2; i < defaults.Count; i++)
            {
                bindLabels.Add(new Label(font, Color.White, new Vector2(350, (i - defaults.Count / 2) * 50 + 250), $"{keyTypes[i]} : {binds[i]}", TimeSpan.Zero));
                bindButtons.Add(new Button(b, new Vector2(350, (i - defaults.Count / 2) * 50 + 250), Color.Black, 0, SpriteEffects.None, new Vector2(0, 0), 1, .1f, Color.DarkGray, Color.Gray));
            }
            int finalRow = 0;
            for (int i = 0; i < togs.Count; i++)
            {
                int j = i / 3;
                Vector2 offSet = new Vector2(100 + i * 150 - j * 450, j * 75 + 550);
                if (i % 3 == 0 && togs.Count - i < 3 || finalRow != 0)
                {
                    if (finalRow == 0)
                    {
                        finalRow = 75 * (togs.Count - i - 1);
                    }
                    offSet = new Vector2(offSet.X + finalRow, offSet.Y);
                }
                toggOns[i] = !toggOns[i];
                toggles.Add(new Toggler(template.Image, template.Location + offSet, template.Color, template.Rotation, template.Effects, template.Origin, template.Scale, template.Depth, template.HoverColor, template.ClickedColor,
                    new Sprite(template.ball.Image, template.ball.Location + offSet, template.ball.Color, template.ball.Rotation, template.ball.Effects, template.ball.Origin, template.ball.Scale, template.ball.Depth),
                    new Sprite(template.bottomColor.Image, template.bottomColor.Location + offSet, template.bottomColor.Color, template.bottomColor.Rotation, template.bottomColor.Effects, template.bottomColor.Origin, template.bottomColor.Scale, template.bottomColor.Depth),
                    new ScalableSprite(template.MovingColor.Image, template.MovingColor.Location + offSet, template.MovingColor.Color, template.MovingColor.Rotation, template.MovingColor.Effects, template.MovingColor.Origin, template.MovingColor.scale, template.MovingColor.Depth, template.MovingColor.Scale), font, keyTypes[i + binds.Count], 50, 0, 0, togs[i]));
            }
        }
        public override void Start()
        {
            base.Start();
            oldBinds = binds;
            for (int i = 0; i < toggles.Count; i++)
            {
                toggOns[i] = !toggles[i].on;
                toggles[i].done = false;
            }
        }
        public override List<bool> GetBools()
        {
            var allBools = new List<bool>();
            for (int i = 0; i < toggles.Count; i++)
            {
                allBools.Add(!toggles[i].on);
            }
            return allBools;
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
                    for (int i = 0; i < toggles.Count; i++)
                    {
                        toggles[i].on = !toggles[i].on;
                    }
                    manny.back();
                    return;
                }
                else if (menuButt.check(mousy.Position.ToVector2(), nou))
                {
                    binds = oldBinds;
                    manny.back();                    
                    for (int i = 0; i < toggles.Count; i++)
                    {
                        toggles[i].on = !toggOns[i];
                    }
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
                for (int i = 0; i < toggles.Count; i++)
                {
                    if (toggles[i].check(mousy.Position.ToVector2(), nou))
                    {
                        if (i == 0)
                        {
                            if (toggles[i].on)
                            {
                                StopMusic();
                                playMusic = false;
                            }
                            else
                            {
                                playMusic = true;
                                music.Resume();
                            }
                        }
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
            for (int i = 0; i < toggles.Count; i++)
            {
                toggles[i].Draw(batch);
            }
        }
    }
}
