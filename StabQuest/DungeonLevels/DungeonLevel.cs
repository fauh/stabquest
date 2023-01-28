using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StabQuest.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StabQuest.DungeonLevels
{
    public abstract class DungeonLevel : GameComponent
    {
        protected int _level;
        protected HashSet<Tile> _tiles;
        protected Texture2D _texture;
        protected Vector2 _entryPoint;
        protected Vector2 _exitPoint;

        public DungeonLevel(int level, Texture2D texture)
        {
            _level = level;
            _texture = texture;
            _tiles = new HashSet<Tile>();
            GenerateTiles(level);
        }

        protected abstract void GenerateTiles(int level);

        public Texture2D Texture { get => _texture; }
        public HashSet<Tile> Tiles { get => _tiles; }
        public int Level { get => _level; }
        public Vector2 EntryPoint { get => _entryPoint; }
        public Vector2 ExitPoint { get => _exitPoint; }

        public Vector2 FindEntry()
        {
            foreach (var direction in Direction2D.Directions)
            {
                var tryPos = EntryPoint + direction;
                if (Tiles.Any(t => t.Position == tryPos && t.Walkable))
                {
                    return tryPos;
                }
            }
            return EntryPoint;
        }

        public Vector2 FindExit()
        {
            foreach (var direction in Direction2D.Directions)
            {
                var tryPos = ExitPoint + direction;
                if (Tiles.Any(t => t.Position == tryPos && t.Walkable))
                {
                    return tryPos;
                }
            }

            return ExitPoint;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var tile in _tiles)
            {
                tile.Draw(gameTime, spriteBatch);
            }
            spriteBatch.Draw(Texture, new Vector2(Game1.TILESIZE * EntryPoint.X, Game1.TILESIZE * EntryPoint.Y),
            new Rectangle(8 * Game1.TILESIZE, 3 * Game1.TILESIZE, Game1.TILESIZE, Game1.TILESIZE), Color.Green, 0, Vector2.One, scale: Game1.WORLDSCALE, SpriteEffects.None, 0);
            spriteBatch.Draw(Texture, new Vector2(Game1.TILESIZE * ExitPoint.X, Game1.TILESIZE * ExitPoint.Y),
            new Rectangle(8 * Game1.TILESIZE, 3 * Game1.TILESIZE, Game1.TILESIZE, Game1.TILESIZE), Color.Red, 0, Vector2.One, scale: Game1.WORLDSCALE, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
