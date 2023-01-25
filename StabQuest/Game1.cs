using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StabQuest.Helpers;

namespace StabQuest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameState _currentState;
        private GameState _nextState;

        public OverworldState _overWorldState;

        public int _screenHeight;
        public int _screenWidth;

        public static int TILESIZE = 16;
        public static int WORLDSCALE = 1;

        //public int TileSize = TILESIZE;

        //public int WorldScale = WORLDSCALE;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _screenHeight = _graphics.PreferredBackBufferHeight;
            _screenWidth = _graphics.PreferredBackBufferWidth;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _currentState = new MainMenuState(Content, GraphicsDevice, this);
        }

        public void ChangeState(GameState state)
        {
            _nextState = state;
        }


        protected override void Update(GameTime gameTime)
        {
            KeyboardHelper.GetState();

            if (_nextState != null)
            {
                _currentState = _nextState;
            }
            _currentState.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _currentState.Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);
        }
    }
}