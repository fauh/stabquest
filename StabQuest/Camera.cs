using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StabQuest
{
    public class Camera
    {
        public Camera(int tileSize, int screenHeight, int screenWidth) {
            _tilesize = tileSize;
            _screenHeight = screenHeight;
            _screenWidth = screenWidth;
        }

        private int _tilesize;
        private int _screenHeight;
        private int _screenWidth;

        public Matrix Transform { get; private set; }

        public void Follow(Vector2 target) {
            
            var position = Matrix.CreateTranslation(
                -target.X - (_tilesize / 2),
               - target.Y - (_tilesize / 2),
                0) 
                * Matrix.CreateTranslation(
                _screenWidth / 2, 
                _screenHeight / 2,
                0);

            Transform = position;

        }
    }
}
