using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame1
{
	class FadeAnimation : Animation
	{
		bool increase;
		float fadeSpeed;
		TimeSpan defaultTime, timer;
		float activateValue;
		bool stopUpdating;
		float defaultAlpha;

		public TimeSpan Timer
		{
			set { defaultTime = value; timer = defaultTime; }
			get { return timer; }	
		}

		public float FadeSpeed
		{
			set { fadeSpeed = value; }
			get { return fadeSpeed; }
		}

		public float ActivateValue
		{
			set { activateValue = value; }
			get { return activateValue; }			
		}

		public bool Increase
		{
			get { return increase; }
			set { increase = value; }
		}

		public float DefaultAlpha
		{
			set { defaultAlpha = value; }
		}

		public FadeAnimation()
		{
			increase = false;
			fadeSpeed = 1.0f;
			defaultTime = new TimeSpan(0, 0, 1);
			timer = defaultTime;
			activateValue = 0.0f;
			stopUpdating = false;
			defaultAlpha = 1.0f;
		}

		public override void Update(GameTime gameTime, ref Animation a)
		{
			if (a.IsActive)
			{
				if (!stopUpdating)
				{
					if (!increase)
					{
						a.Alpha -= fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
					}
					else
					{
						a.Alpha += fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
					}
					if (a.Alpha <= 0.0f) //alpha is from 0 to 1 instead of 0 to 255
					{
						a.Alpha = 0.0f;
						increase = true;
					}
					else if (a.Alpha >= 1.0f) //1 is opaque and 0 is transparent
					{
						a.Alpha = 1.0f;
						increase = false;
					}
				}

				if (a.Alpha == activateValue)
				{
					stopUpdating = true;
					timer -= gameTime.ElapsedGameTime;
					if (timer.TotalSeconds <= 0) //delay for fading right in after fading out (nicer transition)
					{
						timer = defaultTime;
						stopUpdating = false;
					}
				}
			}
			else
			{
				a.Alpha = defaultAlpha; //if fading isn't active
				stopUpdating = false;
			}
		}
	}
}
