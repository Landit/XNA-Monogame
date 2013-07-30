using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame1
{
	public class GameplayScreen : GameScreen
	{
		Player player;
		Map map;

		public override void LoadContent(ContentManager content, InputManager input)
		{
			base.LoadContent(content, input);
			player = new Player();
			map = new Map();

			map.LoadContent(content, map, "Map1");
			player.LoadContent(content, input);
			
		}

		public override void UnloadContent()
		{
			base.UnloadContent();
			player.UnloadContent();
			map.UnloadContent();
		}

		public override void Update(GameTime gameTime)
		{
			inputManager.Update();
			//player.Update(gameTime, inputManager, map.collision, map.layer);
			map.Update(gameTime); //trigger map update after player update
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			map.Draw(spriteBatch); //draw map before player so it doesn't overlap player
			player.Draw(spriteBatch);
		}
	}
}
