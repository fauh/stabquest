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
        private int _tileSize = 16;
        private List<SimpleRandomWalkDungeonLevel> _dungeonLevels;
        private SimpleRandomWalkDungeonLevel _currentDungeonLevel;
        int _currentLevel;
        CardinalDirections _playerDirection = CardinalDirections.RIGHT;

        Vector2 _playerPosition = new Vector2(0 ,0);
        private bool _hasMoved;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _currentLevel = 0;
        }

        protected override void Initialize()
        {
            base.Initialize();

            // TODO: Add your initialization logic here

            _dungeonLevels = new List<SimpleRandomWalkDungeonLevel>();
            _currentDungeonLevel = new SimpleRandomWalkDungeonLevel(_currentLevel, _dungeonTileSet, _tileSize);
            _playerPosition = _currentDungeonLevel.DoorPositionStart;
            _hasMoved = true;
            _dungeonLevels.Add(_currentDungeonLevel);            
        }

        

        private Vector2 GetRandomBorderTile(Vector2? positionToAvoid)
        {
            var coinFlip = DiceHelper.CoinFlip();

            var x = coinFlip * DiceHelper.RollDice(_tileSize - 1);
            var y = Math.Abs(coinFlip - 1) * DiceHelper.RollDice(_tileSize - 1);

           
            if (x == 0 && y == 0 || y == _tileSize-1)
            { 
               x++;// Move it from the corner
            }

            if (x == _tileSize-1 && y == 0 || y == _tileSize - 1)
            {
                x++;// Move it from the corner
            }

            var result = new Vector2(x, y);

            while (positionToAvoid != null && result.Equals(positionToAvoid))
            {
                return GetRandomBorderTile(positionToAvoid);
            }

            return result;

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
            if(KeyboardHelper.CheckKeyPress(Keys.Down))
            {
                newPosition = _playerPosition + Direction2D.Get(CardinalDirections.DOWN);
                _hasMoved = true;
            }
            if(KeyboardHelper.CheckKeyPress(Keys.Left))
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
                if (_currentDungeonLevel.Tiles.Any(tile => tile.Position.Equals( newPosition) && tile.Walkable))
                {                    
                    _playerPosition = newPosition;
                }
            }

            if(_playerPosition.Equals(_currentDungeonLevel.DoorPositionEnd) && _hasMoved) {
                var next = _currentLevel + 1;
                var nextLevel = _dungeonLevels.FirstOrDefault(d => d.Level == next);

                if (nextLevel != null)
                {
                    _currentDungeonLevel = nextLevel;
                    
                }
                else {
                    nextLevel = new SimpleRandomWalkDungeonLevel(next, _dungeonTileSet, _tileSize);
                    _currentDungeonLevel = nextLevel;
                    _dungeonLevels.Add(nextLevel);
                }

                _currentLevel = next;
                _playerPosition = nextLevel.DoorPositionStart;
                _hasMoved = false;
            }

            if (_playerPosition.Equals(_currentDungeonLevel.DoorPositionStart) && _hasMoved) {
                var prev = _currentLevel- 1;
                if (_currentLevel < 0) {
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
                else {
                    Exit();                
                }
                _hasMoved= false;

            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            var backgroundColor = new Color(37, 19, 26);
            GraphicsDevice.Clear(backgroundColor);

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _spriteBatch.Begin();

            foreach (var tile in _currentDungeonLevel.Tiles) {
                _spriteBatch.Draw(_currentDungeonLevel.Texture, tile.WorldPosition, tile.SourceRectangle, Color.White);
            }         

            _spriteBatch.Draw(_currentDungeonLevel.Texture, new Vector2(_tileSize * _currentDungeonLevel.DoorPositionStart.X, _tileSize * _currentDungeonLevel.DoorPositionStart.Y), new Rectangle(8*_tileSize, 3*_tileSize, _tileSize, _tileSize), Color.Green);
            _spriteBatch.Draw(_currentDungeonLevel.Texture, new Vector2(_tileSize * _currentDungeonLevel.DoorPositionEnd.X, _tileSize * _currentDungeonLevel.DoorPositionEnd.Y), new Rectangle(8 * _tileSize, 3 * _tileSize, _tileSize, _tileSize), Color.Red);

            var playerSpriteEffect = SpriteEffects.None;
            if (_playerDirection.Equals(CardinalDirections.LEFT)) {
                playerSpriteEffect = SpriteEffects.FlipHorizontally;
            }
            _spriteBatch.Draw(_characterSpriteSheet, new Vector2(_tileSize* _playerPosition.X, _tileSize * _playerPosition.Y ), new Rectangle(3*_tileSize, 0, _tileSize,_tileSize), Color.White);


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}