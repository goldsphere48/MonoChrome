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

        class Obj : GameObject
        {

        }

        [TestMethod]
        public void Attribute_RequireComponentBAndBExist_NotThrowUnfoundRequiredComponentsException()
        {
            try
            {
                Obj o = new Obj();
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
                Obj o = new Obj();
                o.AddComponent<A>();
            });
        }
    }
}
