using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonoChrome.Core.GameObjectSystem;
using MonoChrome.Core.GameObjectSystem.Components;
using MonoChrome.GameObjectSystem.Components.Attributes;
using MonoChrome.GameObjectSystem.Components.Exceptions;

namespace MonoChrome.Tests
{
    [TestClass]
    public class RequireComponentAttributeTest
    {
        [RequireComponent(typeof(B))]
        class A : Component
        {

        }


        class B : Component
        {

        }

        [TestMethod]
        public void Attribute_RequireComponentBAndBExist_NotThrowUnfoundRequiredComponentsException()
        {
            try
            {
                GameObject o = new GameObject();
                o.AddComponent<B>();
                o.AddComponent<A>();
            } catch(UnfoundRequiredComponentsException e)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Attribute_RequireComponentBAndBExist_ThrowUnfoundRequiredComponentsException()
        {
            Assert.ThrowsException<UnfoundRequiredComponentsException>(() => {
                GameObject o = new GameObject();
                o.AddComponent<A>();
            });
        }
    }
}
