namespace InnoGotchiBackendTests.PetTests
{
    public class PetRepositoryTests
    {
        private IFixture _fixture;

        public PetRepositoryTests()
        {
            var contextOptions = new DbContextOptionsBuilder<InnoGotchiContext>()
                    .UseInMemoryDatabase(nameof(PetRepositoryTests))
                    .Options;

            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                    .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _fixture.Register(() => new InnoGotchiUnitOfWork(contextOptions));
        }
        [Fact]
        public async Task CreateTestAsync()
        {
            var uow = _fixture.Create<InnoGotchiUnitOfWork>();
            var pet = CreateValidPet();

            uow.Pets.Create(pet);
            await uow.SaveChangesAsync();
            uow.Pets.Contains(p => p.Id == pet.Id).Should().BeTrue();
        }
        [Fact]
        public async Task DeleteTestAsync()
        {
            var uow = _fixture.Create<InnoGotchiUnitOfWork>();
            var pets = uow.Pets.AllItems();
            var pet = CreateValidPet();
            uow.Pets.Create(pet);
            await uow.SaveChangesAsync();
            uow.Detach(pet);
            uow.Pets.Delete(pet.Id);
            await uow.SaveChangesAsync();
            uow.Pets.Contains(p => p.Id == pet.Id).Should().BeFalse();
        }
        [Fact]
        public async Task UpdateTestAsync()
        {
            var uow = _fixture.Create<InnoGotchiUnitOfWork>();

            var pet = CreateValidPet();
            uow.Pets.Create(pet);
            await uow.SaveChangesAsync();
            pet.Name = "Test";
            uow.Pets.Update(pet);
            await uow.SaveChangesAsync();
            uow.Pets.Contains(p => p.Id == pet.Id && p.Name == "Test").Should().BeTrue();
        }

        [Fact]
        public async Task GetAllTestAsync()
        {
            var uow = _fixture.Create<InnoGotchiUnitOfWork>();

            var pet1 = CreateValidPet();
            var pet2 = CreateValidPet();
            uow.Pets.Create(pet1);
            uow.Pets.Create(pet2);
            await uow.SaveChangesAsync();
            var pets = uow.Pets.AllItems();
            pets.Count().Should().BeGreaterThanOrEqualTo(2);
        }

        private Pet CreateValidPet()
        {
            return _fixture.Build<Pet>()
                           .Without(p => p.Id)
                           .Without(p => p.Farm)
                           .Without(p => p.FarmId)
                           .Create();
        }
    }
}