using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Oblivion
{
    public class PauseMenu
    {
        private Texture2D _overlay;
        private SpriteFont _font;
        private MouseState _prevMouseState;
        private List<Button2D> _pauseButtons = new List<Button2D>();
        string[] _pauseMenuLabels = { "Resume", "Save", "Main Menu" };

        private Texture2D _buttonTexture;

        public event Action OnResumeGame;
        public event Action OnSaveGame;
        public event Action BackToMenu;


        public PauseMenu(GraphicsDevice graphicsDevice, SpriteFont font, ContentManager Content)
        {

            _overlay = new Texture2D(graphicsDevice, 1, 1);
            _overlay.SetData(new[] { new Color(0, 0, 0, 150) });
            _font = font;

            LoadContent(Content, graphicsDevice);
        }
        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {

            _buttonTexture = Content.Load<Texture2D>("Backgrounds/transparent_texture");

            int buttonWidth = 200;
            int buttonHeight = 50;
            int spacing = 20;

            int totalHeight = (_pauseMenuLabels.Length * (buttonHeight + spacing)) - spacing;
            int startY = (graphicsDevice.Viewport.Height - totalHeight) / 2 + 50; 
            for (int i = 0; i < _pauseMenuLabels.Length; i++)
            {
                int x = (graphicsDevice.Viewport.Width - buttonWidth) / 2;
                int y = startY + i * (buttonHeight + spacing);
                Rectangle button_Rect = new Rectangle(x, y, buttonWidth, buttonHeight);

                Button2D _pauseButton = new Button2D(_buttonTexture, button_Rect, Color.White, _pauseMenuLabels[i], _font)
                {
                    MenuHover = AudioManager._menuHover,
                    MenuClicked = AudioManager._pauseMenuClicked
                };

                int index = i;
                _pauseButton.Clicked += () =>
                {
                    switch (index)
                    {
                        case 0: OnResumeGame?.Invoke(); break; // Resume
                        case 1: OnSaveGame?.Invoke(); break;   // Save
                        case 2: BackToMenu?.Invoke(); break;   // Quit 
                    }
                };
                _pauseButtons.Add(_pauseButton);



            }
        }
        public void Update()
        {
            MouseState currentMouseState = Mouse.GetState();

            foreach (var button in _pauseButtons)
            {
                button.Update(currentMouseState, _prevMouseState);
            }
            _prevMouseState = currentMouseState;
        }
        

        public void Draw(SpriteBatch spriteBatch, Viewport viewport)
        {
            spriteBatch.Draw(_overlay,
                new Rectangle(0, 0, viewport.Width, viewport.Height),
                Color.White);

            string text = "PAUSED";
            Vector2 textSize = _font.MeasureString(text);
            Vector2 position = new Vector2(
                (viewport.Width - textSize.X) / 2,
                (viewport.Height - textSize.Y) / 2 - 150);

            spriteBatch.DrawString(_font, text, position, Color.White);

            foreach (var button in _pauseButtons)
            {
                button.Draw(spriteBatch, Mouse.GetState());
            }

        }
    }
}
