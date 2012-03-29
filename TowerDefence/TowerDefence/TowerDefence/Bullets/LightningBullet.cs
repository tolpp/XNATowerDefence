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
    class LightningBullet : Bullet
    {
        public LightningBullet(IServiceProvider serviceProvider, float damage, Vector2 position, Creep destinationCreep)
            : base(serviceProvider, damage,position,destinationCreep)
        {
            base.position = position;
            base.destinationCreep = destinationCreep;
            base.texture = content.Load<Texture2D>("Bullets/LightningBullet");
            base.speed = 3;

        }
    }
}
