using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonoChrome.Core.Exceptions;
using MonoChrome.Core.SceneSystem;

namespace MonoChrome.Tests
{
    [TestClass]
    public class SceneManagerTests
    {
        class SceneA : Scene
        {

        }

        [TestMethod]
        public void UnloadAll_SceneManagerIsEmpty_WithoutExceptions()
        {
            try
            {
                SceneManager.Instance.UnloadAll();
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Register_2SimullarScenes_ThrowSceneAlreadyExistException()
        {
            SceneManager.Instance.UnloadAll();
            Assert.ThrowsException<SceneAlreadyExistException>(() => {
                SceneManager.Instance.Register<SceneA>();
                SceneManager.Instance.Register<SceneA>();
            });
        }

        [TestMethod]
        public void Unload_NotExistingScene_ThrowSceneAlreadyExistException()
        {
            SceneManager.Instance.UnloadAll();
            Assert.ThrowsException<SceneNotFoundException>(() => {
                SceneManager.Instance.UnloadScene<SceneA>();
            });
        }

        [TestMethod]
        public void Unload_ExistingScene_WithoutExceptions()
        {
            SceneManager.Instance.UnloadAll();
            SceneManager.Instance.Register<SceneA>();
            try
            {
                SceneManager.Instance.UnloadScene<SceneA>();
            } catch(Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Load_NotExistingScene_ThrowSceneAlreadyExistException()
        {
            SceneManager.Instance.UnloadAll();
            Assert.ThrowsException<SceneNotFoundException>(() => {
                SceneManager.Instance.LoadScene<SceneA>();
            });
        }

        [TestMethod]
        public void Load_ExistingScene_WithoutExceptions()
        {
            SceneManager.Instance.UnloadAll();
            SceneManager.Instance.Register<SceneA>();
            try
            {
                SceneManager.Instance.LoadScene<SceneA>();
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void SetActive_NotExistingScene_ThrowSceneAlreadyExistException()
        {
            SceneManager.Instance.UnloadAll();
            Assert.ThrowsException<SceneNotFoundException>(() => {
                SceneManager.Instance.SetActiveScene<SceneA>();
            });
        }

        [TestMethod]
        public void SetActive_ExistingButNotInitializedScene_ThrowSceneNotInitializedException()
        {
            SceneManager.Instance.UnloadAll();
            SceneManager.Instance.Register<SceneA>();
            Assert.ThrowsException<SceneNotInitializedException>(() =>
            {
                SceneManager.Instance.SetActiveScene<SceneA>();
            });
        }

        [TestMethod]
        public void SetActive_ExistingInitializedScene_WithoutExceptions()
        {
            SceneManager.Instance.UnloadAll();
            SceneManager.Instance.Register<SceneA>();
            SceneManager.Instance.LoadScene<SceneA>();
            try
            {
                SceneManager.Instance.SetActiveScene<SceneA>();
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
    }
}
