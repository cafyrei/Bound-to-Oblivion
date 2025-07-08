using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Oblivion
{
    public class GameStage
    {
        private List<ScrollingBackground> _scrollingBackground;
        private Player _player;
        private Platform _platform;

        public GameStage(List<ScrollingBackground> scrollingBackground, Player player, Platform platform)
        {
            _scrollingBackground = scrollingBackground;
            _player = player;
            _platform = platform;
        }

        public void Update(GameTime gameTime, Camera2D _camera)
        {
            foreach (var sb in _scrollingBackground)
            {
                sb.Update(gameTime);
            }

            _player.Update(gameTime, _platform.collision);
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
            _player.Draw(_spriteBatch);
        }
    }
}
