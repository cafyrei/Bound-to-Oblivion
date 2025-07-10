using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Oblivion
{
    public class GameStage
    {
        private List<ScrollingBackground> _scrollingBackground;
        private Player _player;
        private List<MinorEnemy> _minorEnemies;
        private Platform _platform;

        public GameStage(List<ScrollingBackground> scrollingBackground, Player player, List<MinorEnemy> minorEnemy, Platform platform)
        {
            _scrollingBackground = scrollingBackground;
            _player = player;
            _minorEnemies = minorEnemy;
            _platform = platform;

        }

        public void Update(GameTime gameTime, Camera2D _camera)
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


        public Vector2 GetPlayerPosition()
        {
            return _player.Position;
        }

        public void Draw(GameTime gameTime, SpriteBatch _spriteBatch)
        {
            _scrollingBackground[0].Draw(gameTime, _spriteBatch);
            _scrollingBackground[1].Draw(gameTime, _spriteBatch);
            _platform.Draw(_spriteBatch);
            _player.Draw(_spriteBatch);
            _scrollingBackground[2].Draw(gameTime, _spriteBatch);
            _scrollingBackground[3].Draw(gameTime, _spriteBatch);

            foreach (var enemy in _minorEnemies)
            {
                enemy.Draw(_spriteBatch);
            }

        }
    }
}
