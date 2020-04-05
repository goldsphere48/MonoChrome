using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonoChrome.Core.GameObjectSystem;
using MonoChrome.Core.GameObjectSystem.Components;
using MonoChrome.GameObjectSystem.Components.Attributes;
using MonoChrome.GameObjectSystem.Components.Exceptions;

namespace MonoChrome.Tests
{
    [TestClass]
    public class CreatedForAttributeTest
    {
        [CreatedFor(typeof(Obj1))]
        class A : Component
        {

        }

        [CreatedFor(typeof(Obj2))]
        class B : Component
        {

        }

        [CreatedFor(typeof(Obj1), Inherit = true)]
        class C : Component
        {

        }

        class Obj1 : GameObject
        {

        }

        class Obj2 : GameObject
        {

        }

        class Obj3 : Obj1
        {

        }

        class Obj4 : Obj1
        {

        }

        [TestMethod]
        public void Attribute_ComponentAttachedToCorrectObject_NotThrowInvalidComponentTargetException()
        {
            try
            {
                Obj1 o = new Obj1();
                o.AddComponent<A>();
            } catch(InvalidComponentTargetException e)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Attribute_ComponentAttachedToAnotherObject_ThrowInvalidComponentTargetException()
        {
            Assert.ThrowsException<InvalidComponentTargetException>(() =>
            {
                Obj1 o = new Obj1();
                o.AddComponent<B>();
            });
        }

        [TestMethod]
        public void Attribute_ComponentAttachedToSubclassAndInheritTrue_NotThrowInvalidComponentTargetException()
        {
            try
            {
                Obj3 o = new Obj3();
                o.AddComponent<C>();
            }
            catch (InvalidComponentTargetException e)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Attribute_ComponentAttachedToSubclassAndInheritFalse_NotThrowInvalidComponentTargetException()
        {
            Assert.ThrowsException<InvalidComponentTargetException>(() =>
            {
                Obj4 o = new Obj4();
                o.AddComponent<A>();
            });
        }
    }
}
