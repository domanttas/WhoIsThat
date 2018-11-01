using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Shouldly;
using WhoIsThat.LogicUtil;
using WhoIsThat.Models;

namespace WhoIsThatUnitTests
{
    public class ViewModelsUtilTests
    {
        [Test]
        public void SortList_ShouldReturnSorted()
        {
            //Arrange
            var firstObject = new ImageObject()
            {
                Id = 1,
                ImageContentUri = "test",
                ImageName = "test",
                PersonFirstName = "test",
                PersonLastName = "test",
                DescriptiveSentence = "test",
                Score = 1
            };

            var secondObject = new ImageObject()
            {
                Id = 1,
                ImageContentUri = "test",
                ImageName = "test",
                PersonFirstName = "test",
                PersonLastName = "test",
                DescriptiveSentence = "test",
                Score = 2
            };
            
            var imagesList = new List<ImageObject>()
            {
                firstObject,
                secondObject
            };

            var expectedImagesList = new List<ImageObject>()
            {
                secondObject,
                firstObject
            };

            //Act
            var result = ViewModelsUtil.SortList(imagesList);

            //Assert
            CollectionAssert.IsOrdered(result);
            Assert.IsTrue(expectedImagesList.SequenceEqual(result));
        }
    }
}
