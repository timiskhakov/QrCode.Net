using Gma.QrCodeNet.Encoding.Windows.Render;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace Gma.QrCodeNet.Encoding.Tests.GraphicsRendererTests
{
    [TestClass]
    public class WriteToStreamParallelSafeTests
    {
        private const string Data = "https://github.com";
        
        [TestMethod]
        public void ShouldReturnStream()
        {
            // Arrange
            var qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);
            var qrCode = qrEncoder.Encode(Data);
            var renderer = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Two), Brushes.Black, Brushes.White);

            // Act
            byte[] actual;
            using (var ms = new MemoryStream())
            {
                renderer.WriteToStreamParallelSafe(qrCode.Matrix, ImageFormat.Png, ms);
                actual = ms.GetBuffer();
            }

            // Assert
            var base64 = Convert.ToBase64String(actual);
            Assert.IsTrue(base64.Length > 0);
        }

        [TestMethod]
        public void ShouldSuccessInParallelRunning()
        {
            // Arrange
            var qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);
            var qrCode = qrEncoder.Encode("Hello World");
            var list = new List<Task>();

            // Act
            for (var i = 0; i < 1000; i++)
            {
                list.Add(Task.Factory.StartNew(() =>
                {
                    using (var ms = new MemoryStream())
                    {
                        var renderer = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Two), Brushes.Black, Brushes.White);
                        renderer.WriteToStreamParallelSafe(qrCode.Matrix, ImageFormat.Png, ms);
                    }
                }));
            }

            // Expect no exceptions
            Task.WaitAll(list.ToArray());
        }
    }
}
