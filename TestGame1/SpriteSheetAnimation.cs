using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace TestGame1
{
	public class SpriteSheetAnimation : Animation
	{
		private int frameCounter;
		private int switchFrame;
		Vector2 currentFrame;

		public SpriteSheetAnimation()
		{
			frameCounter = 0;
			switchFrame = 100;
		}

		public override void Update(GameTime gameTime, ref Animation a)
		{
			currentFrame = a.CurrentFrame;
			if (a.IsActive) //isActive is set when player presses down input key to move for example
			{
				frameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
				if (frameCounter >= switchFrame) //every 100 milliseconds switch frame
				{
					frameCounter = 0;
					currentFrame.X++; //if animations are positioned in image according to x axis - then switch to next frame

					if (currentFrame.X * FrameWidth >= a.Image.Width)
					{
						currentFrame.X = 0;
					}
				}
			}
			else
			{
				frameCounter = 0;
				currentFrame.X = 1;
			}
			a.CurrentFrame = currentFrame;
			a.SourceRect = new Rectangle((int)currentFrame.X * FrameWidth, (int)currentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
		}
	}
}
