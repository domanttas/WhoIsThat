using NUnit.Framework;
using WhoIsThat.ConstantsUtil;
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
            var homeViewModel = new HomeViewModel();
            Assert.IsTrue(homeViewModel.IsIdentified(message));
        }

        [TestCase(Constants.NoMatchFound)]
        [TestCase(Constants.NoFacesIdentified)]
        [Test]
        public void IsIdentified_ShouldReturnFalse(string message)
        {
            var homeViewModel = new HomeViewModel();
            Assert.IsFalse(homeViewModel.IsIdentified(message));
        }
    }
}