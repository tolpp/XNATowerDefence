using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TowerDefence.Towers;
using TowerDefence.Creeps;
using TowerDefence.Bullets;
using TowerDefence.Maps;
using TowerDefence.Buttons;

namespace TowerDefence
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<Tower> TowerList;
        List<Creep> CreepList;

        List<Creep> CreepsToBeDeleted;
        List<Creep> CreepsHasBeenReached;

        // Sag menu
        Texture2D texMenuBack;
        Vector2 vecMenuBack;
        SpriteFont Font1;
        Vector2 vecTimerPosition;
        Vector2 vecLevelPosition;
        List<Tower> menuTowers;
        SpriteFont costFont;
        Map map1;
        List<Button> buttonList;
        Button selectedButton;
        Vector2 vecStatPosition;

        //Game Over
        Texture2D texGameOver;

        Texture2D texDenied;
        Texture2D circle;

        Tower selectedTower;

        int numberOfCreepsToBeAdded = 0; // how much creeps remain to be added.
        int level = 0;
        int money = 30;

        int intake = 0;
        int maxIntake = 30;

        bool leftMouseButtonPressed = false;
        bool leftMouseButtonReleased = false;
        bool isThereTowerCollision = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        // Initialize iþlemleri burada
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            TowerList = new List<Tower>();
            CreepList = new List<Creep>();
            CreepsToBeDeleted = new List<Creep>();
            CreepsHasBeenReached = new List<Creep>();

            Creep.lastCreateTime = new TimeSpan(0);


            map1 = new Map(Services);


            this.IsMouseVisible = true;

            base.Initialize();
        }

        // Texture.. için load islemi burada
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            selectedTower = null;
            // Menu itemlerinin yuklanmesi
            Font1 = Content.Load<SpriteFont>("Menu/Font1");
            costFont = Content.Load<SpriteFont>("Menu/costFont");
            texMenuBack = Content.Load<Texture2D>("Menu/menu-back");
            vecMenuBack = new Vector2(620, 0);
            vecTimerPosition = new Vector2(710, 15);
            vecLevelPosition = new Vector2(625, 100);
            menuTowers = new List<Tower>();
            
            Tower standartTower = new Tower(Services, new Vector2(630, 35));
            standartTower.IsActive = false;
            menuTowers.Add(standartTower);

            Tower fireTower = new FireTower(Services, new Vector2(726, 35));
            fireTower.IsActive = false;
            menuTowers.Add(fireTower);

            Tower iceTower = new IceTower(Services, new Vector2(662, 35));
            iceTower.IsActive = false;
            menuTowers.Add(iceTower);

            Tower lightningTower = new LightningTower(Services, new Vector2(694, 35));
            lightningTower.IsActive = false;
            menuTowers.Add(lightningTower);

            circle = CreateCircle(100);
            buttonList = new List<Button>();
            vecStatPosition = new Vector2(625,220);
            buttonList.Add(new NextWaveButton(Services, new Vector2(625, 180)));
            buttonList.Add(new UpgradeTowerButton(Services, new Vector2(625, 350)));
            buttonList.Add(new SellTowerButton(Services, new Vector2(625, 430)));

            texGameOver = Content.Load<Texture2D>("game-over");
            texDenied = Content.Load<Texture2D>("denied");


            // TODO: use this.Content to load your game content here
        }

        //Unload islemi
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        // Bu fonksiyon belli aralýklarla çalisarak oyun istemlerini gerceklestirir
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            if (intake < maxIntake) // Henüz oyun bitmemisse
            {

                MouseState mouseState = Mouse.GetState();

                if (ButtonState.Pressed == mouseState.LeftButton)
                {
                    if (leftMouseButtonPressed)
                    {
                    }
                    else
                    {
                        if(mouseState.X < 620)
                            selectedTower = null;

                        foreach (Tower t in TowerList)
                        {
                            if (t.Contains(mouseState.X, mouseState.Y))
                                selectedTower = t;
                        }
                        foreach (Tower t in menuTowers)
                        {
                            if (t.Contains(mouseState.X, mouseState.Y) && money >= t.Cost)
                                selectedTower = t;
                        }


                        if (selectedButton == null)
                        {
                            foreach (Button b in buttonList)
                            {
                                if (b.Contains(mouseState.X, mouseState.Y))
                                    selectedButton = b;
                            }
                        }
                        leftMouseButtonPressed = true;
                        leftMouseButtonReleased = false;
                    }
                }
                if (ButtonState.Released == mouseState.LeftButton)
                {
                    if (leftMouseButtonReleased)
                    {
                    }
                    else
                    {
                        //
                        //Lef Mouse button release check on towers
                        //
                        if (selectedTower == null)
                        {
                        }
                        else
                        {
                            if (selectedTower.IsActive) // Oyun icindeki bir tower ise
                            {
                                if (!selectedTower.Contains(mouseState.X, mouseState.Y) && mouseState.X < 620)
                                    selectedTower = null;
                                else
                                {
                                    circle = CreateCircle((int)selectedTower.Range);
                                }

                            }
                            else // Menuden suruklenmekte olan bir tower ise
                            {
                                if (mouseState.X < 620 && !isThereTowerCollision) // Bir tower menu uzerine eklenemez
                                {
                                    if (money >= selectedTower.Cost)
                                    {
                                        Tower clone = selectedTower.CreateTowerWithSamePosition();
                                        clone.Position = new Vector2(mouseState.X - clone.Width / 2, mouseState.Y - clone.Height / 2);

                                        money -= (int)clone.Cost;

                                        TowerList.Add(clone);
                                    }
                                }
                                selectedTower = null;
                            }
                        }
                        leftMouseButtonReleased = true;
                        leftMouseButtonPressed = false;
                    }

                    //
                    //Mouse relase check on buttons 
                    //
                    if (selectedButton == null)
                    {
                    }
                    else
                    {
                        if (!selectedButton.Contains(mouseState.X, mouseState.Y))
                            selectedButton = null;
                        else
                        {
                            if (selectedButton.GetType() == typeof(NextWaveButton))
                            {
                                if (numberOfCreepsToBeAdded < 1)
                                {
                                    level++;
                                    numberOfCreepsToBeAdded += 25;
                                }
                            }
                            else if (selectedButton.GetType() == typeof(UpgradeTowerButton))
                            {
                                if (selectedTower.UpgradeCost <= this.money) // Upgrade icin yeterli para varsa
                                {
                                    money -= selectedTower.UpgradeCost;
                                    selectedTower.UpgradeTower();
                                }
                            }
                            else if (selectedButton.GetType() == typeof(SellTowerButton))
                            {
                                TowerList.Remove(selectedTower);
                                money += selectedTower.SellCost;
                                selectedTower = null;
                            }
                            
                            selectedButton = null;
                        }
                    }
                }

                TimeSpan totalTime = gameTime.TotalGameTime;
                if (totalTime - Creep.lastCreateTime >= TimeSpan.FromMilliseconds(500))
                {
                    if (numberOfCreepsToBeAdded > 0)
                    {
                        CreepList.Add(new Creep(Services, map1.RoutePoints, level));
                        Creep.lastCreateTime = totalTime;
                        numberOfCreepsToBeAdded--;
                    }
                }


                foreach (Tower t in TowerList)
                {
                    if (t.IsActive == true)
                    {
                        t.Update(gameTime, CreepList);
                    }
                }

                CreepsToBeDeleted = new List<Creep>();
                CreepsHasBeenReached = new List<Creep>();

                foreach (Creep c in CreepList) // Tum creepler icerisinde dolasiyorum
                {
                    if (!c.IsAlive) // Creep olmus ise, olu creep lisesine ekliyorum.
                    {
                        CreepsToBeDeleted.Add(c);
                    }
                    else if (c.HasReached) // Creep ana usse ulasmis ise, ulasan creepler listesine ekliyorum.
                    {
                        CreepsHasBeenReached.Add(c);
                    }
                    else // Hala yasayan ve devam eden bir creepse, u guncellenmeye devam ediyor.
                    {
                        c.Update(gameTime);
                    }
                }

                foreach (Creep c in CreepsToBeDeleted)
                {
                    CreepList.Remove(c);
                    money += (int)c.Charge;
                }
                foreach (Creep c in CreepsHasBeenReached)
                {
                    CreepList.Remove(c);
                    intake++;
                }
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        private bool TowerCollisionCheck(Tower clone)
        {
            isThereTowerCollision = false;
            if (selectedTower != null && !selectedTower.IsActive)
            {
                foreach (Tower t in TowerList)
                {
                    if (clone.isCollised(t))
                    {
                        isThereTowerCollision = true;
                        return isThereTowerCollision;
                    }
                }
                isThereTowerCollision = map1.isCollision(clone);
                
            }

            return isThereTowerCollision;
        }

        // Her update islemi icin cizim yapilir
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            if (intake < maxIntake)
            {
                map1.Draw(spriteBatch);
                drawMenu(spriteBatch, gameTime);
                foreach (Tower t in TowerList)
                {
                    t.Draw(spriteBatch);
                }
                foreach (Creep c in CreepList)
                {
                    c.Draw(spriteBatch);
                }

                drawSelectedTower(spriteBatch);

            }
            else
            {
                drawGameOver(spriteBatch);
            }

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void drawMenuTowerOptions(SpriteBatch spriteBatch)
        {
            string output = "==== Tower ====\nLevel : "+selectedTower.Level+"\nDamage: "+selectedTower.Damage+"\nRange : "+selectedTower.Range+"\nSpeed : "+selectedTower.Speed.ToString()+"/s";
            UpgradeTowerButton.UpgradeCost = selectedTower.UpgradeCost;
            SellTowerButton.SellCost = selectedTower.SellCost;
            spriteBatch.DrawString(Font1, output, vecStatPosition, Color.LightGreen);

            Rectangle r = new Rectangle(640, 200, 30, 30);

            foreach (Button b in buttonList)
            {
                if (b.GetType() != typeof(NextWaveButton))
                    b.Draw(spriteBatch);
            }
        }

        private void drawGameOver(SpriteBatch spriteBatch)
        {
            Rectangle r = new Rectangle(0, 0, 800, 480);

            spriteBatch.Draw(texGameOver, r, Color.Snow);
        }

        private void drawSelectedTower(SpriteBatch spriteBatch)
        {
            if (selectedTower != null)
            {
                if (selectedTower.IsActive)
                {
                    Rectangle r = new Rectangle((int)(selectedTower.Position.X + selectedTower.Width/2 - selectedTower.Range),(int)(selectedTower.Position.Y + selectedTower.Height/2 - selectedTower.Range),(int)selectedTower.Range*2,(int)(selectedTower.Range*2));
                    spriteBatch.Draw(circle,r,Color.White);
                    drawMenuTowerOptions(spriteBatch);
                }
                else
                {
                    Tower clone = selectedTower.CreateTowerWithSamePosition();
                    MouseState mouseState = Mouse.GetState();
                    clone.Position = new Vector2(mouseState.X - clone.Width / 2, mouseState.Y - clone.Height / 2);

                    circle = CreateCircle((int)clone.Range);
                    Rectangle rCircle = new Rectangle((int)(clone.Position.X + clone.Width / 2 - clone.Range), (int)(clone.Position.Y + clone.Height / 2 - clone.Range), (int)clone.Range * 2, (int)(clone.Range * 2));
                    spriteBatch.Draw(circle, rCircle, Color.White);

                    foreach (Tower t in TowerList)
                    {
                        TowerCollisionCheck(clone);
                    }
                    
                    clone.Draw(spriteBatch);
                    if (isThereTowerCollision)
                    {
                        Rectangle r = new Rectangle((int)clone.Position.X, (int)clone.Position.Y, clone.Width, clone.Height);
                        spriteBatch.Draw(texDenied, r, Color.Snow);
                    }

                }
            }
        }

        private void drawMenu(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texMenuBack, vecMenuBack, Color.AntiqueWhite);
            TimeSpan time = gameTime.TotalGameTime;
            string output = time.Minutes.ToString() + "." + time.Seconds.ToString() + "." + time.Milliseconds.ToString();

            Vector2 FontOrigin = Font1.MeasureString(output) / 2;
            spriteBatch.DrawString(Font1, output, vecTimerPosition, Color.LightGreen, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

            foreach (Tower t in menuTowers)
            {
                drawMenuTower(spriteBatch, t);
            }
            foreach (Button b in buttonList)
            {
                if(b.GetType() == typeof(NextWaveButton))
                    b.Draw(spriteBatch);
            }

            output = "Wave : " + level.ToString() + "\nMoney : $" + money.ToString() + "\nIntake: " + intake.ToString() + "/" + maxIntake.ToString();

            FontOrigin = new Vector2(0, 0);
            // Draw the string
            spriteBatch.DrawString(Font1, output, vecLevelPosition, Color.LightGreen,
                0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);


        }

        private void drawMenuTower(SpriteBatch spriteBatch, Tower t)
        {
            t.Draw(spriteBatch);
            spriteBatch.DrawString(costFont, "$" + t.Cost.ToString(), new Vector2(t.Position.X + 2, t.Position.Y + 34), Color.LightGreen);
        }
        public Texture2D CreateCircle(int radius)
        {
            int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
            Texture2D texture = new Texture2D(GraphicsDevice, outerRadius, outerRadius);

            Color[] data = new Color[outerRadius * outerRadius];

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            // Work out the minimum step necessary using trigonometry + sine approximation.
            double angleStep = 1f / radius;

            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle));

                data[y * outerRadius + x + 1] = Color.White;
            }

            texture.SetData(data);
            return texture;
        }
    }
}
