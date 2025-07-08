using System;
using System.Collections.Generic;
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
        private Texture2D _minorEnemyTexture;
        private Player _player;
        private SpriteAnimation2D _playerAnimation;

        // Enemy Variables
        private SpriteAnimation2D _minorEnemyAnimation;
        private List<MinorEnemy> _minorEnemies; // Changed to a list for multiple enemies

        // Background Variables
        private List<ScrollingBackground> _scrollingBackground;
        private Platform _platform1;

        // Camera Variables
        private Camera2D _camera;
        public static int tileWidth = 2800;
        public static int tileHeight = 720;

        // Random number generator
        private Random _random;

        public void Load(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            _random = new Random(); // Initialize the random number generator

            #region Player And Background Declaration
            try
            {
                _samuraiTexture = Content.Load<Texture2D>("Player/Samurai Spritesheet");
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
                Layer =  0.94f,
            };

            _scrollingBackground = new List<ScrollingBackground>()
            {
                new ScrollingBackground(Content.Load<Texture2D>("Parallax_Layers/_stage1"), _player, 1f)
                {
                    Layer = 0.1f,
                },
                new ScrollingBackground(Content.Load<Texture2D>("Parallax_Layers/Layer 1"), _player, 15f)
                {
                    Layer = 0.92f,
                },
                new ScrollingBackground(Content.Load<Texture2D>("Parallax_Layers/Layer 2"), _player, 30f)
                {
                    Layer = 0.95f,
                },
                new ScrollingBackground(Content.Load<Texture2D>("Parallax_Layers/Layer 3"), _player, 40f)
                {
                    Layer = 0.97f,
                }
            };
            #endregion

            #region Minor and Major Enemies 
            try
            {
                _minorEnemyTexture = Content.Load<Texture2D>("Enemies/Skeleton");
            }
            catch (ContentLoadException e)
            {
                Console.WriteLine("Error Loading Skeleton Texture Error Details : " + e);
            }

            _minorEnemyAnimation = new SpriteAnimation2D(
                frameHeight: 64,
                frameWidth: 96,
                rowFrameCount: new Dictionary<int, int>
                {
                    {0, 10}, // Atk 1
                    {1, 9}, // Atk 2
                    {2, 5}, // Take Damage
                    {3, 8}, // Idle
                    {4, 10} // Walk
                },
                frameTime: .2f
            );

            _minorEnemies = new List<MinorEnemy>(); // Initialize the list of enemies
            SpawnEnemies(5); // Spawn 5 enemies

            #endregion
            
            _camera = new Camera2D(graphicsDevice.Viewport);
            _mainMenu = new MainMenu(Content, graphicsDevice);
            _platform1 = new Platform("../../../Data/Stage1map.csv", Content, graphicsDevice);
            _gameStage = new GameStage(_scrollingBackground, _player, _minorEnemies, _platform1);

            foreach (var bg in _scrollingBackground)
            {
                bg.SetCamera(_camera);
            }
        }

        private void SpawnEnemies(int count)
        {
            for (int i = 0; i < count; i++)
            {
                float randomX = (float)_random.Next(0, Game1.ScreenWidth - 96); 
                float randomY = (float)_random.Next(0, Game1.ScreenHeight - 64); 

                var enemy = new MinorEnemy(_minorEnemyTexture, _minorEnemyAnimation, 0, 400)
                {
                    Position = new Vector2(randomX, randomY),
                    Layer =  0.93f,
                };

                _minorEnemies.Add(enemy); 
            }
        }

        // Properties
        public MainMenu MainMenu => _mainMenu;
        public GameStage GameStage => _gameStage;

        public Camera2D Camera { get => _camera; }
    }
}
