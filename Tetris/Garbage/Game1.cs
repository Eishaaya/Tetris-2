using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Tetris
{
    public class Game1 : Game
    {
        enum texas
        {
            menu,
            levels,
            level,
            unlimited
        }
        List<List<Vector2>> locations;
        List<bool> symmetry;
        List<Color> colors;
        List<int> chances;
        List<int> scores;
        List<int> vals;
        List<float> diffs;
        List<Vector2> sizes;
        Sprite image;
        Sprite Gimage;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Grid grid;
        Grid clasgrid;
        Label laby;
        GameScreen game;
        GameScreen oldGame;
        MenuScreen menu;
        PauseScreen pause;
        LoseScreen lose;
        SettingsScreen settings;
        Screenmanager destroyerOfKarens;
        Button butty;
        Button pauser;
        Button resume;
        Button toMenu;
        Button toMenu2;
        Button toMenu3;
        Button apply;
        Button defaults;
        Button arrows;
        Button restart;
        Button restart2;
        Button complexButty;
        Button setting;
        Sprite tint;
        Sprite loser;
        SoundEffect music;
        SoundEffect unlimitedMusic;
        SoundEffect unlimitedIntro;
        SoundEffect menuMusic;
        SoundEffect settingsMusic;
        Sprite box;
        AnimatingSprite bottomScroll;
        SoundEffect turnEffect;
        SoundEffect landEffect;
        SoundEffect boomEffect;
        SoundEffect speedEffect;
        SoundEffect bossEffect;
        Toggler toggleMeUwu;
        Sprite betterTile;
        Timer secTimer;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 600;
            _graphics.PreferredBackBufferHeight = 890;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            secTimer = new Timer(1000);

            ObjectPool<Particle>.Instance.Populate(30, () => new Particle(null, Vector2.Zero, Color.White, 0, SpriteEffects.None, Vector2.Zero, Vector2.Zero, 0, 0, Vector2.Zero));
            ObjectPool<ParticleEffect>.Instance.Populate(3, () => new ParticleEffect());

            //music = Content.Load<Song>("Rick Astley - Never Gonna Give You Up [HQ]");
            music = Content.Load<SoundEffect>("Original Tetris theme (Tetris Soundtrack)-[AudioTrimmer.com]");
            unlimitedMusic = Content.Load<SoundEffect>("UnlimitedMusic");
            unlimitedIntro = Content.Load<SoundEffect>("UnlimitedIntro");
            menuMusic = Content.Load<SoundEffect>("MenuMusic");
            turnEffect = Content.Load<SoundEffect>("Rotate");
            landEffect = Content.Load<SoundEffect>("Landing");
            boomEffect = Content.Load<SoundEffect>("Boom");
            speedEffect = Content.Load<SoundEffect>("Zoom");
            settingsMusic = Content.Load<SoundEffect>("Settings");
            bossEffect = Content.Load<SoundEffect>("Boss");

            locations = new List<List<Vector2>>
            {
                new List<Vector2> { new Vector2 (0, 0) }, // D o t

                new List<Vector2> { new Vector2 (0, 0), new Vector2 (0, 1) }, // Short Beam

                new List<Vector2> { new Vector2 (0, 1), new Vector2 (0, 0), new Vector2 (0, 2) }, // Beam
                new List<Vector2> { new Vector2 (0, 0), new Vector2 (0, 1), new Vector2 (1, 0) }, // Corner

                new List<Vector2> { new Vector2 (0, 1), new Vector2 (0, 0), new Vector2 (0, 2), new Vector2(0, 3) }, // Long Beam
                new List<Vector2> { new Vector2 (0, 1), new Vector2 (0, 0), new Vector2 (1, 1), new Vector2(1, 0) }, // Square
                new List<Vector2> { new Vector2 (1, 1), new Vector2 (0, 0), new Vector2 (0, 1), new Vector2(1, 0), new Vector2(2, 1) }, // SquareNub L
                new List<Vector2> { new Vector2 (1, 1), new Vector2 (0, 0), new Vector2 (0, 1), new Vector2(1, 0), new Vector2(2, 0) }, // SquareNub R

                new List<Vector2> { new Vector2 (0, 1), new Vector2 (0, 0), new Vector2 (0, 2), new Vector2(1, 2) }, // L L
                new List<Vector2> { new Vector2 (1, 1), new Vector2 (1, 0), new Vector2 (1, 2), new Vector2(0, 2) }, // L R
                new List<Vector2> { new Vector2 (0, 1), new Vector2 (0, 0), new Vector2 (0, 2), new Vector2(1, 1) }, // half plus
                new List<Vector2> { new Vector2 (1, 0), new Vector2 (1, 1), new Vector2 (2, 0), new Vector2(0, 1) }, // Z R
                new List<Vector2> { new Vector2 (1, 0), new Vector2 (0, 0), new Vector2 (1, 1), new Vector2(2, 1) }, // Z l

                new List<Vector2> { new Vector2 (1, 1), new Vector2 (1, 0), new Vector2 (0, 1), new Vector2(2, 1), new Vector2(1, 2) }, // +
                new List<Vector2> { new Vector2 (1, 1), new Vector2 (0, 0), new Vector2 (0, 1), new Vector2(2, 1), new Vector2(2, 0) }, // U
                new List<Vector2> { new Vector2 (1, 1), new Vector2 (0, 0), new Vector2 (0, 1), new Vector2(1, 2), new Vector2(2, 2) }, // Diaganol W
                new List<Vector2> { new Vector2 (1, 1), new Vector2 (0, 0), new Vector2 (1, 0), new Vector2(2, 0), new Vector2(1, 2) }, // Dirty British Goods (throw it in the damned harbor)

                new List<Vector2> { new Vector2 (2, 1), new Vector2 (0, 1), new Vector2 (1, 1), new Vector2(1, 0), new Vector2(3, 1), new Vector2(4, 1) }, // Lump Line L
                new List<Vector2> { new Vector2 (2, 1), new Vector2 (0, 1), new Vector2 (1, 1), new Vector2(3, 0), new Vector2(3, 1), new Vector2(4, 1) }, //Lump Line R

                new List<Vector2> { new Vector2 (2, 0), new Vector2 (1, 1), new Vector2 (2, 1), new Vector2(3, 1), new Vector2(0, 2), new Vector2(1, 2), new Vector2 (3, 2), new Vector2 (4, 2), new Vector2 (1, 3), new Vector2(2, 3), new Vector2(3, 3), new Vector2(2, 4) } //Death Donut
      
            };

            sizes = new List<Vector2>
            {
                new Vector2(60, 60),

                 new Vector2(60, 120),

                 new Vector2(60, 180),
                 new Vector2(120, 120),

                 new Vector2(60, 240),
                 new Vector2(120, 120),
                 new Vector2(180, 120),
                 new Vector2(180, 120),

                 new Vector2(120, 180),
                 new Vector2(120, 180),
                 new Vector2(120, 180),
                 new Vector2(180, 120),
                 new Vector2(180, 120),

                 new Vector2(180, 180),
                 new Vector2(180, 120),
                 new Vector2(180, 180),
                 new Vector2(180, 180),

                 new Vector2(300, 120),
                 new Vector2(300, 120),

                 new Vector2(300, 300)
            };

            vals = new List<int>
            {
                2,

                3,

                6,
                8,

                8,
                10,
                11,
                11,

                10,
                10,
                10,
                10,
                10,

                20,
                15,
                18,
                18,

                27,
                27,

                75,
            };

            chances = new List<int>
            {
                40,

                55,

                62,
                70,

                65,
                100,
                50,
                50,

                120,
                120,
                120,
                120,
                120,

                45,
                65,
                60,
                55,

                15,
                15,

                4,
            };

            colors = new List<Color>
            {
                Color.Gray,

                Color.Navy,

                Color.DarkBlue,
                Color.LimeGreen,

                Color.Blue,
                Color.MediumSlateBlue,
                Color.SlateBlue,
                Color.DarkSlateBlue,

                Color.Purple,
                Color.MediumPurple,
                Color.Crimson,
                Color.DarkGreen,
                Color.LawnGreen,

                Color.Red,
                Color.CornflowerBlue,
                Color.ForestGreen,
                Color.DarkRed,

                Color.HotPink,
                Color.DeepPink,

                Color.DarkGray,
            };

            symmetry = new List<bool>
            {
                true,

                false,

                false,
                false,

                false,
                true,
                false,
                false,

                false,
                false,
                false,
                false,
                false,

                true,
                false,
                false,
                false,

                false,
                false,

                true
            };

            diffs = new List<float>
            {
                -2,

                -1.75f,

                -1.5f,
                -.5f,

                -.5f,
                0,
                0.15f,
                0.15f,

                0,
                0,
                0,
                0,
                0,

                2,
                1,
                1.5f,
                1.8f,

                3,
                3,

                3
            };

            bottomScroll = new AnimatingSprite(Content.Load<Texture2D>("bottom"), new Vector2(0, 800), Color.White, 0, SpriteEffects.None, new Rectangle(0, 0, 0, 0), new Vector2(0, 0), 1, .025f, new RectangleFrame[]
            {
                new Rectangle(0, 0, 600, 90),
                new Rectangle(0, 91, 600, 90),
                new Rectangle(0, 182, 600, 90),
                new Rectangle(0, 273, 600, 90),
                new Rectangle(0, 364, 600, 90),
                new Rectangle(0, 455, 600, 90),
                new Rectangle(0, 546, 600, 90),
                new Rectangle(0, 637, 600, 90),
                new Rectangle(0, 728, 600, 90),
                new Rectangle(0, 819, 600, 90),
                new Rectangle(0, 910, 600, 90),
                new Rectangle(0, 1001, 600, 90),
                new Rectangle(0, 1092, 600, 90),
                new Rectangle(0, 1183, 600, 90),
                new Rectangle(0, 1274, 600, 90)
            }, 80);
            laby = new Label(Content.Load<SpriteFont>("Font"), Color.White, new Vector2(425, 20), "Score: \n", new TimeSpan(0, 0, 0), new Vector2(0, 0), 0, SpriteEffects.None, 1, .8f);
            image = new Sprite(Content.Load<Texture2D>("Tile"), new Vector2(30, 30), Color.White, 0, SpriteEffects.None, new Vector2(30, 30), 1, 0.1f);
            betterTile = new Sprite(Content.Load<Texture2D>("Better Tile"), new Vector2(30, 30), Color.White, 0, SpriteEffects.None, new Vector2(30, 30), 1, 0.1f);
            tint = new Sprite(Content.Load<Texture2D>("darkener"), new Vector2(0, 0), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, .9f);
            box = new Sprite(Content.Load<Texture2D>("Nexts"), new Vector2(400, 290), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, .8f);
            loser = new Sprite(Content.Load<Texture2D>("Game Over Screen"), new Vector2(150, 198), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, .95f);
            Gimage = new Sprite(Content.Load<Texture2D>("grid"), new Vector2(30, 30), Color.White, 0, SpriteEffects.None, new Vector2(30, 30), 0, 0);
            grid = new Grid(new Vector2(10, 20), Gimage, locations, symmetry, colors, chances, vals, diffs, sizes, image, turnEffect, landEffect, boomEffect, speedEffect, bossEffect, Content.Load<Texture2D>("ShadowImage"), .6667f, false, Content.Load<Texture2D>("TileCover"), Content.Load<Texture2D>("Maybe Explosive"), Content.Load<Texture2D>("Speeder Tile"), Content.Load<Texture2D>("BoomBit"), Content.Load<Texture2D>("Speed Particle"));
            butty = new Button(Content.Load<Texture2D>("Classic button"), new Vector2(GraphicsDevice.Viewport.Width / 2 - 150, 60), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1, Color.Gray, Color.DarkGray);
            pauser = new Button(Content.Load<Texture2D>("Pause symbol alt"), new Vector2(435, 120), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1, Color.Gray, Color.DarkGray);
            complexButty = new Button(Content.Load<Texture2D>("Power Unlimited Power"), new Vector2(GraphicsDevice.Viewport.Width / 2 + 16, 60), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1, Color.Gray, Color.DarkGray);
            resume = new Button(Content.Load<Texture2D>("Resume Button"), new Vector2(GraphicsDevice.Viewport.Width / 2 - 134, 60), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1, Color.Gray, Color.DarkGray);




            toMenu = new Button(Content.Load<Texture2D>("Menu button"), new Vector2(GraphicsDevice.Viewport.Width / 2, 60), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1, Color.Gray, Color.DarkGray);
            toMenu2 = new Button(Content.Load<Texture2D>("Menu button"), new Vector2(GraphicsDevice.Viewport.Width / 2 + 16, 504), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1, Color.Gray, Color.DarkGray);
            toMenu3 = new Button(Content.Load<Texture2D>("Menu button"), new Vector2(GraphicsDevice.Viewport.Width / 2 + 25, 750), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1, Color.Gray, Color.DarkGray);
            setting = new Button(Content.Load<Texture2D>("SettingsButt"), new Vector2(GraphicsDevice.Viewport.Width / 2 - 67, 172), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1, Color.Gray, Color.DarkGray);
            restart = new Button(Content.Load<Texture2D>("Restart Button"), new Vector2(GraphicsDevice.Viewport.Width / 2 - 150, 504), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1, Color.Gray, Color.DarkGray);

            restart2 = restart.Clone();
            restart2.Location = new Vector2(GraphicsDevice.Viewport.Width / 2 - 134, 140);
            //restart2 = new Button(Content.Load<Texture2D>("Restart Button"), new Vector2(GraphicsDevice.Viewport.Width / 2 - 67, 140), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1, Color.Gray, Color.DarkGray);

            var pSetting = setting.Clone();
            pSetting.Location = new Vector2(GraphicsDevice.Viewport.Width / 2, 140);

            apply = new Button(Content.Load<Texture2D>("Applu"), new Vector2(GraphicsDevice.Viewport.Width / 2 - 159, 750), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1, Color.Gray, Color.DarkGray);
            defaults = new Button(Content.Load<Texture2D>("Default"), new Vector2(GraphicsDevice.Viewport.Width / 2 - 159, 75), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1, Color.Gray, Color.DarkGray);
            arrows = new Button(Content.Load<Texture2D>("Arrows"), new Vector2(GraphicsDevice.Viewport.Width / 2 + 29, 75), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1, Color.Gray, Color.DarkGray);


            toggleMeUwu = new Toggler(Content.Load<Texture2D>("Toggle Base"), new Vector2(0, 0), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, .9f, Color.Gray, Color.Black,
            new Sprite(Content.Load<Texture2D>("Toggle Ball"), new Vector2(0, 0), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1),
            new Sprite(Content.Load<Texture2D>("Toggle Color"), new Vector2(0, 0), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, .7f),
            new ScalableSprite(Content.Load<Texture2D>("ToggleBaseColor"), new Vector2(0, 0), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), new Vector2(0, 1), .8f));
            clasgrid = new Grid(
            new Vector2(10, 20), Gimage, new List<List<Vector2>>
            {
                new List<Vector2> { new Vector2 (0, 1), new Vector2 (0, 0), new Vector2 (0, 2), new Vector2(0, 3) }, // Long Beam
                new List<Vector2> { new Vector2 (0, 1), new Vector2 (0, 0), new Vector2 (1, 1), new Vector2(1, 0) }, // Square

                new List<Vector2> { new Vector2 (0, 1), new Vector2 (0, 0), new Vector2 (0, 2), new Vector2(1, 2) }, // L L
                new List<Vector2> { new Vector2 (1, 1), new Vector2 (1, 0), new Vector2 (1, 2), new Vector2(0, 2) }, // L R
                new List<Vector2> { new Vector2 (0, 1), new Vector2 (0, 0), new Vector2 (0, 2), new Vector2(1, 1) }, // half plus
                new List<Vector2> { new Vector2 (1, 0), new Vector2 (1, 1), new Vector2 (2, 0), new Vector2(0, 1) }, // Z R
                new List<Vector2> { new Vector2 (1, 0), new Vector2 (0, 0), new Vector2 (1, 1), new Vector2(2, 1) } // Z l
            },
            new List<bool>
            {
                false,
                true,

                false,
                false,
                false,
                false,
                false
            },
            new List<Color>
            {
                Color.Blue,
                Color.MediumSlateBlue,
                Color.Purple,
                Color.MediumPurple,
                Color.Crimson,
                Color.DarkGreen,
                Color.LawnGreen
            },
            new List<int>
            {
                75,
                100,

                100,
                100,
                100,
                100,
                100,
            },
            new List<int>
            {
                6,
                8,

                8,
                8,
                8,
                8,
                8
            },
            new List<float>
            {
                -3,
                0,

                0,
                0,
                0,
                0,
                0
            },
            new List<Vector2>
            {
                 new Vector2(60, 240),
                 new Vector2(120, 120),

                 new Vector2(120, 180),
                 new Vector2(120, 180),
                 new Vector2(120, 180),
                 new Vector2(180, 120),
                 new Vector2(180, 120),
            },
            image, turnEffect, landEffect, boomEffect, null, null, Content.Load<Texture2D>("ShadowImage"), .6667f, true);

            var boxPlaces = new List<Vector2>
            {
                new Vector2(406, 296),
                new Vector2(406, 496),
                new Vector2(406, 696)
            };

            bool[] toggleSettings;

            if (!File.Exists("gameData.json"))
            {
                //backwards compatability/file creation
                if (File.Exists("data.json"))
                {
                    StorageObject.Instance.OldRead();
                }
                StorageObject.Instance.binds = new List<Keys>
                {
                    Keys.S,
                    Keys.W,
                    Keys.A,
                    Keys.D,
                    Keys.Space,
                    Keys.D1,
                    Keys.D2,
                    Keys.D3,
                    Keys.D4,
                    Keys.Escape
                };

                StorageObject.Instance.settings = new List<bool>
                {
                    true,
                    true,
                    true,
                    true,
                    true,
                    true,
                    true,
                    false,
                };
                StorageObject.Instance.Write();
            }
            else
            {
                StorageObject.Instance.Read();
            }

            pause = new PauseScreen(tint, toMenu, resume, restart2, pSetting, 3);
            game = new GameScreen(grid, laby, pauser, box, bottomScroll, Content.Load<Texture2D>("hoverBrick"), boxPlaces, unlimitedMusic, unlimitedIntro, 1);
            oldGame = new GameScreen(clasgrid, laby, pauser, box, bottomScroll, Content.Load<Texture2D>("hoverBrick"), boxPlaces, music, null, 2);
            lose = new LoseScreen(tint, loser, toMenu2, restart, Content.Load<SpriteFont>("File"), 4);
            menu = new MenuScreen(butty, complexButty, menuMusic, setting, 0);
            settings = new SettingsScreen(defaults, arrows, apply, toMenu3, Content.Load<Texture2D>("BindButt"), StorageObject.Instance.binds, new List<Keys>
            {
                Keys.S,
                Keys.W,
                Keys.A,
                Keys.D,
                Keys.Space,
                Keys.D1,
                Keys.D2,
                Keys.D3,
                Keys.D4,
                Keys.Escape
            }, new List<Keys>
            {
                Keys.Down,
                Keys.Up,
                Keys.Left,
                Keys.Right,
                Keys.Space,
                Keys.D1,
                Keys.D2,
                Keys.D3,
                Keys.D4,
                Keys.Escape
            }, new List<string>
            {
                "Down",
                "Turn",
                "Left",
                "Right",
                "Drop",
                "Next",
                "Last",
                "Swap", 
                "Queue",
                "Pause",
                "Music",
                "Sound",
                "Held Turning",
                "Held Vertical",
                "Held Horizontal",
                "Piece Projection",
                "Restart On Menu",
                "Store New Settings"
            }, StorageObject.Instance.settings, toggleMeUwu, Content.Load<SpriteFont>("File"), settingsMusic, resume.Image, 5, GraphicsDevice, Color.Black);
            destroyerOfKarens = new Screenmanager(new List<Screen> { menu, game, oldGame, pause, lose, settings });

            destroyerOfKarens.assignBinds();
        }

        protected override void Update(GameTime gameTime)
        {
            //debug condition IGNORE
            //secTimer.Tick(gameTime);
            //if (secTimer.Ready())
            //{
            //    System.Diagnostics.Debug.WriteLine($"{IsActive}");
            //}
            destroyerOfKarens.Update(gameTime, IsActive);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack);
            destroyerOfKarens.Draw(_spriteBatch);
            //toggleMeUwu.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
