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
    class FireTower : Tower
    {
        public FireTower(IServiceProvider serviceProvider, Vector2 position)
            : base(serviceProvider,position)
        {
            base.Texture = content.Load<Texture2D>("Towers/FireTower");
            base.Cost = 200;
            base.Damage = 80;
            base.startDamage = 80;
            base.Range = 150;
            base.Speed = 2;
            base.totalSpentMoneyOnTower = cost;
        }
        public override Tower CreateTowerWithSamePosition()
        {
            Tower t = new FireTower(serviceProvider, this.Position);
            return t;
        }
        public override void shootTheCreep(Creep creep)
        {
            Vector2 towerCenter = new Vector2(this.Position.X + this.Width / 2, this.Position.Y + this.Height / 2);
            Bullet bullet = new FireBullet(serviceProvider, this.Damage, towerCenter, creep);
            AliveBullets.Add(bullet);
        }
    }
}
