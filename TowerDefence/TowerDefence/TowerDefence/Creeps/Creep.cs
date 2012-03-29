using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TowerDefence.Creeps
{
    class Creep
    {
        #region Variables
        // Creep'in hizi
        private float speed;
        public float Speed
        {
            get { return this.speed; }
        }

        // Creep'in cani
        private float health;
        public float Health
        {
            get { return this.health; }
        }

        private int charge;
        public int Charge
        {
            get { return this.charge; }
        }

        //Creep' in canli olup olmadigini tutar
        private bool isAlive;
        public bool IsAlive
        {
            get { return isAlive; }
            set { this.isAlive = value; }
        }

        //Creep' in ana usse varip varmadigini tutar.
        private bool hasReached;
        public bool HasReached
        {
            get { return hasReached; }
        }

        //Creep'in pozisyonu
        private Vector2 position;
        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        private Texture2D texture;
        public Texture2D Texture
        {
            get { return this.texture; }
        }

        public int Height
        {
            get { return this.texture.Height; }
        }

        public int Width
        {
            get { return this.texture.Width; }
        }

        private List<Vector2> routePoints;
        public List<Vector2> RoutePoints
        {
            get { return this.routePoints; }
            set 
            { 
                this.routePoints = value;
                if (routePoints != null && this.routePoints.Count > 1)
                {
                    this.position = routePoints.ElementAt(0);
                    this.nextRoutePoint = routePoints.ElementAt(1);
                    nextRoutePointIndex = 1;
                }
            }
        }

        private Vector2 nextRoutePoint;
        private int nextRoutePointIndex;

        public static TimeSpan lastCreateTime;      

        private ContentManager content;

        #endregion

        public Creep(IServiceProvider serviceProvider, int level)
        {
            this.content = new ContentManager(serviceProvider, "Content");

            this.isAlive = true;
            this.speed = 1;
            this.health = 5 * level * level;
            this.charge = 5 * level;
            this.texture = content.Load<Texture2D>("Creeps/Creep");
        }
        public Creep(IServiceProvider serviceProvider, List<Vector2> routePoints, int level)
            : this(serviceProvider,level)
        {
            this.routePoints = routePoints;
            if (routePoints != null && this.routePoints.Count > 1)
            {
                this.position = routePoints.ElementAt(0);
                this.nextRoutePoint = routePoints.ElementAt(1);
            }
        }

        public Creep(IServiceProvider serviceProvider, Vector2 position, int level)
            : this(serviceProvider,level)
        {
            this.position = position;
        }

        public void Update(GameTime gameTime)
        {
            if (this.routePoints != null && hasReached == false)
            {
                if (hasReachedToNextPoint())
                {
                    this.position = nextRoutePoint;
                    nextRoutePointIndex++;
                    if (nextRoutePointIndex >= routePoints.Count)
                    {
                        hasReached = true; // Eger son noktaya kadar gelinmisse, ana usse ulasilmis demektir.
                    }
                    else
                    {
                        nextRoutePoint = routePoints.ElementAt(nextRoutePointIndex);
                    }
                }
                else
                {
                    moveToNextPoint();
                }

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.AntiqueWhite);
        }

        public void ReduceHealth(float value)
        {
            health -= value;
            if (health < 0)
                isAlive = false;
        }

        public void moveToNextPoint()
        {
            double radian = Math.Atan2((nextRoutePoint.Y - Position.Y), (nextRoutePoint.X - position.X));

            this.position.X += (float)(speed * Math.Cos(radian));
            this.position.Y += (float)(speed * Math.Sin(radian));
        }

        public bool hasReachedToNextPoint()
        {
            double distance = Distance(nextRoutePoint, this.Position);

            if (distance <= speed)
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
    }
}
