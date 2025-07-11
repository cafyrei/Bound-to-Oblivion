using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Oblivion
{
    public class Boss : Sprite
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

        // Targeting
        private Player _player;

        // Animation Cool Down
        private float _animationCoolDown = 1.3f;
        private float _stateTimer = 0f;
        private bool _isColliding;

        // Health Related Variables

        float _minHealth = 0;
        float _maxHealth = 100f;
        float _currentHealth;
        Random randDamage;
        float _damage;
        float _damageCooldown = 1.5f;
        float _damageTimer = 0f;


        private bool _isDying = false;
        private float _deathTimer = 0f;
        private float _deathDuration = 1.3f;

        private bool _isHit = false;
        private float _hitTimer = 0f;
        private float _hitDuration = 0.3f;
        public bool IsDead { get; private set; } = false;

        public Rectangle Hitbox { get => _hitbox; }

        public Boss(Texture2D texture, SpriteAnimation2D animation, float leftBound, float rightBound, Camera2D camera)
            : base(texture)
        {
            _animation = animation;
            _enemyVelocity = new Vector2(_walkSpeed, 0); // Start moving right
            _leftBound = leftBound;
            _rightBound = rightBound;
            randDamage = new Random();

            _currentHealth = _maxHealth;
        }

        public void Update(GameTime gameTime, Dictionary<Vector2, Rectangle> collisionBlocks, Camera2D camera)
        {
            Console.WriteLine("Enemy Health " + _currentHealth);
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds; // Delta Time

            _isColliding = _player.Hitbox.Intersects(_hitbox); // Condition Checking

            int newAnimationRow = 3;

            ApplyGravity(deltaTime);

            Position.Y += _enemyVelocity.Y * deltaTime;

            if (_isDying)
            {
                _deathTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                _animation.Update(gameTime);

                _enemyVelocity.Y = -3f;

                if (_deathTimer >= _deathDuration)
                {
                    IsDead = true;
                }

                return;
            }

            if (_isHit)
            {
                _hitTimer += deltaTime;
                _animation.SetRow(2);
                _animation.Update(gameTime);
                _enemyVelocity.Y = -3f;

                if (_hitTimer >= _hitDuration)
                {
                    _isHit = false;
                }

                return;
            }

            switch (_currentState)
            {
                case EnemyState.Idle:
                    _idleTimer += deltaTime;
                    if (_idleTimer >= _idleDuration)
                    {
                        _idleTimer = 3;
                        _currentState = EnemyState.Patrol;
                    }
                    break;

                case EnemyState.Patrol:
                    // Check if player is in range
                    float distanceToPlayer = Vector2.Distance(Position, _player.Position);

                    if (distanceToPlayer < 150f) // chase if player is close
                    {
                        _currentState = EnemyState.Chase;
                        break;
                    }

                    PatrolMovement(deltaTime);
                    newAnimationRow = 4;
                    break;

                case EnemyState.Chase:
                    {
                        float distance = Vector2.Distance(Position, _player.Position);

                        if (_player.Hitbox.Intersects(_hitbox))
                        {
                            _currentState = EnemyState.Attack;
                            _stateTimer = 0f;
                            newAnimationRow = 0;
                            break;
                        }

                        if (distance > 500f) // If Distance Back to Patrol
                        {
                            _currentState = EnemyState.Patrol;
                            newAnimationRow = 4;
                            break;
                        }

                        // Still chasing
                        ChasePlayer(deltaTime);
                        newAnimationRow = 4;
                        break;
                    }

                case EnemyState.Attack:
                    {
                        _stateTimer += deltaTime;
                        _damageTimer += deltaTime;
                        newAnimationRow = 1; // attack row

                        if (_isColliding)
                        {

                            // Collision Just started (valid attack)
                            if (_damageTimer >= _damageCooldown)
                            {
                                _damage = randDamage.Next(5, 11); // Damage Per Slah (min,max)
                                _player.TakeDamage(_damage, camera);
                                _damageTimer = 0f;
                            }
                        }
                        else
                        {
                            if (_stateTimer >= _animationCoolDown)
                            {
                                _stateTimer = 0f;

                                float distance = Vector2.Distance(Position, _player.Position);

                                if (distance <= 500f)
                                {
                                    _currentState = EnemyState.Chase;
                                }
                                else
                                {
                                    _currentState = EnemyState.Patrol;
                                }
                            }
                        }
                        break;
                    }
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
                (int)(Position.Y + verticalTrim),
                Math.Max(1, (int)(_animation.FrameWidthAccess * HitboxScale) - horizontalTrim * 2),
                Math.Max(1, (int)(_animation.FrameHeightAccess * HitboxScale) - verticalTrim * 2)
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

        private void SetHealth(float value)
        {
            _currentHealth = MathHelper.Clamp(value, _minHealth, _maxHealth);
            Console.WriteLine("Enemy Health Damage " + _currentHealth);

            if (_currentHealth <= 0f && !_isDying)
            {
                _isDying = true;
                _animation.SetRow(2);
                _deathTimer = 0f;
            }

        }

        public void TakeDamage(float dmg)
        {
            if (_isDying || IsDead) return;

            SetHealth(_currentHealth - dmg);

            if (!_isDying)
            {
                _isHit = true;
                _hitTimer = 0f;
                _animation.SetRow(2);
            }
        }

        public void SetTarget(Player player)
        {
            _player = player;
        }

        private void ChasePlayer(float deltaTime)
        {
            if (_player.Position.X > Position.X)
            {
                _enemyVelocity.X = _walkSpeed;
                Flip = SpriteEffects.None;  // facing right
            }
            else
            {
                _enemyVelocity.X = -_walkSpeed;
                Flip = SpriteEffects.FlipHorizontally; // facing left
            }


            Position.X += _enemyVelocity.X * deltaTime;
        }

        private void PatrolMovement(float deltaTime)
        {
            if (Position.X <= _leftBound)
            {
                Position.X = _leftBound;
                _enemyVelocity.X = _walkSpeed;
                Flip = SpriteEffects.None;
            }
            else if (Position.X >= _rightBound)
            {
                Position.X = _rightBound;
                _enemyVelocity.X = -_walkSpeed;
                Flip = SpriteEffects.FlipHorizontally;
            }

            Position.X += _enemyVelocity.X * deltaTime;
        }

    }
}
