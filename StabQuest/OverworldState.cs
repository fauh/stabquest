using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StabQuest.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StabQuest
{
    public class OverworldState : GameState
    {
        private SpriteFont _font;
        private Texture2D _dungeonTileSet;
        private Texture2D _characterSpriteSheet;

        private List<SimpleRandomWalkDungeonLevel> _dungeonLevels;
        private SimpleRandomWalkDungeonLevel _currentDungeonLevel;
        private Player _player;
        int _currentLevel;

        private Camera _camera;

        public OverworldState(ContentManager content, GraphicsDevice graphicsDevice, Game1 game) : base(content, graphicsDevice, game)
        {
            _currentLevel = 0;
            _font = content.Load<SpriteFont>("MyFont");
            _dungeonTileSet = content.Load<Texture2D>("Images/Dungeon_Tileset");
            _characterSpriteSheet = content.Load<Texture2D>("Images/Dungeon_Character_2");

            _dungeonLevels = new List<SimpleRandomWalkDungeonLevel>();
            _currentDungeonLevel = new SimpleRandomWalkDungeonLevel(_currentLevel, _dungeonTileSet, Game1.TILESIZE, Game1.WORLDSCALE);

            _player = new Player(_currentDungeonLevel, _characterSpriteSheet);
            _player.CurrentDungeonLevel = _currentDungeonLevel;
            _camera = new Camera(Game1.TILESIZE, game._screenHeight, game._screenWidth);
            _dungeonLevels.Add(_currentDungeonLevel);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var backgroundColor = new Color(37, 19, 26);
            _graphicsDevice.Clear(backgroundColor);

            spriteBatch.Begin(transformMatrix: _camera.Transform);

            _currentDungeonLevel.Draw(gameTime, spriteBatch);
            _player.Draw(gameTime, spriteBatch);

            var topLeftWithMargin = new Vector2(_player.WorldPosition.X - (_game._screenWidth / 2) + 10, _player.WorldPosition.Y - (_game._screenHeight / 2) + 10);
            spriteBatch.DrawString(_font, $"Current Level: {_currentLevel}", topLeftWithMargin, Color.White);
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            if (KeyboardHelper.CheckKeyPress(Keys.Escape))
            {
                _game.ChangeState(new PauseMenuState(_content, _graphicsDevice, _game));
            }

            _player.Update(gameTime);
            _camera.Follow(_player.WorldPosition);

            HandleLevelExiting();
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
                    nextLevel = new SimpleRandomWalkDungeonLevel(next, _dungeonTileSet, Game1.TILESIZE, Game1.WORLDSCALE);
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
                    _game.ChangeState(new MainMenuState(_content, _graphicsDevice, _game));
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
                    _game.ChangeState(new MainMenuState(_content, _graphicsDevice, _game));
                }
                _player.HasMoved = false;

            }
        }
    }
}
