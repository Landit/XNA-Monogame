﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace TestGame1
{
	public class InputManager
	{
		KeyboardState prevKeyState, keyState;

		public KeyboardState PrevKeyState
		{
			get { return prevKeyState; }
			set { prevKeyState = value; }
		}

		public KeyboardState KeyState
		{
			get { return keyState; }
			set { keyState = value; }
		}

		public void Update()
		{
			prevKeyState = keyState;
			keyState = Keyboard.GetState();
		}

		public bool KeyPressed(Keys key) //checks for single key presses (not holding down)
		{
			if (keyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool KeyPressed(params Keys[] keys)
		{
			foreach (Keys key in keys)
			{
				if (keyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key))
				{
					return true;
				}
			}
			return false;
		}

		public bool KeyReleased(Keys key) //checks for single key presses (not holding down)
		{
			if (keyState.IsKeyUp(key) && prevKeyState.IsKeyDown(key))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool KeyReleased(params Keys[] keys)
		{
			foreach (Keys key in keys)
			{
				if (keyState.IsKeyUp(key) && prevKeyState.IsKeyDown(key))
				{
					return true;
				}
			}
			return false;
		}

		public bool KeyDown(Keys key)
		{
			if (keyState.IsKeyDown(key))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool KeyDown(params Keys[] keys)
		{
			foreach (Keys key in keys)
			{
				if (keyState.IsKeyDown(key))
				{
					return true;
				}
			}
			return false;
		}
	}
}
