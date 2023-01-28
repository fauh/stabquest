using System;
using System.Collections.Generic;
using static StabQuest.Helpers.DiceHelper;

namespace StabQuest.Legacy_CombatSim
{
    public class Character
    {
        /// <summary>
        /// Simplied constructor, sets all stats to 0. IsPlayer = false
        /// </summary>
        /// <param name="name"></param>
        public Character(string name) : this(name, 0, 0, 0, 0, 0, 0, false)
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

        public string Name { get; set; }

        int[] Stats { get; set; }
        public bool IsPlayer { get; }
        public int MaxHealth { get; }
        public int CurrentHealth { get; set; }

        public Armor Armor { get; set; }

        public Weapon Weapon { get; set; }

        public Guid _guid { get; }


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
    }
}
