using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RESTful_API.Controllers;
using RESTful_API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http.Results;
using System.Web.Http.Routing;

namespace UnitTest
{
    [TestClass]
    public class TestChildrenController
    {
        [TestMethod]
        public void GetAllSwimmers()
        {
            var data = new List<Child>
        {
            new Child
            {
                DateOfBirth = DateTime.Now,
                Family = new Family{FamilyId = 2 },
                Firstname = "Ben",
                Lastname = "Smith",
                Gender = "M",
                Permission = false
            },
              new Child
            {
                DateOfBirth = DateTime.Now,
                Family = new Family{FamilyId = 2 },
                Firstname = "Jade",
                Lastname = "Potter",
                Gender = "F",
                Permission = true
            }
        }.AsQueryable();

            var mockSet = new Mock<DbSet<Child>>();
            mockSet.As<IQueryable<Child>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Child>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Child>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Child>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<DatabaseEntities>();
            mockContext.Setup(m => m.Children).Returns(mockSet.Object);

            var mock = new Mock<UrlHelper>();
            mock.Setup(m => m.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("https://localhost:44366/api/");

            var controller = new ChildrenController(mockContext.Object) {Url = mock.Object };
            var result = controller.GetChildren();

            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void GetAllSwimmersByAge()
        {
            var data = new List<Child>
        {
            new Child
            {
                DateOfBirth = DateTime.Parse("Jan 1, 2005"),//16
                Family = new Family{FamilyId = 2 },
                Firstname = "Ben",
                Lastname = "Smith",
                Gender = "M",
                Permission = false
            },
              new Child
            {
                DateOfBirth = DateTime.Parse("Jan 1, 2007"),//14
                Family = new Family{FamilyId = 2 },
                Firstname = "Jade",
                Lastname = "Potter",
                Gender = "F",
                Permission = true
            }
        }.AsQueryable();

            var mockSet = new Mock<DbSet<Child>>();
            mockSet.As<IQueryable<Child>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Child>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Child>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Child>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<DatabaseEntities>();
            mockContext.Setup(m => m.Children).Returns(mockSet.Object);

            var mock = new Mock<UrlHelper>();
            mock.Setup(m => m.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("https://localhost:44366/api/");

            var controller = new ChildrenController(mockContext.Object) { Url = mock.Object };
            var result = controller.GetChildrenByAge(14);

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void GetAllSwimmersByStroke()
        {
            var data = new List<Child>
        {
            new Child
            {
                DateOfBirth = DateTime.Parse("Jan 1, 2005"),//16
                Family = new Family{FamilyId = 2 },
                Firstname = "Ben",
                Lastname = "Smith",
                Gender = "M",
                Permission = true,
                Participants = new List<Participant>(){
                     new Participant{
                Event =  new Event
                {
                     Stroke = "Backstroke"
                }
                }
                }
            },
              new Child
            {
                DateOfBirth = DateTime.Parse("Jan 1, 2007"),//14
                Family = new Family{FamilyId = 2 },
                Firstname = "Jade",
                Lastname = "Potter",
                Gender = "F",
                Permission = true
            }
        }.AsQueryable();

            var mockSet = new Mock<DbSet<Child>>();
            mockSet.As<IQueryable<Child>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Child>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Child>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Child>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<DatabaseEntities>();
            mockContext.Setup(m => m.Children).Returns(mockSet.Object);

            var mock = new Mock<UrlHelper>();
            mock.Setup(m => m.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("https://localhost:44366/api/");

            var controller = new ChildrenController(mockContext.Object) { Url = mock.Object };
            var result = controller.GetChildrenByStroke("Backstroke");

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void GetSwimmersByName()
        {
            var data = new List<Child>
        {
            new Child
            {
                DateOfBirth = DateTime.Parse("Jan 1, 2005"),//16
                Family = new Family{FamilyId = 2 },
                Firstname = "Ben",
                Lastname = "Smith",
                Gender = "M",
                Permission = true
            },
              new Child
            {
                DateOfBirth = DateTime.Parse("Jan 1, 2007"),//14
                Family = new Family{FamilyId = 2 },
                Firstname = "Jade",
                Lastname = "Potter",
                Gender = "F",
                Permission = true
            }
        }.AsQueryable();

            var mockSet = new Mock<DbSet<Child>>();
            mockSet.As<IQueryable<Child>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Child>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Child>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Child>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<DatabaseEntities>();
            mockContext.Setup(m => m.Children).Returns(mockSet.Object);

            var mock = new Mock<UrlHelper>();
            mock.Setup(m => m.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("https://localhost:44366/api/");

            var controller = new ChildrenController(mockContext.Object) { Url = mock.Object };
            var result = controller.GetChildrenByName("Jade Potter");

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void GetSwimmerById()
        {
            var data = new List<Child>
        {
            new Child
            {
                ChildrenId = 1,
                DateOfBirth = DateTime.Parse("Jan 1, 2005"),//16
                Family = new Family{FamilyId = 2 },
                Firstname = "Ben",
                Lastname = "Smith",
                Gender = "M",
                Permission = true
            },
              new Child
            {
                ChildrenId = 2,
                DateOfBirth = DateTime.Parse("Jan 1, 2007"),//14
                Family = new Family{FamilyId = 2 },
                Firstname = "Jade",
                Lastname = "Potter",
                Gender = "F",
                Permission = true
            }
        }.AsQueryable();

            var mockSet = new Mock<DbSet<Child>>();
            mockSet.As<IQueryable<Child>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Child>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Child>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Child>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<DatabaseEntities>();
            mockContext.Setup(m => m.Children).Returns(mockSet.Object);

            var mock = new Mock<UrlHelper>();
            mock.Setup(m => m.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("https://localhost:44366/api/");

            var controller = new ChildrenController(mockContext.Object) { Url = mock.Object };
            var result = controller.GetChild(1);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void updateSwimmer()
        {
            var data = new List<Child>
        {
            new Child
            {
                ChildrenId = 1,
                Family = new Family{FamilyId = 2 },
                Participants = null,
                DateOfBirth = DateTime.Parse("Jan 1, 2005"),//16
                Firstname = "Ben",
                FamilyId = 2,
                Lastname = "Smith",
                Gender = "M",
                Permission = true
            },
              new Child
            {
                ChildrenId = 2,
                DateOfBirth = DateTime.Parse("Jan 1, 2007"),//14
                Family = new Family{FamilyId = 2 },
                Firstname = "Jade",
                Lastname = "Potter",
                Gender = "F",
                Permission = true
            }
        }.AsQueryable();

            var mockSet = new Mock<DbSet<Child>>();
            mockSet.As<IQueryable<Child>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Child>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Child>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Child>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<DatabaseEntities>();
            mockContext.Setup(m => m.Children).Returns(mockSet.Object);

            var mock = new Mock<UrlHelper>();
            mock.Setup(m => m.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("https://localhost:44366/api/");

            var controller = new ChildrenController(mockContext.Object) { Url = mock.Object };
            var child = new Child
            {
                ChildrenId = 1,
                DateOfBirth = DateTime.Parse("Jan 1, 2005"),//16
                Family = new Family { FamilyId = 2 },
                FamilyId = 2,
                Firstname = "Changed", //Original value Ben
                Lastname = "Smith",
                Gender = "M",
                Permission = true
            };
            var result = controller.PutChild(1, child);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void createSwimmer()
        {
            var data = new List<Child>
        {
            new Child
            {
                ChildrenId = 1,
                Family = new Family{FamilyId = 2 },
                Participants = null,
                DateOfBirth = DateTime.Parse("Jan 1, 2005"),//16
                Firstname = "Ben",
                FamilyId = 2,
                Lastname = "Smith",
                Gender = "M",
                Permission = true
            }
        }.AsQueryable();

            var mockSet = new Mock<DbSet<Child>>();
            mockSet.As<IQueryable<Child>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Child>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Child>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Child>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<DatabaseEntities>();
            mockContext.Setup(m => m.Children).Returns(mockSet.Object);

            var mock = new Mock<UrlHelper>();
            mock.Setup(m => m.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("https://localhost:44366/api/");

            var controller = new ChildrenController(mockContext.Object) { Url = mock.Object };
            var child = new Child
            {
                ChildrenId = 2,
                DateOfBirth = DateTime.Parse("Jan 1, 2007"),//14
                Family = new Family { FamilyId = 2 },
                Firstname = "Jade",
                Lastname = "Potter",
                Gender = "F",
                Permission = true
            };
            var result = controller.PostChild(child);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void deleteSwimmer()
        {
            var data = new List<Child>
        {
            new Child
            {
                ChildrenId = 1,
                Family = new Family{FamilyId = 2 },
                Participants = null,
                DateOfBirth = DateTime.Parse("Jan 1, 2005"),//16
                Firstname = "Ben",
                FamilyId = 2,
                Lastname = "Smith",
                Gender = "M",
                Permission = true
            },
              new Child
            {
                ChildrenId = 2,
                DateOfBirth = DateTime.Parse("Jan 1, 2007"),//14
                Family = new Family{FamilyId = 2 },
                Firstname = "Jade",
                Lastname = "Potter",
                Gender = "F",
                Permission = true
            }
        }.AsQueryable();

            var mockSet = new Mock<DbSet<Child>>();
            mockSet.As<IQueryable<Child>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Child>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Child>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Child>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<DatabaseEntities>();
            mockContext.Setup(m => m.Children).Returns(mockSet.Object);

            var mock = new Mock<UrlHelper>();
            mock.Setup(m => m.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("https://localhost:44366/api/");

            var controller = new ChildrenController(mockContext.Object) { Url = mock.Object };

            var result = controller.DeleteChild(2);

            Assert.IsNotNull(result);
        }
    }
}
