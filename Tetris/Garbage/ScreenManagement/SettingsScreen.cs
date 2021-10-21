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
        bool firstOpen = true;
        Sprite backGround;
        Dictionary<int, Texture2D> backTextures;
        Button defaultButt;
        Button arrowButt;
        Button applyButt;
        Button backButt;
        List<Button> bindButtons;
        List<Label> bindLabels;
        List<Keys> defaults;
        List<Keys> arrows;
        List<string> keyTypes;
        List<Keys> oldBinds;
        int index;
        public List<Toggler> toggles;
        public List<bool> ToggOns { get; private set; }

        public SettingsScreen(Button d, Button a, Button ap, Button menuButton, Texture2D b, List<Keys> ck, List<Keys> dk, List<Keys> ak,
                              List<string> kt, List<bool> togs, Toggler template, SpriteFont font, SoundEffect effect, Texture2D otherBack, int number, GraphicsDevice graphicsDevice, Color backColor)
            : this(d, a, ap, menuButton, b, ck, dk, ak, kt, togs, template, font, effect, otherBack, number, null)
        {
            var backTexture = new Texture2D(graphicsDevice, 1, 1);
            backTexture.SetData(new Color[] { backColor });
            backGround = new Sprite(backTexture, Vector2.Zero);
            backGround.Scale = 2000;
            backGround.Depth = 1;
        }

        public SettingsScreen(Button defaults, Button arrows, Button apply, Button menuButton, Texture2D backTxt, List<Keys> currentKeys, List<Keys> defaultKeys, List<Keys> arrowKeys,
                              List<string> kt, List<bool> togs, Toggler template, SpriteFont font, SoundEffect effect, Texture2D otherBack, int number, Texture2D backGroundTxt)
            : base(effect, number)
        {
            bindButtons = new List<Button>();
            toggles = new List<Toggler>();
            bindLabels = new List<Label>();
            keyTypes = new List<string>();
            applyButt = apply;
            this.defaults = defaultKeys;
            this.arrows = arrowKeys;
            backButt = menuButton;
            binds = new List<Keys>(currentKeys);
            defaultButt = defaults;
            arrowButt = arrows;
            keyTypes = kt;
            oldBinds = binds;
            index = -1;
            ToggOns = new List<bool>(togs);

            backTextures = new Dictionary<int, Texture2D>() { [0] = menuButton.Image, [3] = otherBack };
            if (backGroundTxt != null)
            {
                backGround = new Sprite(backGroundTxt, Vector2.Zero);
            }


            for (int i = 0; i < this.defaults.Count / 2; i++)
            {
                bindLabels.Add(new Label(font, Color.White, new Vector2(150, i * 50 + 225), $"{keyTypes[i]} : {binds[i]}", TimeSpan.Zero));
                bindButtons.Add(new Button(backTxt, new Vector2(150, i * 50 + 215), Color.Black, 0, SpriteEffects.None, new Vector2(0, 0), 1, .1f, Color.DarkGray, Color.Gray));
            }
            for (int i = this.defaults.Count / 2; i < this.defaults.Count; i++)
            {
                bindLabels.Add(new Label(font, Color.White, new Vector2(350, (i - this.defaults.Count / 2) * 50 + 225), $"{keyTypes[i]} : {binds[i]}", TimeSpan.Zero));
                bindButtons.Add(new Button(backTxt, new Vector2(350, (i - this.defaults.Count / 2) * 50 + 215), Color.Black, 0, SpriteEffects.None, new Vector2(0, 0), 1, .1f, Color.DarkGray, Color.Gray));
            }
            int finalRow = 0;
            for (int i = 0; i < ToggOns.Count; i++)
            {
                int j = i / 3;
                Vector2 offSet = new Vector2(100 + i * 150 - j * 450, j * 75 + 500);
                if (i % 3 == 0 && ToggOns.Count - i < 3 || finalRow != 0)
                {
                    if (finalRow == 0)
                    {
                        finalRow = 75 * (3 - (ToggOns.Count - i));
                    }
                    offSet = new Vector2(offSet.X + finalRow, offSet.Y);
                }
                toggles.Add(new Toggler(template.Image, template.Location + offSet, template.Color, template.rotation, template.effect, template.Origin, template.Scale, template.Depth, template.HoverColor, template.ClickedColor,
                    new Sprite(template.ball.Image, template.ball.Location + offSet, template.ball.Color, template.ball.rotation, template.ball.effect, template.ball.Origin, template.ball.Scale, template.ball.Depth),
                    new Sprite(template.bottomColor.Image, template.bottomColor.Location + offSet, template.bottomColor.Color, template.bottomColor.rotation, template.bottomColor.effect, template.bottomColor.Origin, template.bottomColor.Scale, template.bottomColor.Depth),
                    new ScalableSprite(template.MovingColor.Image, template.MovingColor.Location + offSet, template.MovingColor.Color, template.MovingColor.rotation, template.MovingColor.effect, template.MovingColor.Origin, template.MovingColor.scale, template.MovingColor.Depth, template.MovingColor.Scale), font, keyTypes[i + binds.Count], 50, 0, 0, !ToggOns[i]));
            }
        }
        public void setTexture(int i)
        {
            backButt.Image = backTextures[i];
        }

        public override void Start(int caller)
        {
            base.Start(caller);

            if (firstOpen)
            {
                binds = StorageObject.Instance.binds;
                ToggOns = StorageObject.Instance.settings;

                firstOpen = false;
            }


            if (backTextures.ContainsKey(caller))
            {
                backButt.Image = backTextures[caller];
            }


            oldBinds = binds;
            for (int i = 0; i < toggles.Count; i++)
            {
                ToggOns[i] = !toggles[i].on;
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

        Screen getGame(Screenmanager manny)
        {
            var prevScreens = new Stack<Screen>(manny.previousScreens);
            Screen gameScreen = this;
            while (!(gameScreen is GameScreen))
            {
                gameScreen = prevScreens.Pop();
            }
            return gameScreen;
        }
        public override void Update(GameTime time, Screenmanager manny, bool isActiveWindow)
        {
            base.Update(time, manny, isActiveWindow);
            if (!isActiveWindow) return;
            if (caller == 3)
            {
                StopMusic();
            }
            if (heldMouse)
            {
                return;
            }
            if (index < 0)
            {
                if (defaultButt.check(mousy.Position.ToVector2(), isMouseClicked))
                {
                    binds = defaults;
                    for (int i = 0; i < binds.Count; i++)
                    {
                        bindLabels[i].SetText($"{keyTypes[i]} : {binds[i]}");
                    }
                }
                else if (arrowButt.check(mousy.Position.ToVector2(), isMouseClicked))
                {
                    binds = arrows;
                    for (int i = 0; i < binds.Count; i++)
                    {
                        bindLabels[i].SetText($"{keyTypes[i]} : {binds[i]}");
                    }
                }
                else if (applyButt.check(mousy.Position.ToVector2(), isMouseClicked))
                {

                    if (toggles[7].on)
                    {
                        StorageObject.Instance.binds = binds;
                        for (int i = 0; i < toggles.Count; i++)
                        {
                            StorageObject.Instance.settings[i] = toggles[i].on;
                        }
                    }
                    else
                    {
                        StorageObject.Instance.settings[7] = false;
                    }
                    StorageObject.Instance.Write();


                    manny.bindsChanged = true;
                    for (int i = 0; i < toggles.Count; i++)
                    {
                        toggles[i].on = !toggles[i].on;
                    }

                    manny.back();
                    return;
                }
                else if (backButt.check(mousy.Position.ToVector2(), isMouseClicked))
                {
                    binds = oldBinds;
                    manny.back();
                    for (int i = 0; i < toggles.Count; i++)
                    {
                        toggles[i].on = !ToggOns[i];
                    }
                    return;
                }

                for (int i = 0; i < binds.Count; i++)
                {
                    if (bindButtons[i].check(mousy.Position.ToVector2(), isMouseClicked))
                    {
                        bindLabels[i].SetText($"{keyTypes[i]} : ");
                        index = i;
                    }
                }
                for (int i = 0; i < toggles.Count; i++)
                {
                    if (toggles[i].check(mousy.Position.ToVector2(), isMouseClicked))
                    {
                        if (i == 0)
                        {
                            if (toggles[i].on)
                            {
                                StopMusic();
                                WillPlayMusic = false;
                                if (caller == 3)
                                {
                                    var gameScreen = getGame(manny);
                                    gameScreen.StopMusic();
                                    gameScreen.WillPlayMusic = false;
                                }
                            }
                            else
                            {
                                WillPlayMusic = true;
                                if (caller != 3)
                                {
                                    music.Resume();
                                }
                                else
                                {
                                    var gameScreen = getGame(manny);
                                    gameScreen.WillPlayMusic = true;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (!bindButtons[index].check(mousy.Position.ToVector2(), isMouseClicked) && isMouseClicked)
                {
                    bindLabels[index].SetText($"{keyTypes[index]} : {binds[index]}");
                    index = -1;
                    return;
                }
                if (Maryland.GetPressedKeyCount() > 0)
                {
                    binds[index] = Maryland.GetPressedKeys()[0];
                    bindLabels[index].SetText($"{keyTypes[index]} : {binds[index]}");
                    index = -1;
                    return;
                }
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.End();
            batch.Begin();
            base.Draw(batch);
            backGround.Draw(batch);
            defaultButt.Draw(batch);
            arrowButt.Draw(batch);
            backButt.Draw(batch);
            applyButt.Draw(batch);
            for (int i = 0; i < binds.Count; i++)
            {
                bindButtons[i].Draw(batch);
                bindLabels[i].Print(batch);
            }
            for (int i = 0; i < toggles.Count; i++)
            {
                toggles[i].Draw(batch);
            }
        }
    }
}
