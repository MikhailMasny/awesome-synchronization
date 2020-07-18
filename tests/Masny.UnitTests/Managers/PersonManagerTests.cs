using AutoMapper;
using Masny.Application.Interfaces;
using Masny.Application.Managers;
using Masny.Application.Models;
using Masny.Domain.Models.App;
using Masny.Infrastructure.AppContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Masny.UnitTests.Managers
{
    public class PersonManagerTests
    {
        private readonly IPersonManager _personManager;
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public PersonManagerTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            serviceCollection.AddAutoMapper(Assembly.Load("Masny.Application"));

            var serviceProvider = serviceCollection.BuildServiceProvider();
            _appDbContext = serviceProvider.GetRequiredService<AppDbContext>();
            _mapper = serviceProvider.GetRequiredService<IMapper>();

            _personManager = new PersonManager(_appDbContext, _mapper);
        }

        [Fact]
        public void Constructor_Throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new PersonManager(null, null));
            Assert.Throws<ArgumentNullException>(() => new PersonManager(_appDbContext, null));
        }

        [Fact]
        public void Method_Throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _personManager.CreateAsync(null).GetAwaiter().GetResult());
            Assert.Throws<ArgumentNullException>(() => _personManager.UpdateAsync(null).GetAwaiter().GetResult());
        }

        [Fact]
        public void CreatePerson_WhenEmptyContext_ReturnsSuccessfulOperation()
        {
            // Arrange
            var peopleCount = _appDbContext.Persons.Count();

            // Act
            var personDto = new PersonDto
            {
                CloudId = 1,
                Email = "fake@fake.com",
                Name = "fakename",
            };
            var operationResult = _personManager.CreateAsync(personDto).GetAwaiter().GetResult();
            var peopleCountAfterOperation = _appDbContext.Persons.Count();

            // Assert
            Assert.Equal(1, operationResult);
            Assert.NotEqual(peopleCount, peopleCountAfterOperation);
        }

        [Fact]
        public void GetPerson_WhenEmptyContext_ReturnsNull()
        {
            // Arrange
            var id = 1;

            // Act
            var personDto = _personManager.GetAsync(id).GetAwaiter().GetResult();

            // Assert
            Assert.Null(personDto);
        }

        [Fact]
        public void GetPerson_WhenNotEmptyContext_ReturnsPerson()
        {
            // Arrange
            var person = new Person
            {
                Id = 1,
                CloudId = 1,
                Email = "fake@fake.com",
                Name = "fakename",
            };
            _appDbContext.Persons.Add(person);
            _appDbContext.SaveChanges();

            // Act
            var personDto = _personManager.GetAsync(person.Id).GetAwaiter().GetResult();

            // Assert
            Assert.NotNull(personDto);
            Assert.Equal(person.Id, personDto.Id);
        }

        [Fact]
        public void GetPersonByCloudId_WhenEmptyContext_ReturnsNull()
        {
            // Arrange
            var cloudId = 1;

            // Act
            var personDto = _personManager.GetByCloudIdAsync(cloudId).GetAwaiter().GetResult();

            // Assert
            Assert.Null(personDto);
        }

        [Fact]
        public void GetPersonByCloudId_WhenNotEmptyContext_ReturnsPerson()
        {
            // Arrange
            var person = new Person
            {
                Id = 1,
                CloudId = 1,
                Email = "fake@fake.com",
                Name = "fakename",
            };
            _appDbContext.Persons.Add(person);
            _appDbContext.SaveChanges();

            // Act
            var personDto = _personManager.GetByCloudIdAsync(person.CloudId).GetAwaiter().GetResult();

            // Assert
            Assert.NotNull(personDto);
            Assert.Equal(person.Id, personDto.Id);
        }

        [Fact]
        public void GetPeople_WhenEmptyContext_ReturnsEmptyList()
        {
            // Arrange

            // Act
            var peopleDtos = _personManager.GetAllAsync().GetAwaiter().GetResult();

            // Assert
            Assert.Empty(peopleDtos);
        }

        [Fact]
        public void GetPeople_WhenNotEmptyContext_ReturnsPeople()
        {
            // Arrange
            var person = new Person
            {
                Id = 1,
                CloudId = 1,
                Email = "fake@fake.com",
                Name = "fakename",
            };
            _appDbContext.Persons.Add(person);
            _appDbContext.SaveChanges();

            // Act
            var peopleDtos = _personManager.GetAllAsync().GetAwaiter().GetResult();

            // Assert
            Assert.NotEmpty(peopleDtos);
        }

        [Fact]
        public void UpdatePerson_WhenEmptyContext_ReturnsUnsuccessfulOperation()
        {
            // Arrange

            // Act
            var personDto = new PersonDto
            {
                Id = 1,
                CloudId = 1,
                Email = "fake@fake.com",
                Name = "fakename",
            };
            var operationResult = _personManager.UpdateAsync(personDto).GetAwaiter().GetResult();

            // Assert
            Assert.Equal(0, operationResult);
        }

        [Fact]
        public void UpdatePerson_WhenNotEmptyContext_ReturnsSuccessfulOperation()
        {
            // Arrange
            var person = new Person
            {
                Id = 1,
                CloudId = 1,
                Email = "fake@fake.com",
                Name = "fakename",
            };
            _appDbContext.Persons.Add(person);
            _appDbContext.SaveChanges();

            // Act
            var personDto = _personManager.GetAsync(person.Id).GetAwaiter().GetResult();
            personDto.CloudId = 2;
            personDto.Email = "anotherfake@fake.com";
            personDto.Name = "anotherfakename";

            var operationResult = _personManager.UpdateAsync(personDto).GetAwaiter().GetResult();
            var updatedPerson = _appDbContext.Persons.FirstOrDefault(c => c.Id == person.Id);

            // Assert
            Assert.Equal(1, operationResult);
            Assert.Equal(person.CloudId, updatedPerson.CloudId);
            Assert.Equal(person.Email, updatedPerson.Email);
            Assert.Equal(person.Name, updatedPerson.Name);
        }

        [Fact]
        public void DeletePerson_WhenEmptyContext_ReturnsUnsuccessfulOperation()
        {
            // Arrange
            var peopleCount = _appDbContext.Persons.Count();
            var id = 1;

            // Act
            var operationResult = _personManager.DeleteAsync(id).GetAwaiter().GetResult();
            var peopleCountAfterOperation = _appDbContext.Persons.Count();

            // Assert
            Assert.Equal(0, operationResult);
            Assert.Equal(peopleCount, peopleCountAfterOperation);
        }

        [Fact]
        public void DeletePerson_WhenNotEmptyContext_ReturnsSuccessfulOperation()
        {
            // Arrange
            var person = new Person
            {
                Id = 1,
                CloudId = 1,
                Email = "fake@fake.com",
                Name = "fakename",
            };
            _appDbContext.Persons.Add(person);
            _appDbContext.SaveChanges();
            var peopleCount = _appDbContext.Persons.Count();

            // Act
            var operationResult = _personManager.DeleteAsync(person.Id).GetAwaiter().GetResult();
            var peopleCountAfterOperation = _appDbContext.Persons.Count();

            // Assert
            Assert.Equal(1, operationResult);
            Assert.NotEqual(peopleCount, peopleCountAfterOperation);
        }

        [Fact]
        public void DeletePersonByCloudId_WhenEmptyContext_ReturnsUnsuccessfulOperation()
        {
            // Arrange
            var peopleCount = _appDbContext.Persons.Count();
            var cloudId = 1;

            // Act
            var operationResult = _personManager.DeleteByCloudIdAsync(cloudId).GetAwaiter().GetResult();
            var peopleCountAfterOperation = _appDbContext.Persons.Count();

            // Assert
            Assert.Equal(0, operationResult);
            Assert.Equal(peopleCount, peopleCountAfterOperation);
        }

        [Fact]
        public void DeletePersonByCloudId_WhenNotEmptyContext_ReturnsSuccessfulOperation()
        {
            // Arrange
            var person = new Person
            {
                Id = 1,
                CloudId = 1,
                Email = "fake@fake.com",
                Name = "fakename",
            };
            _appDbContext.Persons.Add(person);
            _appDbContext.SaveChanges();
            var peopleCount = _appDbContext.Persons.Count();

            // Act
            var operationResult = _personManager.DeleteByCloudIdAsync(person.CloudId).GetAwaiter().GetResult();
            var peopleCountAfterOperation = _appDbContext.Persons.Count();

            // Assert
            Assert.Equal(1, operationResult);
            Assert.NotEqual(peopleCount, peopleCountAfterOperation);
        }
    }
}
