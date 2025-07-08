using System;
using System.Collections.Generic;
using System.Drawing.Design;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Oblivion
{
    public class TextureManager
    {
        // Game Stages Variables;

        private MainMenu _mainMenu;
        private GameStage _gameStage;

        // Player Variables
        private Texture2D _samuraiTexture;
        private Player _player;
        private SpriteAnimation2D _playerAnimation;

        // Background Variables
        private List<ScrollingBackground> _scrollingBackground;

        // Camera Variables
        private Camera2D _camera;
        public static int tileWidth = 2800;
        public static int tileHeight = 720;


        public void Load(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            #region Player And Backgroud Declaration
            try
            {
                _samuraiTexture = Content.Load<Texture2D>("Player/thumbnail_sprite_sheet");
            }
            catch (ContentLoadException e)
            {
                Console.WriteLine("Error Loading _samuraiTexture Error Details : " + e);

            }

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
                frameTime: 0.2f
                );


            _player = new Player(_samuraiTexture, _playerAnimation)
            {
                Position = new Vector2(50, Game1.ScreenHeight - 150),
                Layer = 1f,
            };

            _scrollingBackground = new List<ScrollingBackground>()
            {
                new ScrollingBackground(Content.Load<Texture2D>("Parallax_Layers/_stage1"), _player, 1f)
                {
                    Layer = 0.1f,
                },
                new ScrollingBackground(Content.Load<Texture2D>("Parallax_Layers/Layer 1"), _player, 15f)
                {
                    Layer = 0.93f,
                },
                new ScrollingBackground(Content.Load<Texture2D>("Parallax_Layers/Layer 2"), _player, 30f)
                {
                    Layer = 0.95f,
                },
                new ScrollingBackground(Content.Load<Texture2D>("Parallax_Layers/Layer 3"), _player,40f)
                {
                    Layer = 0.97f,
                }
                // new ScrollingBackground(Content.Load<Texture2D>("Parallax_Layers/Layer 4"), _player, 0f, true)
                // {
                //     Layer = 0.99f,
                // }
            };
            #endregion

            _camera = new Camera2D(graphicsDevice.Viewport);
            _mainMenu = new MainMenu(Content, graphicsDevice);
            _gameStage = new GameStage(_scrollingBackground, _player);

            foreach (var bg in _scrollingBackground)
            {
                bg.SetCamera(_camera);
            }

        } 

        // Properties
        public MainMenu MainMenu => _mainMenu;
        public GameStage GameStage =>_gameStage;

        public Camera2D Camera { get => _camera;}
    }
}