using InnoGotchi.BLL.DTO;

namespace InnoGotchiBackendTests
{
    public class PetTests
    {
        [Fact]
        public void GetAgeTest()
        {
            PetDTO pet = new PetDTO
            {
                Name = "pet",
                CreateTime = new DateTime(2022, 10, 3),
                LastDrinkingTime = new DateTime(2022, 10, 3, 20, 10, 0),
                LastFeedingTime = new DateTime(2022, 10, 3, 16, 12, 0)
            };
            int actualAge = pet.GetAge();
            int expectedAge = 3;
            Assert.Equal(actualAge, expectedAge);
        }
    }
}