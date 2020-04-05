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
        public void Transform_Without_Parent_And_Childrens_Change_LocalPosition_AbsolutePosition_Equals_LocalPosition()
        {
            //Arrange
            Transform t1 = new Transform();
            //Actual
            t1.LocalPosition = new Vector2(1, 1);
            //Expected
            Assert.AreEqual(t1.LocalPosition, t1.Position);
        }

        [TestMethod]
        public void Transform_Without_Parent_And_Childrens_Change_AbsolutePosition_AbsolutePosition_Equals_LocalPosition()
        {
            //Arrange
            Transform t1 = new Transform();
            //Actual
            t1.Position = new Vector2(1, 1);
            //Expected
            Assert.AreEqual(t1.LocalPosition, t1.Position);
        }

        [TestMethod]
        public void Transform_Group_Change_AbsolutePosition_Children_Change_AbsolutePosition_Realetive_Parent()
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
        public void Transform_Group_Change_AbsolutePosition_Children_LocalPosition_Doesnt_Change()
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
    }
}
