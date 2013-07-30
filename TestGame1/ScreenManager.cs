using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame1
{
    public class ScreenManager
    {
		//THIS IS A SINGLETON
        #region Variables

		/// <summary>
		/// Creating custom content manager
		/// </summary>
		ContentManager content;

		GameScreen currentScreen;
		GameScreen newScreen; //screen at top of stack

        /// <summary>
        /// ScreenManger Instance
        /// </summary>
        private static ScreenManager instance;

        /// <summary>
        /// Screen Stack
        /// </summary>

        Stack<GameScreen> screenStack = new Stack<GameScreen>();

        /// <summary>
        /// Screens width and height
        /// </summary>
        Vector2 dimensions;

		bool transition;

		FadeAnimation fade = new FadeAnimation();
		Animation animation = new Animation();

		Texture2D fadeTexture;
		Texture2D nullImage;

		InputManager inputManager;

        #endregion

        #region Properties

        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ScreenManager();
                }
				return instance;
                
            }
        }

		public Vector2 Dimensions
		{
			get { return dimensions; }
			set { dimensions = value; }
		}

		public Texture2D NullImage
		{
			get { return nullImage; }
		}

		public ContentManager Content
		{
			get { return content; }
		}

        #endregion

		#region Main Methods

		public void AddScreen(GameScreen screen, InputManager inputManager)
		{
			transition = true; //lets us know we are transitioning
			fade.IsActive = true;
			fade.Alpha = 0.0f; //0 is fully transparent
			fade.ActivateValue = 1.0f; //1 is fully opaque
			fade.Increase = true;
			newScreen = screen; //here in case some other function deletes top screen from stack
			this.inputManager = inputManager;
		}

		public void AddScreen(GameScreen screen, InputManager inputManager, float alpha)
		{
			transition = true; //lets us know we are transitioning
			fade.IsActive = true;
			fade.ActivateValue = 1.0f; //1 is fully opaque
			newScreen = screen; //here in case some other function deletes top screen from stack
			this.inputManager = inputManager;

			if (alpha != 1.0f)
			{
				fade.Alpha = 1.0f - alpha;
			}
			else
			{
				fade.Alpha = alpha;
				fade.Increase = true;
			}
		}

		public void Initialize() 
		{
			currentScreen = new SplashScreen();
			fade = new FadeAnimation();
			inputManager = new InputManager();
		}
		public void LoadContent(ContentManager Content, InputManager inputManager)
		{
			content = new ContentManager(Content.ServiceProvider, "Content");
			currentScreen.LoadContent(content, inputManager);

			nullImage = this.content.Load<Texture2D>("null");
			fadeTexture = this.content.Load<Texture2D>("fade");
			animation.LoadContent(content, fadeTexture, "", Vector2.Zero);
			animation.Scale = dimensions.X;
		}
		public void Update(GameTime gameTime) 
		{
			if (!transition)
			{
				currentScreen.Update(gameTime);
			}
			else
			{
				Transition(gameTime);
			}
		}
		public void Draw(SpriteBatch spriteBatch) 
		{
			currentScreen.Draw(spriteBatch);
			if (transition)
			{
				animation.Draw(spriteBatch); //fade out what's on screen
			}
		}

		#endregion

		#region Private Methods

		private void Transition(GameTime gameTime)
		{
			fade.Update(gameTime, ref animation); //increase alpha until it equals 1
			if (fade.Alpha == 1.0f && fade.Timer.TotalSeconds == 1.0f) //put total seconds in here so we don't run more than once
			{
				screenStack.Push(newScreen);
				currentScreen.UnloadContent();
				currentScreen = newScreen;
				currentScreen.LoadContent(content, this.inputManager);
			}
			else if (fade.Alpha == 0.0f)
			{
				transition = false;
				fade.IsActive = false;
			}
		}

		#endregion
	}
}
