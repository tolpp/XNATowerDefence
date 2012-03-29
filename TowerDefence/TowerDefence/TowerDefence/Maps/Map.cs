using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TowerDefence.Towers;

namespace TowerDefence.Maps
{
    class Map
    {
        private List<Vector2> routePoints;
        public List<Vector2> RoutePoints
        {
            get { return this.routePoints; }
        }

        Texture2D texBack;
        Texture2D texRoad;
        IServiceProvider serviceProvider;
        ContentManager content;

        public Map()
        {
            routePoints = new List<Vector2>();

            Vector2 p1 = new Vector2(40, -20);
            Vector2 p2 = new Vector2(40, 440);
            Vector2 p3 = new Vector2(300, 440);
            Vector2 p4 = new Vector2(300, 50);
            Vector2 p5 = new Vector2(550, 50);
            Vector2 p6 = new Vector2(550, 440);

            routePoints.Add(p1);
            routePoints.Add(p2);
            routePoints.Add(p3);
            routePoints.Add(p4);
            routePoints.Add(p5);
            routePoints.Add(p6);
        }

        public Map(IServiceProvider serviceProvider) : this()
        {
            this.serviceProvider = serviceProvider;
            this.content = new ContentManager(serviceProvider, "Content");

            this.texBack = content.Load<Texture2D>("Maps/mud");
            this.texRoad = content.Load<Texture2D>("Maps/road");
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texBack, new Rectangle(0, 0, 620, 480), Color.Snow);

            for (int i = 0; i < routePoints.Count - 1; i++)
            {
                Vector2 v1 = routePoints.ElementAt(i);
                Vector2 v2 = routePoints.ElementAt(i + 1);

                double distance = Distance(v1, v2);

                Rectangle r = new Rectangle((int)v1.X+16, (int)v1.Y+16, (int)distance, 38);
                
                float rotation = (float)Math.Atan2((v2.Y - v1.Y), (v2.X - v1.X));

                spriteBatch.Draw(texRoad, r, null, Color.Snow, rotation, new Vector2(0, texRoad.Height/2), SpriteEffects.None, 0);

                
            }

        }
        private double Distance(Vector2 v1, Vector2 v2)
        {
            double a = v1.X - v2.X;
            double b = v1.Y - v2.Y;

            double distance = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));

            return distance;
        }

        public bool isCollision(Tower t)
        {
            //for (int i = 0; i < routePoints.Count - 1; i++)
            //{
            //    Vector2 v1 = routePoints.ElementAt(i);
            //    Vector2 v2 = routePoints.ElementAt(i + 1);

            //    float towerCenterX = ((float)(t.Position.X + t.Width)) / 2;
            //    float towerCenterY = ((float)(t.Position.Y + t.Height)) / 2;

            //    Vector2 v3 = new Vector2(towerCenterX, towerCenterY);

            //    // distance of v3 point to v1-v2 line shall be calculated by area of triangular

            //    double line1 = Distance(v1, v2);
            //    double line2 = Distance(v1, v3);
            //    double line3 = Distance(v2, v3);

            //    double halfPerimeter = (line1 + line2 + line3) / 2;
            //    double area = Math.Sqrt(halfPerimeter * (halfPerimeter - line1) * (halfPerimeter - line2) * (halfPerimeter - line3));

            //    double distance = area / line1;

                

            //    if (distance < t.Width/2 + 19)
            //        return true;
            //}

            return false;
        }

    }
}
