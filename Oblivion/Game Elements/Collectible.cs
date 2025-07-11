using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Text.RegularExpressions;
using System;

namespace Oblivion
{
    public class Collectible
    {
        private SpriteAnimation2D _animation;
        private Texture2D _texture;
        private float _hitbox = 1f;
        private Vector2 _position;
        public Collectible(Texture2D texture, SpriteAnimation2D animation, Vector2 position)
        {
            _texture = texture;
            _animation = animation;
            _position = position;
        }

        public void Update(GameTime gameTime)
        {
            int newAnimationRow = 0;
            _animation.SetRow(newAnimationRow);
            _animation.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _texture,
                _position,
                _animation.GetSourceRect(),
                Color.White,
                0f,
                Vector2.Zero,
                _hitbox,
                SpriteEffects.None,
                0f
            );
        }
        
        private void HandleCollision
    }
}