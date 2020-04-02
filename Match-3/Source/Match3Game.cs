using Match_3.Source.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
            SceneManager.Instance.Register(
                new MainMenuScene(
                    new SceneId(
                        nameof(MainMenuScene)
                    )
                )
            );
            SceneManager.Instance.Register(
                null
            );
            SceneManager.Instance.Register(
                new GameScene(
                    new SceneId(
                        nameof(GameScene)
                    )
                )
            );
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SceneManager.Instance.LoadScene(new SceneId(nameof(MainMenuScene)));
            SceneManager.Instance.SetActiveScene(new SceneId(nameof(MainMenuScene)));
        }

        protected override void UnloadContent()
        {
            SceneManager.Instance.UnloadAll();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            SceneManager.Instance.UpdateActiveScreen(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
            SceneManager.Instance.DrawActiveScreen(gameTime);
        }
    }
}
