using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Meziantou.WpfFontAwesome.Tests
{
    [TestClass]
    public class DeploymentTests
    {
        [TestMethod]
        public void IsStrongNamed()
        {
            var assembly = typeof(ProIconAttribute).Assembly;
            Assert.AreNotEqual(0, assembly.GetName().GetPublicKey().Length);
        }
    }
}
