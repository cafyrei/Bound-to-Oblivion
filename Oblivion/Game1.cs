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
        private SpriteBatch _spriteBatch;
        private GraphicsDeviceManager _graphics;

        public enum GameState { MainMenu, GamePlay, Continue, Controls, Credits, GameOver };
        public static GameState currentState = GameState.MainMenu;

        // Content Managers
        private TextureManager _textureManager;

        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;

        PlayerData loadedData = SaveSystem.LoadPlayerData();

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
            AudioManager.Load(Content); // Handles Audio for the whole game

            _textureManager = new TextureManager();
            _textureManager.Load(Content, GraphicsDevice); // Handles Txture for Background and Player

            // // Load Data
            // if (loadedData != null)
            // {
            //     _textureManager.GameStage.LoadProgress(loadedData);
            // }
        }
        protected override void Update(GameTime gameTime)
        {
            IsMouseVisible = (currentState != GameState.GamePlay) || _textureManager.GameStage.GamePause;

            if (currentState == GameState.GamePlay && !_textureManager.GameStage.GamePause)
            {
                Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            }


            switch (currentState)
            {
                case GameState.MainMenu:
                    _textureManager.MainMenu.Update();
                    AudioManager.PlayMenuBGM();

                    if (MainMenu.StartPressed)
                    {
                        currentState = GameState.GamePlay;
                        AudioManager.StopMusic();
                    }
                    else if (MainMenu.ContinuePressed)
                    {
                        // Load game state and player data
                        loadedData = SaveSystem.LoadPlayerData();
                        _textureManager.ResetGameStage(Content, GraphicsDevice);

                        if (loadedData != null)
                        {
                            _textureManager.GameStage.LoadProgress(loadedData);
                        }

                        currentState = GameState.GamePlay;
                        AudioManager.StopMusic();
                    }
                    else if (MainMenu.ControlsPressed)
                    {
                        currentState = GameState.Controls;
                    }
                    else if (MainMenu.CreditsPressed)
                    {
                        currentState = GameState.Credits;
                    }
                    else if (MainMenu.ExitPressed)
                    {
                        Exit();
                    }
                    break;

                case GameState.GamePlay:
                    _textureManager.GameStage.Update(gameTime, _textureManager.Camera);
                    break;

                case GameState.GameOver:
                    AudioManager.StopMusic();
                    _textureManager.GameOver.Update(gameTime);
                    break;

                case GameState.Controls:
                    _textureManager.Controls.Update();
                    break;

                case GameState.Credits:
                    _textureManager.Credits.Update();
                    break;
            }
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (currentState)
            {
                case GameState.MainMenu:
                    _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                    _textureManager.MainMenu.Draw(_spriteBatch);
                    _spriteBatch.End();
                    break;

                case GameState.GamePlay:
                    _textureManager.Camera.Follow(_textureManager.GameStage.GetPlayerPosition(), TextureManager.tileWidth, TextureManager.tileHeight, gameTime); // Follow the player
                    _spriteBatch.Begin(transformMatrix: _textureManager.Camera.GetViewMatrix(), samplerState: SamplerState.PointClamp);
                    _textureManager.GameStage.Draw(gameTime, _spriteBatch);
                    _spriteBatch.End();

                    _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                    _textureManager.HPBarAccess.Draw(_spriteBatch);
                    _textureManager.GameStage.DrawUI(_spriteBatch, GraphicsDevice.Viewport);
                    _textureManager.objectiveHUD.Draw(_spriteBatch, GameStage.aliveEnemies);
                    _spriteBatch.End();
                    break;
                case GameState.GameOver:
                    _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                    _textureManager.GameOver.Draw(_spriteBatch, GraphicsDevice.Viewport);
                    _spriteBatch.End();
                    break;

                case GameState.Controls:
                    _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                    _textureManager.Controls.Draw(_spriteBatch);
                    _spriteBatch.End();
                    break;

                case GameState.Credits:
                    _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                    _textureManager.Credits.Draw(_spriteBatch);
                    _spriteBatch.End();
                    break;
            }
            base.Draw(gameTime);
        }
    }
}

