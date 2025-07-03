using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SharpDX.Direct2D1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace Oblivion
{
    internal class GamePlay
    {
        
        // Game Background
        Texture2D background_Texture;
        public Background background;
        Rectangle backgroundRect;

        // Asset Sprite
        Texture2D ball_Texture;
        Player ball;
        Vector2 ballPos;

        //Movement
        float moveSpeed = 4f;
        float VelocityX;

        //Input
        Input input;

        //Camera
        public Camera camera;
        public GamePlay(ContentManager content, GraphicsDevice graphics)
        {

            LoadContent(content, graphics);
            input = new Input();
            camera = new Camera();
        }


        public void Update(GameTime gametime, GraphicsDevice graphics)
        {
            Vector2 position = ball.Player_Position;
            if (input.IsKeyDown(Keys.A))
            {
                VelocityX = -moveSpeed;
                position.X += VelocityX;
            }
            if (input.IsKeyDown(Keys.D))
            {
                VelocityX = moveSpeed;
                position.X += VelocityX;
            }

            //CLAMPING
            position.X = MathHelper.Clamp(position.X, 0, Game1.screenWidth - ball.Player_Rectangle.Width);
            position.Y = MathHelper.Clamp(position.Y, 0, Game1.screenHeight);

            bool lockCamera = false;
            float deadzoneLeft = 250f;

            ball.Player_Position = position;

                if (ball.Player_Position.X > deadzoneLeft)
                {
                    lockCamera = true;
                }
                camera.Follow(ball, background.Background_Rectangle.X, background.Background_Rectangle.Y, lockCamera);

            input.Update();
        }

        public void LoadContent(ContentManager Content, GraphicsDevice graphics)
        {
            background_Texture = Content.Load<Texture2D>("Backgrounds/stage1BG");
            backgroundRect = new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height);
            background = new Background(background_Texture, backgroundRect, Color.White);

            ball_Texture = Content.Load<Texture2D>("Sprites/Ball");
            ballPos = new Vector2(300, 600);
            ball = new Player(ball_Texture, ballPos, Color.White);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: camera.Transform);
            spriteBatch.Draw(background.Background_Texture, background.Background_Rectangle, background.Background_color);
            spriteBatch.Draw(ball.Player_Texture, ball.Player_Position, ball.Player_color);
            spriteBatch.End();
        }
    }
}