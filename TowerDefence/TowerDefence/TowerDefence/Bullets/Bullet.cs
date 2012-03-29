using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TowerDefence.Creeps;

namespace TowerDefence.Bullets
{
    class Bullet
    {
        #region Variables

        private bool isAlive;
        public bool IsAlive
        {
            get { return this.isAlive; }
        }

        // Bullet'in hizi
        protected float speed;
        public float Speed
        {
            get { return this.speed; }
        }

        // Bullet'in verecegi hasar
        protected float damage;
        public float Damage
        {
            get { return this.damage; }
        }

        //Tower'in pozisyonu
        protected Vector2 position;
        public Vector2 Position
        {
            get { return this.position; }
        }

        protected Creep destinationCreep;
        public Creep DestinationCreep
        {
            get { return this.destinationCreep; }
        }

        protected Texture2D texture;
        public Texture2D Texture
        {
            get { return this.texture; }
        }

        protected ContentManager content;

        #endregion

        public Bullet(IServiceProvider serviceProvider,float damage)
        {
            content = new ContentManager(serviceProvider, "Content");

            this.isAlive = true;
            speed = 2;
            this.damage = damage;
            this.texture = content.Load<Texture2D>("Bullets/Bullet");
        }

        public Bullet(IServiceProvider serviceProvider, float damage, Vector2 position)
            : this(serviceProvider, damage)
        {
            this.position = position;
        }

        public Bullet(IServiceProvider serviceProvider, float damage, Vector2 position, Creep destinationCreep)
            : this(serviceProvider, damage)
        {
            this.position = position;
            this.destinationCreep = destinationCreep;
        }

        public void Update(GameTime gameTime)
        {
            if (destinationCreep != null)
            {
                if (collisionCheck(destinationCreep))
                {
                    destinationCreep.ReduceHealth(this.Damage);
                }
                else
                {
                    moveToDestinationCreep();
                }
            }
            
        }

        private void moveToDestinationCreep()
        {
            double creepCenterX = ((double)(destinationCreep.Position.X * 2 + destinationCreep.Width)) / 2;
            double creepCenterY = ((double)(destinationCreep.Position.Y * 2 + destinationCreep.Height)) / 2;

            double bulletCenterX = ((double)(this.Position.X * 2 + this.Texture.Width)) / 2;
            double bulletCenterY = ((double)(this.Position.Y * 2 + this.Texture.Height)) / 2;

            double radian = Math.Atan2((creepCenterY - bulletCenterY), (creepCenterX - bulletCenterX));

            this.position.X += (float)(speed * Math.Cos(radian));
            this.position.Y += (float)(speed * Math.Sin(radian));
        }

        private bool collisionCheck(Creep creep)
        {
            if (distanceToCreep(creep) <= (creep.Width / 2) + (this.Texture.Width / 2))
            {
                this.isAlive = false;
                return true;
            }
            return false;
        }
        private double distanceToCreep(Creep creep)
        {
            double creepCenterX = ((double)(destinationCreep.Position.X*2 + destinationCreep.Width)) / 2;
            double creepCenterY = ((double)(destinationCreep.Position.Y*2 + destinationCreep.Height)) / 2;

            double bulletCenterX = ((double)(this.Position.X*2 + this.Texture.Width)) / 2;
            double bulletCenterY = ((double)(this.Position.Y*2 + this.Texture.Height)) / 2;

            return Math.Sqrt((creepCenterX - bulletCenterX) * (creepCenterX - bulletCenterX) + (creepCenterY - bulletCenterY) * (creepCenterY - bulletCenterY));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.AntiqueWhite);
        }
    }
}
