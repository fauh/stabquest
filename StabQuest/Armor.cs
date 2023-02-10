using System;

namespace StabQuest
{
    public class Armor
    {
        public Armor(string name, int baseArmorValue, Stat stat)
        {
            Name = name;
            BaseArmorValue = baseArmorValue;
            Modifier = stat;
            _guid = new Guid();
        }

        public string Name { get; }
        public int BaseArmorValue { get; private set; }
        public Stat Modifier { get; private set; }

        public Guid _guid { get; }

        public void Print()
        {
            Console.WriteLine($"{Name}, reduces damage taken by {BaseArmorValue} + {Modifier}");
        }
    }

}
