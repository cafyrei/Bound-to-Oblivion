using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Oblivion
{
    public enum EnemyState
    {
        Idle,
        Patrol,
        Chase,
        Attack
    }

    public class MinorEnemy : Sprite
    {
        private SpriteAnimation2D _animation;
        public SpriteEffects Flip = SpriteEffects.None;
        private Rectangle _hitbox;

        // Movement & Physics
        private Vector2 _enemyVelocity = Vector2.Zero;
        private float _walkSpeed = 60f;
        private float _leftBound, _rightBound;
        private bool _isOnGround = false;

        // Constants
        private const float Gravity = 500f;
        private const float FallMultiplier = 2.5f;
        private const float HitboxScale = 2f;

        // AI
        private EnemyState _currentState = EnemyState.Idle;
        private float _idleTimer = 0f;
        private float _idleDuration = 2f;

        public MinorEnemy(Texture2D texture, SpriteAnimation2D animation, float leftBound, float rightBound)
            : base(texture)
        {
            _animation = animation;
            _enemyVelocity = new Vector2(_walkSpeed, 0); // Start moving right
            _leftBound = leftBound;
            _rightBound = rightBound;
        }

        public void Update(GameTime gameTime, Dictionary<Vector2, Rectangle> collisionBlocks)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            int newAnimationRow = 3;

            ApplyGravity(deltaTime);
            Position.Y += _enemyVelocity.Y * deltaTime;

            switch (_currentState)
            {
                case EnemyState.Idle:
                    _idleTimer += deltaTime;
                    if (_idleTimer >= _idleDuration)
                    {
                        _idleTimer = 0;
                        _currentState = EnemyState.Patrol;
                    }
                    break;

                case EnemyState.Patrol:
                    float nextX = Position.X + _enemyVelocity.X * deltaTime;

                    // Reverse direction at bounds
                    if (nextX <= _leftBound)
                    {
                        Position.X = _leftBound;
                        _enemyVelocity.X = _walkSpeed;
                        Flip = SpriteEffects.None;
                    }
                    else if (nextX >= _rightBound)
                    {
                        Position.X = _rightBound;
                        _enemyVelocity.X = -_walkSpeed;
                        Flip = SpriteEffects.FlipHorizontally;
                    }

                    // Move
                    Position.X += _enemyVelocity.X * deltaTime;
                    newAnimationRow = 4;
                    break;
            }

            UpdateHitbox();
            HandleCollision(collisionBlocks);

            _animation.SetRow(newAnimationRow);
            _animation.Update(gameTime);
        }

        private void ApplyGravity(float deltaTime)
        {
            if (!_isOnGround)
            {
                _enemyVelocity.Y += Gravity * deltaTime;
                if (_enemyVelocity.Y > 0)
                {
                    _enemyVelocity.Y += Gravity * (FallMultiplier - 1f) * deltaTime;
                }
            }
        }

        private void UpdateHitbox()
        {
            int horizontalTrim = 40;
            int verticalTrim = 10;

            _hitbox = new Rectangle(
                (int)(Position.X + horizontalTrim),
                (int)(Position.Y + verticalTrim / 2),
                Math.Max(1, (int)(_animation.FrameWidthAccess * HitboxScale) - horizontalTrim * 2),
                Math.Max(1, (int)(_animation.FrameHeightAccess * HitboxScale) - verticalTrim)
            );
        }

        private void HandleCollision(Dictionary<Vector2, Rectangle> tiles)
        {
            _isOnGround = false;

            foreach (var tile in tiles.Values)
            {
                if (_hitbox.Intersects(tile))
                {
                    Rectangle intersection = Rectangle.Intersect(_hitbox, tile);

                    if (intersection.Width < intersection.Height)
                    {
                        // Horizontal collision (wall)
                        if (_hitbox.Center.X < tile.Center.X)
                        {
                            Position.X -= intersection.Width;
                            _enemyVelocity.X = -_walkSpeed;
                            Flip = SpriteEffects.FlipHorizontally;
                        }
                        else
                        {
                            Position.X += intersection.Width;
                            _enemyVelocity.X = _walkSpeed;
                            Flip = SpriteEffects.None;
                        }
                    }
                    else
                    {
                        // Vertical collision
                        if (_hitbox.Center.Y < tile.Center.Y && intersection.Height < _hitbox.Height / 2f)
                        {
                            Position.Y -= intersection.Height;
                            _enemyVelocity.Y = 0;
                            _isOnGround = true;
                        }
                        else
                        {
                            Position.Y += intersection.Height;
                            _enemyVelocity.Y = 0;
                        }
                    }

                    UpdateHitbox();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _texture,
                Position,
                _animation.GetSourceRect(),
                Color.White,
                0f,
                Vector2.Zero,
                HitboxScale,
                Flip,
                Layer
            );
        }
    }
}

