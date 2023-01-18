using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using static StabQuest.DiceHelper;

namespace StabQuest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        private SpriteFont _font;
        private Texture2D _wallTexture;
        private Texture2D _grassTexture;
        private Texture2D _playerTexture;
        private Texture2D _doorTexture;
        private Texture2D _dungeonTileSet;
        private Texture2D _characterSpriteSheet;
        
        private List<SimpleRandomWalkDungeonLevel> _dungeonLevels;
        private SimpleRandomWalkDungeonLevel _currentDungeonLevel;
        int _currentLevel;
        CardinalDirections _playerDirection = CardinalDirections.RIGHT;

        Vector2 _playerPosition = new Vector2(0 ,0);
        private bool _hasMoved;
        private int _screenHeight;
        private int _screenWidth;
        private int _worldScale = 1;
        private int _tileSize = 16;

        private Camera _camera;
        private Vector2 _playerWorldPosition;

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
            _currentDungeonLevel = new SimpleRandomWalkDungeonLevel(_currentLevel, _dungeonTileSet, _tileSize);
            _playerPosition = _currentDungeonLevel.DoorPositionStart;
            _camera = new Camera(_tileSize, _screenHeight, _screenWidth);
            _hasMoved = true;
            _dungeonLevels.Add(_currentDungeonLevel);            
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _font = this.Content.Load<SpriteFont>("MyFont");
            _wallTexture = this.Content.Load<Texture2D>("Images/wall");
            _grassTexture = this.Content.Load<Texture2D>("Images/grass");
            _playerTexture = this.Content.Load<Texture2D>("Images/character");
            _doorTexture = this.Content.Load<Texture2D>("Images/door");
            _dungeonTileSet = this.Content.Load<Texture2D>("Images/Dungeon_Tileset");
            _characterSpriteSheet = this.Content.Load<Texture2D>("Images/Dungeon_Character_2");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardHelper.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            _hasMoved = false;
            Vector2 newPosition = _playerPosition;

            if (KeyboardHelper.CheckKeyPress(Keys.Up))
            {
                newPosition = _playerPosition + Direction2D.Get(CardinalDirections.UP);
                _hasMoved = true;
            }
            if (KeyboardHelper.CheckKeyPress(Keys.Down))
            {
                newPosition = _playerPosition + Direction2D.Get(CardinalDirections.DOWN);
                _hasMoved = true;
            }
            if (KeyboardHelper.CheckKeyPress(Keys.Left))
            {
                newPosition = _playerPosition + Direction2D.Get(CardinalDirections.LEFT);
                _playerDirection = CardinalDirections.LEFT;
                _hasMoved = true;
            }
            if (KeyboardHelper.CheckKeyPress(Keys.Right))
            {
                newPosition = _playerPosition + Direction2D.Get(CardinalDirections.RIGHT);
                _playerDirection = CardinalDirections.RIGHT;
                _hasMoved = true;
            }

            if (!newPosition.Equals(_playerPosition)) // has actually moved
            {
                if (_currentDungeonLevel.Tiles.Any(tile => tile.Position.Equals(newPosition) && tile.Walkable))
                {
                    _playerPosition = newPosition;
                }
            }

            _playerWorldPosition = new Vector2(_playerPosition.X * _tileSize, _playerPosition.Y * _tileSize);

            _camera.Follow(_playerWorldPosition);
            //HandleCameraTransform();

            HandleLevelExiting();

            base.Update(gameTime);
        }

        private void HandleLevelExiting()
        {
            if (_playerPosition.Equals(_currentDungeonLevel.DoorPositionEnd) && _hasMoved)
            {
                var next = _currentLevel + 1;
                var nextLevel = _dungeonLevels.FirstOrDefault(d => d.Level == next);

                if (nextLevel != null)
                {
                    _currentDungeonLevel = nextLevel;

                }
                else
                {
                    nextLevel = new SimpleRandomWalkDungeonLevel(next, _dungeonTileSet, _tileSize);
                    _currentDungeonLevel = nextLevel;
                    _dungeonLevels.Add(nextLevel);
                }

                _currentLevel = next;
                _playerPosition = nextLevel.DoorPositionStart;
                _hasMoved = false;
            }

            if (_playerPosition.Equals(_currentDungeonLevel.DoorPositionStart) && _hasMoved)
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
                    _playerPosition = _currentDungeonLevel.DoorPositionEnd;
                    _hasMoved = false;
                }
                else
                {
                    Exit();
                }
                _hasMoved = false;

            }
        }
        protected override void Draw(GameTime gameTime)
        {
            var backgroundColor = new Color(37, 19, 26);
            GraphicsDevice.Clear(backgroundColor);

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //_spriteBatch.Begin(transformMatrix: _cameraTransform);
            _spriteBatch.Begin(transformMatrix: _camera.Transform);

            foreach (var tile in _currentDungeonLevel.Tiles) {
                _spriteBatch.Draw(_currentDungeonLevel.Texture, tile.WorldPosition, tile.SourceRectangle, Color.White, 0, Vector2.One, scale: _worldScale, SpriteEffects.None, 0);
            }

            _spriteBatch.DrawString(_font, $"Current Level: {_currentLevel}", new Vector2(_playerWorldPosition.X-(_screenWidth/2) + 10, _playerWorldPosition.Y - (_screenHeight / 2)+ 10), Color.White);

            _spriteBatch.Draw(_currentDungeonLevel.Texture, new Vector2(_tileSize * _currentDungeonLevel.DoorPositionStart.X, _tileSize * _currentDungeonLevel.DoorPositionStart.Y), 
                new Rectangle(8*_tileSize, 3*_tileSize, _tileSize, _tileSize), Color.Green, 0, Vector2.One, scale: _worldScale, SpriteEffects.None, 0);
            _spriteBatch.Draw(_currentDungeonLevel.Texture, new Vector2(_tileSize * _currentDungeonLevel.DoorPositionEnd.X, _tileSize * _currentDungeonLevel.DoorPositionEnd.Y), 
                new Rectangle(8 * _tileSize, 3 * _tileSize, _tileSize, _tileSize), Color.Red, 0, Vector2.One, scale: _worldScale, SpriteEffects.None, 0);

            var playerSpriteEffect = SpriteEffects.None;
            if (_playerDirection.Equals(CardinalDirections.LEFT)) {
                playerSpriteEffect = SpriteEffects.FlipHorizontally;
            }
            _spriteBatch.Draw(_characterSpriteSheet, position: _playerWorldPosition, new Rectangle(3*_tileSize, 0, _tileSize,_tileSize), Color.White, 0, Vector2.One, scale:_worldScale, playerSpriteEffect, 0);


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}