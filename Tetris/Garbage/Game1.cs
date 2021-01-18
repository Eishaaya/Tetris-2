using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        Screenmanager destroyerOfKarens;
        Button butty;
        Button pauser;
        Button resume;
        Button toMenu;
        Button toMenu2;
        Button restart;
        Button complexButty;
        Sprite tint;
        Sprite loser;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 600;
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            if (!File.Exists("data.json"))
            {
                var stuffToWrite = JsonSerializer.Serialize(new List<int>());
                File.WriteAllText("data.json", stuffToWrite);
            }
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            locations = new List<List<Vector2>>
            {
                new List<Vector2> { new Vector2 (0, 0) }, // D o t

                new List<Vector2> { new Vector2 (0, 0), new Vector2 (0, 1) }, // Short Beam

                new List<Vector2> { new Vector2 (0, 1), new Vector2 (0, 0), new Vector2 (0, 2) }, // Beam
                new List<Vector2> { new Vector2 (0, 0), new Vector2 (0, 1), new Vector2 (1, 0) }, // Corner

                new List<Vector2> { new Vector2 (0, 1), new Vector2 (0, 0), new Vector2 (0, 2), new Vector2(0, 3) }, // Long Beam
                new List<Vector2> { new Vector2 (0, 1), new Vector2 (0, 0), new Vector2 (1, 1), new Vector2(1, 0) }, // Square
                new List<Vector2> { new Vector2 (1, 1), new Vector2 (0, 0), new Vector2 (0, 1), new Vector2(1, 0), new Vector2(2, 1) }, // SquareNub


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

            vals = new List<int>
            {
                2,

                3,

                6,
                8,

                8,
                10,
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

                70,
                75,

                75,
                85,
                90,

                100,
                100,
                100,
                100,
                100,

                50,
                80,
                65,
                60,

                20,
                20,

                7,
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
                -1,

                -.5f,
                0,
                0.1f,

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


            laby = new Label(Content.Load<SpriteFont>("Font"), Color.White, new Vector2(450, 20), "Score: \n", new TimeSpan(0, 0, 0));
            image = new Sprite(Content.Load<Texture2D>("tile"), new Vector2(30, 30), Color.White, 0, SpriteEffects.None, new Vector2(30, 30), 1, 1);
            tint = new Sprite(Content.Load<Texture2D>("darkener"), new Vector2(0, 0), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1);
            loser = new Sprite(Content.Load<Texture2D>("Game Over Screen"), new Vector2(150, 198), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1);
            Gimage = new Sprite(Content.Load<Texture2D>("grid"), new Vector2(30, 30), Color.White, 0, SpriteEffects.None, new Vector2(30, 30), 1, 1);
            grid = new Grid(new Vector2(10, 20), Gimage, locations, symmetry, colors, chances, vals, diffs, image, (float).6667);
            butty = new Button(Content.Load<Texture2D>("Classic button"), new Vector2(GraphicsDevice.Viewport.Width / 2 - 150, 60), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1, Color.Gray, Color.Gray);
            pauser = new Button(Content.Load<Texture2D>("Pause symbol alt"), new Vector2(435, 120), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1, Color.Gray, Color.Gray);
            complexButty = new Button(Content.Load<Texture2D>("Power Unlimited Power"), new Vector2(GraphicsDevice.Viewport.Width / 2 + 16, 60), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1, Color.Gray, Color.Gray);
            resume = new Button(Content.Load<Texture2D>("Resume Button"), new Vector2(GraphicsDevice.Viewport.Width / 2 - 134, 60), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1, Color.Gray, Color.Gray);
            toMenu = new Button(Content.Load<Texture2D>("Menu button"), new Vector2(GraphicsDevice.Viewport.Width / 2, 60), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1, Color.Gray, Color.Gray);
            toMenu2 = new Button(Content.Load<Texture2D>("Menu button"), new Vector2(GraphicsDevice.Viewport.Width / 2 + 16, 504), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1, Color.Gray, Color.Gray);
            restart = new Button(Content.Load<Texture2D>("Restart Button"), new Vector2(GraphicsDevice.Viewport.Width / 2 - 150, 504), Color.White, 0, SpriteEffects.None, new Vector2(0, 0), 1, 1, Color.Gray, Color.Gray);
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
                10,
                10,

                10,
                10,
                10,
                10,
                10
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
            image, .6667f);


            pause = new PauseScreen(tint, toMenu, resume);
            game = new GameScreen(grid, laby, pauser);
            oldGame = new GameScreen(clasgrid, laby, pauser);
            lose = new LoseScreen(tint, loser, toMenu2, restart, Content.Load<SpriteFont>("File"));
            menu = new MenuScreen(butty, complexButty);
            destroyerOfKarens = new Screenmanager(new List<Screen> { menu, game, oldGame, pause, lose });
        }

        protected override void Update(GameTime gameTime)
        {
            destroyerOfKarens.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            destroyerOfKarens.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
