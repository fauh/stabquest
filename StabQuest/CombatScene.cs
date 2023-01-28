using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< Updated upstream
using static StabQuest.DiceHelper;
=======
using StabQuest.Legacy_CombatSim;
using static StabQuest.Helpers.DiceHelper;
>>>>>>> Stashed changes

namespace rpgcombatsim
{
    class CombatScene
    {

        public enum CombatState
        {
            ACTIVE,
            INACTIVE
        }

        public CombatState CurrentState { get; set; }
        public SortedList<int, List<Character>> Initiative { get; private set; }
        public List<Character> Players { get; }
        public List<Character> Monsters { get; }
        public int CurrentTurn { get; set; }

        public CombatScene(List<Character> players, List<Character> monsters)
        {
            this.CurrentTurn = 1;
            this.Players = players;
            this.Monsters = monsters;
            CurrentState = CombatState.ACTIVE;
            this.Initiative = new SortedList<int, List<Character>>();
        }


        public void Play()
        {
            Console.WriteLine("======= Combat starts! ======");
            while (CurrentState.Equals(CombatState.ACTIVE))
            {
                Console.WriteLine($"======= TURN {CurrentTurn} ======");
                CombatTurn();

            }
        }

        private void CombatTurn()
        {
            RecalculateInitative();

            foreach (var initiativeOrder in Initiative)
            {
                foreach (var character in initiativeOrder.Value)
                {
                    if (CurrentState != CombatState.ACTIVE)
                    {
                        return;
                    }
                    Console.WriteLine("~~~~~~~~~~~~");
                    if (character.CurrentHealth > 0)
                    {
                        TakeAction(character);
                    }
                    else
                    {
                        Console.WriteLine($"{character.Name} lies dead on the ground.");
                    }
                }
            }

            CurrentTurn++;
        }

        private void TakeAction(Character character)
        {
            if (CombatStateCheck() != CombatState.ACTIVE)
            {
                return;
            }

            if (character.IsPlayer)
            {
                Console.WriteLine($"Health: {character.CurrentHealth}");
                Console.WriteLine("What would you like to do? 1. Attack 2. Run away");
                var choice = Console.ReadLine();

                if (int.Parse(choice) == 1)
                {
                    Console.WriteLine("Choose your target:");
                    for (int i = 1; i <= Monsters.Count; i++)
                    {
                        Console.WriteLine($"{i}. {Monsters[i - 1].Name}");
                    }
                    var chosenMonster = Console.ReadLine();
                    character.MakeAttack(Monsters[int.Parse(chosenMonster) - 1]);
                }
                else
                {
                    Console.WriteLine("You ran away!");
                    CurrentState = CombatState.INACTIVE;
                }
            }
            else
            {
                var chosenPlayer = Players[RollDice(Players.Count) - 1];
                character.MakeAttack(chosenPlayer);
            }

            CombatStateCheck();
        }

        private CombatState CombatStateCheck()
        {
            if (Players.All(p => p.CurrentHealth == 0))
            {
                Console.WriteLine("All heroes lay defeated on the ground... Better luck next time!");
                EndCombat();
            }
            if (Monsters.All(m => m.CurrentHealth == 0))
            {
                Console.WriteLine("All monsters were defeated, you are victorious!");
                EndCombat();
            }

            return CurrentState;
        }

        private void EndCombat()
        {
            Console.WriteLine("======== END OF COMBAT ========");

            CurrentState = CombatState.INACTIVE;
        }

        private void RecalculateInitative()
        {
            // Clear initiatve.
            Initiative = new SortedList<int, List<Character>>();
            List<Character> allCharacters = new List<Character>();
            allCharacters.AddRange(Players);
            allCharacters.AddRange(Monsters);

            foreach (var character in allCharacters)
            {
                var initiativeOrder = RollDice(20);

                if (Initiative.ContainsKey(initiativeOrder))
                {
                    Initiative[initiativeOrder].Add(character);
                }
                else
                {
                    Initiative.Add(initiativeOrder, new List<Character> { character });
                }
            }
        }
    }
}

