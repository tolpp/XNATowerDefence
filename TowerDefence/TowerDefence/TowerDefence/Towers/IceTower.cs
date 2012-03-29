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
    class IceTower : Tower
    {
        public IceTower(IServiceProvider serviceProvider, Vector2 position)
            : base(serviceProvider,position)
        {
            base.Texture = content.Load<Texture2D>("Towers/IceTower");
            base.Cost = 30;
            base.Damage = 25;
            base.startDamage = 25;
            base.Range = 110;
            base.Speed = 1;
            base.totalSpentMoneyOnTower = cost;
        }
        public override Tower CreateTowerWithSamePosition()
        {
            Tower t = new IceTower(serviceProvider, this.Position);
            return t;
        }
        public override void shootTheCreep(Creep creep)
        {
            Vector2 towerCenter = new Vector2(this.Position.X + this.Width / 2, this.Position.Y + this.Height / 2);
            Bullet bullet = new IceBullet(serviceProvider, this.Damage, towerCenter, creep);
            AliveBullets.Add(bullet);
        }
    }
}
