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
    class LightningTower : Tower
    {
        public LightningTower(IServiceProvider serviceProvider, Vector2 position)
            : base(serviceProvider,position)
        {
            base.Texture = content.Load<Texture2D>("Towers/LightningTower");
            base.Cost = 50;
            base.Damage = 40;
            base.startDamage = 40;
            base.Range = 120;
            base.Speed = 1;
            base.totalSpentMoneyOnTower = cost;
        }
        public override Tower CreateTowerWithSamePosition()
        {
            Tower t = new LightningTower(serviceProvider, this.Position);
            return t;
        }
        public override void shootTheCreep(Creep creep)
        {
            Vector2 towerCenter = new Vector2(this.Position.X + this.Width / 2, this.Position.Y + this.Height / 2);
            Bullet bullet = new LightningBullet(serviceProvider, this.Damage, towerCenter, creep);
            AliveBullets.Add(bullet);
        }
    }
}
