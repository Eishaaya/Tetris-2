using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        public Keys downKey;
        public Keys leftKey;
        public Keys rightKey;
        public Keys turnKey;
        public Keys TeleKey;
        public List<Keys> switchKeys;
        KeyboardState California;
        Timer NevadaCheck;
        List<List<Vector2>> locations;
        List<bool> symmetry;
        List<Color> colors;
        List<int> spawnChances;
        List<int> values;
        List<float> difficulty;
        List<Vector2> sizes;
        List<ParticleEffect> effects;
        List<List<Vector2>> explosives;
        public int score;
        Sprite image;
        bool fullDown;
        bool downHeld;
        public RatPooeys nextPooey;
        public RatPooeys lastPooey;
        public RatPooeys savedPooey;
        int nevadaready;
        public bool lose;
        public int progression;
        int hold;
        public bool isClassic;
        float tempScale;
        bool saveGo = false;
        TimeSpan lifitime;
        float badFactor;
        float scoreBonus;
        public float freeMoves = 10;
        public bool overused;
        public bool dangerUse;
        SoundEffectInstance rotate;
        SoundEffectInstance land;
        SoundEffectInstance boom;
        Texture2D explosiveImage;
        Texture2D pixel;
        public Grid(Vector2 s, Sprite e, List<List<Vector2>> ln, List<bool> sy, List<Color> c, List<int> ch, List<int> va, List<float> ds, List<Vector2> ss, Sprite im, SoundEffect ro, SoundEffect la, SoundEffect b, float sc = 1, bool ic = false, Texture2D ei = null, Texture2D pi = null, int n = 100, Keys d = Keys.S, Keys t = Keys.W, Keys l = Keys.A, Keys r = Keys.D, Keys D = Keys.Space, Keys s1 = Keys.D1, Keys s2 = Keys.D2, Keys s3 = Keys.D3, Keys s4 = Keys.D4)
        {
            pixel = pi;
            switchKeys = new List<Keys>();
            rotate = ro.CreateInstance();
            land = la.CreateInstance();
            boom = b.CreateInstance();
            effects = new List<ParticleEffect>();
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
            switchKeys.Add(s1);
            switchKeys.Add(s2);
            switchKeys.Add(s3);
            switchKeys.Add(s4);
            scale = sc;
            size = new Vector2(s.X, s.Y + 6);
            empty = e;
            locations = ln;
            symmetry = sy;
            colors = c;
            values = va;
            sizes = ss;
            spawnChances = ch;
            difficulty = ds;
            isClassic = ic;
            badFactor = 0;
            explosiveImage = ei;
            map = new List<List<Coordinate>>();
            for (int i = 0; i < size.X; i++)
            {
                map.Add(new List<Coordinate>());
                for (int j = 0; j < size.Y; j++)
                {
                    map[i].Add(new Coordinate(new Sprite(empty.Image, empty.Location, Color.White, empty.Rotation, empty.Effects, empty.Origin, (float)scale, empty.Depth), new Vector2(i, j), 0, 0, 0));
                    Vector2 oragami = new Vector2(map[i][j].image.Origin.X * (float)scale, map[i][j].image.Origin.Y * (float)scale);
                    map[i][j].image.Location = new Vector2(i * (float)Math.Round(60 * scale), j * (float)Math.Round(60 * scale) - (float)Math.Round(360 * scale)) + oragami;
                }
            }
            generate(3);
        }

        public void Reset()
        {
            savedPooey = null;
            freeMoves = 10;
            scoreBonus = 0;
            badFactor = 0;
            hold = 0;
            progression = 0;
            lose = false;
            score = 0;
            map = new List<List<Coordinate>>();
            NevadaCheck = new Timer(new TimeSpan(0, 0, 0, 0, nevadaready));
            for (int i = 0; i < size.X; i++)
            {
                map.Add(new List<Coordinate>());
                for (int j = 0; j < size.Y; j++)
                {
                    map[i].Add(new Coordinate(new Sprite(empty.Image, empty.Location, Color.White, empty.Rotation, empty.Effects, empty.Origin, (float)scale, empty.Depth), new Vector2(i, j), 0, 0, 0));
                    Vector2 oragami = new Vector2(map[i][j].image.Origin.X * (float)scale, map[i][j].image.Origin.Y * (float)scale);
                    map[i][j].image.Location = new Vector2(i * (float)Math.Round(60 * scale), j * (float)Math.Round(60 * scale) - (float)Math.Round(360 * scale)) + oragami;
                }
            }
            generate(3);
        }

        public void Switch(int switcher)
        {
            if (overused || isClassic)
            {
                return;
            }
            badFactor++;
            var blah = savedPooey;
            if (switcher == 0)
            {
                savedPooey = nextPooey;
                if (blah == null)
                {
                    nextPooey = lastPooey;
                    lastPooey = gen();
                    lastPooey.Display(new Vector2(450, 540), 140);
                }
                else
                {
                    nextPooey = blah;
                }
                savedPooey.Display(new Vector2(450, 740), 140);
                nextPooey.Display(new Vector2(450, 340), 140);
                freeMoves--;
                return;
            }
            if (switcher == 1)
            {
                savedPooey = lastPooey;
                if (blah == null)
                {
                    lastPooey = gen();
                }
                else
                {
                    lastPooey = blah;
                }
                savedPooey.Display(new Vector2(450, 740), 140);
                lastPooey.Display(new Vector2(450, 540), 140);
                freeMoves--;
                badFactor += 4;
                return;
            }
            if (switcher == 2)
            {
                float tempProg;
                tempProg = (30 - progression) / 6;
                if (tempProg <= 0)
                {
                    tempProg = 0;
                }
                float tempProg2;
                tempProg2 = progression / 30;
                if (tempProg2 >= 1)
                {
                    tempProg2 /= 5;
                }
                if (savedPooey == null)
                {
                    if (lifitime.Seconds <= 2 - tempProg2 && pooey.boxes[0].place.Y - 5 <= 5 + tempProg && freeMoves >= 5)
                    {
                        freeMoves -= 5;
                        pooey.Revert();
                        savedPooey = pooey;
                        generate();
                        lastPooey.Display(new Vector2(450, 540), 140);
                        badFactor++;
                    }
                    return;
                }
                else if (lifitime.Seconds <= 2 - tempProg2 && pooey.boxes[0].place.Y - 5 <= 5 + tempProg && freeMoves >= 5)
                {
                    freeMoves -= 5;
                    pooey.Revert();
                    var temperPooey = pooey;
                    saveGo = true;
                    generate();
                    savedPooey = temperPooey;
                    savedPooey.Display(new Vector2(450, 740), 140);
                    badFactor++;
                    return;
                }
                saveGo = true;
                return;
            }
            if (switcher == 3)
            {
                if (savedPooey != null)
                {
                    saveGo = true;
                }
            }
        }

        public void generate()
        {
            lifitime = TimeSpan.Zero;
            fullDown = false;
            if (saveGo)
            {
                pooey = savedPooey;
                pooey.Ready();
                savedPooey = null;
                saveGo = false;
                return;
            }
            if (!isClassic)
            {
                badFactor--;
                if (badFactor <= 0)
                {
                    scoreBonus += 5;
                    badFactor = 0;
                }
                else
                {
                    scoreBonus -= 15;
                    if (scoreBonus <= 0)
                    {
                        scoreBonus = 0;
                    }
                }
                freeMoves++;
                if (freeMoves >= 15)
                {
                    overused = false;
                }
                if (freeMoves > 15)
                {
                    freeMoves = 15;
                }
            }
            generate(1);

        }
        public void generate(int num)
        {
            if (num >= 3)
            {
                pooey = gen();
                pooey.Ready();
                nextPooey = gen();
                nextPooey.Display(new Vector2(450, 340), 140);
                lastPooey = gen();
                lastPooey.Display(new Vector2(440, 540), 140);
                return;
            }
            if (num >= 2)
            {
                pooey = nextPooey;
                pooey.Ready();
                nextPooey = gen();
                nextPooey.Display(new Vector2(440, 340), 140);
                lastPooey = gen();
                lastPooey.Display(new Vector2(440, 540), 140);
                return;
            }
            pooey = nextPooey;
            pooey.Ready();
            nextPooey = lastPooey;
            nextPooey.Display(new Vector2(440, 340), 140);
            lastPooey = gen();
            lastPooey.Display(new Vector2(440, 540), 140);
        }
        public RatPooeys gen()
        {
            int ex = 0;
            int blah;
            int chonk = 0;
            hold--;
            while (true)
            {
                blah = random.Next(0, locations.Count);
                float size = random.Next(0, 100);
                var diffTemp = progression;
                var diffAdd = badFactor * difficulty[blah] / Math.Abs(difficulty[blah]);
                if (difficulty[blah] == 0)
                {
                    diffAdd = 0;
                }
                if (badFactor < 0)
                {
                    badFactor = 0;
                }
                float diffFactor = (100 + difficulty[blah] * diffTemp) / 100;
                float helpFactor = (100 - difficulty[blah] * ((hold - 22) * (4 - progression / 25))) / 100;
                if (hold <= 15)
                {
                    helpFactor = 1;
                }
                if (!isClassic && blah == spawnChances.Count - 1)
                {
                    if (hold <= 0 && (spawnChances[blah] * diffFactor) + diffAdd >= size)
                    {
                        hold = 35;
                        freeMoves += 10;
                        break;
                    }
                }
                else if ((spawnChances[blah] * diffFactor * helpFactor) >= size - diffAdd)
                {
                    if (!isClassic && size <= 1 * (progression / 10) && helpFactor == 1 && hold <= 5)
                    {
                        chonk = 3 + progression / 10;
                        break;
                    }
                    size = random.Next(100);
                    if (!isClassic && size <= 15 / (progression / 33 + 1))
                    {
                        ex = (int)((size - 1) / (5 / (progression / 33 + 1))) + 1;
                    }
                    break;
                }
            }

            //blah = 4;
            var temp = progression * 10;
            if (temp > 300)
            {
                temp = 300;
            }
            return new RatPooeys(new Sprite(image.Image, image.Location, image.Color, image.Rotation, image.Effects, image.Origin, (float)scale, image.Depth), locations[blah], sizes[blah], colors[blah], values[blah], (float)scale, symmetry[blah], 650 - temp, chonk, ex, explosiveImage);
        }
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < effects.Count; i++)
            {
                effects[i].Update(gameTime);
            }
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Count; j++)
                {
                    map[i][j].Animate();
                }
            }
            if (freeMoves <= 0)
            {
                overused = true;
            }
            else if (freeMoves <= 5)
            {
                dangerUse = true;
            }
            else
            {
                dangerUse = false;
            }
            lifitime += gameTime.ElapsedGameTime;
            var tempTexas = California;
            California = Keyboard.GetState();
            for (int i = 0; i < switchKeys.Count; i++)
            {
                if (California.IsKeyDown(switchKeys[i]) && !tempTexas.IsKeyDown(switchKeys[i]))
                {
                    Switch(i);
                }
            }
            if (!fullDown)
            {
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
                        rotate.Stop();
                        pooey.rotate();
                        rotate.Play();
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
                land.Stop();
                land.Play();
                score += pooey.score * (int)((100 + scoreBonus + progression) / 100);
                if (score - 3500 >= progression * 1000 && progression < 100)
                {
                    progression++;
                }
                explosives = new List<List<Vector2>>();
                if (!hippityHoppityYourRowIsNowMyProperty())
                {
                    for (int i = 0; i < explosives.Count; i++)
                    {
                        for (int j = 0; j < explosives[i].Count; j++)
                        {
                            if (map[(int)explosives[i][j].X][(int)explosives[i][j].Y].explosive > 0)
                            {
                                explosives.Add(map[(int)explosives[i][j].X][(int)explosives[i][j].Y].Explode(map));
                            }
                            map[(int)explosives[i][j].X][(int)explosives[i][j].Y].empty(empty);
                            map[(int)explosives[i][j].X][(int)explosives[i][j].Y].image.Scale = (float)scale;
                            //map[(int)explosives[i][j].X][(int)explosives[i][j].Y].image.Color = Color.Cyan;
                        }
                    }
                    boom.Stop();
                    boom.Play();
                    for (int i = 0; i < map.Count; i++)
                    {
                        for (int j = 0; j < map[i].Count; j++)
                        {
                            if (map[i][j].image.Scale > scale && map[i][j].explosive == 0)
                            {
                                map[i][j].image.Scale = (float)scale;
                            }
                        }
                    }
                }
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
                    if (piece.boxes[i].place.Y - 1 < 0)
                    {
                        return 2;
                    }
                    if (piece.boxes[i].place.Y >= size.Y || !fullDown && map[(int)piece.boxes[i].place.X][(int)piece.boxes[i].place.Y - 1].isfull)
                    {
                        for (int j = 0; j < piece.boxes.Count; j++)
                        {
                            if (piece.boxes[j].place.Y - 1 < 0)
                            {
                                return 2;
                            }
                            map[(int)piece.boxes[j].place.X][(int)piece.boxes[j].place.Y - 1].fill(piece.boxes[j]);
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
                            map[(int)piece.boxes[j].place.X][(int)piece.boxes[j].place.Y - 1].fill(piece.boxes[j]);
                        }
                    }
                    return 1;
                }
            }
            for (int i = 0; i < piece.boxes.Count; i++)
            {
                if (piece.goDown && (piece.boxes[i].place.Y + 1 >= size.Y || !fullDown && map[(int)piece.boxes[i].place.X][(int)piece.boxes[i].place.Y + 1].isfull))
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
                        map[(int)piece.boxes[j].place.X][(int)piece.boxes[j].place.Y].fill(piece.boxes[j]);
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
                bool fullChonk = true;
                bool isFull = true;
                for (int j = 0; j < size.X; j++)
                {
                    if (!map[j][i].isfull)
                    {
                        isFull = false;
                    }
                    if (!map[j][i].Chonker())
                    {
                        fullChonk = false;
                    }
                }
                if (isFull)
                {
                    for (int j = 0; j < size.X; j++)
                    {
                        score += map[j][i].score * (int)((100 + scoreBonus + progression) / 100);
                        if (map[j][i].chonker <= 1 || fullChonk)
                        {
                            if (map[j][i].explosive > 0)
                            {
                                effects.Add(new ParticleEffect(pixel, map[j][i].image.Location, new List<Color> { Color.Yellow, Color.Orange, Color.Red }, 99, (int)map[j][i].explosive, new List<double> { 3, 6, 9 }, new List<int> { 20, 25, 30 }));
                                var deads = map[j][i].Explode(map);
                                explosives.Add(deads);
                            }
                            map[j][i].empty(empty);
                            for (int q = i; q > 0; q--)
                            {
                                if (map[j][q - 1].isfull)
                                {
                                    map[j][q - 1].image.Scale = (float)scale;
                                    map[j][q - 1].image.offset = Vector2.Zero;
                                    map[j][q].fill(map[j][q - 1]);
                                }
                                else
                                {
                                    map[j][q].empty(empty);
                                }

                            }

                            map[j][0] = new Coordinate(new Sprite(empty.Image, empty.Location, Color.White, empty.Rotation, empty.Effects, empty.Origin, (float)scale, empty.Depth), new Vector2(j, 0), map[j][0].score, 0, 0);
                            Vector2 oragami = new Vector2(map[j][0].image.Origin.X * (float)scale, map[j][0].image.Origin.Y * (float)scale);
                            map[j][0].image.Location = new Vector2(j * (float)Math.Round(60 * scale), 0 * (float)Math.Round(60 * scale) - (float)Math.Round(360 * scale)) + oragami;
                        }
                        else
                        {
                            map[j][i].reduceChonk();
                        }

                    }
                    while (true)
                    {
                        if (hippityHoppityYourRowIsNowMyProperty())
                        {
                            break;
                        }
                    }
                    return false;
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
            for (int i = 0; i < effects.Count; i++)
            {
                effects[i].Draw(bunch);
            }
            pooey.Draw(bunch);
            nextPooey.Draw(bunch);
            lastPooey.Draw(bunch);
            if (savedPooey != null)
            {
                savedPooey.Draw(bunch);
            }
        }
    }
}