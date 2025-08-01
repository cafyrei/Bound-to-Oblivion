using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Oblivion
{
    public class Ending
    {
        private Texture2D _overlay;
        private SoundEffect _gameOverSFX;
        private float _fadeAlpha = 0f;
        private float _fadeSpeed = 0.5f;
        private bool _playedOnce = false;

        private Vector2 _shakeOffset = Vector2.Zero;
        private readonly System.Random _rand = new();



        public Ending(ContentManager Content, GraphicsDevice graphics)
        {
            _gameOverSFX = AudioManager._gameOverSFX;

            _overlay = Content.Load<Texture2D>("Backgrounds/Oblivion");
        }

        public void Update(GameTime gameTime)
        {
            _fadeAlpha += _fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _fadeAlpha = MathHelper.Clamp(_fadeAlpha, 0f, 1f);

            AudioManager.EndingGameBGM();

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Game1.currentState = Game1.GameState.MainMenu;
                MainMenu.ResetFlags();
                Game1.ResetToStage1();
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

            Color fadeColor = Color.White * _fadeAlpha;
            spriteBatch.Draw(_overlay, shakeRect, fadeColor);
        }


    }


}
