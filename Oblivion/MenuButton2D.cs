using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Oblivion
{
    internal class MenuButton2D : Button2D
    {

        public Texture2D background_Texture;

        public MenuButton2D(Texture2D texture, Rectangle rectangle, Color? color, string text = "", SpriteFont font = null) :
        base(texture, rectangle, color, text, font)
        {

        }

        
        
    }
}

