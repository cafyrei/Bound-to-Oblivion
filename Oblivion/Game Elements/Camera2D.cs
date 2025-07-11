using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Oblivion
{
    public class Camera2D
    {
        private Vector2 _position;
        private readonly Viewport _viewport;
        private float _zoom;
        private float _rotation;
        private bool Clamped;

        // Shake variables
        private float _shakeTimer;
        private float _shakeDuration;
        private float _shakeMagnitude;
        private Vector2 _shakeOffset;
        private readonly Random _random = new Random();

        public bool IsClamped => Clamped;

        // Properties
        public float Rotation
        {
            get => _rotation;
            set => _rotation = value;
        }

        public float Zoom
        {
            get => _zoom;
            set => _zoom = MathHelper.Clamp(value, 0.1f, 10f);
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
        public void Shake(float duration, float magnitude)
        {
            _shakeDuration = duration;
            _shakeTimer = duration;
            _shakeMagnitude = magnitude;
        }

        public void Follow(Vector2 target, int worldWidth, int worldHeight, GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            float halfScreenWidth = _viewport.Width / 2f;
            float halfScreenHeight = _viewport.Height / 2f;

            float x = MathHelper.Clamp(target.X, halfScreenWidth, worldWidth - halfScreenWidth);
            float y = MathHelper.Clamp(target.Y, halfScreenHeight, worldHeight - halfScreenHeight);

            Clamped = (target.X != x) || (target.Y != y);

            // Apply shake effect
            if (_shakeTimer > 0)
            {
                _shakeTimer -= delta;

                float shakeAmount = _shakeMagnitude * (_shakeTimer / _shakeDuration); 
                _shakeOffset = new Vector2(
                    (_random.NextFloat() - 0.5f) * 2 * shakeAmount,
                    (_random.NextFloat() - 0.5f) * 2 * shakeAmount
                );
            }
            else
            {
                _shakeOffset = Vector2.Zero;
            }

            _position = new Vector2(x, y) + _shakeOffset;
        }

        public Matrix GetViewMatrix()
        {
            return
                Matrix.CreateTranslation(new Vector3(-_position, 0.0f)) *
                Matrix.CreateRotationZ(_rotation) *
                Matrix.CreateScale(_zoom, _zoom, 1) *
                Matrix.CreateTranslation(new Vector3(_viewport.Width * 0.5f, _viewport.Height * 0.5f, 0));
        }
    }

    public static class RandomExtensions
    {
        public static float NextFloat(this Random rng)
        {
            return (float)rng.NextDouble();
        }
    }
}
