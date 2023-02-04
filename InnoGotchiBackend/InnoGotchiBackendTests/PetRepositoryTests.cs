namespace InnoGotchiBackendTests
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
        public void CreateTest()
        {
            var uow = _fixture.Create<InnoGotchiUnitOfWork>();
            var pet = CreateValidPet();

            uow.Pets.Create(pet);
            uow.SaveChanges();
            uow.Pets.Contains(p => p.Id == pet.Id).Should().BeTrue();
        }
        [Fact]
        public void DeleteTest()
        {
            var uow = _fixture.Create<InnoGotchiUnitOfWork>();
            var pets = uow.Pets.GetAll();
            var pet = CreateValidPet();
            uow.Pets.Create(pet);
            uow.SaveChanges();
            uow.Pets.Delete(pet.Id);
            uow.SaveChanges();
            uow.Pets.Contains(p => p.Id == pet.Id).Should().BeFalse();
        }
        [Fact]
        public void UpdateTest()
        {
            var uow = _fixture.Create<InnoGotchiUnitOfWork>();

            var pet = CreateValidPet();
            uow.Pets.Create(pet);
            uow.SaveChanges();
            pet.Name = "Test";
            uow.Pets.Update(pet);
            uow.SaveChanges();
            uow.Pets.Contains(p => p.Id == pet.Id && p.Name == "Test").Should().BeTrue();
        }

        [Fact]
        public void GetAllTest()
        {
            var uow = _fixture.Create<InnoGotchiUnitOfWork>();

            var pet1 = CreateValidPet();
            var pet2 = CreateValidPet();
            uow.Pets.Create(pet1);
            uow.Pets.Create(pet2);
            uow.SaveChanges();
            var pets = uow.Pets.GetAll();
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