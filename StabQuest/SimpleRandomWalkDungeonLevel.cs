﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StabQuest.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static StabQuest.Helpers.DiceHelper;

namespace StabQuest
{
    public class SimpleRandomWalkDungeonLevel : GameComponent
    {


        private int _level;
        private HashSet<Tile> _tiles;
        private Texture2D _texture;
        private int _tileSize;
        private Vector2 _doorPositionStart;
        private Vector2 _doorPositionEnd;
        private int _worldscale;

        public SimpleRandomWalkDungeonLevel(int level, Texture2D texture, int tileSize, int worldscale)
        {
            _level = level;
            _texture = texture;
            _tileSize = tileSize;
            _tiles = new HashSet<Tile>();
            _worldscale = worldscale;
            GenerateTiles(level);
        }

        public Texture2D Texture { get => _texture; }
        public HashSet<Tile> Tiles { get => _tiles; }
        public int Level { get => _level; }
        public Vector2 DoorPositionStart { get => _doorPositionStart; }
        public Vector2 DoorPositionEnd { get => _doorPositionEnd; }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach(var tile in _tiles)
            {
                tile.Draw(gameTime, spriteBatch);
            }
            spriteBatch.Draw(this.Texture, new Vector2(Game1.TILESIZE * this.DoorPositionStart.X, Game1.TILESIZE * this.DoorPositionStart.Y),
            new Rectangle(8 * Game1.TILESIZE, 3 * Game1.TILESIZE, Game1.TILESIZE, Game1.TILESIZE), Color.Green, 0, Vector2.One, scale: Game1.WORLDSCALE, SpriteEffects.None, 0);
            spriteBatch.Draw(this.Texture, new Vector2(Game1.TILESIZE * this.DoorPositionEnd.X, Game1.TILESIZE * this.DoorPositionEnd.Y),
            new Rectangle(8 * Game1.TILESIZE, 3 * Game1.TILESIZE, Game1.TILESIZE, Game1.TILESIZE), Color.Red, 0, Vector2.One, scale: Game1.WORLDSCALE, SpriteEffects.None, 0);
        }
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

        public override void Update(GameTime gameTime)
        {
            // DO NOTHING?
        }

        private void GenerateTiles(int level)
        {
            var positions = DungeonHelper.GetRandomWalkDungeon(new Vector2(24, 24 / 2), 10, 100 + (10* level));

            // add floor tiles
            foreach (var position in positions)
            {
                var randomSourceRect = new Rectangle((7 + RollDice(4)) * _tileSize, RollDiceZeroIndex(3) * _tileSize, _tileSize, _tileSize);
                var tile = new Tile(position, randomSourceRect, _tileSize, true, _texture, _worldscale);
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

                        _tiles.Add(new Tile(neighbour, sourceRect, _tileSize, false, _texture, _worldscale));
                    }
                }
            }

            _doorPositionStart = positions.First();
            _doorPositionEnd = positions.Last();
        }
    }
}
