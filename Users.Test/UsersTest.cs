using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Users.Controllers;
using Users.Dto;

namespace Users.Test
{
    public class UsersTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CheckForBadRequest()
        {
            var users = new List<string>();
            var controller = new UsersController();

            var response = controller.RetrieveUsers(users);
            var result = response.Result as BadRequestObjectResult;

            Assert.IsTrue(result.StatusCode == 400);
        }

        [Test]
        public void NoMatchingRecords()
        {
            var users = new List<string>();
            users.Add("asdfasdfsadf0000998777");

            var controller = new UsersController();
            var response = controller.RetrieveUsers(users);
            var result = response.Result as OkObjectResult;
            var userDtoList = result.Value as List<UserDto>;

            Assert.IsTrue(result.StatusCode == 200);
            Assert.IsTrue(userDtoList.Count == 0);
        }

        [Test]
        public void MatchingRecordFound()
        {
            var users = new List<string>();
            users.Add("sharmilabogireddy");

            var controller = new UsersController();
            var response = controller.RetrieveUsers(users);
            var result = response.Result as OkObjectResult;
            var userDtoList = result.Value as List<UserDto>;

            Assert.IsTrue(result.StatusCode == 200);
            Assert.IsTrue(userDtoList.Count == 1);
            Assert.IsTrue(userDtoList[0].Name.Equals("Sharmila"));
        }

        [Test]
        public void NoDuplicatesInResponse()
        {
            var users = new List<string>();
            users.Add("sharmilabogireddy");
            users.Add("SharmilaBogireddy");

            var controller = new UsersController();
            var response = controller.RetrieveUsers(users);
            var result = response.Result as OkObjectResult;
            var userDtoList = result.Value as List<UserDto>;

            Assert.IsTrue(result.StatusCode == 200);
            Assert.IsTrue(userDtoList.Count == 1);
            Assert.IsTrue(userDtoList[0].Name.Equals("Sharmila"));
        }

        [Test]
        public void CheckForSortByNameInResponse()
        {
            var users = new List<string>();
            users.Add("sharmilabogireddy");
            users.Add("SharmilaBogireddy");
            users.Add("azure");

            var controller = new UsersController();
            var response = controller.RetrieveUsers(users);
            var result = response.Result as OkObjectResult;
            var userDtoList = result.Value as List<UserDto>;

            Assert.IsTrue(result.StatusCode == 200);
            Assert.IsTrue(userDtoList.Count == 2);
            Assert.IsTrue(userDtoList[0].Name.Equals("Microsoft Azure"));
            Assert.IsTrue(userDtoList[1].Name.Equals("Sharmila"));
        }


    }
}