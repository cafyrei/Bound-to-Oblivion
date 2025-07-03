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
        private SpriteBatch _spriteBatch;

        //Game States
        MainMenu _mainMenu;
        GamePlay _gamePlay;

        public enum GameState { MainMenu, GamePlay, Continue, Credits };

        GameState currentState = GameState.MainMenu;


        public static int screenWidth = 1280;
        public static int screenHeight = 720;
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
            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.PreferredBackBufferHeight = screenHeight;
            _graphics.ApplyChanges();
            #endregion


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _mainMenu = new MainMenu(Content, GraphicsDevice);
            _gamePlay = new GamePlay(Content, GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {

            switch (currentState)
            {
                case GameState.MainMenu:
                    _mainMenu.Update(gameTime);
                    if (_mainMenu.StartPressed)
                        currentState = GameState.GamePlay;

                    else if (_mainMenu.ContinuePressed)
                        currentState = GameState.Continue;
                    else if (_mainMenu.CreditsPressed)
                        currentState = GameState.Credits;
                    else if (_mainMenu.ExitPressed)
                        Exit();
                    break;
            }
            _gamePlay.Update(gameTime, GraphicsDevice);
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            switch (currentState)
            {
                case GameState.MainMenu:
                    _mainMenu.Draw(_spriteBatch);
                    break;
                case GameState.GamePlay:
                    _gamePlay.Draw(_spriteBatch);
                    break;

            }

            base.Draw(gameTime);
        }
    }
}
