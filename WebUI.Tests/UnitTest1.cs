using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Domain.Entities;
using Domain;
using Moq;
using WebUI.Controllers;
using System.Linq;
using WebUI.Infrastructure.Abstract;
using WebUI.Models;
using System.Web.Mvc;
using WebUI.HtmlHelpers;

namespace WebUI.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (arrange)
            Mock<IRepository> repository = new Mock<IRepository>();
            repository.Setup(m => m.Profiles).Returns(new List<Profile>
            {
                new Profile { ProfileId = Guid.NewGuid(), fName = "fName1", Gender = Gender.none },
                new Profile { ProfileId = Guid.NewGuid(), fName = "fName2", Gender = Gender.none },
                new Profile { ProfileId = Guid.NewGuid(), fName = "fName3", Gender = Gender.none },
                new Profile { ProfileId = Guid.NewGuid(), fName = "fName4", Gender = Gender.none },
                new Profile { ProfileId = Guid.NewGuid(), fName = "fName5", Gender = Gender.none },
                new Profile { ProfileId = Guid.NewGuid(), fName = "fName6", Gender = Gender.none },



            });
            ProfilesListController controller = new ProfilesListController(repository.Object);



            ProfilesListViewModel model = new ProfilesListViewModel();

            int page = 2;
            int pageSize = 4;

            model.Profiles = new List<Profile>
            {
                new Profile { ProfileId = Guid.NewGuid(), fName = "fName1" , Gender = Gender.none },
                new Profile { ProfileId = Guid.NewGuid(), fName = "fName2" , Gender = Gender.none },
                new Profile { ProfileId = Guid.NewGuid(), fName = "fName3" , Gender = Gender.none },
                new Profile { ProfileId = Guid.NewGuid(), fName = "fName4" , Gender = Gender.none },
                new Profile { ProfileId = Guid.NewGuid(), fName = "fName5" , Gender = Gender.none },
                new Profile { ProfileId = Guid.NewGuid(), fName = "fName6" , Gender = Gender.male },


            }.OrderBy(profile => profile.fName).Skip((page - 1) * pageSize).Take(pageSize);

            model.PagingInfo = new PagingInfo()
            {
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItems = model.Profiles.Count(),
            };

            SearchProfileListViewModel sProfile = new SearchProfileListViewModel() { Gender = Gender.male };

            // Действие (act)
            var resultAllProfiles = (ProfilesListViewModel)controller.AllProfiles(page).Model;
            var resultFilter = (ProfilesListViewModel)controller.Filter(sProfile, page).Model;


            // Утверждение (assert)
            var profilesAllProfiles = resultAllProfiles.Profiles.ToList();
            Assert.IsTrue(profilesAllProfiles.Count == 2);
            Assert.AreEqual(profilesAllProfiles[0].fName, "fName5");
            Assert.AreEqual(profilesAllProfiles[1].fName, "fName6");


            var profilesFilter = resultFilter.Profiles.ToList();
            Assert.IsTrue(profilesFilter.Count == 1);
            Assert.AreEqual(profilesFilter[0].fName, "fName6");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {

            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);


            // Утверждение
            Assert.AreEqual(@"<ul class=""pagination""><li><a href=""Page1"">1</a></li><li class=""active""><a href=""Page2"">2</a></li><li><a href=""Page3"">3</a></li></ul>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<IRepository> mock = new Mock<IRepository>();
            mock.Setup(m => m.Profiles).Returns(new List<Profile>
            {
                new Profile { ProfileId = Guid.NewGuid(), fName = "fName1" },
                new Profile { ProfileId = Guid.NewGuid(), fName = "fName2" },
                new Profile { ProfileId = Guid.NewGuid(), fName = "fName3" },
                new Profile { ProfileId = Guid.NewGuid(), fName = "fName4" },
                new Profile { ProfileId = Guid.NewGuid(), fName = "fName5" },
            });
            ProfilesListController controller = new ProfilesListController(mock.Object);
            controller.pageSize = 4;

            // Act
            ProfilesListViewModel result = (ProfilesListViewModel)controller.AllProfiles(2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 4);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        









    }
}

