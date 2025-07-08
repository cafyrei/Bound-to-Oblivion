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


        public GameStage(List<ScrollingBackground> scrollingBackground, Player player, List<MinorEnemy> minorEnemy)
        {
            _scrollingBackground = scrollingBackground;
            _player = player;
            _minorEnemies = minorEnemy;
        }

        public void Update(GameTime gameTime, Camera2D _camera)
        {
            foreach (var sb in _scrollingBackground)
            {
                sb.Update(gameTime);
            }

            _player.Update(gameTime);

            foreach (var enemy in _minorEnemies)
            {
                enemy.Update(gameTime);
            }
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

            foreach (var enemy in _minorEnemies)
            {
                enemy.Draw(_spriteBatch);
            }

            _player.Draw(_spriteBatch);

        }
    }
}
