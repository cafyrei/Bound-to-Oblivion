using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Oblivion
{
    public class GameStage
    {
        private List<ScrollingBackground> _scrollingBackground;
        private Player _player;

        public GameStage( List<ScrollingBackground> scrollingBackground, Player player)
        {
            _scrollingBackground = scrollingBackground;
            _player = player;
        }

        public void Update(GameTime gameTime, Camera2D _camera)
        {
            foreach (var sb in _scrollingBackground)
            {
                sb.Update(gameTime);
            }

            _player.Update(gameTime);
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

            _player.Draw(_spriteBatch);
        }
    }
}
