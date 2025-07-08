using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        public SpriteEffects Flip = SpriteEffects.None;
        private Vector2 _enemyVelocity = Vector2.Zero;
        private SpriteAnimation2D _animation;

        private float _walkSpeed = 60f;
        private float _idleTimer = 0f;
        private float _idleDuration = 2f;

        private EnemyState _currentState = EnemyState.Idle;

        private float _leftBound;
        private float _rightBound;

        public MinorEnemy(Texture2D texture, SpriteAnimation2D animation, float leftBound, float rightBound)
            : base(texture)
        {
            _animation = animation;
            _enemyVelocity = new Vector2(1, 0);
            _leftBound = leftBound;
            _rightBound = rightBound;
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            int newAnimationRow = 3;

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
                    Position.X += _enemyVelocity.X * _walkSpeed * deltaTime;
                    newAnimationRow = 4;

                    if (Position.X <= _leftBound)
                    {
                        Position.X = _leftBound;
                        _enemyVelocity.X = 1; // turn right
                        Flip = SpriteEffects.None;
                        _currentState = EnemyState.Patrol;
                    }
                    else if (Position.X >= _rightBound)
                    {
                        Position.X = _rightBound;
                        _enemyVelocity.X = -1; // turn left
                        Flip = SpriteEffects.FlipHorizontally;
                        _currentState = EnemyState.Patrol;

                    }
                    break;

                case EnemyState.Chase:
                    // Logic for player chase
                    break;

                case EnemyState.Attack:
                    // Logic for attack 
                    break;
            }

            _animation.SetRow(newAnimationRow);
            _animation.Update(gameTime);
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
                2f,
                Flip,
                Layer
            );
        }

    }

}