using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BankTests
{
    [TestClass]
    public class BankAccountTests
    {
        [TestMethod]
        public void LoadTest1()
        {
            Assert.ThrowsException<Exception>(()=> new bgl.Graphics.Core.Image(@"does not exist"));
        }

        [TestMethod]
        public void LoadTest2()
        {
            var image = new bgl.Graphics.Core.Image(@"assets/image.jpg");
            Assert.AreEqual(image.Width, 256u);
            Assert.AreEqual(image.Width, 256u);
            Assert.AreEqual((uint)image.Data.Length, image.Width * image.Height * 3);
        }

        [TestMethod]
        public void LoadTest3()
        {
            var image = new bgl.Graphics.Core.Image(@"assets/TransparentImage.jpg");
            Assert.AreEqual(image.Width, 256u);
            Assert.AreEqual(image.Width, 256u);
            Assert.AreEqual((uint)image.Data.Length, image.Width * image.Height * 4);
        }
    }
}
