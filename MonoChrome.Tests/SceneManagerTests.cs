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
        public void Unload_Empty_SceneManager()
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
        public void Register_2_Simullar_Scenes_Throw_SceneAlreadyExistException()
        {
            SceneManager.Instance.UnloadAll();
            Assert.ThrowsException<SceneAlreadyExistException>(() => {
                SceneManager.Instance.Register<SceneA>();
                SceneManager.Instance.Register<SceneA>();
            });
        }

        [TestMethod]
        public void Register_Scene_After_SceneManagerUnload_Correct()
        {
            SceneManager.Instance.UnloadAll();
            try
            {
                SceneManager.Instance.Register<SceneA>();
            } catch(SceneAlreadyExistException e)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Unload_Not_Existing_Scene_Throw_SceneAlreadyExistException()
        {
            SceneManager.Instance.UnloadAll();
            Assert.ThrowsException<SceneNotFoundException>(() => {
                SceneManager.Instance.UnloadScene<SceneA>();
            });
        }

        [TestMethod]
        public void Unload_Existing_Scene_Throw_SceneAlreadyExistException()
        {
            SceneManager.Instance.UnloadAll();
            SceneManager.Instance.Register<SceneA>();
            try
            {
                SceneManager.Instance.UnloadScene<SceneA>();
            } catch(SceneNotFoundException e)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Load_Not_Existing_Scene_Throw_SceneAlreadyExistException()
        {
            SceneManager.Instance.UnloadAll();
            Assert.ThrowsException<SceneNotFoundException>(() => {
                SceneManager.Instance.LoadScene<SceneA>();
            });
        }

        [TestMethod]
        public void Load_Existing_Scene_Throw_SceneAlreadyExistException()
        {
            SceneManager.Instance.UnloadAll();
            SceneManager.Instance.Register<SceneA>();
            try
            {
                SceneManager.Instance.LoadScene<SceneA>();
            }
            catch (SceneNotFoundException e)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void SetActive_Not_Existing_Scene_Throw_SceneAlreadyExistException()
        {
            SceneManager.Instance.UnloadAll();
            Assert.ThrowsException<SceneNotFoundException>(() => {
                SceneManager.Instance.SetActiveScene<SceneA>();
            });
        }

        [TestMethod]
        public void SetActive_Existing_But_Not_Initialized_Scene_Throw_SceneAlreadyExistException()
        {
            SceneManager.Instance.UnloadAll();
            SceneManager.Instance.Register<SceneA>();
            Assert.ThrowsException<SceneNotInitializedException>(() =>
            {
                SceneManager.Instance.SetActiveScene<SceneA>();
            });
        }

        [TestMethod]
        public void SetActive_Existing_Initialized_Scene_Throw_SceneAlreadyExistException()
        {
            SceneManager.Instance.UnloadAll();
            SceneManager.Instance.Register<SceneA>();
            SceneManager.Instance.LoadScene<SceneA>();
            try
            {
                SceneManager.Instance.SetActiveScene<SceneA>();
            }
            catch (SceneNotInitializedException e)
            {
                Assert.Fail();
            }
        }
    }
}
