using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StabQuest.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static StabQuest.Helpers.DiceHelper;

namespace StabQuest.GameStates
{
    public class LevelUpState : GameState
    {
        private SpriteFont _font;
        private Character _character;
        private int[] _dirtyStatArray;
        private List<GameComponent> _components;
        private int _unspent;
        public LevelUpState(ContentManager content, GraphicsDevice graphicsDevice, Game1 game, Character character) : base(content, graphicsDevice, game)
        {
            var boxTexture = content.Load<Texture2D>("Images/box");
            var buttonTexture = content.Load<Texture2D>("Images/button");
            _font = content.Load<SpriteFont>("MyFont");
            _character = character;

            ResetNewStats();
            InitiateUIElements(boxTexture, buttonTexture);
        }

        private void InitiateUIElements(Texture2D boxTexture, Texture2D buttonTexture)
        {
            var str_up = new Button(new Vector2(50, 100), boxTexture, _font)
            {
                Text = "+"
            };
            var dex_up = new Button(new Vector2(50, 150), boxTexture, _font)
            {
                Text = "+"
            };
            var con_up = new Button(new Vector2(50, 200), boxTexture, _font)
            {
                Text = "+"
            };
            var wis_up = new Button(new Vector2(50, 250), boxTexture, _font)
            {
                Text = "+"
            };
            var int_up = new Button(new Vector2(50, 300), boxTexture, _font)
            {
                Text = "+"
            };
            var cha_up = new Button(new Vector2(50, 350), boxTexture, _font)
            {
                Text = "+"
            };

            var str_down = new Button(new Vector2(350, 100), boxTexture, _font)
            {
                Text = "-"
            };

            var dex_down = new Button(new Vector2(350, 150), boxTexture, _font)
            {
                Text = "-"
            };

            var con_down = new Button(new Vector2(350, 200), boxTexture, _font)
            {
                Text = "-"
            };

            var wis_down = new Button(new Vector2(350, 250), boxTexture, _font)
            {
                Text = "-"
            };

            var int_down = new Button(new Vector2(350, 300), boxTexture, _font)
            {
                Text = "-"
            };

            var cha_down = new Button(new Vector2(350, 350), boxTexture, _font)
            {
                Text = "-"
            };

            var saveStatsButton = new Button(new Vector2(400, 400), buttonTexture, _font)
            {
                Text = "Save and Return"
            };
            var resetButton = new Button(new Vector2(550, 400), buttonTexture, _font)
            {
                Text = "Reset Stats"
            };

            str_up.Click += STR_UpClick;
            dex_up.Click += DEX_UpClick;
            con_up.Click += CON_UpClick;
            wis_up.Click += WIS_UpClick;
            int_up.Click += INT_UpClick;
            cha_up.Click += CHA_UpClick;
            str_down.Click += STR_DownClick;
            dex_down.Click += DEX_DownClick;
            con_down.Click += CON_DownClick;
            wis_down.Click += WIS_DownClick;
            int_down.Click += INT_DownClick;
            cha_down.Click += CHA_DownClick;
            resetButton.Click += ResetClick;
            saveStatsButton.Click += SaveStatsClick;

            _components = new List<GameComponent> {
                str_up,
                dex_up,
                con_up,
                wis_up,
                int_up,
                cha_up,
                str_down,
                dex_down,
                con_down,
                wis_down,
                int_down,
                cha_down,
                saveStatsButton,
                resetButton
            };

            IsActiveScene = true;
        }

        private void STR_DownClick(object sender, EventArgs e)
        {
            StatDown(Stat.STR);
        }

        private void STR_UpClick(object sender, EventArgs e)
        {
            StatUp(Stat.STR);
        }

        private void DEX_DownClick(object sender, EventArgs e)
        {
            StatDown(Stat.DEX);
        }

        private void DEX_UpClick(object sender, EventArgs e)
        {
            StatUp(Stat.DEX);
        }

