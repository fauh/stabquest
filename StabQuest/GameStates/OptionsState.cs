using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StabQuest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StabQuest.GameStates
{
    internal class OptionsState : GameState
    {

        SoundService _soundService = SoundService.Instance;

        public OptionsState(ContentManager content, GraphicsDevice graphicsDevice, Game1 game) : base(content, graphicsDevice, game)
        {
        }

        private void SetBackgroundMusicVolume(int volume) {
            _soundService.BackgroundMusicVolumePercentage = volume;
        }

        private void SetSoundEffectsVolume(int volume) {
            _soundService.SoundEffectsVolumePercentage = volume;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
