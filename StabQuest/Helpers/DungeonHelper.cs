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

                currentPosition = RollDice(6) switch
                {
                    1 => PickRandomPositionFromPath(path),
                    2 => PickRandomPositionFromPath(currentDungeon),
                    3 => GetPathStartPosition(path),
                    4 => GetPathEndPosition(path),
                    5 => GetPathStartPosition(path),
                    6 => GetPathEndPosition(path),
                    _ => currentPosition
                };
            }

            return currentDungeon;
        }

        private static Vector2 GetPathEndPosition(HashSet<Vector2> path)
        {
            return path.ElementAt(path.Count() - 1);
        }

        private static Vector2 GetPathStartPosition(HashSet<Vector2> path)
        {
            return path.ElementAt(0);
        }

        private static Vector2 PickRandomPositionFromPath(HashSet<Vector2> path)
        {
            return path.ElementAt(RollDiceZeroIndex(path.Count()));
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