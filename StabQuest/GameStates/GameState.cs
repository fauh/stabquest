using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace StabQuest.GameStates
{
    public abstract class GameState
    {
        protected ContentManager _content;
        protected GraphicsDevice _graphicsDevice;
        protected Game1 _game;

        public bool IsActiveScene { get; set; }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract void Update(GameTime gameTime);

        public abstract void PostUpdate(GameTime gameTime);

        public GameState(ContentManager content, GraphicsDevice graphicsDevice, Game1 game)
        {
            _content = content;
            _graphicsDevice = graphicsDevice;
            _game = game;
        }

    }
}
