using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Oblivion
{
    public class Portal
    {
        private Texture2D _texture;
        private SpriteAnimation2D _animation;
        private Vector2 _position;
        private float _scale = 2.5f;
        private Rectangle _portalRect;
        private bool _isActivated;

        public bool TriggerStageTransition => _isActivated;

        public Portal(Texture2D texture, SpriteAnimation2D animation, Vector2 position)
        {
            _texture = texture;
            _animation = animation;
            _position = position;
            UpdateHitbox();
        }

        public void Update(GameTime gameTime, Player player, int NumOfEnemies)
        {
            UpdateHitbox();
            _animation.Update(gameTime);

            if (player.Hitbox.Intersects(_portalRect))
            {
                Console.WriteLine("Teleport");
            }
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
                _scale,
                SpriteEffects.None,
                0f
            );
        }

        private void UpdateHitbox()
        {
            int offset = 20;
            _portalRect = new Rectangle(
                (int)(_position.X + offset),
                (int)(_position.Y + offset / 2),
                (int)(_animation.FrameWidthAccess * _scale) - offset * 2,
                (int)(_animation.FrameHeightAccess * _scale) - offset
            );
        }
    }
}
