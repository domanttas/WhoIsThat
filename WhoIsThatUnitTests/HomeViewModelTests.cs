using NUnit.Framework;
using WhoIsThat.ConstantsUtil;
using WhoIsThat.Models;
using WhoIsThat.ViewModels;

namespace WhoIsThatUnitTests
{
    public class HomeViewModelTests
    {
        [TestCase("Domantas")]
        [TestCase("AnyName")]
        [Test]
        public void IsIdentified_ShouldReturnTrue(string message)
        {
            var homeViewModel = new HomeViewModel(new ImageObject());
            Assert.IsTrue(homeViewModel.IsIdentified(message));
        }

        [TestCase(Constants.NoMatchFound)]
        [TestCase(Constants.NoFacesIdentified)]
        [Test]
        public void IsIdentified_ShouldReturnFalse(string message)
        {
            var homeViewModel = new HomeViewModel(new ImageObject());
            Assert.IsFalse(homeViewModel.IsIdentified(message));
        }
    }
}