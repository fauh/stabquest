using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StabQuest.DungeonLevels;
using StabQuest.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StabQuest.GameStates
{
    public class OverworldState : GameState
    {
        private SpriteFont _font;
        private Texture2D _dungeonTileSet;
        private Texture2D _characterSpriteSheet;
        private Texture2D _lightMask;
        private List<SimpleRandomWalkDungeonLevel> _dungeonLevels;
        private SimpleRandomWalkDungeonLevel _currentDungeonLevel;
        private Player _player;
        int _currentLevel;


        public static Effect _lightEffect;
        RenderTarget2D lightsTarget;
        RenderTarget2D mainTarget;

        private Camera _camera;

        public OverworldState(ContentManager content, GraphicsDevice graphicsDevice, Game1 game) : base(content, graphicsDevice, game)
        {
            _currentLevel = 0;
            _font = content.Load<SpriteFont>("MyFont");
            _dungeonTileSet = content.Load<Texture2D>("Images/Dungeon_Tileset");
            _characterSpriteSheet = content.Load<Texture2D>("Images/Dungeon_Character_2");
            _lightMask = content.Load<Texture2D>("Images/lightmask");
            _lightEffect = content.Load<Effect>("Effects/lighteffect");
            _dungeonLevels = new List<SimpleRandomWalkDungeonLevel>();
            _currentDungeonLevel = new SimpleRandomWalkDungeonLevel(_currentLevel, _dungeonTileSet);

            _player = new Player(_currentDungeonLevel.EntryPoint, _characterSpriteSheet);
            _player.CurrentDungeonLevel = _currentDungeonLevel;
            _camera = new Camera(Game1.TILESIZE, game._screenHeight, game._screenWidth);
            _dungeonLevels.Add(_currentDungeonLevel);

            lightsTarget = new RenderTarget2D(graphicsDevice, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);
            mainTarget = new RenderTarget2D(graphicsDevice, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.SetRenderTarget(lightsTarget);
            _graphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, transformMatrix: _camera.Transform);

            //draw light mask where there should be torches etc...
            spriteBatch.Draw(_lightMask, new Vector2(_player.WorldPosition.X - (_lightMask.Width), _player.WorldPosition.Y - (_lightMask.Height)), sourceRectangle: null, Color.Wheat, rotation: 0, origin: Vector2.Zero, scale: 2, effects: SpriteEffects.None, layerDepth: 1);

            spriteBatch.End();

            _graphicsDevice.SetRenderTarget(mainTarget);
            _graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, transformMatrix: _camera.Transform);

            // draw everything that should be lit by lights.
            _currentDungeonLevel.Draw(gameTime, spriteBatch);
            _player.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            _graphicsDevice.SetRenderTarget(null);
            _graphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            _lightEffect.Parameters["lightMask"].SetValue(lightsTarget);
            _lightEffect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(mainTarget, Vector2.Zero, Color.White);


            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate);
            var topLeft = new Vector2(_player.WorldPosition.X - _game._screenWidth / 2, _player.WorldPosition.Y - _game._screenHeight / 2);
            var topLeftWithMargin = new Vector2(topLeft.X + 10, topLeft.Y + 10);
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

        private void ChangeLevel(DungeonLevel dungeonLevel)
        {

        }

        private void HandleLevelExiting()
        {
            if (_player.Position.Equals(_currentDungeonLevel.ExitPoint) && _player.HasMoved)
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
                    nextLevel = new SimpleRandomWalkDungeonLevel(next, _dungeonTileSet);
                    _currentDungeonLevel = nextLevel;
                    _player.CurrentDungeonLevel = _currentDungeonLevel;
                    _dungeonLevels.Add(nextLevel);
                }

                _currentLevel = next;
                _player.Position = nextLevel.EntryPoint;
                _player.HasMoved = false;
            }

            if (_player.Position.Equals(_currentDungeonLevel.EntryPoint) && _player.HasMoved)
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
                    _player.Position = _currentDungeonLevel.ExitPoint;
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