        private void CON_DownClick(object sender, EventArgs e)
        {
            StatDown(Stat.CON);
        }

        private void CON_UpClick(object sender, EventArgs e)
        {
            StatUp(Stat.CON);
        }

        private void WIS_DownClick(object sender, EventArgs e)
        {
            StatDown(Stat.WIS);
        }

        private void WIS_UpClick(object sender, EventArgs e)
        {
            StatUp(Stat.WIS);
        }

        private void INT_DownClick(object sender, EventArgs e)
        {
            StatDown(Stat.INT);
        }

        private void INT_UpClick(object sender, EventArgs e)
        {
            StatUp(Stat.INT);
        }

        private void CHA_DownClick(object sender, EventArgs e)
        {
           StatDown(Stat.CHA);
        }

        private void CHA_UpClick(object sender, EventArgs e)
        {
            StatUp(Stat.CHA);
        }

        private void ResetClick(object sender, EventArgs e)
        {
            ResetNewStats();
        }

        private void StatDown(Stat stat)
        {
            var originalStatValue = _character.GetStatValue(stat);
            var newValue = _dirtyStatArray[(int)stat] - 1;

            // dont allow decreasing of stats!!!
            if (newValue >= originalStatValue && newValue >= 0)
            {
                _dirtyStatArray[(int)stat] = newValue;
                _unspent++;
            }
        }

        private void StatUp(Stat stat)
        {
            if (_unspent <= 0)
            {
                // no points left to spend
                return;
            }

            _dirtyStatArray[(int)stat]++;
            _unspent--;
        }


        private void ResetNewStats()
        {
            _dirtyStatArray = new int[6];

            _dirtyStatArray[(int)Stat.STR] = _character.GetStatValue(Stat.STR);
            _dirtyStatArray[(int)Stat.DEX] = _character.GetStatValue(Stat.DEX);
            _dirtyStatArray[(int)Stat.CON] = _character.GetStatValue(Stat.CON);
            _dirtyStatArray[(int)Stat.WIS] = _character.GetStatValue(Stat.WIS);
            _dirtyStatArray[(int)Stat.INT] = _character.GetStatValue(Stat.INT);
            _dirtyStatArray[(int)Stat.CHA] = _character.GetStatValue(Stat.CHA);

            _unspent = _character.UnspentSkillPoints;
        }

        private void SaveStatsClick(object sender, EventArgs e)
        {
            _character.Stats = _dirtyStatArray;
            var healthIncrease = _character.GetStatValue(Stat.CON) + RollDice(_character.GetStatValue(Stat.CON));

            _character.MaxHealth += healthIncrease;
            _character.CurrentHealth += healthIncrease;
            _character.ShouldLevelUp = false;
            _character.UnspentSkillPoints = _unspent;

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

            spriteBatch.DrawString(_font, $"You have reached Level {_character.Level}", new Vector2(200, 20), Color.White);

            spriteBatch.DrawString(_font, $"Unspent points: {_unspent}", new Vector2(200, 50), Color.White);

            spriteBatch.DrawString(_font, $"STR: { _dirtyStatArray[(int)Stat.STR] }", new Vector2(200, 100), Color.White);
            spriteBatch.DrawString(_font, $"DEX: { _dirtyStatArray[(int)Stat.DEX] }", new Vector2(200, 150), Color.White);
            spriteBatch.DrawString(_font, $"CON: { _dirtyStatArray[(int)Stat.CON] }", new Vector2(200, 200), Color.White);
            spriteBatch.DrawString(_font, $"WIS: { _dirtyStatArray[(int)Stat.WIS] }", new Vector2(200, 250), Color.White);
            spriteBatch.DrawString(_font, $"INT: { _dirtyStatArray[(int)Stat.INT] }", new Vector2(200, 300), Color.White);
            spriteBatch.DrawString(_font, $"CHA: { _dirtyStatArray[(int)Stat.CHA] }", new Vector2(200, 350), Color.White);

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
