using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TowerDefence.Buttons
{
    class NextWaveButton : Button
    {

        public NextWaveButton(IServiceProvider serviceProvider, Vector2 position) : base(serviceProvider,position)
        {
            base.text = "Next Wave";
        }
    }
}
