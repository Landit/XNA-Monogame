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
	public class MenuManager
	{
		#region Variables
		List<string> menuItems, animationTypes, linkType, linkID;
		List<List<string>> attributes, contents;
		List<Animation> tempAnimation;
		List<Animation> animation;
		List<Texture2D> menuImages;
		FileManager fileManager;
		ContentManager content;
		FadeAnimation fAnimation;
		SpriteSheetAnimation ssAnimation;

		Rectangle source;
		SpriteFont font;
		Vector2 position;
		int axis;
		int itemNumber;
		string align;
		#endregion

		#region Private Methods
		private void SetMenuItems()
		{
			for (int i = 0; i < menuItems.Count; i++)
			{
				if (menuImages.Count == i) //add a null image for every menu item
				{
					menuImages.Add(ScreenManager.Instance.NullImage);
				}
			}

			for (int i = 0; i < menuImages.Count; i++)
			{
				if (menuItems.Count == i)
				{
					menuItems.Add("");
				}
			}
		}

		private void SetAnimations()
		{
			Vector2 pos = Vector2.Zero;
			Vector2 dimensions = Vector2.Zero;

			if (align.Contains("Center"))
			{
				for (int i = 0; i < menuItems.Count; i++)
				{
					//calculate the total height/width of all menu items combined
					dimensions.X += font.MeasureString(menuItems[i]).X + menuImages[i].Width;
					dimensions.Y += font.MeasureString(menuItems[i]).Y + menuImages[i].Height;
				}

				if (axis == 1)
				{
					//take screen width - menu item width and divide by 2
					pos.X = (ScreenManager.Instance.Dimensions.X - dimensions.X) / 2; //make sure items are centered on screen
				}
				else if (axis == 2)
				{
					pos.Y = (ScreenManager.Instance.Dimensions.Y - dimensions.Y) / 2; //make sure items are centered on screen
				}
			}
			else
			{
				pos = position;
			}

			for (int i = 0; i < menuImages.Count; i++)
			{
				dimensions = new Vector2(font.MeasureString(menuItems[i]).X + menuImages[i].Width, font.MeasureString(menuItems[i]).Y + menuImages[i].Height);

				if (axis == 1)
				{
					pos.Y = (ScreenManager.Instance.Dimensions.Y - dimensions.Y) / 2;
				}
				else
				{
					pos.X = (ScreenManager.Instance.Dimensions.X - dimensions.X) / 2;
				}

				animation.Add(new Animation());
				animation[animation.Count - 1].LoadContent(content, menuImages[i], menuItems[i], pos);
				animation[animation.Count - 1].Font = font;

				if(axis == 1) //1 = horizontal, 2 = vertical
				{
					pos.X += dimensions.X; //x pos of menu items
				}
				else
				{
					pos.Y += dimensions.Y; //y pos of menu items
				}
			}
		}
		#endregion

		#region Public Methods
		public void LoadContent(ContentManager content, string id)
		{
			this.content = new ContentManager(content.ServiceProvider, "Content");
			menuItems = new List<string>();
			menuImages = new List<Texture2D>();
			animation = new List<Animation>();
			animationTypes = new List<string>();
			attributes = new List<List<string>>();
			contents = new List<List<string>>();
			linkType = new List<string>();
			linkID = new List<string>();
			position = Vector2.Zero;
			fileManager = new FileManager();
			fAnimation = new FadeAnimation();
			ssAnimation = new SpriteSheetAnimation();
			itemNumber = 0;


			fileManager.LoadContent("Load/Menus.txt", attributes, contents, id);
			for (int i = 0; i < attributes.Count; i++)
			{
				for (int j = 0; j < attributes[i].Count; j++)
				{
					switch (attributes[i][j])
					{
						case "Font":
							font = this.content.Load<SpriteFont>(contents[i][j]);
							break;
						case "Item":
							menuItems.Add(contents[i][j]);
							break;
						case "Image":
							menuImages.Add(this.content.Load<Texture2D>(contents[i][j]));
							break;
						case "Axis":
							axis = int.Parse(contents[i][j]);
							break;
						case "Position":
							string[] temp = contents[i][j].Split(' ');
							position = new Vector2(float.Parse(temp[0]), float.Parse(temp[1]));
							break;
						case "Source":
							temp = contents[i][j].Split(' ');
							source = new Rectangle(int.Parse(temp[0]), int.Parse(temp[1]), int.Parse(temp[2]), int.Parse(temp[3]));
							break;
						case "Animation":
							animationTypes.Add(contents[i][j]);
							break;
						case "Align":
							align = contents[i][j];
							break;
						case "LinkType":
							linkType.Add(contents[i][j]);
							break;
						case "LinkID":
							linkID.Add(contents[i][j]);
							break;
					}
				}
			}

			SetMenuItems();
			SetAnimations();
		}

		public void UnloadContent()
		{
			content.Unload();
			position = Vector2.Zero;
			fileManager = null;
			animation.Clear();
			menuItems.Clear();
			menuImages.Clear();
			animationTypes.Clear();
		}

		public void Update(GameTime gameTime, InputManager inputManager)
		{
			if (axis == 1) //axis is horizontal
			{
				if (inputManager.KeyPressed(Keys.Right, Keys.D))
				{
					itemNumber++; //item we are highlighting
				}
				else if (inputManager.KeyPressed(Keys.Left, Keys.A))
				{
					itemNumber--;
				}
			}
			else //axis is vertical
			{
				if (inputManager.KeyPressed(Keys.Down, Keys.S))
				{
					itemNumber++; //item we are highlighting
				}
				else if (inputManager.KeyPressed(Keys.Up, Keys.W))
				{
					itemNumber--;
				}
			}

			if (inputManager.KeyPressed(Keys.Enter, Keys.Z))
			{
				if (linkType[itemNumber] == "Screen")
				{
					//create type based on namespace and the linkID (from menus.txt)
					//thus this assigns Type to the class name established in the file manager txt
					Type newClass = Type.GetType("TestGame1." + linkID[itemNumber]);
					//cast classname as a game screen through activator and add that instance of game screen through screen manager
					ScreenManager.Instance.AddScreen((GameScreen)Activator.CreateInstance(newClass), inputManager);
				}
			}

			//can do menu item wrapping here depending on key strokes
			if (itemNumber < 0)
			{
				itemNumber = 0;
			}
			else if (itemNumber > menuItems.Count - 1)
			{
				itemNumber = menuItems.Count - 1;
			}

			for (int i = 0; i < animation.Count; i++)
			{
				for (int j = 0; j < animationTypes.Count; j++)
				{
					if (itemNumber == i)
					{
						animation[i].IsActive = true;
					}
					else
					{
						animation[i].IsActive = false;
					}

					Animation a = animation[i];

					switch (animationTypes[i])
					{
						case "Fade":
							fAnimation.Update(gameTime, ref a);
							break;
						case "SSheet":
							ssAnimation.Update(gameTime, ref a);
							break;
					}
					animation[i] = a;
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < animation.Count; i++)
			{
				animation[i].Draw(spriteBatch);
			}
		}
		#endregion
	}
}
