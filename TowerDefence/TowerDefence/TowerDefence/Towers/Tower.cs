using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TowerDefence.Creeps;
using TowerDefence.Bullets;

namespace TowerDefence.Towers
{
    class Tower
    {
        #region Variables
        // Tower'in bir saniyedeki atis sayisi
        protected float speed;
        public float Speed
        {
            get { return this.speed; }
            set { this.speed = value; }
        }

        // Tower'in atislarinin ne kadar hasar verdigi
        protected float damage;
        public float Damage
        {
            get { return this.damage; }
            set { this.damage = value; }
        }

        protected float startDamage;
        protected float totalSpentMoneyOnTower;

        public int UpgradeCost
        {
            get { return level * level * (int)cost; }
        }

        public int SellCost
        {
            get { return (int)totalSpentMoneyOnTower / 2; }
        }

        protected int level;
        public int Level
        {
            get { return this.level; }
            set { this.level = value; }
        }

        private TimeSpan lastShootTime;
        private TimeSpan totalTime;

        //Tower'in ne kadar menzile sahip oldugu
        protected float range;
        public float Range
        {
            get { return this.range; }
            set { this.range = value; }
        }

        // Tower'in maliyeti
        protected float cost;
        public float Cost
        {
            get { return this.cost; }
            set { this.cost = value; }
        }

        //Tower'in kullanimda olup olmadigi
        private bool isActive;
        public bool IsActive
        {
            get { return this.isActive; }
            set { this.isActive = value; }
        }
        public int Height
        {
            get { return this.texture.Height; }
        }

        public int Width
        {
            get { return this.texture.Width; }
        }

        //Tower'in pozisyonu
        protected Vector2 position;
        public Vector2 Position
        {
            get { return this.position; }
            set
            {
                position = value;
            }
        }

        // Tower icin kullanilan texture
        protected Texture2D texture;
        public Texture2D Texture
        {
            get { return this.texture; }
            set { this.texture = value; }
        }

        private SpriteFont levelFont;

        protected ContentManager content;
        protected IServiceProvider serviceProvider;

        protected List<Bullet> AliveBullets;

        #endregion

        #region Constructors
        public Tower(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.content = new ContentManager(serviceProvider, "Content");

            this.AliveBullets = new List<Bullet>();

            this.speed = 1;
            this.damage = 15;
            this.startDamage = 15;
            this.cost = 15;
            this.totalSpentMoneyOnTower = cost;
            this.isActive = true;
            this.range = 80;
            this.level = 1;
            this.texture = content.Load<Texture2D>("Towers/Tower");
            this.levelFont = content.Load<SpriteFont>("Towers/towerFont");

        }
        public Tower(IServiceProvider serviceProvider, Vector2 position)
            : this(serviceProvider)
        {
            Position = position;
        }
        #endregion

        #region Update
        public void Update(GameTime gameTime, List<Creep> CreepList)
        {
            List<Bullet> deadBullets = new List<Bullet>();
            foreach (Bullet bullet in AliveBullets)
            {
                bullet.Update(gameTime);
                if (!bullet.IsAlive)
                    deadBullets.Add(bullet);
            }


            foreach (Bullet bullet in deadBullets) // Ölü mermileri temizler
            {
                AliveBullets.Remove(bullet);
            }

            totalTime = gameTime.TotalGameTime;
            if (totalTime - lastShootTime >= TimeSpan.FromMilliseconds(1000 / speed))
            {
                Creep nearestCreep = getNearestCreepInRange(CreepList);
                if (nearestCreep != null)
                {
                    shootTheCreep(nearestCreep);
                    lastShootTime = totalTime;
                }
            }
            else
            {

            }
        }
        #endregion

