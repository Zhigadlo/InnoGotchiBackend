﻿namespace InnoGotchi.BLL.Models
{
    /// <summary>
    /// Model for pet filtration that helps to get page from server
    /// </summary>
    public class PetFilterModel
    {
        public long Age { get; set; } = 0;
        public long GameYear { get; set; } = 0;

        public int HungerLavel { get; set; } = -1;
        public long FeedingPeriod { get; set; } = 0;
        public bool IsLastHungerStage { get; set; } = false;

        public int ThirstyLavel { get; set; } = -1;
        public long DrinkingPeriod { get; set; } = 0;
        public bool IsLastThirstyStage { get; set; } = false;
    }
}
