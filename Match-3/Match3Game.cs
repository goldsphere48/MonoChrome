using Match_3.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core.SceneSystem;

namespace Match_3
{
    public class Match3Game : Game
    {
        private GraphicsDeviceManager _graphics;
        public Match3Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            SceneManager.Instance.GraphicsDevice = GraphicsDevice;
            SceneManager.Instance.Register<MainMenuScene>();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SceneManager.Instance.LoadScene<MainMenuScene>();
            SceneManager.Instance.SetActiveScene<MainMenuScene>();
        }

        protected override void UnloadContent()
        {
            SceneManager.Instance.UnloadAll();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            SceneManager.Instance.UpdateActiveScreen();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
            SceneManager.Instance.DrawActiveScreen();
        }
    }
}
