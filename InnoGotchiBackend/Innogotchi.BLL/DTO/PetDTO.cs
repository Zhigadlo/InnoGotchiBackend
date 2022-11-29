namespace InnoGotchi.BLL.DTO
{
    public class PetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FeedingCount { get; set; }
        public int DrinkingCount { get; set; }

        public AppearanceDTO Appearance { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime LastFeedingTime { get; set; }
        public DateTime LastDrinkingTime { get; set; }
        public DateTime FirstHappinessDate { get; set; }

        public int FarmId { get; set; }
        public FarmDTO Farm { get; set; }

        public PetState GetPetState(DateTime feedingPeriod, DateTime drinkingPeriod)
        {
            if(GetHungryLavel(feedingPeriod) != HungerLavel.Dead 
                && GetThirstyLavel(drinkingPeriod) != ThirstyLavel.Dead)
                return PetState.Alive;
            else
                return PetState.Dead;
        } 

        public HungerLavel GetHungryLavel(DateTime feedingPeriod)
        {
            if ((DateTime.Now - LastFeedingTime).Seconds > feedingPeriod.Second * 3)
                return HungerLavel.Dead;
            else if ((DateTime.Now - LastFeedingTime).Seconds > feedingPeriod.Second * 2)
                return HungerLavel.Hungry;
            else if ((DateTime.Now - LastFeedingTime).Seconds > feedingPeriod.Second)
                return HungerLavel.Normal;
            else
                return HungerLavel.Full;
        }

        public ThirstyLavel GetThirstyLavel(DateTime drinkingPeriod)
        {
            if ((DateTime.Now - LastFeedingTime).Seconds > drinkingPeriod.Second * 3)
                return ThirstyLavel.Dead;
            else if ((DateTime.Now - LastFeedingTime).Seconds > drinkingPeriod.Second * 2)
                return ThirstyLavel.Thirsty;
            else if ((DateTime.Now - LastFeedingTime).Seconds > drinkingPeriod.Second)
                return ThirstyLavel.Normal;
            else
                return ThirstyLavel.Full;
        }
    }
}
