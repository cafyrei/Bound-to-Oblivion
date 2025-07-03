using Microsoft.Xna.Framework.Input;

namespace Oblivion
{
    internal class Input
    {

        private KeyboardState currentKeyState;
        private KeyboardState prevKeyState;
        public Input()
        {
            currentKeyState = Keyboard.GetState();
            prevKeyState = currentKeyState;
        }

        public void Update()
        {
            prevKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();
        }

        public bool IsKeyDown(Keys keys)
        {
            return currentKeyState.IsKeyDown(keys);
        }

        public bool IsKeyUp(Keys keys)
        {
            return currentKeyState.IsKeyUp(keys);
        }


    }
}