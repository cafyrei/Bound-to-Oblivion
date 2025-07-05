using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Oblivion
{
    public class Player
    {
        Texture2D player_Texture;
        Vector2 player_Position;

        Rectangle player_Rectangle;
        Color player_color;

        public Player(Texture2D texture, Vector2 position, Color color)
        {
            player_Texture = texture;
            player_Position = position;
            player_Rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            player_color = color;
        }

        public Texture2D Player_Texture
        {
            get => player_Texture;
        }
        public Vector2 Player_Position
        {
            get => player_Position;
            set => player_Position = value;
        }

        public Rectangle Player_Rectangle
        {
            get => player_Rectangle;
        }
        public Color Player_color
        {
            get => player_color;
        }

    }
}
