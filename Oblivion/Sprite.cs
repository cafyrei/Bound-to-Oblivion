using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Oblivion
{
    internal class Sprite
    {
        Texture2D sprite_Texture;
        Rectangle sprite_Rectangle;
        Color sprite_color;

        public Sprite(Texture2D texture, Rectangle rectangle, Color color)
        {
            sprite_Texture = texture;
            sprite_Rectangle = rectangle;
            sprite_color = color;
        }

        public Texture2D Sprite_Texture
        {
            get => sprite_Texture;
        }
        public Rectangle Sprite_Rectangle
        {
            get => sprite_Rectangle;
        }
        public Color Sprite_color
        {
            get => sprite_color;
        }
    }
}
