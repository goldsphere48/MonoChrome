using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonoChrome.Core.GameObjectSystem;

namespace MonoChrome.Tests
{
    [TestClass]
    public class GroupTest
    {
        [TestMethod]
        public void Add_Add_1_GameObjectToGroup_ChildrenCountEquals_1()
        {
            Group g = new Group();
            GameObject go = new GameObject();
            g.Add(go);
            Assert.AreEqual(1, g.Count);
        }

        [TestMethod]
        public void Remove_Remove_1_GameObjectFromGroupWithThisObject_ChildrenCountEquals_0()
        {
            Group g = new Group();
            GameObject go = new GameObject();
            g.Add(go);
            g.Remove(go);
            Assert.AreEqual(0, g.Count);
        }

        [TestMethod]
        public void Remove_Remove_1_GameObjectFromGroupWithThisObject_ReturnTrue()
        {
            Group g = new Group();
            GameObject go = new GameObject();
            g.Add(go);
            Assert.AreEqual(true, g.Remove(go));
        }

        [TestMethod]
        public void Remove_Remove_1_GameObjectFromGroupWithOutThisObject_ReturnFalse()
        {
            Group g = new Group();
            GameObject go = new GameObject();
            Assert.AreEqual(false, g.Remove(go));
        }

        [TestMethod]
        public void Add_AddGameObjectToGroup_GameObjectParentNotNull()
        {
            Group g = new Group();
            GameObject go = new GameObject();
            g.Add(go);
            Assert.AreNotEqual(go.Transform.Parent, null);
        }

        [TestMethod]
        public void Remove_RemoveGameObjectFromGroup_GameObjectParentNull()
        {
            Group g = new Group();
            GameObject go = new GameObject();
            g.Add(go);
            g.Remove(go);
            Assert.AreEqual(go.Transform.Parent, null);
        }

        [TestMethod]
        public void Add_AddNullToGroup_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                Group g = new Group();
                g.Add(null);
            });
        }

        [TestMethod]
        public void Remove_RemoveNullFromGroup_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                Group g = new Group();
                g.Remove(null);
            });
        }
    }
}
