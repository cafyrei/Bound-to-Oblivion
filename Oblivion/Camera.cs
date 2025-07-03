using System;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Oblivion
{
    public class Camera
{
    public Matrix Transform { get; private set; }

    public float Zoom { get; set; } = 1.5f;
        public void Follow(Player target, int worldWidth, int worldHeight, bool locked)
        {
            Matrix position;
            
            if (!locked) //IF NOT TRUE === FALSE CANCEL CAMERA
            {
                Console.WriteLine("HIIII");
                position = Matrix.CreateTranslation(
                -target.Player_Position.X - (target.Player_Rectangle.Width / 2) - 250,
                -target.Player_Position.Y - (target.Player_Rectangle.Height / 2) + 170,
                0);
            }
            
            else //LOCK CAMERA ON CHARACTER AGAIN
            {
           
                position = Matrix.CreateTranslation(-target.Player_Position.X - 300, -target.Player_Position.Y - (target.Player_Rectangle.Height / 2) + 170, 0);
            }

            var offset = Matrix.CreateTranslation(
                Game1.screenWidth / 2,
                Game1.screenHeight / 2,
                0);

            var scale = Matrix.CreateScale(Zoom);

            Transform = position * scale * offset;
        }
}
}


