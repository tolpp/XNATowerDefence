using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TowerDefence.Buttons
{
    class Button
    {
        Texture2D texButtonTexture;
        Vector2 position;
        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        private int width = 170;
        public int Width
        {
            get { return width; }
            set { this.width = value; }
        }
        private int height = 30;
        public int Height
        {
            get { return height; }
            set { this.height = value; }
        }

        protected string text = "Standart";

        private IServiceProvider serviceProvider;
        protected SpriteFont Font1;
        private ContentManager content;

        public Button(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.content = new ContentManager(serviceProvider, "Content");

            texButtonTexture = content.Load<Texture2D>("Buttons/button-back");
            Font1 = content.Load<SpriteFont>("Buttons/ButtonFont");
            position = new Vector2(0, 0);
        }
        public Button(IServiceProvider serviceProvider,Vector2 position) : this(serviceProvider)
        {
            this.position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texButtonTexture, new Rectangle((int)position.X, (int)position.Y, (int)width, (int)height), Color.Snow);

            string output = getText();
            Vector2 FontOrigin = Font1.MeasureString(output) / 2;
            Vector2 textPosition = new Vector2(position.X + this.Width / 2, position.Y + this.Height / 2);
            spriteBatch.DrawString(Font1, output, textPosition, Color.Black,
                0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
        }
        public bool Contains(int X, int Y)
        {
            if (X > this.Position.X && X < this.Position.X + this.Width && Y > this.Position.Y && Y < this.Position.Y + this.Height)
                return true;
            else return false;
        }
        protected virtual string getText()
        {
            return text;
        }
    }
}
