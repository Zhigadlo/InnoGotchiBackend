using AutoMapper;
using InnoGotchi.BLL.DTO;
using InnoGotchi.BLL.Mapper;
using InnoGotchi.BLL.Services;
using InnoGotchi.Web.Controllers;
using InnoGotchi.Web.Mapper;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchiBackendTests.RequestTests
{
    public class RequestControllerTests
    {
        private IFixture _fixture;
        public RequestControllerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                    .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var contextOptions = new DbContextOptionsBuilder<InnoGotchiContext>()
                    .UseInMemoryDatabase(nameof(RequestControllerTests))
                    .Options;
            var config = new MapperConfiguration(cnf => cnf.AddProfiles(new List<Profile> { new MapperProfile(), new ViewModelProfile() }));
            _fixture.Register(() => new RequestService(new InnoGotchiUnitOfWork(contextOptions), new Mapper(config)));
            _fixture.Register(() => new UserService(new InnoGotchiUnitOfWork(contextOptions), new Mapper(config)));
            _fixture.Register(() => new RequestsController(_fixture.Create<RequestService>(), new Mapper(config)));
        }
        [Fact]
        public async Task ConfirmTestAsync()
        {
            var requestService = _fixture.Create<RequestService>();
            var controller = _fixture.Create<RequestsController>();
            var userService = _fixture.Create<UserService>();
            var owner = CreateValidUserDTO();
            var receipient = CreateValidUserDTO();
            var ownerId = await userService.CreateAsync(owner);
            var receipientId = await userService.CreateAsync(receipient);
            var request = CreateValidRequestDTO();
            request.RequestOwnerId = ownerId;
            request.RequestReceipientId = receipientId;
            var requestId = await requestService.CreateAsync(request);
            requestId.Should().NotBe(-1);
            var result = await controller.ConfirmRequestAsync(requestId);
            result.Should().BeOfType<OkResult>();
        }

        private ColoborationRequestDTO CreateValidRequestDTO()
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
