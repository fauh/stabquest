using System;

namespace StabQuest.Legacy_CombatSim
{
    public class Weapon
    {
        public Weapon(string name, int diceCount, int faceCount, Stat stat) : base()
        {
            Name = name;
            DiceCount = diceCount;
            FaceCount = faceCount;
            Modifier = stat;
            _guid = new Guid();
        }

        public string Name { get; }
        public int DiceCount { get; private set; }
        public int FaceCount { get; private set; }
        public Stat Modifier { get; private set; }

        public Guid _guid { get; }

        public void Print()
        {
            Console.WriteLine($"{Name} , {DiceCount}d{FaceCount} + {Modifier} damage");
        }
    }

}
