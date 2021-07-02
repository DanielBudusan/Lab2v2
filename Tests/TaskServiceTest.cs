using AutoMapper;
using Lab2v2.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab2v2.Models;

namespace Tests
{
    class TaskServiceTest
    {

        private ApplicationDbContext _context; 
        private IMapper _mapper;
        [SetUp]
        public void Setup()
        {
            Console.WriteLine("In setup.");
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDB")
            .Options;

            _context = new ApplicationDbContext(options, new OperationalStoreOptionsForTests());
            _context.Tasks.Add(new Lab2v2.Models.Task { Title = "test1", Description = "asdasda", Status = "closed", Importance = "high" });
            
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

            foreach (var task in _context.Tasks)
            {
                _context.Remove(task);
            }
            _context.SaveChanges();
        }


        [Test]
        public void TestGetAll()
        {
            var service = new TaskService(_context, _mapper);
            Assert.IsTrue(service.GetTasks().IsCompletedSuccessfully);
        }
    }
}
