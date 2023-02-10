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
            foreach (var pc in _player.Characters) {
                pc.CurrentHealth--;
                pc.CurrentExperience += 25;
            }

            IsActiveScene = true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsActiveScene)
            {
                return;
            }
            _graphicsDevice.Clear(Color.Red);
        }

        public override void PostUpdate(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        private void CheckHealth()
        {
            if (!IsActiveScene)
            {
                return;
            }
            if (_player.Characters.All(c => c.IsDead))
            {
                _game.ChangeState(new GameOverState(_content, _graphicsDevice, _game, _player));
                IsActiveScene = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsActiveScene)
            {
                return;
            }
            if (KeyboardHelper.CheckKeyPress(Keys.Escape))
            {
                _game.ChangeState(_game._overWorldState);
                IsActiveScene = false;
            }
            CheckHealth();
        }
    }
}
