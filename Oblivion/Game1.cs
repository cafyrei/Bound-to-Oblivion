using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Oblivion
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;

        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;
        private SpriteBatch _spriteBatch;
        private SpriteAnimation2D _playerAnimation;

        MainMenu _mainMenu;
        GameStage _gameStage;

        public enum GameState { MainMenu, GamePlay, Credits };

        GameState currentState = GameState.MainMenu;
        private List<ScrollingBackground> _scrollingBackground;
        private Player _player;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            #region Game Window Configuration
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
            _graphics.ApplyChanges();
            #endregion


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var _samuraiTexture = Content.Load<Texture2D>("Player/thumbnail_sprite_sheet");
            _playerAnimation = new SpriteAnimation2D(
                frameWidth: 64,
                frameHeight: 64,
                rowFrameCount: new Dictionary<int, int>
                {
                    {0,6}, // Atk 1
                    {1,6}, // Atk 2
                    {2,6}, // Death
                    {3,5}, // Hit
                    {4,6}, // Idle
                    {5,6}, // Jump
                    {6,7}, // Sprint
                    {7,6}  // Walk
                },
                frameTime: 0.1f
                );

            _player = new Player(_samuraiTexture, _playerAnimation)
            {
                Position = new Vector2(50, (ScreenHeight - _samuraiTexture.Height) - 20),
                Layer = 1f,
            };

            _scrollingBackground = new List<ScrollingBackground>()
            {
                new ScrollingBackground(Content.Load<Texture2D>("Parallax_Layers/_stage1"), _player, 3f)
                {
                    Layer = 0.1f,
                },
                new ScrollingBackground(Content.Load<Texture2D>("Parallax_Layers/Layer 1"), _player, 30f)
                {
                    Layer = 0.93f,
                },
                new ScrollingBackground(Content.Load<Texture2D>("Parallax_Layers/Layer 2"), _player, 50f)
                {
                    Layer = 0.95f,
                },
                new ScrollingBackground(Content.Load<Texture2D>("Parallax_Layers/Layer 3"), _player,70f)
                {
                    Layer = 0.97f,
                },
                new ScrollingBackground(Content.Load<Texture2D>("Parallax_Layers/Layer 4"), _player, 0f)
                {
                    Layer = 0.99f,
                }
            };

            _mainMenu = new MainMenu(Content, GraphicsDevice);
            _gameStage = new GameStage(_spriteBatch, _scrollingBackground,_player);

        }

        protected override void Update(GameTime gameTime)
        {
            switch (currentState)
            {
                case GameState.MainMenu:
                    _mainMenu.Update(gameTime);
                    if (_mainMenu.StartPressed)
                        currentState = GameState.GamePlay;
                    else if (_mainMenu.CreditsPressed)
                        currentState = GameState.Credits;
                    else if (_mainMenu.ExitPressed)
                        Exit();
                    break;

                case GameState.GamePlay:
                    _gameStage.Update(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            switch (currentState)
            {
                case GameState.MainMenu:
                    _mainMenu.Draw(_spriteBatch);
                    break;

                case GameState.GamePlay:
                    _gameStage.Draw(gameTime);
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
