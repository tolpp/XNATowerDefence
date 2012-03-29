using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TowerDefence.Buttons
{
    class SellTowerButton : Button
    {
        public static int SellCost = 0;

        public SellTowerButton(IServiceProvider serviceProvider, Vector2 position)
            : base(serviceProvider, position)
        {
            base.text = "Sell Tower";
        }

        protected override string getText()
        {
            return "Sell Tower : $" + SellCost.ToString();
        }
    }
}
