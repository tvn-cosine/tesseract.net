using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tesseract;

namespace tesseract.net.test
{
    [TestClass()]
    public class TessBaseAPITests
    {
        [TestMethod()]
        public void GetVersionTest()
        {
            TessBaseAPI tessBaseAPI = new TessBaseAPI();

            Assert.AreEqual("4.00.00alpha", tessBaseAPI.GetVersion());
        }
    }
}
