using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using StabQuest.Helpers;
using StabQuest.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StabQuest.GameStates
{
    public class PauseMenuState : GameState
    {
        List<GameComponent> _components;
        private SpriteFont _font;
        private Player _player;

        public PauseMenuState(ContentManager content, GraphicsDevice graphicsDevice, Game1 game, Player player) : base(content, graphicsDevice, game)
        {
            var buttonTexture = content.Load<Texture2D>("Images/button");
            _font = content.Load<SpriteFont>("MyFont");
                               
            var returnButton = new Button(new Vector2(325, 250), buttonTexture, _font)
            {
                Text = "Return"
            };

            var mainMenuButton = new Button(new Vector2(325, 275), buttonTexture, _font)
            {
                Text = "Quit"
            };


            mainMenuButton.Click += MainMenuButton_Click;

            returnButton.Click += ReturnButton_Click;

            _components = new List<GameComponent>() {
                mainMenuButton,
                returnButton
            };

            _player = player;
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
            if (!IsActiveScene)
            {
                return;
            }
            _graphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            foreach (var component in _components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            var xval = 10;
            foreach (var pc in _player.Characters)
            {
                pc.PrintCharacter(spriteBatch, pc, xval, _font);
                xval += 200;
            }

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        { }


        public override void Update(GameTime gameTime)
        {
            if(!IsActiveScene)
            {
                return;
            }
            if (KeyboardHelper.CheckKeyPress(Keys.Escape))
            {
                _game.ChangeState(_game._overWorldState);
                IsActiveScene = false;
            }

            foreach (var component in _components)
            {
                component.Update(gameTime);
            }

        }
    }
}
