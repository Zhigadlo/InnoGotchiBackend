using AutoMapper;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Mapper;
using InnoGotchi.BLL.Services;

namespace InnoGotchiBackendTests
{
    public class PetServiceTests
    {
        private IFixture _fixture;
        public PetServiceTests() 
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                    .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            
            var contextOptions = new DbContextOptionsBuilder<InnoGotchiContext>()
                    .UseInMemoryDatabase(nameof(PetRepositoryTests))
                    .Options;
            var config = new MapperConfiguration(cnf => cnf.AddProfile<MapperProfile>());
            _fixture.Register(() => new PetService(new InnoGotchiUnitOfWork(contextOptions), new Mapper(config)));
        }

        [Fact]
        public void CreateTest()
        {
            var service = _fixture.Create<PetService>();
            var newPet = CreateValidPetDTO();
            int newPetId = service.Create(newPet);
            newPetId.Should().NotBe(-1);
            var actualPet = service.Get(newPetId);
            actualPet.Should().NotBeNull();
            actualPet?.Id.Should().Be(newPetId);
        }

        [Fact]
        public void DeleteTest()
        {
            var service = _fixture.Create<PetService>();
            var newPet = CreateValidPetDTO();
            int newPetId = service.Create(newPet);
            newPetId.Should().NotBe(-1);
            service.Delete(newPetId).Should().BeTrue();
            service.Get(newPetId).Should().BeNull();
        }

        [Fact]
        public void UpdateTest()
        {
            var service = _fixture.Create<PetService>();
            var newPet = CreateValidPetDTO();
            int newPetId = service.Create(newPet);
            newPetId.Should().NotBe(-1);
            newPet.Id = newPetId;
            newPet.Name = "Test";
            service.Update(newPet).Should().BeTrue();
            var updatedPet = service.Get(newPetId);
            updatedPet.Should().NotBeNull();
            updatedPet?.Id.Should().Be(newPetId);
            updatedPet?.Name.Should().Be("Test");
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
