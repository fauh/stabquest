using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StabQuest.DiceHelper;

namespace StabQuest
{
    public class SimpleRandomWalkDungeonLevel     {


        private int _level;
        private HashSet<Tile> _tiles;
        private Texture2D _texture;
        private int _tileSize;
        private Vector2 _doorPositionStart;
        private Vector2 _doorPositionEnd;

        public SimpleRandomWalkDungeonLevel(int level, Texture2D texture, int tileSize)
        {
            _level = level;
            _texture = texture;
            _tileSize = tileSize;
            _tiles = new HashSet<Tile>();
            GenerateTiles(level);
        }

        public Texture2D Texture { get => _texture; }
        public HashSet<Tile> Tiles { get => _tiles; }
        public int Level { get => _level; }
        public Vector2 DoorPositionStart { get => _doorPositionStart; }
        public Vector2 DoorPositionEnd { get => _doorPositionEnd; }

        public Vector2 FindEntry()
        {
            foreach(var direction in Direction2D.Directions)
            {
                var tryPos = DoorPositionStart + direction;
                if (Tiles.Any(t => t.Position == tryPos && t.Walkable)) { 
                    return tryPos;
                }
            }

            return DoorPositionStart;
        }

        public Vector2 FindExit()
        {
            foreach (var direction in Direction2D.Directions)
            {
                var tryPos = DoorPositionEnd + direction;
                if (Tiles.Any(t => t.Position == tryPos && t.Walkable))
                {
                    return tryPos;
                }
            }

            return DoorPositionEnd;
        }

        private void GenerateTiles(int level)
        {
            var positions = DungeonHelper.GetRandomWalkDungeon(new Vector2(24, 24 / 2), 10, 100 + (10* level));

            // add floor tiles
            foreach (var position in positions)
            {
                var randomSourceRect = new Rectangle((7 + RollDice(4)) * _tileSize, RollDiceZeroIndex(3) * _tileSize, _tileSize, _tileSize);
                var tile = new Tile(position, randomSourceRect, _tileSize, true);
                _tiles.Add(tile);
            }


            // add walls!
            foreach (var position in positions)
            {
                for (int i = 0; i < Direction2D.CardinalDirections.Count; i++)
                { // normal for loop here because we want the index of the direction
                    var neighbour = position + Direction2D.CardinalDirections[i];
                    if (!positions.Contains(neighbour))
                    {

                        //TODO: Handle corners also

                        var sourceRect = i switch
                        {
                            (int)CardinalDirections.DOWN => new Rectangle(RollDice(3) * _tileSize, 4 * _tileSize, _tileSize, _tileSize),
                            (int)CardinalDirections.RIGHT => new Rectangle(5 * _tileSize, RollDice(3) * _tileSize, _tileSize, _tileSize),
                            (int)CardinalDirections.UP => new Rectangle(RollDice(3) * _tileSize, 0, _tileSize, _tileSize),
                            (int)CardinalDirections.LEFT => new Rectangle(0, RollDice(3) * _tileSize, _tileSize, _tileSize),
                            _ => new Rectangle(9 * _tileSize, 8 * _tileSize, _tileSize, _tileSize)
                        }; ;

                        _tiles.Add(new Tile(neighbour, sourceRect, _tileSize, false));
                    }
                }
            }

            _doorPositionStart = positions.First();
            _doorPositionEnd = positions.Last();
        }
    }
}
