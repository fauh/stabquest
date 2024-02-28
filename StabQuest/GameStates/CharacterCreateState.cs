using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StabQuest.UI;
using System;
using System.Collections.Generic;

namespace StabQuest.GameStates
{
    public class CharacterCreateState : GameState
    {

        private Player _player;
        private List<GameComponent> _components;
        private TextBox _nameBox;

        public CharacterCreateState(ContentManager content, GraphicsDevice graphicsDevice, Game1 game) : base(content, graphicsDevice, game)
        {
            var buttonTexture = content.Load<Texture2D>("Images/button");

            var _characterSpriteSheet = content.Load<Texture2D>("Images/Dungeon_Character_2");
            var font = content.Load<SpriteFont>("MyFont");
            _player = new Player(_characterSpriteSheet);
            var mainMenuButton = new Button(new Vector2(300, 350), buttonTexture, font)
            {
                Text = "Main Menu"
            };

            var launchButton = new Button(new Vector2(500, 350), buttonTexture, font)
            {
                Text = "Start Game"
            };

            _nameBox = new TextBox(new Rectangle(100, 100, 200, 30), font, buttonTexture, _graphicsDevice, _game);

            mainMenuButton.Click += MainMenuButton_Click;
            launchButton.Click += Launch_Click;

            _components = new List<GameComponent> {
                mainMenuButton,
                launchButton,
                _nameBox
            };

            IsActiveScene = true;
        }

        private void MainMenuButton_Click(object sender, EventArgs e)
        {
            _game._overWorldState = null;
            _game.ChangeState(new MainMenuState(_content, _graphicsDevice, _game));
        }

        private void Launch_Click(object sender, EventArgs e)
        {
            _player.Characters.Add(new Character(_nameBox.Text));
            var overWorld = new OverworldState(_content, _graphicsDevice, _game, _player);
            _game._overWorldState = overWorld;
            _game.ChangeState(overWorld);
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
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
            {
                component.Update(gameTime);
            }
        }
    }
}
