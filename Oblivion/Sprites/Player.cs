using System;
using System.Collections.Generic;
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
        private const float Gravity = 950f;
        private const float FallMultiplier = 2.5f;
        private const float AttackDuration = 0.5f;
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
        float _maxHealth;
        float _health;
        float _maxLowestHealth = 23;

        public Player(Texture2D texture, SpriteAnimation2D animation, ContentManager Content, HPBar _hpBar, float maxValue) : base(texture)
        {
            _texture = texture;
            _animation = animation;
            _maxHealth = maxValue;
            _health = maxValue;
            _HPbar = _hpBar;
        }

        public void Update(GameTime gameTime, Dictionary<Vector2, Rectangle> collisionBlocks)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var input = Keyboard.GetState();
            var mouseInput = Mouse.GetState();

            velocity = new Vector2(0, velocity.Y);

            // if (_isOnGround)
            // {
            //     Console.WriteLine("Is on Ground");
            // }
            // else
            // {
            //     Console.WriteLine("NOT IN GROUND");
            // }

            HandleMovement(input);
            HandleJump(input);
            HandleAttack(input, deltaTime);
            ApplyGravity(deltaTime);

            Move(deltaTime);
            UpdateHitbox();
            HandleCollision(collisionBlocks);

            TakeDamage(10f, mouseInput, _previousMouse);
            Heal(10f, mouseInput, _previousMouse);

            _animation.Update(gameTime);
            _previousKeyboard = input;
            _previousMouse = mouseInput;

            _HPbar.Update(_health);
        }

        private void HandleMovement(KeyboardState input)
        {
            bool moveLeft = input.IsKeyDown(Keys.A);
            bool moveRight = input.IsKeyDown(Keys.D);
            bool run = input.IsKeyDown(Keys.LeftShift);

            float moveSpeed = run ? _runSpeed : _walkSpeed;

            if (moveRight && !moveLeft)
            {
                velocity.X = moveSpeed;
                Flip = SpriteEffects.FlipHorizontally;
                _animation.SetRow(run ? 6 : 7);
            }
            else if (moveLeft && !moveRight)
            {
                velocity.X = -moveSpeed;
                Flip = SpriteEffects.None;
                _animation.SetRow(run ? 6 : 7);
            }
            else
            {
                _animation.SetRow(4); // Idle
            }
        }

        private void HandleJump(KeyboardState input)
        {
            if (input.IsKeyDown(Keys.Space) && !_previousKeyboard.IsKeyDown(Keys.Space) && _isOnGround)
            {
                velocity.Y = -JumpSpeed;
                _isOnGround = false;
            }
        }

        private void HandleAttack(KeyboardState input, float deltaTime)
        {
            bool attackPressed = input.IsKeyDown(Keys.E) && !_previousKeyboard.IsKeyDown(Keys.E);

            if (_isAttacking)
            {
                _attackTimer += deltaTime;
                if (_attackTimer >= AttackDuration)
                {
                    _isAttacking = false;
                    _attackTimer = 0f;
                }
            }
            else if (attackPressed)
            {
                _isAttacking = true;
                _attackTimer = 0f;
                _attackTurn = !_attackTurn;
                _animation.SetRow(_attackTurn ? 0 : 1);
                AudioManager.PlayAttack();
            }
        }

        private void TakeDamage(float dmg, MouseState input, MouseState prev)
        {
            if (prev.LeftButton == ButtonState.Released && input.LeftButton == ButtonState.Pressed)
            {
                _health -= dmg;
                if (_health < _maxLowestHealth) _health = _maxLowestHealth;
                Console.WriteLine(_health);
            }
        }

        private void Heal(float heal, MouseState input, MouseState prev)
        {
            if (prev.RightButton == ButtonState.Released && input.RightButton == ButtonState.Pressed)
            {
                _health += heal;
                if (_health > _maxHealth) _health = _maxHealth;
                Console.WriteLine(_health);
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
