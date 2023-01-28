using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StabQuest
{
    public class Tile
    {
        private Vector2 _position;
        private Rectangle _sourceRectangle;
        private int _size;
        private bool _walkable;
        private Texture2D _texture;
        private int _worldScale;

        public Tile(Vector2 position, Rectangle sourceRectangle, int size, bool walkable, Texture2D texture, int worldScale)
        {
            _position = position;
            _sourceRectangle = sourceRectangle;
            _size = size;
            _walkable = walkable;
            _texture = texture;
            _worldScale = worldScale;
        }

        public Vector2 Position { get { return _position; } }
        public Rectangle SourceRectangle { get { return _sourceRectangle; } }
        public int Size { get { return _size; } }

        public bool Walkable { get { return _walkable;} }

        public Vector2 WorldPosition { get { return new Vector2(_position.X * _size, _position.Y * _size); }  }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Draw(_texture, this.WorldPosition, this.SourceRectangle, Color.White, 0, Vector2.One, scale: _worldScale, SpriteEffects.None, 0);
        }
    }
}
