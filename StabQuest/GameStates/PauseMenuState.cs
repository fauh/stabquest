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
            _graphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            foreach (var component in _components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            var topLeft = new Vector2(10, 10);

            spriteBatch.DrawString(_font, $"Player Name: {_player.Characters.First().Name}", topLeft, Color.White);
            spriteBatch.DrawString(_font, $"Health: {_player.Characters.First().CurrentHealth}/{_player.Characters.First().MaxHealth}", new Vector2(topLeft.X, topLeft.Y+ 20), Color.White);
            spriteBatch.DrawString(_font, $"Armor: {_player.Characters.First().Armor?.ToString() ?? "None" }", new Vector2(topLeft.X, topLeft.Y + 40), Color.White);
            spriteBatch.DrawString(_font, $"Weapon: {_player.Characters.First().Weapon?.ToString() ?? "None"}", new Vector2(topLeft.X, topLeft.Y + 60), Color.White);
            spriteBatch.DrawString(_font, $"STR: {_player.Characters.First().GetStatValue(Legacy_CombatSim.Stat.STR)}", new Vector2(topLeft.X, topLeft.Y + 80), Color.White);
            spriteBatch.DrawString(_font, $"DEX: {_player.Characters.First().GetStatValue(Legacy_CombatSim.Stat.DEX)}", new Vector2(topLeft.X, topLeft.Y + 100), Color.White);
            spriteBatch.DrawString(_font, $"CON: {_player.Characters.First().GetStatValue(Legacy_CombatSim.Stat.CON)}", new Vector2(topLeft.X, topLeft.Y + 120), Color.White);
            spriteBatch.DrawString(_font, $"WIS: {_player.Characters.First().GetStatValue(Legacy_CombatSim.Stat.WIS)}", new Vector2(topLeft.X, topLeft.Y + 140), Color.White);
            spriteBatch.DrawString(_font, $"INT: {_player.Characters.First().GetStatValue(Legacy_CombatSim.Stat.INT)}", new Vector2(topLeft.X, topLeft.Y + 160), Color.White);
            spriteBatch.DrawString(_font, $"CHA: {_player.Characters.First().GetStatValue(Legacy_CombatSim.Stat.CHA)}", new Vector2(topLeft.X, topLeft.Y + 180), Color.White);

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
