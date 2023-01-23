using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StabQuest.Helpers;
using StabQuest.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using static StabQuest.Helpers.DiceHelper;

namespace StabQuest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont _font;
        private Texture2D _dungeonTileSet;
        private Texture2D _characterSpriteSheet;
        private Texture2D _buttonTexture;

        private List<SimpleRandomWalkDungeonLevel> _dungeonLevels;
        private SimpleRandomWalkDungeonLevel _currentDungeonLevel;
        private Player _player;
        int _currentLevel;

        private int _screenHeight;
        private int _screenWidth;

        public static int TileSize = 16;

        public static int WorldScale = 1;

        private Camera _camera;
        private Button _exitButton;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _currentLevel = 0;
            _screenHeight = _graphics.PreferredBackBufferHeight;
            _screenWidth = _graphics.PreferredBackBufferWidth;
        }

        protected override void Initialize()
        {
            base.Initialize();

            // TODO: Add your initialization logic here

            _dungeonLevels = new List<SimpleRandomWalkDungeonLevel>();
            _currentDungeonLevel = new SimpleRandomWalkDungeonLevel(_currentLevel, _dungeonTileSet, TileSize, WorldScale);

            _player = new Player(_currentDungeonLevel.DoorPositionStart, _characterSpriteSheet);
            _player.CurrentDungeonLevel = _currentDungeonLevel;
            _camera = new Camera(TileSize, _screenHeight, _screenWidth);
            _dungeonLevels.Add(_currentDungeonLevel);

            _exitButton = new Button(new Vector2(_player.WorldPosition.X - (_screenWidth / 2) + 10, _player.WorldPosition.Y - (_screenHeight / 2) + _buttonTexture.Height + 5), _buttonTexture, _font) { 
                Text = "Quit Game"
            };

            _exitButton.Click += ExitButton_Click;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _font = this.Content.Load<SpriteFont>("MyFont");
            _dungeonTileSet = this.Content.Load<Texture2D>("Images/Dungeon_Tileset");
            _characterSpriteSheet = this.Content.Load<Texture2D>("Images/Dungeon_Character_2");
            _buttonTexture = this.Content.Load<Texture2D>("Images/button");
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardHelper.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            _player.Update(gameTime);
            _camera.Follow(_player.WorldPosition);

            _exitButton.Position = new Vector2(_player.WorldPosition.X - (_screenWidth / 2) + 10, _player.WorldPosition.Y - (_screenHeight / 2) + _buttonTexture.Height + 5);
            _exitButton.Update(gameTime);

          
            HandleLevelExiting();

            base.Update(gameTime);
       
        }
    

        private void ExitButton_Click(object sender, System.EventArgs e)
        {
            Exit();
        }

        

        private void HandleLevelExiting()
        {
            if (_player.Position.Equals(_currentDungeonLevel.DoorPositionEnd) && _player.HasMoved)
            {
                var next = _currentLevel + 1;
                var nextLevel = _dungeonLevels.FirstOrDefault(d => d.Level == next);

                if (nextLevel != null)
                {
                    _currentDungeonLevel = nextLevel;
                    _player.CurrentDungeonLevel = _currentDungeonLevel;

                }
                else
                {
                    nextLevel = new SimpleRandomWalkDungeonLevel(next, _dungeonTileSet, TileSize, WorldScale);
                    _currentDungeonLevel = nextLevel;
                    _player.CurrentDungeonLevel = _currentDungeonLevel;
                    _dungeonLevels.Add(nextLevel);
                }

                _currentLevel = next;
                _player.Position = nextLevel.DoorPositionStart;
                _player.HasMoved = false;
            }

            if (_player.Position.Equals(_currentDungeonLevel.DoorPositionStart) && _player.HasMoved)
            {
                var prev = _currentLevel - 1;
                if (_currentLevel < 0)
                {
                    Exit();
                }
                _currentLevel = prev;

                var previousLevel = _dungeonLevels.FirstOrDefault(d => d.Level == prev);

                if (previousLevel != null)
                {
                    _currentDungeonLevel = previousLevel;
                    _player.CurrentDungeonLevel = _currentDungeonLevel;
                    _player.Position = _currentDungeonLevel.DoorPositionEnd;
                    _player.HasMoved = false;
                }
                else
                {
                    Exit();
                }
                _player.HasMoved = false;

            }
        }
        protected override void Draw(GameTime gameTime)
        {
            var backgroundColor = new Color(37, 19, 26);
            GraphicsDevice.Clear(backgroundColor);

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _spriteBatch.Begin(transformMatrix: _camera.Transform);

            foreach (var tile in _currentDungeonLevel.Tiles) {
                tile.Draw(gameTime, _spriteBatch);
            }

            _exitButton.Draw(gameTime, _spriteBatch);

            _spriteBatch.DrawString(_font, $"Current Level: {_currentLevel}", new Vector2(_player.WorldPosition.X-(_screenWidth/2) + 10, _player.WorldPosition.Y - (_screenHeight / 2)+ 10), Color.White);

            _spriteBatch.Draw(_currentDungeonLevel.Texture, new Vector2(TileSize * _currentDungeonLevel.DoorPositionStart.X, TileSize * _currentDungeonLevel.DoorPositionStart.Y), 
                new Rectangle(8* TileSize, 3* TileSize, TileSize, TileSize), Color.Green, 0, Vector2.One, scale: WorldScale, SpriteEffects.None, 0);
            _spriteBatch.Draw(_currentDungeonLevel.Texture, new Vector2(TileSize * _currentDungeonLevel.DoorPositionEnd.X, TileSize * _currentDungeonLevel.DoorPositionEnd.Y), 
                new Rectangle(8 * TileSize, 3 * TileSize, TileSize, TileSize), Color.Red, 0, Vector2.One, scale: WorldScale, SpriteEffects.None, 0);

            _player.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}