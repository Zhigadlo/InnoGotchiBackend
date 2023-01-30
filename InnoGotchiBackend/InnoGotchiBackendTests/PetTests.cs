using AutoMapper;
using InnnoGotchi.DAL.Entities;
using InnoGotchi.BLL.Mapper;

namespace InnoGotchiBackendTests
{
    public class PetTests
    {
        [Fact]
        public void Test()
        {
            var config = new MapperConfiguration(cnf => cnf.AddProfile<MapperProfile>());
            var mapper = new Mapper(config);
            var test = mapper.Map<Farm>(null);
            Assert.Equal(null, test);
        }
    }
}