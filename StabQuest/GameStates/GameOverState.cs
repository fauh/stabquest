using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StabQuest.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StabQuest.GameStates
{
    public class GameOverState : GameState
    {
        private SpriteFont _font;

        private List<GameComponent> _components;

        private Player _player;
        public GameOverState(ContentManager content, GraphicsDevice graphicsDevice, Game1 game, Player player) : base(content, graphicsDevice, game)
        {
            var buttonTexture = content.Load<Texture2D>("Images/button");
            _font = content.Load<SpriteFont>("MyFont");
            _player= player;
            var mainMenuButton = new Button(new Vector2(300, 350), buttonTexture, _font)
            {
                Text = "Main Menu"
            };

            mainMenuButton.Click += MainMenuButton_Click;

            _components = new List<GameComponent> { 
                mainMenuButton 
            };
        }

        private void MainMenuButton_Click(object sender, EventArgs e)
        {
            _game._overWorldState = null;
            _game.ChangeState(new MainMenuState(_content, _graphicsDevice, _game));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            
            foreach(var component in _components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.DrawString(_font, $"Game Over!", new Vector2(275, 225), Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 1);

            var xval = 10;
            foreach (var pc in _player.Characters)
            {
                pc.PrintCharacter(spriteBatch, pc, xval, _font);
                xval += 200;
            }

            spriteBatch.DrawString(_font, $"You reached dungeon level {_game._overWorldState.CurrentLevel}", new Vector2(175, 250), Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 1);

            spriteBatch.End();      
        }

        public override void PostUpdate(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            foreach(var component in _components) 
            {
                component.Update(gameTime);    
            }
        }
    }
}
