using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonoChrome.Core.GameObjectSystem;
using MonoChrome.Core.GameObjectSystem.Components;
using MonoChrome.GameObjectSystem.Components.Attributes;
using MonoChrome.GameObjectSystem.Components.Exceptions;

namespace MonoChrome.Tests
{
    [TestClass]
    public class ComponentUsageAttributeTest
    {
        [ComponentUsage(AllowMultipleComponentUsage = false)]
        class A : Component
        {

        }

        [ComponentUsage(AllowMultipleComponentUsage = true)]
        class B : Component
        {

        }

        class Obj : GameObject
        {

        }


        [TestMethod]
        public void Attribute_AllowMultipleComponentUsageEqualsFalseAndUseOneAttribue_NotThrowInvalidComponentDuplicateException()
        {
            try
            {
                Obj o = new Obj();
                o.AddComponent<A>();
            } catch(InvalidComponentDuplicateException e)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Attribute_AllowMultipleComponentUsageEqualsFalseAndUseTwoAttribue_ThrowInvalidComponentDuplicateException()
        {
            Assert.ThrowsException<InvalidComponentDuplicateException>(() =>
            {
                Obj o = new Obj();
                o.AddComponent<A>();
                o.AddComponent<A>();
            });
        }

        [TestMethod]
        public void Attribute_AllowMultipleComponentUsageEqualsTrueAndUseTwoAttribue_NotThrowInvalidComponentDuplicateException()
        {
            try
            {
                Obj o = new Obj();
                o.AddComponent<B>();
                o.AddComponent<B>();
            }
            catch (InvalidComponentDuplicateException e)
            {
                Assert.Fail();
            }
        }
    }
}
