using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using MonoChrome.Core.GameObjectSystem.Components;

namespace MonoChrome.Tests
{
    [TestClass]
    public class TransformTest
    {
        [TestMethod]
        public void LocalPosition_WithoutParentAndChildrensChangeLocalPosition_AbsolutePositionEqualsLocalPosition()
        {
            //Arrange
            Transform t1 = new Transform();
            //Actual
            t1.LocalPosition = new Vector2(1, 1);
            //Expected
            Assert.AreEqual(t1.LocalPosition, t1.Position);
        }

        [TestMethod]
        public void Position_WithoutParentAndChildrensChangeLocalPosition_AbsolutePositionEqualsLocalPosition()
        {
            //Arrange
            Transform t1 = new Transform();
            //Actual
            t1.Position = new Vector2(1, 1);
            //Expected
            Assert.AreEqual(t1.LocalPosition, t1.Position);
        }

        [TestMethod]
        public void Position_HasChildrenTransformChangeParentPosition_ChildrenPositionChanged()
        {
            //Arrange
            Transform t1 = new Transform();
            Transform t2 = new Transform();
            t2.Position = new Vector2(2, 2);
            t2.Parent = t1;
            var t2Local = t2.LocalPosition;
            //Actual
            t1.Position = new Vector2(1, 1);
            //Expected
            Assert.AreEqual(new Vector2(3, 3), t2.Position);
        }

        [TestMethod]
        public void Position_HasChildrenTransformChangeParentPosition_ChildrenLocalPositionNotChanged()
        {
            //Arrange
            Transform t1 = new Transform();
            Transform t2 = new Transform();
            t2.Position = new Vector2(2, 2);
            t2.Parent = t1;
            var t2Local = t2.LocalPosition;
            //Actual
            t1.Position = new Vector2(1, 1);
            //Expected
            Assert.AreEqual(t2Local, t2.LocalPosition);
        }

        [TestMethod]
        public void Position_HasParentChangeChildrenPosition_ChildLocalPositionChanged()
        {
            //Arrange
            Transform t1 = new Transform();
            t1.Position = new Vector2(5, 5);
            Transform t2 = new Transform();
            t2.Position = new Vector2(2, 2);
            t2.Parent = t1;
            //Actual
            t2.Position = new Vector2(1, 1);
            //Expected
            Assert.AreEqual(new Vector2(-4, -4), t2.LocalPosition);
        }

        [TestMethod]
        public void Parent_Add_1_Children_ChildrenCountReturn_1()
        {
            //Arrange
            Transform t1 = new Transform();
            Transform t2 = new Transform();
            t2.Parent = t1;
            //Actual
            var actual = t1.Childrens.Count;
            var expected = 1;
            //Expected
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Parent_Remove_1_Children_ChildrenCountReturn_0()
        {
            //Arrange
            Transform t1 = new Transform();
            Transform t2 = new Transform();
            t2.Parent = t1;
            t2.Parent = null;
            //Actual
            var actual = t1.Childrens.Count;
            var expected = 0;
            //Expected
            Assert.AreEqual(expected, actual);
        }
    }
}
