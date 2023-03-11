using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StabQuest.Services;
using StabQuest.UI;
using System;
using System.Collections.Generic;

namespace StabQuest.GameStates
{
    internal class OptionsState : GameState
    {
        SoundService _soundService = SoundService.Instance;
        private bool _isFullScreen;
        private int _screenWidth;
        private int _screenHeight;
        private int _backgroundMusicVolumePercentage;
        private int _soundEffectsVolumePercentage;

        private int _backgroundMusicVolumePercentage_original;
        private int _soundEffectsVolumePercentage_original;

        private bool _isDirty = false;
        private SpriteFont _font;
        private List<GameComponent> _components;

        public int ScreenWidth
        {
            get { return _screenWidth; }
            private set
            {
                if (_screenWidth != value)
                {
                    _screenWidth = value;
                    _isDirty = true;
                }
            }
        }
        public int ScreenHeight
        {
            get
            { return _screenHeight; }
            private set
            {
                if (_screenHeight != value)
                {
                    _screenHeight = value;
                    _isDirty = true;
                }
            }
        }
        public bool IsFullScreen
        {
            get { return _isFullScreen; }
            private set
            {
                if (_isFullScreen != value)
                {
                    _isFullScreen = value;
                    _isDirty = true;
                }
            }
        }
        public int BackgroundMusicVolumePercentage
        {
            get { return _backgroundMusicVolumePercentage; }
            private set
            {
                if (_backgroundMusicVolumePercentage != value)
                {
                    _backgroundMusicVolumePercentage = value;
                    _isDirty = true;
                }
            }
        }
        public int SoundEffectsVolumePercentage
        {
            get { return _soundEffectsVolumePercentage; }
            private set
            {
                if (_soundEffectsVolumePercentage != value)
                {
                    _soundEffectsVolumePercentage = value;
                    _isDirty = true;
                }
            }
        }

        public OptionsState(ContentManager content, GraphicsDevice graphicsDevice, Game1 game) : base(content, graphicsDevice, game)
        {
            _isFullScreen = _game.IsFullScreen;
            _screenHeight = _game.ScreenHeight;
            _screenWidth = _game.ScreenWidth;

            var buttonTexture = content.Load<Texture2D>("Images/button");
            _font = content.Load<SpriteFont>("MyFont");


            var saveButton = new Button(new Vector2(50, 400), buttonTexture, _font)
            {
                Text = "Save"
            };
            var resetButton = new Button(new Vector2(200, 400), buttonTexture, _font)
            {
                Text = "Reset"
            };

            saveButton.Click += SaveButton_Click;
            resetButton.Click += ResetButton_Click;
            _components = new List<GameComponent>() {
                saveButton,
                resetButton
            };
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void SetBackgroundMusicVolume(int volume)
        {
            BackgroundMusicVolumePercentage = volume;
        }

        private void SetSoundEffectsVolume(int volume)
        {
            SoundEffectsVolumePercentage = volume;
        }

        private void SetFullScreen(bool isFullScreen)
        {
            IsFullScreen = isFullScreen;
        }

        private void SetScreenResolution(int height, int width)
        {
            ScreenWidth = width;
            ScreenHeight = height;
        }

        private void SaveSettings()
        {
            if (!_isDirty)
            {
                return;
            }

            _soundService.BackgroundMusicVolumePercentage = BackgroundMusicVolumePercentage;
            _soundService.SoundEffectsVolumePercentage = SoundEffectsVolumePercentage;
            _game.SetFullScreen(_isFullScreen);
            _game.SetScreenSize(ScreenWidth, ScreenHeight);

            _game.ChangeState(new MainMenuState(_content, _graphicsDevice, _game));
        }

        private void Reset()
        {
            _isFullScreen = _game.IsFullScreen;
            _screenHeight = _game.ScreenHeight;
            _screenWidth = _game.ScreenWidth;
            _soundEffectsVolumePercentage = _soundEffectsVolumePercentage_original;
            _backgroundMusicVolumePercentage = _backgroundMusicVolumePercentage_original;
            _isDirty = false;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            foreach (var component in _components)
            {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
            {
                component.Update(gameTime);
            }
        }
    }
}
