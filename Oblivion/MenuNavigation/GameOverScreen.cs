using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Oblivion
{
    public class GameOverScreen
    {
        private SpriteFont _font;
        private Texture2D _overlay;
        private SoundEffect _gameOverSFX;
        private bool _playedOnce = false;
        private string _message = "GAME OVER\nPress Enter to return to Main Menu";

        public GameOverScreen(ContentManager Content, GraphicsDevice graphics)
        {
            _font = Content.Load<SpriteFont>("Fonts/Blade Stroke");
            _gameOverSFX = AudioManager._gameOverSFX;

            _overlay = Content.Load<Texture2D>("Backgrounds/game_over");
        }

        public void Update()
        {
            if (!_playedOnce)
            {
                _gameOverSFX.Play();
                _playedOnce = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Game1.currentState = Game1.GameState.MainMenu;
                MainMenu.ResetFlags();
                _playedOnce = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Viewport viewport)
        {
            spriteBatch.Draw(_overlay, new Rectangle(0, 0, viewport.Width, viewport.Height), Color.White);

            string[] lines = _message.Split('\n');
            float totalHeight = lines.Length * _font.LineSpacing;

            float startY = (viewport.Height - totalHeight) / 2;

            for (int i = 0; i < lines.Length; i++)
            {
                Vector2 lineSize = _font.MeasureString(lines[i]);
                Vector2 linePosition = new Vector2(
                    (viewport.Width - lineSize.X) / 2,
                    startY + i * _font.LineSpacing
                );

                spriteBatch.DrawString(_font, lines[i], linePosition, Color.White);
            }
        }

    }
}
