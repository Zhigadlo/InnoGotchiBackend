using AutoMapper;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Mapper;
using InnoGotchi.BLL.Services;
using InnoGotchi.Web.Controllers;
using InnoGotchi.Web.Mapper;
using InnoGotchi.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchiBackendTests.PetTests
{
    public class PetControllerTests
    {
        private IFixture _fixture;
        public PetControllerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                    .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var contextOptions = new DbContextOptionsBuilder<InnoGotchiContext>()
                    .UseInMemoryDatabase(nameof(PetRepositoryTests))
                    .Options;
            var config = new MapperConfiguration(cnf => cnf.AddProfiles(new List<Profile> { new MapperProfile(), new ViewModelProfile() }));
            _fixture.Register(() => new PetService(new InnoGotchiUnitOfWork(contextOptions), new Mapper(config)));
            _fixture.Register(() => new PetsController(_fixture.Create<PetService>(), new Mapper(config)));
        }
        [Fact]
        public async Task PostTestAsync()
        {
            var controller = _fixture.Create<PetsController>();
            var newPet = CreateValidPetModel();
            var result = await controller.CreateAsync(newPet);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task UpdateNameTestAsync()
        {
            var service = _fixture.Create<PetService>();
            var controller = _fixture.Create<PetsController>();
            var pet = CreateValidPetDTO();
            var petId = await service.CreateAsync(pet);
            petId.Should().NotBe(-1);
            var result = await controller.UpdateNameAsync(petId, "NewName");
            result.Should().BeOfType<OkResult>();
        }

        private PetModel CreateValidPetModel()
        {
            return _fixture.Build<PetModel>()
                           .Without(p => p.Id)
                           .Create();
        }
        private PetDTO CreateValidPetDTO()
        {
            return _fixture.Build<PetDTO>()
                           .Without(p => p.Id)
                           .Without(p => p.Farm)
                           .Create();
        }
    }
}
