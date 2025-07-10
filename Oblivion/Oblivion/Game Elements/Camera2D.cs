using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Oblivion
{
    public class Camera2D
    {
        private Vector2 _position;
        private readonly Viewport _viewport;
        private float _zoom;
        private float _rotation;
        private bool Clamped ;

        public bool IsClamped { get => Clamped; }

        // Properties for each variables
        public float Rotation
        {
            get => _rotation;
            set => _rotation = value;
        }

        public float Zoom
        {
            get => _zoom;
            set => _zoom = MathHelper.Clamp(value, .1f, 10f);
        }

        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        // Constructor

        public Camera2D(Viewport viewport)
        {
            _viewport = viewport;
            _zoom = 1f;
        }

        public Matrix GetViewMatrix()
        {
            return
                Matrix.CreateTranslation(new Vector3(-_position, 0.0f)) *
                Matrix.CreateRotationZ(_rotation) *
                Matrix.CreateScale(_zoom, _zoom, 1) *
                Matrix.CreateTranslation(new Vector3(_viewport.Width * 0.5f, _viewport.Height * 0.5f, 0));
        }

        public void Follow(Vector2 target, int worldWidth, int worldHeight)
        {
            float halfScreenWidth = _viewport.Width / 2f;
            float halfScreenHeight = _viewport.Height / 2f;

            float x = MathHelper.Clamp(target.X, halfScreenWidth, worldWidth - halfScreenWidth);
            float y = MathHelper.Clamp(target.Y, halfScreenHeight, worldHeight - halfScreenHeight);

            Clamped = (target.X != x) || (target.Y != y);
            _position = new Vector2(x, y);
        }

        
    }
}