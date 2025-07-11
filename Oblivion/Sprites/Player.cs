using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Oblivion
{
    public class Player : Sprite
    {
        // Constants
        private const float BaseSpeed = 100f;
        private const float JumpSpeed = 550f;
        private const float Gravity = 800f;
        private const float FallMultiplier = 2.5f;
        private const float AttackDuration = 0.5f;
        private float AttackAnimDuration = 2.5f;
        private const float HitboxScale = 1.5f;

        // Movement and State
        private Vector2 velocity;
        private bool _isOnGround;
        private bool _isAttacking;
        private float _attackTimer;
        private bool _attackTurn;
        private KeyboardState _previousKeyboard;

        private MouseState _previousMouse;

        // Movement Speeds
        private readonly float _walkSpeed = 1f;
        private readonly float _runSpeed = 4f;

        // Graphics and Animation
        private SpriteAnimation2D _animation;
        private Rectangle _hitbox;
        public SpriteEffects Flip { get; private set; } = SpriteEffects.None;

        // Accessor
        public Vector2 Velocity => velocity;
        public Rectangle Hitbox => _hitbox;

        //HP System
        public HPBar _HPbar;

        float _maxHealth = 100f;
        float _minHealth = 0f;
        float _currentHealth;

        // Animation Control
        bool _isHit = false;
        float _hitTimer = 0f;
        float _hitDuration = 3f;

        public Player(Texture2D texture, SpriteAnimation2D animation, ContentManager Content, HPBar hpbar) : base(texture)
        {
            _texture = texture;
            _animation = animation;

            _currentHealth = _maxHealth;
            _HPbar = hpbar;
        }

        public void Update(GameTime gameTime, Dictionary<Vector2, Rectangle> collisionBlocks, List<MinorEnemy> enemies)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var input = Keyboard.GetState();
            var mouseInput = Mouse.GetState();

            velocity = new Vector2(0, velocity.Y);

            HandleMovement(input, gameTime);
            HandleJump(input);
            HandleAttack(mouseInput, deltaTime, enemies);
            ApplyGravity(deltaTime);

            Move(deltaTime);
            UpdateHitbox();
            HandleCollision(collisionBlocks);

            _animation.Update(gameTime);
            _previousKeyboard = input;
            _previousMouse = mouseInput;
        }

        private void HandleMovement(KeyboardState input, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            bool moveLeft = input.IsKeyDown(Keys.A);
            bool moveRight = input.IsKeyDown(Keys.D);
            bool run = input.IsKeyDown(Keys.LeftShift);

            float moveSpeed = run ? _runSpeed : _walkSpeed;

            if (_isAttacking) return;

            if (_isHit && !moveLeft && !moveRight)
            {
                _hitTimer += deltaTime;
                _animation.SetRow(3); // Hit
                if (_hitTimer >= _hitDuration)
                {
                    _isHit = false;
                    _hitTimer = 0f;
                }
                return;
            }

            // Movement logic
            if (moveRight && !moveLeft)
            {
                velocity.X = moveSpeed;
                Flip = SpriteEffects.FlipHorizontally;
            }
            else if (moveLeft && !moveRight)
            {
                velocity.X = -moveSpeed;
                Flip = SpriteEffects.None;
            }
            else
            {
                velocity.X = 0;
            }

            if (!_isOnGround)
            {
                if (velocity.Y < 0 && Math.Abs(velocity.X) < 0.1f)
                {
                    _animation.SetRow(5);
                }
                else 
                {
                    _animation.SetRow(run ? 6:7); 
                }
            }
            else
            {
                if (velocity.X == 0)
                {
                    _animation.SetRow(4);
                }
                else
                {
                    _animation.SetRow(run ? 6 : 7);
                }
            }
        }



        private void SetHealth(float value) // Update
        {
            _currentHealth = MathHelper.Clamp(value, _minHealth, _maxHealth);
            if (_currentHealth <= 0f)
            {
                Console.WriteLine("Game Over");
            }
            _HPbar.Update(_currentHealth);
        }
        public void TakeDamage(float dmg, Camera2D camera)
        {
            _isHit = true;
            _hitTimer = 0f;
            SetHealth(_currentHealth - dmg);
            camera.Shake(0.25f, 10f);
        }


        public void Heal(float heal)
        {
            SetHealth(_currentHealth + heal);
        }

        private void HandleAttack(MouseState input, float deltaTime, List<MinorEnemy> enemies)
        {

            bool leftClick = input.LeftButton == ButtonState.Pressed && _previousMouse.LeftButton == ButtonState.Released;
            bool rightClick = input.RightButton == ButtonState.Pressed && _previousMouse.RightButton == ButtonState.Released;

            if (_isAttacking)
            {
                _attackTimer += deltaTime;
                _animation.SetRow(_attackTurn ? 0 : 1);

                if (_attackTimer >= AttackDuration)
                {
                    _isAttacking = false;
                    _attackTimer = 0f;
                }

                _animation.SetRow(_attackTurn ? 0 : 1);
                return;
            }

            if (leftClick || rightClick)
            {
                float _damage = leftClick ? 10f : 25f;
                int newAnimationRow = leftClick ? 0 : 1;

                foreach (var enemy in enemies)
                {
                    if (_hitbox.Intersects(enemy.Hitbox))
                    {
                        enemy.TakeDamage(_damage);
                    }
                }

                _isAttacking = true;
                _attackTimer = 0f;
                _attackTurn = leftClick;

                // Set attack animation frame
                _animation.SetRow(_attackTurn ? 0 : 1);

                AudioManager.PlayAttack();
            }
        }

        private void HandleJump(KeyboardState input)
        {
            if (input.IsKeyDown(Keys.Space) && !_previousKeyboard.IsKeyDown(Keys.Space) && _isOnGround)
            {
                velocity.Y = -JumpSpeed;
                _animation.SetRow(5);
                _isOnGround = false;
            }
        }


        private void ApplyGravity(float deltaTime)
        {
            if (!_isOnGround)
            {
                velocity.Y += Gravity * deltaTime;
                if (velocity.Y > 0)
                {
                    velocity.Y += Gravity * (FallMultiplier - 1f) * deltaTime;
                }
            }
        }

        private void Move(float deltaTime)
        {
            Position.X += velocity.X * BaseSpeed * deltaTime;
            Position.X = MathHelper.Clamp(Position.X, 0, TextureManager.tileWidth - _animation.FrameWidthAccess * HitboxScale);

            Position.Y += velocity.Y * deltaTime;
        }

        private void UpdateHitbox()
        {
            int offset = 41;

            _hitbox = new Rectangle(
                (int)(Position.X + offset),
                (int)(Position.Y + offset / 2),
                (int)(_animation.FrameWidthAccess * HitboxScale) - offset * 2,
                (int)(_animation.FrameHeightAccess * HitboxScale) - offset
            );
        }

        private void HandleCollision(Dictionary<Vector2, Rectangle> tiles)
        {
            _isOnGround = false;

            Rectangle feetSensor = new Rectangle(
                _hitbox.X,
                _hitbox.Bottom,
                _hitbox.Width,
                2
            );

            foreach (var tile in tiles.Values)
            {
                if (_hitbox.Intersects(tile))
                {
                    Rectangle intersection = Rectangle.Intersect(_hitbox, tile);

                    if (intersection.Width < intersection.Height)
                    {
                        // Horizontal collision
                        if (_hitbox.Center.X < tile.Center.X)
                        {
                            Position.X -= intersection.Width;
                        }
                        else
                        {
                            Position.X += intersection.Width;
                        }
                    }
                    else
                    {
                        // Vertical collision
                        if (_hitbox.Center.Y < tile.Center.Y)
                        {
                            // Landing
                            Position.Y -= intersection.Height;
                            velocity.Y = 0;
                        }
                        else
                        {
                            // Hitting ceiling
                            Position.Y += intersection.Height;
                            velocity.Y = 0;
                        }
                    }

                    UpdateHitbox();
                }

                if (feetSensor.Intersects(tile))
                {
                    _isOnGround = true;
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