        #region Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.AntiqueWhite);
            if (isActive)
            {
                Vector2 centerPosition = new Vector2(this.Position.X + this.Width/2 , this.Position.Y + this.Height/2);
                string str = this.Level.ToString();
                Vector2 FontOrigin = levelFont.MeasureString(str) / 2;
                spriteBatch.DrawString(levelFont, str, centerPosition, Color.LightGreen,0 ,FontOrigin, 1.0f,SpriteEffects.None,0);
            }

            foreach (Bullet bullet in AliveBullets)
            {
                bullet.Draw(spriteBatch);
            }
        }
        #endregion

        public bool isCreepInRange(Creep creep)
        {
            float creepCenterX = ((float)(creep.Position.X + creep.Width)) / 2;
            float creepCenterY = ((float)(creep.Position.Y + creep.Height)) / 2;

            float towerCenterX = ((float)(this.Position.X + this.Width)) / 2;
            float towerCenterY = ((float)(this.Position.Y + this.Height)) / 2;

            if (Math.Sqrt((towerCenterX - creepCenterX) * (towerCenterX - creepCenterX) + (towerCenterY - creepCenterY) * (towerCenterY - creepCenterY)) < this.Range)
            {
                return true;
            }

            return false;
        }

        public virtual void shootTheCreep(Creep creep)
        {
            Vector2 towerCenter = new Vector2(this.Position.X + this.Width / 2, this.Position.Y + this.Height / 2);
            Bullet bullet = new Bullet(serviceProvider, this.Damage, towerCenter, creep);
            AliveBullets.Add(bullet);
        }

        public Creep getNearestCreepInRange(List<Creep> CreepList)
        {
            double minDistance = 90000000;
            Creep nearestCreep = null;

            foreach (Creep c in CreepList)
            {
                double distance = distanceToCreep(c);
                if (distance < minDistance && distance <= this.Range) // En küçük distance ise ve Range içerisinde ise
                {
                    minDistance = distance;
                    nearestCreep = c;
                }
            }

            return nearestCreep;
        }

        private double distanceToCreep(Creep creep)
        {
            double creepCenterX = ((double)(creep.Position.X * 2 + creep.Width)) / 2;
            double creepCenterY = ((double)(creep.Position.Y * 2 + creep.Height)) / 2;

            double towerCenterX = ((double)(this.Position.X * 2 + this.Texture.Width)) / 2;
            double towerCenterY = ((double)(this.Position.Y * 2 + this.Texture.Height)) / 2;

            double distance = Math.Sqrt((creepCenterX - towerCenterX) * (creepCenterX - towerCenterX) + (creepCenterY - towerCenterY) * (creepCenterY - towerCenterY));

            return distance;
        }

        public bool Contains(int X, int Y)
        {
            if (X > this.Position.X && X < this.Position.X + this.Width && Y > this.Position.Y && Y < this.Position.Y + this.Height)
                return true;
            else return false;
        }
        public virtual Tower CreateTowerWithSamePosition()
        {
            Tower t = new Tower(serviceProvider, this.Position);
            return t;
        }

        public bool isCollised(Tower t)
        {
            double t1CenterX = ((double)(this.Position.X * 2 + this.Width)) / 2;
            double t1CenterY = ((double)(this.Position.Y * 2 + this.Height)) / 2;

            double t2CenterX = ((double)(t.Position.X * 2 + t.Width)) / 2;
            double t2CenterY = ((double)(t.Position.Y * 2 + t.Height)) / 2;

            double distance = Distance(new Vector2((float)t1CenterX,(float)t1CenterY), new Vector2((float)t2CenterX,(float)t2CenterY));
            if (distance <= this.Width / 2 + t.Width / 2)
                return true;
            return false;
        }

        public double Distance(Vector2 v1, Vector2 v2)
        {
            double a = v1.X - v2.X;
            double b = v1.Y - v2.Y;

            double distance = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));

            return distance;
        }

        public virtual void UpgradeTower()
        {
            totalSpentMoneyOnTower = Cost + UpgradeCost;
            level++;
            damage = startDamage * level;
        }

    }
}
