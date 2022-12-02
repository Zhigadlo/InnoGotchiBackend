using InnoGotchi.BLL.BusinessModels;

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

        public int GetAge()
        {
            if (GetPetState() != PetState.Dead)
                return (int)((DateTime.Now - CreateTime).Ticks / Globals.InnoGotchiDay.Ticks);
            else
                return (int)((DeadTime - CreateTime).Ticks / Globals.InnoGotchiDay.Ticks);
        }
        public int GetHappinessDaysCount()
        {
            return (int)((DateTime.Now - FirstHappinessDate).Ticks / Globals.InnoGotchiDay.Ticks);
        }

        public double GetAverageFeedingPeriod() => (DateTime.Now - CreateTime).Ticks / FeedingCount / Globals.InnoGotchiDay.Ticks;
        public double GetAverageDrinkingPeriod() => (DateTime.Now - CreateTime).Ticks / DrinkingCount / Globals.InnoGotchiDay.Ticks;


        public PetState GetPetState()
        {
            if (GetHungryLavel() != HungerLavel.Dead
                && GetThirstyLavel() != ThirstyLavel.Dead)
                return PetState.Alive;
            else
                return PetState.Dead;
        }
        public HungerLavel GetHungryLavel()
        {
            if ((DateTime.Now - LastFeedingTime).Ticks > Globals.FeedingPeriod.Ticks * 3)
                return HungerLavel.Dead;
            else if ((DateTime.Now - LastFeedingTime).Ticks > Globals.FeedingPeriod.Ticks * 2)
                return HungerLavel.Hungry;
            else if ((DateTime.Now - LastFeedingTime).Ticks > Globals.FeedingPeriod.Ticks)
                return HungerLavel.Normal;
            else
                return HungerLavel.Full;
        }
        public ThirstyLavel GetThirstyLavel()
        {
            if ((DateTime.Now - LastFeedingTime).Ticks > Globals.DrinkingPeriod.Ticks * 3)
                return ThirstyLavel.Dead;
            else if ((DateTime.Now - LastFeedingTime).Ticks > Globals.DrinkingPeriod.Ticks * 2)
                return ThirstyLavel.Thirsty;
            else if ((DateTime.Now - LastFeedingTime).Ticks > Globals.DrinkingPeriod.Ticks)
                return ThirstyLavel.Normal;
            else
                return ThirstyLavel.Full;
        }
    }
}
