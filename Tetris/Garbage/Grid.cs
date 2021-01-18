using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    //10x20
    class Grid
    {
        Random random;
        Sprite empty;
        Vector2 size;
        List<List<Coordinate>> map;
        double scale;
        RatPooeys pooey;
        Keys downKey;
        Keys leftKey;
        Keys rightKey;
        Keys turnKey;
        Keys TeleKey;
        KeyboardState California;
        Timer NevadaCheck;
        List<List<Vector2>> locations;
        List<bool> symmetry;
        List<Color> colors;
        List<int> spawnChances;
        List<int> values;
        List<float> difficulty;
        public int score;
        Sprite image;
        bool fullDown;
        bool downHeld;
        RatPooeys nextPooey;
        RatPooeys lastPooey;
        int nevadaready;
        public bool lose;
        int progression;
        public Grid(Vector2 s, Sprite e, List<List<Vector2>> ln, List<bool> sy, List<Color> c, List<int> ch, List<int> va, List<float> ds, Sprite im, float sc = 1, int n = 100, Keys d = Keys.S, Keys t = Keys.W, Keys l = Keys.A, Keys r = Keys.D, Keys D = Keys.Space)
        {
            lose = false;
            nevadaready = n;
            score = 0;
            random = new Random();
            NevadaCheck = new Timer(new TimeSpan(0, 0, 0, 0, n));
            downKey = d;
            image = im;
            leftKey = l;
            rightKey = r;
            turnKey = t;
            TeleKey = D;
            scale = sc;
            size = new Vector2(s.X, s.Y + 6);
            empty = e;
            locations = ln;
            symmetry = sy;
            colors = c;
            values = va;
            spawnChances = ch;
            difficulty = ds;
            map = new List<List<Coordinate>>();
            for (int i = 0; i < size.X; i++)
            {
                map.Add(new List<Coordinate>());
                for (int j = 0; j < size.Y; j++)
                {
                    map[i].Add(new Coordinate(new Sprite(empty.Image, empty.Location, Color.White, empty.Rotation, empty.Effects, empty.Origin, (float)scale, empty.Depth), new Vector2(i, j), 0));
                    Vector2 oragami = new Vector2(map[i][j].image.Origin.X * (float)scale, map[i][j].image.Origin.Y * (float)scale);
                    map[i][j].image.Location = new Vector2(i * (float)Math.Round(60 * scale), j * (float)Math.Round(60 * scale) - (float)Math.Round(360 * scale)) + oragami;
                }
            }
            generate(3);
        }

        public void Reset()
        {
            lose = false;
            score = 0;
            map = new List<List<Coordinate>>();
            NevadaCheck = new Timer(new TimeSpan(0, 0, 0, 0, nevadaready));
            for (int i = 0; i < size.X; i++)
            {
                map.Add(new List<Coordinate>());
                for (int j = 0; j < size.Y; j++)
                {
                    map[i].Add(new Coordinate(new Sprite(empty.Image, empty.Location, Color.White, empty.Rotation, empty.Effects, empty.Origin, (float)scale, empty.Depth), new Vector2(i, j), 0));
                    Vector2 oragami = new Vector2(map[i][j].image.Origin.X * (float)scale, map[i][j].image.Origin.Y * (float)scale);
                    map[i][j].image.Location = new Vector2(i * (float)Math.Round(60 * scale), j * (float)Math.Round(60 * scale) - (float)Math.Round(360 * scale)) + oragami;
                }
            }
            generate(3);
        }

        public void generate()
        {
            generate(1);
        }
        public void generate(int num)
        {
            if (num >= 3)
            {
                pooey = gen();
                nextPooey = gen();
                lastPooey = gen();
                return;
            }
            if (num >= 2)
            {
                pooey = nextPooey;
                nextPooey = gen();
                lastPooey = gen();
                return;
            }
            pooey = nextPooey;
            nextPooey = lastPooey;
            lastPooey = gen();
        }
        public RatPooeys gen()
        {
            fullDown = false;
            int blah;
            while (true)
            {
                blah = random.Next(0, locations.Count);
                int size = random.Next(0, 100);
                int diffFactor = (int)(100 + difficulty[blah] * progression);
                if ((spawnChances[blah] * diffFactor) / 100 >= size)
                {
                    break;
                }
            }

            //blah = 4;
            return new RatPooeys(new Sprite(image.Image, image.Location, image.Color, image.Rotation, image.Effects, image.Origin, (float)scale, image.Depth), locations[blah], colors[blah], values[blah], (float)scale, symmetry[blah], 650 - progression * 10);
        }
        public void Update(GameTime gameTime)
        {            
            if (!fullDown)
            {
                California = Keyboard.GetState();
                if (California.IsKeyDown(downKey))
                {
                    if (NevadaCheck.ready())
                    {
                        pooey.moveDown();
                    }
                }
                else if (California.IsKeyDown(turnKey))
                {
                    if (NevadaCheck.ready())
                    {
                        pooey.rotate();
                    }
                }
                else if (California.IsKeyDown(leftKey))
                {
                    if (NevadaCheck.ready())
                    {
                        pooey.moveSide(-1);
                    }
                }
                else if (California.IsKeyDown(rightKey))
                {
                    if (NevadaCheck.ready())
                    {
                        pooey.moveSide();
                    }
                }
                if (!California.IsKeyDown(downKey))
                {
                    pooey.Update(gameTime);
                }
            }
            if (California.IsKeyDown(TeleKey))
            {
                if (!downHeld)
                {
                    fullDown = true;
                    pooey.goDown = true;
                }
                downHeld = true;
            }
            else
            {
                downHeld = false;
            }
            if (fullDown)
            {
                pooey.moveDown();
            }

            NevadaCheck.tick(gameTime);
            int result = istouching();
            if (result > 0)
            {
                if (result == 2)
                {
                    lose = true;
                    return;
                }
                score += pooey.score;
                if (score - 3500 >= progression * 1000 && progression < 30)
                {
                    progression++;
                }
                hippityHoppityYourRowIsNowMyProperty();
                generate();
            }
        }
        public int istouching()
        {
            return (istouching(pooey));
        }
        public int istouching(RatPooeys piece)
        {
            for (int i = 0; i < piece.boxes.Count; i++)
            {
                if (piece.boxes[i].place.Y >= size.Y || map[(int)piece.boxes[i].place.X][(int)piece.boxes[i].place.Y].isfull)
                {
                    bool good = true;
                    if (piece.rotated)
                    {
                        good = false;
                        piece.rotate(-1);
                        piece.rotated = false;
                    }
                    if (piece.sideways != 0)
                    {
                        piece.forceSide(-piece.sideways);
                        piece.sideways = 0;
                        return 0;
                    }
                    if (!good)
                    {
                        return 0;
                    }
                    if  (piece.boxes[i].place.Y - 1 < 0)
                    {
                        return 2;
                    }
                    if (piece.boxes[i].place.Y - 1 >= size.Y || map[(int)piece.boxes[i].place.X][(int)piece.boxes[i].place.Y - 1].isfull)
                    {
                        for (int j = 0; j < piece.boxes.Count; j++)
                        {                            
                            if (piece.boxes[j].place.Y - 1 < 0)
                            {
                                return 2;
                            }
                            map[(int)piece.boxes[j].place.X][(int)piece.boxes[j].place.Y - 1].fill(piece);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < piece.boxes.Count; j++)
                        {
                            if (piece.boxes[j].place.Y - 1 < 0)
                            {
                                return 2;
                            }
                            map[(int)piece.boxes[j].place.X][(int)piece.boxes[j].place.Y - 1].fill(piece);
                        }
                    }
                    return 1;
                }
                else if (piece.goDown && (piece.boxes[i].place.Y + 1 >= size.Y || !fullDown && map[(int)piece.boxes[i].place.X][(int)piece.boxes[i].place.Y + 1].isfull))
                {
                    if (piece.sideways != 0)
                    {
                        piece.moveSide(-piece.sideways);
                        piece.sideways = 0;
                        return 0;
                    }
                    for (int j = 0; j < piece.boxes.Count; j++)
                    {
                        if (piece.boxes[j].place.Y < 0)
                        {
                            return 2;
                        }
                        map[(int)piece.boxes[j].place.X][(int)piece.boxes[j].place.Y].fill(piece);
                    }
                    return 1;
                }
            }
            piece.sideways = 0;
            piece.rotated = false;
            return 0;
        }
        public bool hippityHoppityYourRowIsNowMyProperty()
        {
            for (int i = 0; i < size.Y; i++)
            {
                bool isFull = true;
                for (int j = 0; j < size.X; j++)
                {
                    if (!map[j][i].isfull)
                    {
                        isFull = false;
                    }
                }
                if (isFull)
                {
                    for (int j = 0; j < size.X; j++)
                    {
                        score += map[j][i].score;
                        map[j][i].empty(empty);
                        for (int q = i; q > 0; q--)
                        {
                            map[j][q] = map[j][q - 1];
                            map[j][q].image.Location = new Vector2(map[j][q].image.Location.X, map[j][q].image.Location.Y + (float)Math.Round(60 * scale));
                        }
                        map[j][0] = new Coordinate(new Sprite(empty.Image, empty.Location, Color.White, empty.Rotation, empty.Effects, empty.Origin, (float)scale, empty.Depth), new Vector2(j, 0), map[j][0].score);
                        Vector2 oragami = new Vector2(map[j][0].image.Origin.X * (float)scale, map[j][0].image.Origin.Y * (float)scale);
                        map[j][0].image.Location = new Vector2(j * (float)Math.Round(60 * scale), 0 * (float)Math.Round(60 * scale) - (float)Math.Round(360 * scale)) + oragami;

                    }
                    while (true)
                    {
                        if (hippityHoppityYourRowIsNowMyProperty())
                        {
                            break;
                        }
                    }
                }
            }
            return true;
        }
        public void Draw(SpriteBatch bunch)
        {
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 6; j < map[i].Count; j++)
                {
                    map[i][j].image.Draw(bunch);
                }
            }
            pooey.Draw(bunch);
        }
    }
}