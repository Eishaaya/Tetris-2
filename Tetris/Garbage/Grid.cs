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
        public double scale;
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
        RatPooeys shadowPooey;
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
        SoundEffectInstance zoom;
        SoundEffectInstance boss;
        Texture2D explosiveImage;
        Texture2D chonkImage;
        Texture2D speedImage;
        Texture2D pixel;
        Texture2D speedParticle;
        Texture2D projectionImage;
        public bool playSounds;
        public bool holdTurn;
        public bool holdDown;
        public bool holdSide;
        public int rowBonus;
        int speedAdd;
        int speedTime;
        public float scoreFactor = 1;
        int changeDone;
        int finishedColors;
        public bool willProject = true;

        List<int> explosiveScales = new List<int> { 15, 20, 25 };
    public Grid(Vector2 s, Sprite e, List<List<Vector2>> ln, List<bool> sy, List<Color> c, List<int> ch, List<int> va, List<float> ds, List<Vector2> ss, Sprite im, SoundEffect ro, SoundEffect la, SoundEffect b, SoundEffect sp, SoundEffect bs, Texture2D spi, float sc = 1, bool ic = false, Texture2D chi = null, Texture2D ei = null, Texture2D si = null, Texture2D pi = null, Texture2D spp = null, int n = 100, Keys d = Keys.S, Keys t = Keys.W, Keys l = Keys.A, Keys r = Keys.D, Keys D = Keys.Space, Keys s1 = Keys.D1, Keys s2 = Keys.D2, Keys s3 = Keys.D3, Keys s4 = Keys.D4)
        {
            speedTime = 0;
            finishedColors = -1;
            changeDone = -1;
            speedAdd = 1;
            rowBonus = 1;
            playSounds = true;
            holdDown = true;
            holdSide = true;
            holdTurn = true;
            pixel = pi;
            speedParticle = spp;
            switchKeys = new List<Keys>();
            rotate = ro.CreateInstance();
            land = la.CreateInstance();
            if (!ic)
            {
                zoom = sp.CreateInstance();
                boom = b.CreateInstance();
                boss = bs.CreateInstance();
                boss.IsLooped = true;
            }
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
            chonkImage = chi;
            speedImage = si;
            projectionImage = spi;
            map = new List<List<Coordinate>>();
            for (int i = 0; i < size.X; i++)
            {
                map.Add(new List<Coordinate>());
                for (int j = 0; j < size.Y; j++)
                {
                    map[i].Add(new Coordinate(new Sprite(empty.Image, empty.Location, Color.White, empty.rotation, empty.effect, empty.Origin, (float)scale, empty.Depth), new Vector2(i, j), 0, 0, 0, false));
                    Vector2 oragami = new Vector2(map[i][j].image.Origin.X * (float)scale, map[i][j].image.Origin.Y * (float)scale);
                    map[i][j].image.Location = new Vector2(i * (float)Math.Round(60 * scale), j * (float)Math.Round(60 * scale) - (float)Math.Round(360 * scale)) + oragami;
                }
            }
            generate(4);
            if (willProject)
            {
                shadowPlace();
            }
        }

        public void Reset()
        {
            overused = false;
            speedTime = 0;
            finishedColors = -1;
            changeDone = -1;
            speedAdd = 1;
            rowBonus = 1;
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
                    map[i].Add(new Coordinate(new Sprite(empty.Image, empty.Location, Color.White, empty.rotation, empty.effect, empty.Origin, (float)scale, empty.Depth), new Vector2(i, j), 0, 0, 0, false));
                    Vector2 oragami = new Vector2(map[i][j].image.Origin.X * (float)scale, map[i][j].image.Origin.Y * (float)scale);
                    map[i][j].image.Location = new Vector2(i * (float)Math.Round(60 * scale), j * (float)Math.Round(60 * scale) - (float)Math.Round(360 * scale)) + oragami;
                }
            }
            generate(3);
            if (willProject)
            {
                shadowPlace();
            }
        }

        public void Switch(int switcher)
        {
            saveGo = false;
            if (overused || isClassic)
            {
                return;
            }
            badFactor++;
            var savedPieceState = savedPooey;
            if (switcher == 0)
            {
                savedPooey = nextPooey;
                if (savedPieceState == null)
                {
                    nextPooey = lastPooey;
                    lastPooey = gen();
                    lastPooey.Display(new Vector2(450, 540), 140);
                }
                else
                {
                    nextPooey = savedPieceState;
                }
                savedPooey.Display(new Vector2(450, 740), 140);
                nextPooey.Display(new Vector2(450, 340), 140);
                freeMoves--;
                return;
            }
            if (switcher == 1)
            {
                savedPooey = lastPooey;
                if (savedPieceState == null)
                {
                    lastPooey = gen();
                }
                else
                {
                    lastPooey = savedPieceState;
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
                        savedPooey.Display(new Vector2(450, 740), 140);
                        badFactor++;
                    }
                    return;
                }
                else if (lifitime.Seconds <= 2 - tempProg2 && pooey.boxes[0].place.Y - 5 <= 5 + tempProg && freeMoves >= 5)
                {                    
                    boss.Stop();
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
            if (savedPooey == null)
            {
                saveGo = false;
            }
            if (saveGo)
            {
                pooey = savedPooey;
                pooey.Ready();
                if (willProject)
                {
                    shadowPooey = new RatPooeys(pooey);
                    shadowPlace();
                }
                savedPooey = null;
                saveGo = false;
                boss.Stop();
                if (pooey.boxes.Count > 10)
                {
                    boss.Play();
                }
                return;
            }
            if (!isClassic)
            {
                badFactor--;
                if (badFactor <= 0)
                {
                    scoreBonus += 3;
                    if (scoreBonus > 30)
                    {
                        scoreBonus = 3;
                    }
                    badFactor = 0;
                }
                else
                {
                    scoreBonus -= 9;
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
            if (boss != null)
            {
                boss.Stop();
            }
            if (pooey.boxes.Count > 10)
            {
                boss.Play();
            }
        }
        public bool ChangeBackColor(Color newColor, int speed = 5)
        {
            var doneUP = false;
            var allTouched = true;
            if (changeDone < 0)
            {
                changeDone = 0;
                return false;
            }
            changeDone++;
            int xSize = changeDone / speed;
            int ySIze = changeDone / speed + 6;
            if (xSize > map.Count)
            {
                xSize = map.Count;
            }
            if (ySIze > map[0].Count)
            {
                ySIze = map[0].Count;
            }
            for (int i = 0; i < xSize; i++)
            {
                for (int j = 6; j < ySIze; j++)
                {
                    if (!map[i][j].isfull)
                    {
                        if (map[i][j].image.Color == newColor || i <= finishedColors && j <= finishedColors + 6)
                        {
                            if (i > finishedColors || j > finishedColors + 6)
                            {
                                doneUP = true;
                                //Console.WriteLine($"{i}  {j - 6}");
                            }
                            map[i][j].image.ChangeColor(empty.Color, .05f);
                        }
                        else
                        {
                            map[i][j].image.ChangeColor(newColor, .05f);
                            allTouched = false;
                        }
                    }
                }
            }
            if (doneUP)
            {
                finishedColors++;
            }
            if (allTouched && xSize == map.Count && ySIze == map[0].Count)
            {
                var done = true;
                for (int q = 0; q < xSize; q++)
                {
                    for (int e = 6; e < ySIze; e++)
                    {
                        if (!map[q][e].isfull)
                        {
                            if (map[q][e].image.Color != Color.White)
                            {
                                done = false;
                                break;
                            }
                            else
                            {
                                ;
                            }
                        }
                    }
                }
                if (done)
                {
                    changeDone = -1;
                    finishedColors = -1;
                    return true;
                }
            }
            return false;
        }
        public void generate(int num)
        {
            if (num >= 4)
            {
                if (isClassic)
                {
                    pooey = gen();
                    pooey.Ready();
                    if (willProject)
                    {
                        shadowPooey = new RatPooeys(pooey);
                        shadowPlace();
                    }
                    nextPooey = gen();
                    nextPooey.Display(new Vector2(450, 340), 140);
                    lastPooey = gen();
                    lastPooey.Display(new Vector2(440, 540), 140);
                    savedPooey = gen();
                    savedPooey.Display(new Vector2(440, 740), 140);
                    return;
                }
                else
                {
                    num = 3;
                }
            }
            if (num >= 3)
            {
                pooey = gen();
                pooey.Ready();
                if (willProject)
                {
                    shadowPooey = new RatPooeys(pooey);
                    shadowPlace();
                }
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
                if (willProject)
                {
                    shadowPooey = new RatPooeys(pooey);
                    shadowPlace();
                }
                nextPooey = gen();
                nextPooey.Display(new Vector2(440, 340), 140);
                lastPooey = gen();
                lastPooey.Display(new Vector2(440, 540), 140);
                return;
            }
            pooey = nextPooey;
            pooey.Ready();
            if (willProject)
            {
                shadowPooey = new RatPooeys(pooey);
                shadowPlace();
            }
            nextPooey = lastPooey;
            nextPooey.Display(new Vector2(440, 340), 140);
            lastPooey = gen();
            if (isClassic)
            {
                if (savedPooey == null)
                {
                    savedPooey = gen();
                }
                lastPooey = savedPooey;
                savedPooey = gen();
                savedPooey.Display(new Vector2(440, 740), 140);
            }
            lastPooey.Display(new Vector2(440, 540), 140);
        }
        //SPAWNING
        public RatPooeys gen()
        {
            int ex = 0;
            bool sp = false;
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
                    if (!isClassic && size <= 2 * (progression / 10 + 1) && helpFactor == 1 && hold <= 5)
                    {
                        chonk = 3 + progression / 10;
                        break;
                    }
                    size = random.Next(1000);
                    if (!isClassic && size <= 51)
                    {
                        ex = (int)(size - 1) / 17 + 1;
                        break;
                    }
                    size = random.Next(1000);
                    if (!isClassic && size <= 45 * (progression / 10 + 1) && helpFactor == 1 && hold <= 5)
                    {
                        sp = true;
                    }
                    break;
                }
            }

            //blah = 4;
            var temp = progression * 5;
            var currentSpeed = temp;
            if (speedAdd != 1)
            {
                temp += speedAdd;
            }
            if (temp > 325)
            {
                if (speedAdd != 1)
                {
                    if (temp > 375)
                    {
                        temp = 375;
                    }
                }
                else
                {
                    temp = 325;
                }
            }
            return new RatPooeys(new Sprite(image.Image, image.Location, image.Color, image.rotation, image.effect, image.Origin, (float)scale, image.Depth), locations[blah], sizes[blah], colors[blah], values[blah], (float)scale, symmetry[blah], 650 - temp, chonk, chonkImage, sp, speedImage, ex, explosiveImage);
        }
        public void Update(GameTime gameTime)
        {
            if (pooey.explosive > 0 && willProject)
            {
                shadowPooey = new RatPooeys(pooey);
                shadowPlace();
            }
            if (speedTime <= 0)
            {
                speedTime = 0;
                speedAdd = 1;
            }
            scoreFactor = ((100 + scoreBonus + progression) * (speedAdd / 100 + 1) / 100) * rowBonus;
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
            if (pooey.boxes[0].place.Y + pooey.pieceSize.Y / 120 >= 6)
            {
                lifitime += gameTime.ElapsedGameTime;
            }
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
                if (California.IsKeyDown(downKey) && (holdDown || !tempTexas.IsKeyDown(downKey)))
                {
                    if (NevadaCheck.Ready())
                    {
                        pooey.moveDown();
                    }
                }
                else if (California.IsKeyDown(turnKey) && (holdTurn || !tempTexas.IsKeyDown(turnKey)))
                {
                    if (NevadaCheck.Ready())
                    {
                        if (pooey.rotate())
                        {
                            if (willProject)
                            {
                                shadowPooey = new RatPooeys(pooey);
                                shadowPlace();
                            }
                            rotate.Stop();
                            if (playSounds)
                            {
                                rotate.Play();
                            }
                        }
                    }
                }
                else if (California.IsKeyDown(leftKey) && (holdSide || !tempTexas.IsKeyDown(leftKey)))
                {
                    if (NevadaCheck.Ready())
                    {
                        pooey.moveSide(-1);
                        if (willProject)
                        {
                            shadowPooey = new RatPooeys(pooey);
                            shadowPlace();
                        }
                    }
                }
                else if (California.IsKeyDown(rightKey) && (holdSide || !tempTexas.IsKeyDown(rightKey)))
                {
                    if (NevadaCheck.Ready())
                    {
                        pooey.moveSide();
                        if (willProject)
                        {
                            shadowPooey = new RatPooeys(pooey);
                            shadowPlace();
                        }
                    }
                }
                shadowPooey.Animate();
                if (!(California.IsKeyDown(downKey) && (holdDown || !tempTexas.IsKeyDown(downKey))))
                {
                    pooey.Update(gameTime);
                }
                else
                {
                    pooey.Animate();
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
                pooey.Animate();
            }

            NevadaCheck.Tick(gameTime);
            int result = istouching();
            if (result > 0)
            {
                if (result == 2)
                {
                    lose = true;
                    return;
                }
                land.Stop();
                if (playSounds)
                {
                    land.Play();
                }
                score += (int)(pooey.score * scoreFactor);
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
                            var explosiveSpot = map[(int)explosives[i][j].X][(int)explosives[i][j].Y];


                            if (explosiveSpot.explosive > 0)
                            {
                                explosives.Add(explosiveSpot.Explode(map));
                                CreateExplosionParticles(explosiveSpot);
                            }
                            score += (int)(explosiveSpot.score * scoreFactor);
                            if (explosiveSpot.chonker <= 1)
                            {
                                explosiveSpot.empty(empty);
                                explosiveSpot.image.Scale = (float)scale;
                            }
                            else
                            {
                                explosiveSpot.reduceChonk();
                            }
                            //explosiveSpot.image.Color = Color.Cyan;
                        }
                    }
                    rowBonus++;
                    if (boom != null)
                    {
                        boom.Stop();
                        if (playSounds)
                        {
                            boom.Play();
                        }
                    }
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
                else
                {
                    speedTime--;
                    if (rowBonus > 1)
                    {
                        rowBonus /= 2;
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
                        if (willProject)
                        {
                            shadowPooey = new RatPooeys(piece);
                            shadowPlace();
                        }
                        piece.rotated = false;
                    }
                    if (piece.sideways != 0)
                    {
                        piece.forceSide(-piece.sideways);
                        if (willProject)
                        {
                            shadowPooey = new RatPooeys(piece);
                            shadowPlace();
                        }
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
                        var currentSpot = map[j][i];

                        score += (int)(currentSpot.score * scoreFactor);
                        if (currentSpot.chonker <= 1 || fullChonk)
                        {
                            if (currentSpot.explosive > 0)
                            {
                                CreateExplosionParticles(currentSpot);

                                var deads = currentSpot.Explode(map);
                                explosives.Add(deads);
                            }
                            else if (currentSpot.speed)
                            {
                                zoom.Stop();
                                speedAdd = 200;
                                speedTime = 10;
                                if (playSounds)
                                {
                                    zoom.Play();
                                }
                                effects.Add(new ParticleEffect(ParticleEffect.EffectType.Ray, speedParticle, currentSpot.image.Location,
                                            new List<Color> { Color.White, Color.White }, 50, 5, new List<double> { 10, 10 }, 
                                            new List<float> { 10 / pixel.Width, 10 / pixel.Width }, new List<int> { 1, 1 }, null, 1, 30, 30, 1, 0));
                            }
                            currentSpot.empty(empty);
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

                            map[j][0] = new Coordinate(new Sprite(empty.Image, empty.Location, Color.White, empty.rotation, empty.effect, empty.Origin, (float)scale, empty.Depth), new Vector2(j, 0), map[j][0].score, 0, 0, false);
                            Vector2 oragami = new Vector2(map[j][0].image.Origin.X * (float)scale, map[j][0].image.Origin.Y * (float)scale);
                            map[j][0].image.Location = new Vector2(j * (float)Math.Round(60 * scale), 0 * (float)Math.Round(60 * scale) - (float)Math.Round(360 * scale)) + oragami;
                        }
                        else
                        {
                            currentSpot.reduceChonk();
                        }

                    }
                    int bonusX = 2;
                    while (true)
                    {
                        if (hippityHoppityYourRowIsNowMyProperty())
                        {
                            break;
                        }
                        rowBonus += bonusX;
                        bonusX++;
                    }
                    return false;
                }
            }
            return true;
        }

        void CreateExplosionParticles(Coordinate explosiveSpot)
        {            
            effects.Add(new ParticleEffect(ParticleEffect.EffectType.Explosion, pixel, explosiveSpot.image.Location, new List<Color> { Color.OrangeRed, Color.Crimson, Color.Black },
                               200, (int)explosiveSpot.explosive - 2, new List<double> { 1 * explosiveSpot.explosive, 2 * explosiveSpot.explosive, 3 * explosiveSpot.explosive },
                               new List<float> { 1f * explosiveSpot.explosive / explosiveImage.Width, 2f * explosiveSpot.explosive / pixel.Width, 3f * explosiveSpot.explosive / pixel.Width }
                               , explosiveScales, null, 200, 0, 0, 1, 1, true, 3, 3 + 3 * (int)(explosiveSpot.explosive / 2)));
        }

        void shadowPlace()
        {
            for (int i = 0; i < shadowPooey.boxes.Count; i++)
            {
                if (shadowPooey.boxes[i].explosive > 0)
                {
                    shadowPooey.boxes[i].image.Color = Color.Red;
                }
                else if (shadowPooey.boxes[i].speed)
                {
                    shadowPooey.boxes[i].image.Color = Color.Cyan;
                }
                else if (shadowPooey.boxes[i].Chonker())
                {
                    shadowPooey.boxes[i].chonkImage = null;
                    shadowPooey.boxes[i].image.Color = Color.Black;
                }
                else
                {
                    shadowPooey.boxes[i].image.Color = Color.DarkSlateGray;
                }
                shadowPooey.boxes[i].image.Image = projectionImage;
                shadowPooey.boxes[i].image.Depth -= .01f;
            }
            while (true)
            {
                shadowPooey.moveDown();
                for (int i = 0; i < shadowPooey.boxes.Count; i++)
                {
                    if (map[(int)shadowPooey.boxes[i].place.X][(int)shadowPooey.boxes[i].place.Y].isfull)
                    {
                        shadowPooey.forceUp();
                        return;
                    }
                }
                for (int i = 0; i < shadowPooey.boxes.Count; i++)
                {
                    if (shadowPooey.boxes[i].place.Y >= size.Y - 1)
                    {
                        return;
                    }
                }
            }
        }
        public void Draw(SpriteBatch bunch)
        {
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 6; j < map[i].Count; j++)
                {
                    map[i][j].Draw(bunch);
                }
            }
            for (int i = 0; i < effects.Count; i++)
            {
                effects[i].Draw(bunch);
            }
            if (shadowPooey.boxes[0].place == pooey.boxes[0].place)
            {
                ;
            }
            pooey.Draw(bunch);
            nextPooey.Draw(bunch);
            lastPooey.Draw(bunch);
            if (willProject)
            {
                shadowPooey.Draw(bunch);
            }
            if (savedPooey != null)
            {
                savedPooey.Draw(bunch);
            }
        }
    }
}