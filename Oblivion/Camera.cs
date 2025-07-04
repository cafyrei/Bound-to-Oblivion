using System;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct3D9;


namespace Oblivion
{
    
    
    public class Camera
    {
        public Matrix Transform;
        private Vector2 _position;
    public Vector2 Position => _position;
        float zoom = 1.5f;
        public void Follow(Player target)
        {
            Vector2 targetPos = new Vector2(
            target.Player_Position.X + (target.Player_Rectangle.Width / 2),
            target.Player_Position.Y + (target.Player_Rectangle.Height / 2) - 50);
            Console.Write(target.Player_Position.X);
            float cameraX = targetPos.X - Game1.screenWidth * .15f;
            float cameraY = targetPos.Y - Game1.screenHeight / 2f;

            cameraX = MathHelper.Clamp(cameraX, 0, 525 - target.Player_Rectangle.Width);
            cameraY = MathHelper.Clamp(cameraY, 0, 720);

            _position = new Vector2(cameraX, cameraY);

            // Create the camera transform matrix
            Transform = Matrix.CreateTranslation(-_position.X, -_position.Y, 0f) *
                        Matrix.CreateScale(zoom) *
                        Matrix.CreateTranslation(0, 0, 0);
                        
            //V2 WORKS
            // Vector2 targetPos = new Vector2(
            //             target.Player_Position.X + (target.Player_Rectangle.Width / 2),
            //             target.Player_Position.Y + (target.Player_Rectangle.Height / 2) - 50
            //     );
            //             Console.Write(target.Player_Position.X);
            //             float cameraX = targetPos.X - Game1.screenWidth/2f;
            //             float cameraY = targetPos.Y - Game1.screenHeight/2f;

            //             cameraX = MathHelper.Clamp(cameraX, 0, 525 - target.Player_Rectangle.Width);
            //             cameraY = MathHelper.Clamp(cameraY, 0, 720);

            //             _position = new Vector2(cameraX, cameraY);

            //             Transform = Matrix.CreateTranslation(-_position.X, -_position.Y, 0f) *
            //                         Matrix.CreateScale(zoom) *
            Matrix.CreateTranslation(0, 0, 0);

            //JUST IN CASE SHIT DOESNT WORK
            //      var position = Matrix.CreateTranslation(-target.Player_Position.X - (target.Player_Rectangle.Width / 2),
            // -target.Player_Position.Y - (target.Player_Rectangle.Height / 2), 0);

            // var scale = Matrix.CreateScale(zoom);

            // var offset = Matrix.CreateTranslation(Game1.screenWidth/2, Game1.screenHeight/2, 0);

            // Transform = position * scale * offset;
        }
    }
}


