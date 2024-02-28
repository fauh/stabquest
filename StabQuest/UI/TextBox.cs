using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StabQuest.Helpers;
using System;
using System.Text;

namespace StabQuest.UI
{
    public class TextBox : GameComponent
    {

        private static GameWindow _gw;

        bool _hasFocus = false;
        private bool _textIsDirty;
        private Texture2D _texture;
        StringBuilder _displayCharacters = new StringBuilder();
        private Rectangle _rectangle;
        private SpriteFont _font;
        private GraphicsDevice _graphicsDevice;

        public TextBox(int x, int y, int width, int height, SpriteFont font, Texture2D texture, GraphicsDevice graphicsDevice, Game game) : this(new Rectangle(x, y, width, height), font, texture, graphicsDevice, game)
        {
        }

        public TextBox(Rectangle rectangle, SpriteFont font, Texture2D texture, GraphicsDevice graphicsDevice, Game game)
        {

            _gw = game.Window;
            _graphicsDevice = graphicsDevice;
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
            if (e.Key == Keys.Back)
            {
                if (_displayCharacters.Length > 0)
                {
                    _displayCharacters.Remove(_displayCharacters.Length - 1, 1);
                }
            }
            else if (e.Key == Keys.Enter)
            {
                _hasFocus = false;
                UnRegisterFocusedButtonForTextInput(OnInput);
            }
            else
            {
                Console.WriteLine(e.Character);
                var c = e.Character;

                _displayCharacters.Append(c);
                Console.WriteLine(_displayCharacters);
            }
        }

        public override void Update(GameTime gameTime)
        {
            MouseHelper.Update();

            if (_rectangle.Contains(MouseHelper.GetMousePosition()) && MouseHelper.LeftClicked)
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
            MouseHelper.Draw(gameTime, spriteBatch, _font);
            /*
            Add for debugging!
            spriteBatch.DrawString(_font, $"MouseCoords: X{MouseHelper.GetMousePosition().X} :Y{MouseHelper.GetMousePosition().Y}", new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(_font, $"Left Clicked: {_hasFocus}", new Vector2(0, 20), Color.White);
            */

            if (_hasFocus || Text.Length > 0)
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
