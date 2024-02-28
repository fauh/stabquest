using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StabQuest.Helpers;
using StabQuest.Legacy_CombatSim;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StabQuest.GameStates
{
    public class CombatState : GameState
    {
        private SpriteFont _font;
        private Player _player;
        private List<Character> _monsters = new List<Character>();
        private List<string> gamelog = new List<string>();
        private int combatTurn = 1;

        public SortedList<int, List<Character>> Initiative { get; private set; } = new SortedList<int, List<Character>>();

        public CombatState(ContentManager content, GraphicsDevice graphicsDevice, Game1 game, Player player) : base(content, graphicsDevice, game)
        {

            _font = content.Load<SpriteFont>("MyFont");
            _player = player;

            GenerateMonsters();
            GenerateInitiative();

            IsActiveScene = true;
        }

        private void GenerateInitiative()
        {
            foreach (var playerCharacter in _player.Characters)
            {
                var initiative = DiceHelper.RollDice(20);
                if (Initiative.ContainsKey(initiative))
                {
                    Initiative.Values[Initiative.IndexOfKey(initiative)].Add(playerCharacter);
                }
                else
                {
                    Initiative.Add(initiative, new List<Character> { playerCharacter });
                }

                gamelog.Add($"{playerCharacter.Name} rolled {initiative} for initiative");
            }

            foreach (var monster in _monsters)
            {
                var initiative = DiceHelper.RollDice(20);
                if (Initiative.ContainsKey(initiative))
                {
                    Initiative.Values[Initiative.IndexOfKey(initiative)].Add(monster);
                }
                else
                {
                    Initiative.Add(initiative, new List<Character> { monster });
                }
                gamelog.Add($"{monster.Name} rolled {initiative} for initiative");
            }
        }

        private void GenerateMonsters()
        {
            var numberOfMonsters = DiceHelper.RollDice(_player.Characters.Count) + DiceHelper.RollDiceZeroIndex(_player.Characters.Count);

            for (int i = 1; i < numberOfMonsters + 1; i++)
            {
                var monsterStatLine = MonsterManual.StatLines.ElementAt(DiceHelper.RollDiceZeroIndex(MonsterManual.StatLines.Count));

                var monster = new Character($"{monsterStatLine.Key} {i}", monsterStatLine.Value[0], monsterStatLine.Value[1], monsterStatLine.Value[2], monsterStatLine.Value[3], monsterStatLine.Value[4], monsterStatLine.Value[5], false);

                gamelog.Add($"{monster.Name} attacks!");

                _monsters.Add(monster);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsActiveScene)
            {
                return;
            }
            _graphicsDevice.Clear(Color.Crimson);

            spriteBatch.Begin();
            var ypos = 0;

            foreach (var entry in gamelog)
            {
                ypos += 15;
                spriteBatch.DrawString(_font, entry, new Vector2(10, ypos), Color.Black);
            }
            spriteBatch.End();
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
