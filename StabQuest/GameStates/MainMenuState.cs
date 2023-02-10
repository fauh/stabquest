using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StabQuest.UI;
using System;
using System.Collections.Generic;

namespace StabQuest.GameStates
{


    public class MainMenuState : GameState
    {
        private List<GameComponent> _components;
        private SpriteFont _font;

        public MainMenuState(ContentManager content, GraphicsDevice graphicsDevice, Game1 game) : base(content, graphicsDevice, game)
        {
            var buttonTexture = content.Load<Texture2D>("Images/button");
            _font = content.Load<SpriteFont>("MyFont");
            var newGameButton = new Button(new Vector2(325, 300), buttonTexture, _font)
            {
                Text = "New Game"
            };

            newGameButton.Click += NewGameButton_Click;

            var quitButton = new Button(new Vector2(325, 325), buttonTexture, _font)
            {
                Text = "Quit"
            };

            quitButton.Click += ExitGameButton_Click;

            _components = new List<GameComponent>() {
                newGameButton,
                quitButton
            };
            IsActiveScene = true;
        }

        private void ExitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            var overWorld = new OverworldState(_content, _graphicsDevice, _game);
            _game._overWorldState = overWorld;
            _game.ChangeState(overWorld);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsActiveScene)
            {
                return;
            }
            _graphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            spriteBatch.DrawString(_font, $"STABQUEST!", new Vector2(275, 200), Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 1);
            foreach (var component in _components)
            {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsActiveScene)
            {
                return;
            }

            foreach (var component in _components)
            {
                component.Update(gameTime);
            }
        }
    }
}
