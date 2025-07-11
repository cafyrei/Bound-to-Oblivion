using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Oblivion
{
    public class GameOverScreen
    {
        private Texture2D _overlay;
        private SoundEffect _gameOverSFX;
        private bool _playedOnce = false;
        private float _shakeTimer = 0f;
        private float _shakeDuration = 0.5f;
        private float _shakeMagnitude = 10f;
        private Vector2 _shakeOffset = Vector2.Zero;
        private readonly System.Random _rand = new();



        public GameOverScreen(ContentManager Content, GraphicsDevice graphics)
        {
            _gameOverSFX = AudioManager._gameOverSFX;

            _overlay = Content.Load<Texture2D>("Backgrounds/final_gameover");
        }

        public void Update(GameTime gameTime)
        {
            if (!_playedOnce)
            {
                _gameOverSFX.Play();
                _playedOnce = true;
                _shakeTimer = _shakeDuration;
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_shakeTimer > 0)
            {
                _shakeTimer -= deltaTime;
                _shakeOffset = new Vector2(
                    (float)(_rand.NextDouble() * 2 - 1) * _shakeMagnitude,
                    (float)(_rand.NextDouble() * 2 - 1) * _shakeMagnitude
                );
            }
            else
            {
                _shakeOffset = Vector2.Zero;
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
            Rectangle shakeRect = new Rectangle(
                (int)_shakeOffset.X,
                (int)_shakeOffset.Y,
                viewport.Width,
                viewport.Height
            );

            spriteBatch.Draw(_overlay, shakeRect, Color.White);
        }

    }


}
