using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StabQuest.Helpers;
using StabQuest.UI;
using System;
using System.Collections.Generic;

namespace StabQuest.GameStates
{
    public class PauseMenuState : GameState
    {
        List<GameComponent> _components;
        private SpriteFont _font;

        public PauseMenuState(ContentManager content, GraphicsDevice graphicsDevice, Game1 game) : base(content, graphicsDevice, game)
        {
            var buttonTexture = content.Load<Texture2D>("Images/button");
            _font = content.Load<SpriteFont>("MyFont");

            var mainMenuButton = new Button(new Vector2(300, 300), buttonTexture, _font)
            {
                Text = "Return to Main Menu"
            };

            mainMenuButton.Click += MainMenuButton_Click;

            var returnButton = new Button(new Vector2(300, 200), buttonTexture, _font)
            {
                Text = "Return to Game"
            };

            returnButton.Click += ReturnButton_Click;

            _components = new List<GameComponent>() {
                mainMenuButton,
                returnButton
            };
        }

        private void MainMenuButton_Click(object sender, EventArgs e)
        {
            _game._overWorldState = null;
            _game.ChangeState(new MainMenuState(_content, _graphicsDevice, _game));
        }

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(_game._overWorldState);
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
        { }


        public override void Update(GameTime gameTime)
        {
            if (KeyboardHelper.CheckKeyPress(Keys.Escape))
            {
                _game.ChangeState(_game._overWorldState);
            }

            foreach (var component in _components)
            {
                component.Update(gameTime);
            }

        }
    }
}
