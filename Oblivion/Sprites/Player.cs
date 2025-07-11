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
        public float _currentHealth;
        public float CurrentHealth { get => _currentHealth; set => _currentHealth = value; }

        // Animation Control
        bool _isHit = false;
        float _hitTimer = 0f;
        float _hitDuration = 3f;
        bool _isDead = false;
        private float _deathFadeOpacity = 0f;
        private bool _startFade = false;
        private bool _wasOnGround;

        private float _footstepTimer = 0f;
        private float _footstepCooldown = 0.3f;

        public Player(Texture2D texture, SpriteAnimation2D animation, ContentManager Content, HPBar hpbar) : base(texture)
        {
            _texture = texture;
            _animation = animation;

            _currentHealth = _maxHealth;
            _HPbar = hpbar;
        }

        public void Update(
    GameTime gameTime,
    Dictionary<Vector2, Rectangle> collisionBlocks,
    List<MinorEnemy> minorEnemies,
    List<ZombieEnemies> zombieEnemies,
    Boss boss)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var input = Keyboard.GetState();
            var mouseInput = Mouse.GetState();
            _wasOnGround = _isOnGround;

            velocity = new Vector2(0, velocity.Y);

            if (_isDead)
            {
                if (_animation.CurrentFrame < _animation.TotalFrames - 1)
                {
                    _animation.Update(gameTime);
                }
                else
                {
                    _startFade = true;
                    _deathFadeOpacity += 0.5f * deltaTime;

                    if (_deathFadeOpacity >= 1f)
                    {
                        _deathFadeOpacity = 1f;
                        Game1.currentState = Game1.GameState.GameOver;
                        MainMenu.ResetFlags();
                    }
                }

                return;
            }

            HandleMovement(input, gameTime);
            HandleJump(input);
            HandleAttack(mouseInput, deltaTime, minorEnemies, zombieEnemies, boss);
            ApplyGravity(deltaTime);

            Move(deltaTime);
            UpdateHitbox();
            HandleCollision(collisionBlocks);

            if (!_wasOnGround && _isOnGround)
            {
                AudioManager.PlaySFX(AudioManager._jumpLandSFX, 1f);
            }

            _animation.Update(gameTime);
            _previousKeyboard = input;
            _previousMouse = mouseInput;
        }

        public void Update(
        GameTime gameTime,
        Dictionary<Vector2, Rectangle> collisionBlocks,
        List<MinorEnemy> minorEnemies,
        List<ZombieEnemies> zombieEnemies)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var input = Keyboard.GetState();
            var mouseInput = Mouse.GetState();
            _wasOnGround = _isOnGround;

            velocity = new Vector2(0, velocity.Y);

            if (_isDead)
            {
                if (_animation.CurrentFrame < _animation.TotalFrames - 1)
                {
                    _animation.Update(gameTime);
                }
                else
                {
                    _startFade = true;
                    _deathFadeOpacity += 0.5f * deltaTime;

                    if (_deathFadeOpacity >= 1f)
                    {
                        _deathFadeOpacity = 1f;
                        Game1.currentState = Game1.GameState.GameOver;
                        MainMenu.ResetFlags();
                    }
                }
                return;
            }

            HandleMovement(input, gameTime);
            HandleJump(input);
            HandleAttack(mouseInput, deltaTime, minorEnemies, zombieEnemies);
            ApplyGravity(deltaTime);

            Move(deltaTime);
            UpdateHitbox();
            HandleCollision(collisionBlocks);

            if (!_wasOnGround && _isOnGround)
            {
                AudioManager.PlaySFX(AudioManager._jumpLandSFX, 1f);
            }

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
            Console.WriteLine("Position: " + Position);
            float moveSpeed = run ? _runSpeed : _walkSpeed;

            if (_isAttacking) return;

            // Handle hit state
            if (_isHit && !moveLeft && !moveRight)
            {
                _hitTimer += deltaTime;
                _animation.SetRow(3); // Hit animation row
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

            // Animation and SFX
            if (!_isOnGround)
            {
                // Airborne animation
                if (velocity.Y < 0 && Math.Abs(velocity.X) < 0.1f)
                {
                    _animation.SetRow(5); // Jump up idle
                }
                else
                {
                    _animation.SetRow(run ? 6 : 7); // Jump with movement
                }

                // Reset footstep timer while airborne
                _footstepTimer = 0f;
            }
            else
            {
                if (velocity.X == 0)
                {
                    _animation.SetRow(4); // Idle on ground
                    _footstepTimer = 0f;
                }
                else
                {
                    _animation.SetRow(run ? 6 : 7); // Run animation
                    _footstepTimer += deltaTime;

                    if (_footstepTimer >= _footstepCooldown)
                    {
                        AudioManager.PlaySFX(AudioManager._runGrassSFX, 5f);
                        _footstepTimer = 0f;
                    }
                }
            }
        }

        private void SetHealth(float value) // Update
        {
            _currentHealth = MathHelper.Clamp(value, _minHealth, _maxHealth);
            if (_currentHealth <= 0f)
            {
                _isDead = true;
                velocity = Vector2.Zero;
                _animation.FrameTimeAccess = .25f;
                _animation.SetRow(2);

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


        private void HandleAttack(MouseState input, float deltaTime, List<MinorEnemy> minorEnemies, List<ZombieEnemies> zombieEnemies, Boss boss)
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

                return;
            }

            if (leftClick || rightClick)
            {
                float damage = leftClick ? 10f : 25f;
                _attackTurn = leftClick;

                foreach (var enemy in minorEnemies)
                {
                    if (_hitbox.Intersects(enemy.Hitbox))
                    {
                        enemy.TakeDamage(100f);
                    }
                }

                foreach (var enemy in zombieEnemies)
                {
                    if (_hitbox.Intersects(enemy.Hitbox))
                    {
                        enemy.TakeDamage(100);
                    }
                }

                if (_hitbox.Intersects(boss.Hitbox))
                {
                    boss.TakeDamage(100);
                }

                    _isAttacking = true;
                _attackTimer = 0f;

                _animation.SetRow(_attackTurn ? 0 : 1);
                AudioManager.PlayAttack();
            }
        }

        private void HandleAttack(MouseState input, float deltaTime, List<MinorEnemy> minorEnemies, List<ZombieEnemies> zombieEnemies)
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

                return;
            }

            if (leftClick || rightClick)
            {
                float damage = leftClick ? 10f : 25f;
                _attackTurn = leftClick;

                foreach (var enemy in minorEnemies)
                {
                    if (_hitbox.Intersects(enemy.Hitbox))
                    {
                        enemy.TakeDamage(100f);
                    }
                }

                foreach (var enemy in zombieEnemies)
                {
                    if (_hitbox.Intersects(enemy.Hitbox))
                    {
                        enemy.TakeDamage(100);
                    }
                }

                _isAttacking = true;
                _attackTimer = 0f;

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

        public Vector2 GetPlayerPosition()
        {
            return Position;
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