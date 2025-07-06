using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Oblivion
{
    public class Player : Sprite
    {

        public SpriteEffects Flip = SpriteEffects.None;
        private Vector2 velocity = Vector2.Zero;
        private SpriteAnimation2D _animation;
        private float Speed = 100f;
        bool attackTurn = true;
        private KeyboardState _previousKeyboard;

        public Vector2 Velocity
        {
            get => velocity;
            set => velocity = value;
        }

        public Player(Texture2D texture, SpriteAnimation2D animation) : base(texture)
        {
            _texture = texture;
            _animation = animation;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, _animation.GetSourceRect(), Color.White, 0, new Vector2(0, 0), 2f, Flip, Layer);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState input = Keyboard.GetState();
            velocity = Vector2.Zero;

            // Boolean Controls
            bool moveLeft = input.IsKeyDown(Keys.A);
            bool moveRight = input.IsKeyDown(Keys.D);
            bool run = input.IsKeyDown(Keys.LeftShift);
            bool jump = input.IsKeyDown(Keys.W);
            bool attack = input.IsKeyDown(Keys.E);

            int newAnimationRow = 4;

            if (moveRight && run && !moveLeft)
            {
                velocity.X += 8f;
                Flip = SpriteEffects.FlipHorizontally;
                newAnimationRow = 6; // Run Right
            }
            else if (moveLeft && run && !moveRight)
            {
                velocity.X -= 8f;
                Flip = SpriteEffects.None;
                newAnimationRow = 6; // Run Left
            }
            else if (moveRight && !moveLeft)
            {
                velocity.X += 1f;
                Flip = SpriteEffects.FlipHorizontally;
                newAnimationRow = 7; // Walk Right
            }
            else if (moveLeft && !moveRight)
            {
                velocity.X -= 1f;
                Flip = SpriteEffects.None;
                newAnimationRow = 7; // Walk Left
            }

            bool _attack_Pressed = input.IsKeyDown(Keys.E) && !_previousKeyboard.IsKeyDown(Keys.E);
            if (_attack_Pressed)
            {
                if (attackTurn)
                {
                    newAnimationRow = 0;
                    attackTurn = false;
                }
                else
                {
                    newAnimationRow = 1;
                    attackTurn = true;
                }
                AudioManager.PlayAttack();

            }
            _previousKeyboard = input;


            if (jump)
            {
                velocity.Y -= 1f;
            }

            // Apply Animation Change
            _animation.SetRow(newAnimationRow);

            // Move character
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
                Position += velocity * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            _animation.Update(gameTime);
        }

    }

}
