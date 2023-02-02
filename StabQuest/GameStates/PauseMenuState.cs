using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using StabQuest.Helpers;
using StabQuest.Legacy_CombatSim;
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

            PrintCharacter(spriteBatch, _player.Characters.First(),10);

            spriteBatch.End();
        }

        private void PrintCharacter(SpriteBatch spriteBatch, Character pc, int xvalue)
        {
            var pos = new Vector2(10, xvalue);
            spriteBatch.DrawString(_font, $"Player Name: {pc.Name}", pos, Color.White);
            spriteBatch.DrawString(_font, $"Health: {pc.CurrentHealth} / {pc.MaxHealth}", new Vector2(pos.X, pos.Y + 20), Color.White);
            spriteBatch.DrawString(_font, $"Armor: {pc.Armor?.ToString() ?? "None"}", new Vector2(pos.X, pos.Y + 40), Color.White);
            spriteBatch.DrawString(_font, $"Weapon: {pc.Weapon?.ToString() ?? "None"}", new Vector2(pos.X, pos.Y + 60), Color.White);
            spriteBatch.DrawString(_font, $"STR: {pc.GetStatValue(Legacy_CombatSim.Stat.STR)}", new Vector2(pos.X, pos.Y + 80), Color.White);
            spriteBatch.DrawString(_font, $"DEX: {pc.GetStatValue(Legacy_CombatSim.Stat.DEX)}", new Vector2(pos.X, pos.Y + 100), Color.White);
            spriteBatch.DrawString(_font, $"CON: {pc.GetStatValue(Legacy_CombatSim.Stat.CON)}", new Vector2(pos.X, pos.Y + 120), Color.White);
            spriteBatch.DrawString(_font, $"WIS: {pc.GetStatValue(Legacy_CombatSim.Stat.WIS)}", new Vector2(pos.X, pos.Y + 140), Color.White);
            spriteBatch.DrawString(_font, $"INT: {pc.GetStatValue(Legacy_CombatSim.Stat.INT)}", new Vector2(pos.X, pos.Y + 160), Color.White);
            spriteBatch.DrawString(_font, $"CHA: {pc.GetStatValue(Legacy_CombatSim.Stat.CHA)}", new Vector2(pos.X, pos.Y + 180), Color.White);
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
