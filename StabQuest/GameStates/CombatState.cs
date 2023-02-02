using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StabQuest.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StabQuest.GameStates
{
    public class CombatState : GameState
    {
        private Player _player;

        public CombatState(ContentManager content, GraphicsDevice graphicsDevice, Game1 game, Player player) : base(content, graphicsDevice, game)
        {
            _player = player;
            var pc = _player.Characters.FirstOrDefault();
            if (pc != null)
            {
                pc.CurrentHealth--;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.Red);
        }

        public override void PostUpdate(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        private void CheckHealth()
        {
            if (_player.Characters.All(c => c.CurrentHealth <= 0))
            {
                _game.ChangeState(new MainMenuState(_content, _graphicsDevice, _game));
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (KeyboardHelper.CheckKeyPress(Keys.Escape))
            {
                _game.ChangeState(_game._overWorldState);
            }
            CheckHealth();
        }
    }
}
