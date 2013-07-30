using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame1
{
	/// <summary>
	/// Entity is the base class for Player/Enemy or any other entity class
	/// </summary>
	public class Entity
	{
		protected int health;
		protected Animation moveAnimation;
		protected float moveSpeed;
		protected SpriteSheetAnimation ssAnimation;

		protected ContentManager content;
		protected FileManager fileManager;
		protected Texture2D image;
		protected Vector2 position;
		protected List<List<string>> attributes, contents;

		public virtual void LoadContent(ContentManager content, InputManager input)
		{
			//make new instance instead of doing this.content = content because when we unload this content manager instance
			//we don't want to unload everything else that could been loaded in with that content manager instance
			this.content = new ContentManager(content.ServiceProvider, "Content");
			attributes = new List<List<string>>();
			contents = new List<List<string>>();
		}

		public virtual void UnloadContent()
		{
			content.Unload();
		}

		public virtual void Update(GameTime gameTime, InputManager inputManager, Collision col, Layers layer)
		{

		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{

		}
	}
}
