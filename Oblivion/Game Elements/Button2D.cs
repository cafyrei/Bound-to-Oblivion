using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Oblivion
{
    internal class Button2D
    {
        public Texture2D button_Texture;
        public Rectangle button_Rectangle;
        public Color button_color;
        public string button_Text;
        public SpriteFont button_Font;
        public event Action Clicked;
        private bool _hasHovered = false;

        private SoundEffect _menuHover;
        private SoundEffect _menuClicked;   

        public SoundEffect MenuHover
        {
            set => _menuHover = value;
        }
        public SoundEffect MenuClicked
        {
            set => _menuClicked = value;
        }

        public Button2D(Texture2D texture, Rectangle rectangle, Color? color, string text = "", SpriteFont font = null)
        {
            button_Texture = texture;
            button_Rectangle = rectangle;
            button_color = color ?? Color.White;
            button_Text = text;
            button_Font = font;
        }

        public void Update(MouseState mouseState, MouseState prevMouseState)
        {

            if (button_Rectangle.Contains(mouseState.Position))
            {
                if (!_hasHovered)
                {
                    _menuHover?.Play();
                    _hasHovered = true;
                }

                if (button_Rectangle.Contains(mouseState.Position) &&
                    mouseState.LeftButton == ButtonState.Pressed &&
                    prevMouseState.LeftButton == ButtonState.Released)
                {
                    Clicked?.Invoke();
                    _menuClicked?.Play();
                }
            }
            else
            {
                _hasHovered = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, MouseState mouse)
        {
            Color textColor = button_Rectangle.Contains(mouse.Position) ? Color.Yellow : Color.White;
            float fontSize = button_Rectangle.Contains(mouse.Position) ? 1.1f : 1.0f;


            if (!string.IsNullOrEmpty(button_Text) && button_Font != null)
            {
                Vector2 textSize = button_Font.MeasureString(button_Text);
                Vector2 textPosition = new Vector2(
                    button_Rectangle.X + (button_Rectangle.Width - textSize.X) / 2,
                    button_Rectangle.Y + (button_Rectangle.Height - textSize.Y) / 2
                );
                spriteBatch.DrawString(button_Font,
                button_Text, textPosition,
                textColor, 0f,
                Vector2.Zero,
                fontSize,
                SpriteEffects.None,
                0f);
            }
        }

    }
}