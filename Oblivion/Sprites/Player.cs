using Microsoft.Xna.Framework;
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

        public override void Update(GameTime gameTime)
        {

            velocity = Vector2.Zero;
            KeyboardState input = Keyboard.GetState();

            if (input.IsKeyDown(Keys.A))
            {
                velocity.X -= 1f;
                _animation.SetRow(6);
                Flip = SpriteEffects.None;
            }

            else if (input.IsKeyDown(Keys.D))
            {
                velocity.X += 1f;
                _animation.SetRow(6);
                Flip = SpriteEffects.FlipHorizontally;
            }

            if (input.IsKeyDown(Keys.W))
            {
                velocity.Y -= 1f;
                _animation.SetRow(5);
                Flip = SpriteEffects.None;
            }

            if (velocity == Vector2.Zero)
            {
                _animation.SetRow(4);
            }


            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
                Position += velocity * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _animation.Update(gameTime);
            }
            else
            {
                _animation.Reset();
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, _animation.GetSourceRect(), Color.White, 0, new Vector2(0, 0), 1f, Flip, Layer);
        }
    }

}
