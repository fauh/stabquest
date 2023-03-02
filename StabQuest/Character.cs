using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StabQuest.GameStates;
using System;
using static StabQuest.Helpers.DiceHelper;

namespace StabQuest
{
    public class Character
    {
        private int _currentHealth;
        private int _currentExperience;
        /// <summary>
        /// Simplied constructor, sets all stats to 0. IsPlayer = false
        /// </summary>
        /// <param name="name"></param>
        public Character(string name) : this(name, RollDice(4), RollDice(4), RollDice(4), RollDice(4), RollDice(4), RollDice(4), false)
        {
        }

        /// <summary>
        /// Creates a character
        /// </summary>
        /// <param name="name"></param>
        /// <param name="str"></param>
        /// <param name="dex"></param>
        /// <param name="con"></param>
        /// <param name="_in"></param>
        /// <param name="wis"></param>
        /// <param name="cha"></param>
        public Character(string name, int str, int dex, int con, int _in, int wis, int cha, bool isPlayer)
        {
            _guid = new Guid();
            Name = name;
            Stats = new int[6];

            Stats[(int)Stat.STR] = str;
            Stats[(int)Stat.DEX] = dex;
            Stats[(int)Stat.CON] = con;
            Stats[(int)Stat.INT] = _in;
            Stats[(int)Stat.WIS] = wis;
            Stats[(int)Stat.CHA] = cha;

            IsPlayer = isPlayer;

            MaxHealth = 10 + con;
            CurrentHealth = MaxHealth;

            Level = 0;
            ShouldLevelUp = true;

            IsDead = false;

            CurrentExperience= 0;
        }

        public void Print()
        {
            Console.WriteLine($"----------");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"----------");
            Console.WriteLine($"| STR: {Stats[(int)Stat.STR]} |");
            Console.WriteLine($"| DEX: {Stats[(int)Stat.DEX]} |");
            Console.WriteLine($"| CON: {Stats[(int)Stat.CON]} |");
            Console.WriteLine($"| INT: {Stats[(int)Stat.INT]} |");
            Console.WriteLine($"| WIS: {Stats[(int)Stat.WIS]} |");
            Console.WriteLine($"| CHA: {Stats[(int)Stat.CHA]} |");
            Console.WriteLine($"-----------");
            Console.WriteLine($"Health: {CurrentHealth}");
            Console.WriteLine($"Equipment:");
            //foreach (var equip in this.EquipmentList)
            //{
            //    equip.Print();
            //}
            Weapon?.Print();
            Armor?.Print();
        }

        public int Level { get; set; }

        public int CurrentExperience {
            get { return _currentExperience; }
            set {
                _currentExperience  = value;

                while(_currentExperience >= ExperienceForNextLevel)
                {
                    LevelUp();
                }
            } 
        }

        public void LevelUp()
        {
            Level++;
            UnspentSkillPoints += (int)((10 + Level) / 10) + 1;
            ShouldLevelUp = true;
        }

        public int ExperienceForNextLevel => Level * Level * 100 ;

        public bool IsDead { get; set; }

        public string Name { get; set; }

        public int[] Stats { get; set; }
        public bool IsPlayer { get; }
        public int MaxHealth { get; set; }
        public int CurrentHealth
        {
            get { return _currentHealth; }
            set
            {
                _currentHealth = value;

                if (_currentHealth >= MaxHealth)
                {
                    _currentHealth = MaxHealth;
                } if (_currentHealth <= 0)
                {
                    _currentHealth = 0;
                    IsDead = true;
                }                
            }
        }

        public Armor Armor { get; set; }

        public Weapon Weapon { get; set; }

        public Guid _guid { get; }
        public int UnspentSkillPoints { get; internal set; }
        public bool ShouldLevelUp { get; set; }

        public int GetStatValue(Stat stat)
        {
            if (stat == Stat.NONE)
            {
                return 0;

            }

            return Stats[(int)stat];

        }


      
        public void MakeAttack(Character other)
        {
            var weaponDamage = RollDice(Weapon.DiceCount, Weapon.FaceCount) + GetStatValue(Weapon.Modifier);


            Console.WriteLine($"{Name} attacks {other.Name} with its {Weapon.Name} doing {weaponDamage} damage!");

            other.TakeDamage(weaponDamage);

        }

        public void RegainHealth(int healing)
        {
            CurrentHealth += healing;
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }


        public void TakeDamage(int damage)
        {
            if (CurrentHealth > 0)
            {
                var armorReduction = Armor is null ? 0 : Armor.BaseArmorValue + GetStatValue(Armor.Modifier);

                var reducedDamage = damage - armorReduction;

                if (reducedDamage < 0)
                {
                    reducedDamage = 0;
                }

                if (armorReduction > 0)
                {

                    Console.WriteLine($"{Name} takes {reducedDamage}, reduced by its {Armor.Name}.");
                }
                else
                {
                    Console.WriteLine($"{Name} takes {reducedDamage}");
                }


                CurrentHealth -= reducedDamage;

                if (CurrentHealth <= 0)
                {
                    CurrentHealth = 0;
                    Console.WriteLine($"{Name} succumbs to its wounds and dies.");
                }
            }
            else
            {
                Console.WriteLine($"{Name} is already dead...");
            }
        }

        public void PrintCharacter(SpriteBatch spriteBatch, Character pc, int xvalue, SpriteFont font)
        {
            var pos = new Vector2(xvalue, 10);
           
            spriteBatch.DrawString(font, $"Player Name: {pc.Name}", pos, Color.White);
            spriteBatch.DrawString(font, $"Level: {pc.Level}", new Vector2(pos.X, pos.Y + 20), Color.White);
            spriteBatch.DrawString(font, $"EXP: {pc.CurrentExperience} / {pc.ExperienceForNextLevel}", new Vector2(pos.X, pos.Y + 40), Color.White);
            spriteBatch.DrawString(font, $"Health: {pc.CurrentHealth} / {pc.MaxHealth}", new Vector2(pos.X, pos.Y + 60), Color.White);
            spriteBatch.DrawString(font, $"Armor: {pc.Armor?.Name ?? "None"}", new Vector2(pos.X, pos.Y + 80), Color.White);
            spriteBatch.DrawString(font, $"Weapon: {pc.Weapon?.Name ?? "None"}", new Vector2(pos.X, pos.Y + 100), Color.White);
            spriteBatch.DrawString(font, $"STR: {pc.GetStatValue(Stat.STR)}", new Vector2(pos.X, pos.Y + 120), Color.White);
            spriteBatch.DrawString(font, $"DEX: {pc.GetStatValue(Stat.DEX)}", new Vector2(pos.X, pos.Y + 140), Color.White);
            spriteBatch.DrawString(font, $"CON: {pc.GetStatValue(Stat.CON)}", new Vector2(pos.X, pos.Y + 160), Color.White);
            spriteBatch.DrawString(font, $"WIS: {pc.GetStatValue(Stat.WIS)}", new Vector2(pos.X, pos.Y + 180), Color.White);
            spriteBatch.DrawString(font, $"INT: {pc.GetStatValue(Stat.INT)}", new Vector2(pos.X, pos.Y + 200), Color.White);
            spriteBatch.DrawString(font, $"CHA: {pc.GetStatValue(Stat.CHA)}", new Vector2(pos.X, pos.Y + 220), Color.White);

        }
    }
}
