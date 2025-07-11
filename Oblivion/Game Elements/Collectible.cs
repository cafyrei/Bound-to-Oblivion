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

        private Rectangle _collectbileRect;
        private float _hitbox = 1f;
        private Vector2 _position;
        private int FrameWidth = 7;
        private static readonly Random _random = new Random();
        bool isCollected = false;


        public Collectible(Texture2D texture, SpriteAnimation2D animation, Vector2 position)
        {
            _texture = texture;
            _animation = animation;
            _position = position;
            _collectbileRect = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width / FrameWidth, _texture.Height);
        }

        private void UpdateHitbox()
        {
            int offset = 232;

            _collectbileRect = new Rectangle(
                (int)(_position.X + offset),
                (int)(_position.Y + offset / 2),
                (int)(_texture.Width / FrameWidth * _hitbox) - offset * 2,
                (int)(_texture.Height * _hitbox) - offset
            );
        }
        public void Update(GameTime gameTime, Player player)
        {
            int newAnimationRow = 0;
            UpdateHitbox();
            HandleCollision(player);
            _animation.SetRow(newAnimationRow);
            _animation.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isCollected)
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

        private void HandleCollision(Player player)
        {
            int healPoints;
            Console.WriteLine("+HP OUTSIDE");
            if (!isCollected && player.Rectangle.Intersects(_collectbileRect))
            {
                Console.WriteLine("+HP INSIDE");
                healPoints = _random.Next(5, 11);
                _collectbileRect = Rectangle.Empty;
                isCollected = true;
                player.Heal(healPoints);
            }
        }
    }
}