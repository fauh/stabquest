using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StabQuest.UI
{
    public class Button : GameComponent
    {
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

        private SpriteFont _font;
        private bool _isHovering;
        private Texture2D _texture;

        private Vector2 _position;


        public event EventHandler Click;
        public bool Clicked { get; private set; }
        public Vector2 Position { get => _position; set => _position = value; }

        public Rectangle Rectangle{ get 
            {
                return new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
            } 
        }
        public bool IsHovering { get => _isHovering; private set => _isHovering = value; }

        public string Text { get; set; }

        public Button(Vector2 position, Texture2D texture, SpriteFont font) {
            _position = position;
            _texture = texture;
            _font = font;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            var color = Color.White;

            if (_isHovering) {
                color = Color.Yellow;
            }

            spriteBatch.Draw(_texture, Rectangle, color);

            if(!string.IsNullOrEmpty(Text))
            {
                var x = Rectangle.X + Rectangle.Width / 2 - (_font.MeasureString(Text).X / 2);
                var y = Rectangle.Y + Rectangle.Height / 2 - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), Color.Black);
            }
        }

        public override void Update(GameTime gameTime)
        {
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
            _isHovering = false;

            var mouseRectangle = new Rectangle(_currentMouseState.X, _currentMouseState.Y, 1, 1);

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;

                if (_currentMouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }
    }
}
