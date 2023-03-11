using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Text;

namespace StabQuest.UI
{
    public class TextBox : GameComponent
    {

        private static GameWindow _gw;

        bool _hasFocus = true;
        private bool _textIsDirty;
        private Texture2D _texture;
        StringBuilder _displayCharacters = new StringBuilder();
        private Rectangle _rectangle;
        private SpriteFont _font;

        public TextBox(int x, int y, int width, int height, SpriteFont font, Texture2D texture, Game game) : this(new Rectangle(x, y, width, height), font, texture, game)
        {
        }

        public TextBox(Rectangle rectangle, SpriteFont font, Texture2D texture, Game game)
        {
            _gw = game.Window;
            _rectangle = rectangle;
            _hasFocus = false;
            _textIsDirty = false;
            _texture = texture;
            _font = font;
        }

        public string Text
        {
            get { return _displayCharacters.ToString(); }
            set { _displayCharacters = new StringBuilder(value.ToString()); }
        }

        public static void RegisterFocusedButtonForTextInput(System.EventHandler<TextInputEventArgs> method)
        {
            _gw.TextInput += method;
        }
        public static void UnRegisterFocusedButtonForTextInput(System.EventHandler<TextInputEventArgs> method)
        {
            _gw.TextInput -= method;
        }

        public void OnInput(object sender, TextInputEventArgs e)
        {
            var c = e.Character;
            _displayCharacters.Append(c);
            Console.WriteLine(_displayCharacters);
        }

        public override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var isClicked = mouseState.LeftButton == ButtonState.Pressed;
            if (_rectangle.Contains(mouseState.Position) && isClicked)
            {
                _hasFocus = !_hasFocus;

                if (_hasFocus)
                {
                    if (_textIsDirty)
                    {
                        Text = "";
                        _textIsDirty = true;
                    }
                    RegisterFocusedButtonForTextInput(OnInput);
                }
                else
                {
                    UnRegisterFocusedButtonForTextInput(OnInput);
                }
            }
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_hasFocus)
            {
                spriteBatch.Draw(_texture, new Vector2(_rectangle.X, _rectangle.Y), Color.Wheat);
                spriteBatch.DrawString(_font, Text, new Vector2(_rectangle.X, _rectangle.Y), Color.Black);
            }
            else
            {
                spriteBatch.Draw(_texture, new Vector2(_rectangle.X, _rectangle.Y), Color.LightGray);
                spriteBatch.DrawString(_font, Text, new Vector2(_rectangle.X, _rectangle.Y), Color.DarkGray);
            }
        }
    }
}
