using System.Drawing;
using System.Drawing.Imaging;
using Xunit;
namespace Imager.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async void Should_Create_An_Image_With_The_Correct_Format_And_Dimensions()
        {
            Image image = await Generator.GenerateAsync("image/jpeg", 200, 200);
            Assert.Equal(image.Width, 200);
        }
    }
}
