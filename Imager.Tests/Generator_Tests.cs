namespace Imager.Tests
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using Xunit;

    public class Generator_Tests
    {
        [Fact]
        public async void Should_Create_An_Image_With_The_Correct_Format_And_Dimensions()
        {
            var image = await Generator.GenerateAsync("image/jpeg", 200, 200);
            Assert.Equal(image.Width, 200);
            Assert.Equal(image.Height, 200);
        }

        [Fact]
        public async void Should_Create_A_Proper_Byte_Representation()
        {
            var bytes = await Generator.GenerateAsBytesAsync("image/jpeg", 200, 200);
            var asImage = Generator.BytesToImage(bytes);

            Assert.Equal(asImage.Width, 200);
            Assert.Equal(asImage.Height, 200);
            Assert.Equal(asImage.RawFormat, ImageFormat.Jpeg);
        }
    }
}
