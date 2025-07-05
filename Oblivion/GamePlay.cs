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
        Vector2 velocity = new Vector2(4f,4f);

        //Input
        Input input;

        //Camera
        public Camera camera;

        //Platform
        Platform platform1;
        public GamePlay(ContentManager content, GraphicsDevice graphics, Viewport viewport)
        {

            LoadContent(content, graphics);
            input = new Input();
            camera = new Camera();
            platform1 = new Platform("../../../data/Stage1map.csv");
            platform1.textureStore = new()
            {
                new Rectangle(0,0,16,16)
            };
            platform1.LoadContent(content, graphics);
        }


        public void Update(GameTime gametime, GraphicsDevice graphics)
        {

            
            Vector2 position = ball.Player_Position;
            if (input.IsKeyDown(Keys.A))
            {
                position.X += -velocity.X;
            }
            if (input.IsKeyDown(Keys.D))
            {
                position.X += velocity.X;
            }
            if (input.IsKeyDown(Keys.W))
            {
                position.Y += -velocity.Y;
            }
            if (input.IsKeyDown(Keys.S))
            {
                position.Y += velocity.Y;
            }



            Console.WriteLine();
            

            //CLAMPING
            position.X = MathHelper.Clamp(position.X, 0, Game1.screenWidth - ball.Player_Rectangle.Width);
            position.Y = MathHelper.Clamp(position.Y, 0, Game1.screenHeight- ball.Player_Rectangle.Height);

            // Console.WriteLine("Player X : ");
            ball.Player_Position = position;

                camera.Follow(ball);

            input.Update();
        }

        public void LoadContent(ContentManager Content, GraphicsDevice graphics)
        {
            background_Texture = Content.Load<Texture2D>("Backgrounds/stage1BG");
            backgroundRect = new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height);
            background = new Background(background_Texture, backgroundRect, Color.White);

            ball_Texture = Content.Load<Texture2D>("Sprites/Ball");
            ballPos = new Vector2(Game1.screenWidth / 2 - ball_Texture.Width, Game1.screenHeight / 2);
            ball = new Player(ball_Texture, ballPos, Color.White);
            
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: camera.Transform);
            spriteBatch.Draw(background.Background_Texture, background.Background_Rectangle, background.Background_color);
            platform1.Draw(spriteBatch);
            spriteBatch.Draw(ball.Player_Texture, ball.Player_Position, ball.Player_color);
            spriteBatch.End();
        }
    }
}