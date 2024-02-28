using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StabQuest.Helpers
{
    public static class MouseHelper
    {

        private static MouseState previousState;
        private static MouseState currentState;
        public static bool LeftClicked { get; private set; }
        public static bool RightClicked { get; private set; }

        public static Vector2 GetMousePosition()
        {
            return new Vector2(currentState.X, currentState.Y);
        }

        public static void Update()
        {
            previousState = currentState;
            currentState = Mouse.GetState();

            LeftClicked = currentState.LeftButton != ButtonState.Pressed && previousState.LeftButton == ButtonState.Pressed;
            RightClicked = currentState.RightButton != ButtonState.Pressed && previousState.RightButton == ButtonState.Pressed;


        }

        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont font)
        {

            spriteBatch.DrawString(font, $"MouseCoords: X{MouseHelper.GetMousePosition().X} :Y{MouseHelper.GetMousePosition().Y}", new Vector2(0, 0), Color.White);
        }
    }
}
