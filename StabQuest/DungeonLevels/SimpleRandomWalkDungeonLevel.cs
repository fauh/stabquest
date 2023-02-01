using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StabQuest.Helpers;
using System.Linq;


namespace StabQuest.DungeonLevels
{
    public class SimpleRandomWalkDungeonLevel : DungeonLevel
    {
        public SimpleRandomWalkDungeonLevel(int level, Texture2D texture) : base(level, texture)
        {
        }

        protected override void GenerateTiles(int level)
        {
            var positions = DungeonHelper.GetRandomWalkDungeon(new Vector2(24, 24 / 2), 10, 100 + 10 * level);

            // add floor tiles
            foreach (var position in positions)
            {
                var randomSourceRect = new Rectangle((7 + DiceHelper.RollDice(4)) * Game1.TILESIZE, DiceHelper.RollDiceZeroIndex(3) * Game1.TILESIZE, Game1.TILESIZE, Game1.TILESIZE);
                var tile = new Tile(position, randomSourceRect, Game1.TILESIZE, true, _texture, Game1.WORLDSCALE);
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
                            (int)CardinalDirections.DOWN => new Rectangle(DiceHelper.RollDice(3) * Game1.TILESIZE, 4 * Game1.TILESIZE, Game1.TILESIZE, Game1.TILESIZE),
                            (int)CardinalDirections.RIGHT => new Rectangle(5 * Game1.TILESIZE, DiceHelper.RollDice(3) * Game1.TILESIZE, Game1.TILESIZE, Game1.TILESIZE),
                            (int)CardinalDirections.UP => new Rectangle(DiceHelper.RollDice(3) * Game1.TILESIZE, 0, Game1.TILESIZE, Game1.TILESIZE),
                            (int)CardinalDirections.LEFT => new Rectangle(0, DiceHelper.RollDice(3) * Game1.TILESIZE, Game1.TILESIZE, Game1.TILESIZE),
                            _ => new Rectangle(9 * Game1.TILESIZE, 8 * Game1.TILESIZE, Game1.TILESIZE, Game1.TILESIZE)
                        }; ;

                        _tiles.Add(new Tile(neighbour, sourceRect, Game1.TILESIZE, false, _texture, Game1.WORLDSCALE));
                    }
                }
            }

            _entryPoint = positions.First();
            _exitPoint = positions.Last();
        }
    }
}
