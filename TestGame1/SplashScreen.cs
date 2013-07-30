using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestGame1
{
    public class SplashScreen : GameScreen
    {
		SpriteFont font;
		List<Animation> animation;
		List<Texture2D> images;
		//can add audio here

		FadeAnimation fAnimation;
		FileManager fileManager;

		int imageNumber;

		public override void LoadContent(ContentManager Content, InputManager inputManager)
		{
			base.LoadContent(Content, inputManager);
			if (font == null)
			{
				font = this.content.Load<SpriteFont>("Font1");
			}

			imageNumber = 0;
			fileManager = new FileManager();
			animation = new List<Animation>();
			fAnimation = new FadeAnimation();
			images = new List<Texture2D>();

			fileManager.LoadContent("Load/Splash.txt", attributes, contents);

			for (int i = 0; i < attributes.Count; i++)
			{
				for (int j = 0; j < attributes[i].Count; j++)
				{
					switch(attributes[i][j])
					{
						default:
						case "Image": 
							images.Add(this.content.Load<Texture2D>("SplashScreen/" + contents[i][j]));
							animation.Add(new FadeAnimation());
							break;
						case "Sound": break;
					}
				}
			}

			for (int i = 0; i < animation.Count; i++)
			{
				animation[i].LoadContent(content, images[i], "", Vector2.Zero);
				//in case image is different size than viewport
				// ImageWidth / 2 * scale - (imageWidth /2)
				// ImageHeight / 2 * scale - (imageHeight / 2)
				animation[i].Scale = 1f; //can scale images here in splash screen
				animation[i].IsActive = true;
			}
		}

		public override void UnloadContent()
		{
			base.UnloadContent();
			fileManager = null;
		}

		public override void Update(GameTime gameTime)
		{
			inputManager.Update();

			Animation a = animation[imageNumber];
			fAnimation.Update(gameTime, ref a);
			animation[imageNumber] = a;

			if (animation[imageNumber].Alpha == 0.0f)
			{
				imageNumber++;
			}

			if (imageNumber >= animation.Count - 1 || inputManager.KeyPressed(Keys.Z))
			{
				if (animation[imageNumber].Alpha != 1.0f)
				{
					ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager, animation[imageNumber].Alpha);
				}
				else
				{
					ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager);
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			//spriteBatch.DrawString(font, "SplashScreen", new Vector2(100, 100), Color.Black);

			animation[imageNumber].Draw(spriteBatch);
		}
    }
}
