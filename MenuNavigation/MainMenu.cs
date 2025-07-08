using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace Oblivion
{
    public class MainMenu
    {
        // Game Background
        Texture2D background_Texture;
        Background background;
        Rectangle backgroundRect;

        // All About Buttons
        public Texture2D button_Texture;
        List<Button2D> menuButtons = new List<Button2D>();
        string[] labels = { "Start", "Continue", "Credits", "Exit" };

        // All About Mouse
        MouseState currentMouseState;
        MouseState previousMouseState;

        // Font
        SpriteFont menu_Font;

        public bool StartPressed { get; private set; } = false;
        public bool CreditsPressed { get; private set; } = false;   
        public bool ExitPressed { get; private set; } = false;

        public MainMenu(ContentManager contents, GraphicsDevice graphics)
        {
            LoadContent(contents, graphics);
        }

        private void LoadContent(ContentManager Content, GraphicsDevice graphics)
        {

            background_Texture = Content.Load<Texture2D>("Backgrounds/game_menu");
            backgroundRect = new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height);
            background = new Background(background_Texture, backgroundRect, Color.White);

            AudioManager.PlayMenuBGM();
        
            button_Texture = Content.Load<Texture2D>("Backgrounds/transparent_texture");

            try
            {
                menu_Font = Content.Load<SpriteFont>("Fonts/Blade Stroke");
            }
            catch (ContentLoadException e)
            {
                Console.WriteLine("Error Loading Font : Basara Error Code : " + e.Message);
                // menu_Font = Content.Load<SpriteFont>("Fonts/Blade Stroke");
            } 

            for (int i = 0; i < labels.Length; i++)
            {
                Rectangle button_Rect = new Rectangle(180, 400 + i * 60, 120, 40);
                Button2D menu_Button = new Button2D(button_Texture, button_Rect, Color.White, labels[i], menu_Font)
                {
                    MenuHover = AudioManager._menuHover,
                    MenuClicked = AudioManager._menuClicked
                };

                int index = i;
                menu_Button.Clicked += () =>
                {
                    switch (index)
                    {
                        case 0: StartPressed = true; break;
                        case 1: break;
                        case 2: CreditsPressed = true ; break;
                        case 3: ExitPressed = true; break;
                    }
                    MediaPlayer.Stop();
                };
                menuButtons.Add(menu_Button);
            }
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

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background.Background_Texture, background.Background_Rectangle, background.Background_color);

            foreach (var button in menuButtons)
            {
                button.Draw(spriteBatch, currentMouseState);
            }
        }
    }

}
