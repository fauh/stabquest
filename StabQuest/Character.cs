using System;
using System.Collections.Generic;
using static StabQuest.DiceHelper;

namespace rpgcombatsim
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
            this._guid = new Guid();
            this.Name = name;
            this.Stats = new int[6];


            this.Stats[(int)Stat.STR] = str;
            this.Stats[(int)Stat.DEX] = dex;
            this.Stats[(int)Stat.CON] = con;
            this.Stats[(int)Stat.INT] = _in;
            this.Stats[(int)Stat.WIS] = wis;
            this.Stats[(int)Stat.CHA] = cha;

            this.IsPlayer = isPlayer;

            this.MaxHealth = 10 + con;
            this.CurrentHealth = MaxHealth;


        }

        public void Print()
        {
            Console.WriteLine($"----------");
            Console.WriteLine($"Name: {this.Name}");
            Console.WriteLine($"----------");
            Console.WriteLine($"| STR: {this.Stats[(int)Stat.STR]} |");
            Console.WriteLine($"| DEX: {this.Stats[(int)Stat.DEX]} |");
            Console.WriteLine($"| CON: {this.Stats[(int)Stat.CON]} |");
            Console.WriteLine($"| INT: {this.Stats[(int)Stat.INT]} |");
            Console.WriteLine($"| WIS: {this.Stats[(int)Stat.WIS]} |");
            Console.WriteLine($"| CHA: {this.Stats[(int)Stat.CHA]} |");
            Console.WriteLine($"-----------");
            Console.WriteLine($"Health: {this.CurrentHealth}");
            Console.WriteLine($"Equipment:");
            //foreach (var equip in this.EquipmentList)
            //{
            //    equip.Print();
            //}
            this.Weapon?.Print();
            this.Armor?.Print();
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
            var weaponDamage = RollDice(this.Weapon.DiceCount, this.Weapon.FaceCount) + GetStatValue(Weapon.Modifier);


            Console.WriteLine($"{this.Name} attacks {other.Name} with its {this.Weapon.Name} doing {weaponDamage} damage!");

            other.TakeDamage(weaponDamage);

        }

        public void RegainHealth(int healing)
        {
            this.CurrentHealth += healing;
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }

        public void TakeDamage(int damage)
        {
            if (CurrentHealth > 0)
            {
                var armorReduction = this.Armor is null ? 0 : this.Armor.BaseArmorValue + GetStatValue(Armor.Modifier);

                var reducedDamage = damage - armorReduction;

                if (reducedDamage < 0)
                {
                    reducedDamage = 0;
                }

                if (armorReduction > 0)
                {

                    Console.WriteLine($"{this.Name} takes {reducedDamage}, reduced by its {this.Armor.Name}.");
                }
                else
                {
                    Console.WriteLine($"{this.Name} takes {reducedDamage}");
                }


                this.CurrentHealth -= reducedDamage;

                if (this.CurrentHealth <= 0)
                {
                    this.CurrentHealth = 0;
                    Console.WriteLine($"{this.Name} succumbs to its wounds and dies.");
                }
            }
            else
            {
                Console.WriteLine($"{this.Name} is already dead...");
            }
        }
    }
}
