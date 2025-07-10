using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Oblivion
{
    public class TextureManager
    {
        // Game Stages Variables;
        private MainMenu _mainMenu;
        private GameStage _gameStage;
        private GameOverScreen _gameOver;

        // Player Variables
        private Texture2D _samuraiTexture;
        private Texture2D _minorEnemyTexture;
        private Player _player;
        private SpriteAnimation2D _playerAnimation;
        private HPBar _HpBar;

        // Enemy Variables
        private SpriteAnimation2D _minorEnemyAnimation;
        private List<MinorEnemy> _minorEnemies;
        public static float attackAnimationSpeed = .15f;

        // Background Variables
        private List<ScrollingBackground> _scrollingBackground;
        private Platform _platform1;

        // Camera Variables
        private Camera2D _camera;
        public static int tileWidth = 2800;
        public static int tileHeight = 720;

        public void Load(ContentManager Content, GraphicsDevice graphicsDevice)
        {

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

            _HpBar = new HPBar(Content, 123f);
            _player = new Player(_samuraiTexture, _playerAnimation, Content, _HpBar, _HpBar.MaxValue)
            {
                Position = new Vector2(50, 0),
                Layer = 0.94f,
            };

            _scrollingBackground = new List<ScrollingBackground>()
            {
                new ScrollingBackground(Content.Load<Texture2D>("Parallax_Layers/lyr0"), _player, 1f)
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
                frameTime: attackAnimationSpeed
            );

            _minorEnemies = new List<MinorEnemy>(); // Initialize the list of enemies
            SpawnEnemies(3); // Spawn enemies

            #endregion

            #region Game Over 
            _gameOver = new GameOverScreen(Content, graphicsDevice);
            #endregion

            _camera = new Camera2D(graphicsDevice.Viewport);
            _mainMenu = new MainMenu(Content, graphicsDevice);
            _platform1 = new Platform("../../../Data/Stage1map.csv", Content, graphicsDevice);


            // Game Stage Constructor
            _gameStage = new GameStage(
                _scrollingBackground,
                _player,
                _minorEnemies,
                _platform1,
                () =>
                {
                    Game1.currentState = Game1.GameState.MainMenu;
                    MainMenu.ResetFlags();
                    _mainMenu.StartFadeIn();
                }
            );


            _gameStage.Load(Content, graphicsDevice);

            foreach (var bg in _scrollingBackground)
            {
                bg.SetCamera(_camera);
            }
        }

        private void SpawnEnemies(int count)
        {
            Vector2[] spawnPositions = new Vector2[]
            {
                new Vector2(200, 300),
                new Vector2(500, 320),
                new Vector2(800, 310),
            };

            foreach (var pos in spawnPositions)
            {
                var animationClone = new SpriteAnimation2D(_minorEnemyAnimation);

                var enemy = new MinorEnemy(_minorEnemyTexture, animationClone, pos.X - 100, pos.X + 100)
                {
                    Position = pos,
                    Layer = 0.93f,
                };

                enemy.SetTarget(_player);
                _minorEnemies.Add(enemy);
            }
        }

        // Properties
        public MainMenu MainMenu => _mainMenu;
        public GameStage GameStage => _gameStage;

        public Camera2D Camera { get => _camera; }
        public Player Player1 { get => _player; set => _player = value; }
        public GameOverScreen GameOver { get => _gameOver; }
    }

        
    }
