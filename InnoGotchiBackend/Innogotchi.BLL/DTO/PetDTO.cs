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
        public DateTime DeadTime { get; set; }

        public DateTime LastFeedingTime { get; set; }
        public DateTime LastDrinkingTime { get; set; }
        public DateTime FirstHappinessDate { get; set; }

        public int FarmId { get; set; }
        public FarmDTO Farm { get; set; }

        public int GetAge(DateTime day, DateTime feedingPeriod, DateTime drinkingPeriod)
        {
            if (GetPetState(feedingPeriod, drinkingPeriod) != PetState.Dead)
                return (int)((DateTime.Now - CreateTime).Ticks / day.Ticks);
            else
                return (int)((DeadTime - CreateTime).Ticks / day.Ticks);
        }
        public int GetHappinessDaysCount(DateTime day)
        {
            return (int)((DateTime.Now - FirstHappinessDate).Ticks/ day.Ticks);
        }
           
        public double GetAverageFeedingCount(DateTime day)
        {
            return (DateTime.Now - CreateTime).Ticks / FeedingCount / day.Ticks;
        }
        public double GetAverageDrinkingCount(DateTime day)
        {
            return (DateTime.Now - CreateTime).Ticks / DrinkingCount / day.Ticks;
        }

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
            if ((DateTime.Now - LastFeedingTime).Ticks > feedingPeriod.Ticks * 3)
                return HungerLavel.Dead;
            else if ((DateTime.Now - LastFeedingTime).Ticks > feedingPeriod.Ticks * 2)
                return HungerLavel.Hungry;
            else if ((DateTime.Now - LastFeedingTime).Ticks > feedingPeriod.Ticks)
                return HungerLavel.Normal;
            else
                return HungerLavel.Full;
        }
        public ThirstyLavel GetThirstyLavel(DateTime drinkingPeriod)
        {
            if ((DateTime.Now - LastFeedingTime).Ticks > drinkingPeriod.Ticks * 3)
                return ThirstyLavel.Dead;
            else if ((DateTime.Now - LastFeedingTime).Ticks > drinkingPeriod.Ticks * 2)
                return ThirstyLavel.Thirsty;
            else if ((DateTime.Now - LastFeedingTime).Ticks > drinkingPeriod.Ticks)
                return ThirstyLavel.Normal;
            else
                return ThirstyLavel.Full;
        }
    }
}
