using AutoMapper;
using Lab2v2.Data;
using Lab2v2.Models;
using Lab2v2.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    class ProjectServiceTest
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        [SetUp]
        public void Setup()
        {
            Console.WriteLine("In setup.");
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDB")
            .Options;

            _context = new ApplicationDbContext(options, new OperationalStoreOptionsForTests());
            var user = new ApplicationUser { Email = "dani@dani.com", EmailConfirmed = true };
            _context.Users.Add(user);
            _context.Projects.Add(new Project { Name = "testproject1", CreatedByUserId = user.Id, ProjectUsers = new List<ApplicationUser> { user } });
            _context.SaveChanges();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Lab2v2.MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
        }

        [TearDown]
        public void Teardown()
        {
            Console.WriteLine("In teardown");

            foreach (var project in _context.Projects)
            {
                _context.Remove(project);
            }
            _context.SaveChanges();
        }


        [Test]
        public void TestGetAll()
        {
            var service = new ProjectService(_context, _mapper);
            Assert.IsTrue(service.GetProjects().IsCompletedSuccessfully);
        }
    }
}
