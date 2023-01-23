using System;

namespace StabQuest.Helpers
{
    public static class DiceHelper
    {

        public static int RollDice(int diceCount, int faceCount)
        {
            var total = 0;
            for (int i = 0; i < diceCount; i++)
            {
                total += RollDice(faceCount);
            }

            return total;
        }

        public static int RollDice(int faceCount)
        {
            Random rand = new Random();

            return rand.Next(1, faceCount + 1);
        }

        public static int RollDiceZeroIndex(int faceCount)
        {
            Random rand = new Random();

            return rand.Next(0, faceCount);
        }

        public static int CoinFlip() => new Random().Next(0, 2);
    }
}