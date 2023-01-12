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

        public Tile(Vector2 position, Rectangle sourceRectangle, int size, bool walkable)
        {
            _position = position;
            _sourceRectangle = sourceRectangle;
            _size = size;
            _walkable = walkable;
        }

        public Vector2 Position { get { return _position; } }
        public Rectangle SourceRectangle { get { return _sourceRectangle; } }
        public int Size { get { return _size; } }

        public bool Walkable { get { return _walkable;} }

        public Vector2 WorldPosition { get { return new Vector2(_position.X * _size, _position.Y * _size); }  }
    }
}
