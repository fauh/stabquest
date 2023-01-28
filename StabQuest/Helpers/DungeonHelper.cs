using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using static StabQuest.Helpers.DiceHelper;

namespace StabQuest.Helpers
{

    public static class DungeonHelper
    {
        public static HashSet<Vector2> GetRandomWalkDungeon(Vector2 startPosition, int iterations, int walkLength)
        {
            var currentDungeon = new HashSet<Vector2>();

            var currentPosition = startPosition;

            for (int i = 0; i < iterations; i++)
            {
                var path = SimpleRandomWalk(currentPosition, walkLength);
                currentDungeon.UnionWith(path);

                currentPosition = RollDice(4) switch
                {
                    1 => path.ElementAt(RollDiceZeroIndex(path.Count())),
                    2 => currentDungeon.ElementAt(RollDiceZeroIndex(currentDungeon.Count())),
                    3 => path.ElementAt(0),
                    4 => path.ElementAt(path.Count() - 1),
                    _ => currentPosition
                };
            }

            return currentDungeon;
        }

        public static HashSet<Vector2> SimpleRandomWalk(Vector2 startPosition, int walkLength)
        {
            var path = new HashSet<Vector2>();

            path.Add(startPosition);

            var previousPosition = startPosition;
            for (int i = 0; i < walkLength; i++)
            {
                var newPosition = previousPosition + Direction2D.GetRandomCardinalDirection();
                path.Add(newPosition);
                previousPosition = newPosition;
            }

            return path;
        }
    }
}