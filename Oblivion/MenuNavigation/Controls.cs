using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Oblivion
{
    public class Controls
    {
        // Game Background
        Texture2D background_Texture;
        Background background;
        Rectangle backgroundRect;

        // All About Buttons
        public Texture2D button_Texture;
        List<Button2D> menuButtons = new List<Button2D>();
        string[] labels = { "Back" };

        // All About Mouse
        MouseState currentMouseState;
        MouseState previousMouseState;

        // Font
        SpriteFont menu_Font;

        public bool BackPressed { get; private set; } = false;
        public Controls(ContentManager contents, GraphicsDevice graphics)
        {
            LoadContent(contents, graphics);
        }

        private void LoadContent(ContentManager Content, GraphicsDevice graphics)
        {

            background_Texture = Content.Load<Texture2D>("Backgrounds/Controls");
            backgroundRect = new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height);
            background = new Background(background_Texture, backgroundRect, Color.White);
            button_Texture = Content.Load<Texture2D>("Backgrounds/transparent_texture");

            Rectangle button_Rect = new Rectangle(180, 200, 120, 40);
            Button2D menu_Buttons = new Button2D(button_Texture, button_Rect, Color.White, labels[0], menu_Font)
            {
                MenuHover = AudioManager._menuHover,
                MenuClicked = AudioManager._menuClicked
            };
            menu_Buttons.Clicked += () =>
        }

        public void Update(GameTime gameTime)
        {
            currentMouseState = Mouse.GetState();

            foreach (var menu_button in menuButtons)
            {
                menu_button.Update(currentMouseState, previousMouseState);
            }

            previousMouseState = currentMouseState;
        }
        public void Draw(SpriteBatch _spritebatch)
        {
            _spritebatch.Draw(background.Background_Texture, background.Background_Rectangle, background.Background_color);

            button.Draw(_spritebatch, currentMouseState);
        }
    }
}
