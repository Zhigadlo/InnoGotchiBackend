using AutoMapper;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Mapper;
using InnoGotchi.BLL.Services;

namespace InnoGotchiBackendTests.RequestTests
{
    public class RequestServiceTests
    {
        private IFixture _fixture;
        public RequestServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                    .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var contextOptions = new DbContextOptionsBuilder<InnoGotchiContext>()
                    .UseInMemoryDatabase(nameof(RequestServiceTests))
                    .Options;
            var config = new MapperConfiguration(cnf => cnf.AddProfile<MapperProfile>());
            _fixture.Register(() => new RequestService(new InnoGotchiUnitOfWork(contextOptions), new Mapper(config)));
            _fixture.Register(() => new UserService(new InnoGotchiUnitOfWork(contextOptions), new Mapper(config)));
        }

        [Fact]
        public async Task ConfirmRequestTestAsync()
        {
            var request = CreateValidRequest();
            var userService = _fixture.Create<UserService>();
            var requestService = _fixture.Create<RequestService>();
            var owner = CreateValidUserDTO();
            var receipient = CreateValidUserDTO();
            request.RequestOwnerId = await userService.CreateAsync(owner);
            request.RequestReceipientId = await userService.CreateAsync(receipient);
            var id = await requestService.CreateAsync(request);
            id.Should().NotBe(-1);
            (await requestService.ConfirmAsync(id)).Should().BeTrue();
        }

        private ColoborationRequestDTO CreateValidRequest()
        {
            var request = _fixture.Build<ColoborationRequestDTO>()
                                  .Without(r => r.Id)
                                  .Without(r => r.RequestOwner)
                                  .Without(r => r.RequestReceipient)
                                  .Create();

            request.IsConfirmed = false;
            return request;
        }

        private UserDTO CreateValidUserDTO()
        {
            var user = _fixture.Build<UserDTO>()
                                .Without(u => u.Id)
                                .Without(u => u.ColoborationRequests)
                                .Without(u => u.ReceivedRequests)
                                .Without(u => u.SentRequests)
                                .Without(u => u.Farm)
                                .Create();
            user.Email += "@gmail.com";
            return user;
        }
    }
}
