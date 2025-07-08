using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Oblivion
{
    public class ScrollingBackground : Component
    {
        private bool _constantSpeed;
        private float _layer;
        private float _scrollingSpeed;
        private List<Sprite> _sprites;
        private readonly Player _player;
        private float _speed;
        private Camera2D _camera;
        private Vector2 _previousCameraPosition;

        public float Layer
        {
            get { return _layer; }
            set
            {
                _layer = value;
                foreach (var sprite in _sprites)
                {
                    sprite.Layer = _layer;
                }
            }
        }

        // Constructors
        public ScrollingBackground(Texture2D texture, Player player, float scrollingSpeed, bool constSpeed = false) :
            this(new List<Texture2D>() { texture }, player, scrollingSpeed, constSpeed)
        {
        }

        public ScrollingBackground(List<Texture2D> textures, Player player, float scrollingSpeed, bool constSpeed = false)
        {
            _player = player;
            _sprites = new List<Sprite>();
            _scrollingSpeed = scrollingSpeed;
            _constantSpeed = constSpeed;

            // Calculate how many sprites are needed to cover the screen width
            int spriteCount = (int)Math.Ceiling((float)Game1.ScreenWidth / textures[0].Width) + 2;

            for (int i = 0; i < spriteCount; i++)
            {
                var texture = textures[0]; // Assuming you want to use the first texture
                _sprites.Add(new Sprite(texture)
                {
                    Position = new Vector2(i * texture.Width, Game1.ScreenHeight - texture.Height)
                });
            }
        }

        public override void Update(GameTime gameTime)
        {
            ApplySpeed(gameTime);
            CheckPosition();
        }

        private void ApplySpeed(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_camera == null)
                return;

            Vector2 cameraDelta = _camera.Position - _previousCameraPosition;
            _speed = cameraDelta.X * (_scrollingSpeed * 0.01f);
            _previousCameraPosition = _camera.Position;

            foreach (var sprite in _sprites)
            {
                sprite.Position.X -= _speed;
            }
        }

        private void CheckPosition()
        {
            for (int i = 0; i < _sprites.Count; i++)
            {
                var sprite = _sprites[i];

                // If the sprite has moved off the left side of the screen
                if (sprite.Rectangle.Right <= 0)
                {
                    // Move it to the right of the last sprite
                    int index = (i - 1 + _sprites.Count) % _sprites.Count; // Wrap around
                    sprite.Position.X = _sprites[index].Rectangle.Right;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var sprite in _sprites)
            {
                sprite.Draw(gameTime, spriteBatch);
            }
        }

        public void SetCamera(Camera2D camera)
        {
            _camera = camera;
            _previousCameraPosition = _camera.Position; // store initial
        }
    }
}