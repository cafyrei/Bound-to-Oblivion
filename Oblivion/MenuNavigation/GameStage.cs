using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Oblivion
{
    public class GameStage
    {
        private List<ScrollingBackground> _scrollingBackground;
        private Player _player;
        private List<MinorEnemy> _minorEnemies;
        private Platform _platform;

        private bool _gamePause = false;
        KeyboardState _previousKeyboardState;

        private PauseMenu _pauseMenu;
        private SpriteFont _font;
        private readonly Action _onExitToMenu;
        public GameStage(List<ScrollingBackground> scrollingBackground, Player player, List<MinorEnemy> minorEnemy, Platform platform, Action onExitToMenu)
        {
            _scrollingBackground = scrollingBackground;
            _player = player;
            _minorEnemies = minorEnemy;
            _platform = platform;
            _previousKeyboardState = Keyboard.GetState();
            _onExitToMenu = onExitToMenu;
        }

        public void Load(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            _font = Content.Load<SpriteFont>("Fonts/PauseFont");
            _pauseMenu = new PauseMenu(graphicsDevice, _font, Content);
            _pauseMenu.OnResumeGame += gameResume;
            _pauseMenu.BackToMenu += backToMenu;
        }

        public void Update(GameTime gameTime, Camera2D camera)
        {
            KeyboardState _currentKeyboardState = Keyboard.GetState();
            AudioManager.PlayGameStageBGM();

            if (_currentKeyboardState.IsKeyDown(Keys.Escape) && !_previousKeyboardState.IsKeyDown(Keys.Escape))
            {
                _gamePause = !_gamePause;
            }

            _previousKeyboardState = _currentKeyboardState;

            _pauseMenu.Update();

            if (!_gamePause)
            {
                foreach (var sb in _scrollingBackground)
                {
                    sb.Update(gameTime);
                }

                _player.Update(gameTime, _platform.collision);

                foreach (var enemy in _minorEnemies)
                {
                    enemy.Update(gameTime, _platform.collision);
                }
            }
        }

        private void gameResume()
        {
            _gamePause = false;
        }

        private void backToMenu()
        {
            AudioManager.StopMusic();
            _onExitToMenu?.Invoke();
        }

        public Vector2 GetPlayerPosition()
        {
            return _player.Position;
        }


        public void Draw(GameTime gameTime, SpriteBatch _spriteBatch)
        {
            foreach (var sb in _scrollingBackground)
            {
                sb.Draw(gameTime, _spriteBatch);
            }

            _platform.Draw(_spriteBatch);

            foreach (var enemy in _minorEnemies)
            {
                enemy.Draw(_spriteBatch);
            }

            _player.Draw(_spriteBatch);

        }

        public void DrawUI(SpriteBatch _spriteBatch, Viewport _screenViewPort)
        {
            if (_gamePause)
            {
                _pauseMenu.Draw(_spriteBatch, _screenViewPort);
            }
        }
    }
}
