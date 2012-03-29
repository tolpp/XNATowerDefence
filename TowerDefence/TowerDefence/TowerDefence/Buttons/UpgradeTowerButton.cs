using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TowerDefence.Buttons
{
    class UpgradeTowerButton : Button
    {
        public static int UpgradeCost = 0;

        public UpgradeTowerButton(IServiceProvider serviceProvider, Vector2 position) : base(serviceProvider,position)
        {
            base.text = "Upgrade";
            Height = 50;
        }
        
        protected override string getText()
        {
            return "Upgrade : $"+UpgradeCost.ToString();
        }
    }
}
