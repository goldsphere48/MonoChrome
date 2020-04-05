using Match_3.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoChrome.Core.GameObjectSystem;
using MonoChrome.Core.GameObjectSystem.Components;
using MonoChrome.Core.SceneSystem;
using MonoChrome.GameObjectSystem.Components.Attributes;

namespace Match_3
{
    public class Match3Game : Game
    {
        [CreatedFor(typeof(Obj1), Inherit = true)]
        class A : Component
        {

        }

        class Obj1 : GameObject
        {

        }

        class Obj3 : Obj1
        {

        }

        private GraphicsDeviceManager _graphics;
        public Match3Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Obj3 o = new Obj3();
            o.AddComponent<A>();
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
