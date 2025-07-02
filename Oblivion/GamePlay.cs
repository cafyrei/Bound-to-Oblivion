using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SharpDX.Direct2D1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace Oblivion
{
    internal class GamePlay
    {
        // Game Background
        Texture2D background_Texture;
        Background background;
        Rectangle backgroundRect;

        public GamePlay(ContentManager content, GraphicsDevice graphics)
        {
            LoadContent(content, graphics);
        }


        public void LoadContent(ContentManager Content, GraphicsDevice graphics)
        {
            // background_Texture = Content.Load<Texture2D>("Backgrounds/stage1BG");
            // backgroundRect = new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height);
            // background = new Background(background_Texture, backgroundRect, Color.White);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            // spriteBatch.Draw(background.Background_Texture, background.Background_Rectangle, background.Background_color);
        }
    }
}