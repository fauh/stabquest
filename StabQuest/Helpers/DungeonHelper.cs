using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using static StabQuest.Helpers.DiceHelper;

namespace StabQuest.Helpers
{
    public static class Direction2D
    {
        private static List<Vector2> _cardinalDirections = new List<Vector2>() { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) };
        private static List<Vector2> _diagonalDirections = new List<Vector2>() { new Vector2(-1, 1), new Vector2(1, 1), new Vector2(-1, -1), new Vector2(1, -1) };
        public static List<Vector2> CardinalDirections { get { return _cardinalDirections; } }
        public static List<Vector2> DiagonalDirections { get { return _diagonalDirections; } }
        public static List<Vector2> Directions
        {
            get
            {
                var newList = new List<Vector2>();
                for (int i = 0; i < _cardinalDirections.Count; i++)
                {
                    newList.Add(_cardinalDirections[i]);
                    newList.Add(_diagonalDirections[i]);
                }
                return newList;
            }
        }

        public static Vector2 Get(CardinalDirections direction)
        {
            return _cardinalDirections.ElementAt((int)direction);
        }

        public static Vector2 Get(DiagonalDirections direction)
        {
            return _diagonalDirections.ElementAt((int)direction);
        }

        public static Vector2 GetDiagonalDirection(int index)
        {
            return _diagonalDirections.ElementAt(index);
        }


        public static Vector2 GetRandomCardinalDirection()
        {
            return GetCardinalDirection(RollDiceZeroIndex(4));
        }

        public static Vector2 GetCardinalDirection(int index)
        {
            return _cardinalDirections.ElementAt(index);
        }


        public static Vector2 GetRandomDiagonalDirection()
        {
            return GetDiagonalDirection(RollDiceZeroIndex(4));
        }
    }

    public enum CardinalDirections
    {
        DOWN, RIGHT, UP, LEFT
    }

    public enum DiagonalDirections
    {
        DOWN_LEFT, DOWN_RIGHT, UP_LEFT, UP_RIGHT
    }

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