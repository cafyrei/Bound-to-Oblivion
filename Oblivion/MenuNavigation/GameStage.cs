using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Oblivion
{
    internal class GameStage
    {
        private SpriteBatch _spriteBatch;
        private List<ScrollingBackground> _scrollingBackground;
        private Player _player;

        public GameStage(SpriteBatch spriteBatch, List<ScrollingBackground> scrollingBackground, Player player)
        {
            _spriteBatch = spriteBatch;
            _scrollingBackground = scrollingBackground;
            _player = player;
        }

        public void Update(GameTime gameTime)
        {
            foreach (var sb in _scrollingBackground)
            {
                sb.Update(gameTime);
            }

            _player.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var sb in _scrollingBackground)
            {
                sb.Draw(gameTime, _spriteBatch);
            }

            _player.Draw(_spriteBatch);
        }
    }
}
