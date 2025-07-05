using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Oblivion
{
    internal class Background
    {
        Texture2D background_Texture;
        Rectangle background_Rectangle;
        Color background_color;

        public Background(Texture2D texture, Rectangle rectangle, Color color)
        {
            background_Texture = texture;
            background_Rectangle = rectangle;
            background_color = color;
        }

        public Texture2D Background_Texture
        {
            get => background_Texture;
        }
        public Rectangle Background_Rectangle
        {
            get => background_Rectangle;
        }
        public Color Background_color
        {
            get => background_color;
        }
    }
}
