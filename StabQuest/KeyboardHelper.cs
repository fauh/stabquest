﻿using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StabQuest
{
    public static class KeyboardHelper {

        private static KeyboardState currentState;
        private static KeyboardState previousState;

        public static KeyboardState GetState() {
            previousState = currentState;
            currentState = Keyboard.GetState();
            return currentState;
        }

        public static bool CheckKey(Keys key) { 
            return currentState.IsKeyDown(key);
        }

        public static bool CheckKeyPress(Keys key) { 
            return CheckKey(key) && !previousState.IsKeyDown(key);
        }

        public static bool CheckKeyRelease(Keys key)
        {
            return currentState.IsKeyUp(key);
        }

    }
}
