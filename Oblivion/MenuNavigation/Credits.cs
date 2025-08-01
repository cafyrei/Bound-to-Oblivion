using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Oblivion
{
    public class Credits
    {
        // Game Background
        Texture2D background_Texture;
        Background background;
        Rectangle backgroundRect;

        // All About Buttons
        public Texture2D button_Texture;
        Button2D menu_Button;

        // All About Mouse
        MouseState currentMouseState;
        MouseState previousMouseState;

        // Font
        SpriteFont menu_Font;

        public bool BackPressed { get; private set; } = false;
        public Credits(ContentManager contents, GraphicsDevice graphics, Action OnExitToMenu)
        {
            LoadContent(contents, graphics);
        }

        private void LoadContent(ContentManager Content, GraphicsDevice graphics)
        {
            menu_Font = Content.Load<SpriteFont>("Fonts/Blade Stroke");
            background_Texture = Content.Load<Texture2D>("Backgrounds/Credits");
            backgroundRect = new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height);
            background = new Background(background_Texture, backgroundRect, Color.White);

            button_Texture = Content.Load<Texture2D>("Backgrounds/transparent_texture");
            Rectangle button_Rect = new Rectangle(70, 70, 120, 40);

            menu_Button = new Button2D(button_Texture, button_Rect, Color.White, "Back", menu_Font)
            {
                MenuHover = AudioManager._menuHover,
                MenuClicked = AudioManager._menuClicked
            };
            menu_Button.Clicked += () =>
            {
                BackPressed = true;
                Game1.currentState = Game1.GameState.MainMenu;
            };
        }

        public void Update()
        {
            currentMouseState = Mouse.GetState();
            menu_Button.Update(currentMouseState, previousMouseState);
            previousMouseState = currentMouseState;

             if (BackPressed)
            {
                Game1.currentState = Game1.GameState.MainMenu;
                MainMenu.ResetFlags();
                BackPressed = false; // Reset this so it doesn't immediately trigger again next time
            }
        }

        public void Draw(SpriteBatch _spritebatch)
        {
            _spritebatch.Draw(background.Background_Texture, background.Background_Rectangle, background.Background_color);
            menu_Button.Draw(_spritebatch, currentMouseState);
        }
    }
}
