namespace InnoGotchiBackendTests.RequestTests
{
    public class RequestRepositoryTests
    {
        private IFixture _fixture;

        public RequestRepositoryTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                    .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var contextOptions = new DbContextOptionsBuilder<InnoGotchiContext>()
                    .UseInMemoryDatabase(nameof(RequestRepositoryTests))
                    .Options;
            _fixture.Register(() => new InnoGotchiUnitOfWork(contextOptions));
        }

        [Fact]
        public async Task UpdateTestAsync()
        {
            var uow = _fixture.Create<InnoGotchiUnitOfWork>();
            var request = CreateValidRequest();
            uow.Requests.Create(request);
            await uow.SaveChangesAsync();
            request.IsConfirmed = true;
            uow.Requests.Update(request);
            await uow.SaveChangesAsync();
            var result = uow.Requests.Get(request.Id);
            result.Should().NotBeNull();
            result.IsConfirmed.Should().BeTrue();
        }

        private ColoborationRequest CreateValidRequest()
        {
            var request = _fixture.Build<ColoborationRequest>()
                                  .Without(r => r.Id)
                                  .Create();

            request.IsConfirmed = false;
            return request;
        }
    }
}
