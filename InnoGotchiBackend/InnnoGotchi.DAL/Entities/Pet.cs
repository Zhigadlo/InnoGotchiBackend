﻿using System.ComponentModel.DataAnnotations.Schema;

namespace InnnoGotchi.DAL.Entities
{
    [Table("Pets")]
    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FeedingCount { get; set; }
        public int DrinkingCount { get; set; }

        public string Appearance { get; set; }

        public int FarmId { get; set; }
        public Farm Farm { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime DeathTime { get; set; }
        public DateTime LastFeedingTime { get; set; }
        public DateTime LastDrinkingTime { get; set; }
        public DateTime FirstHappinessDate { get; set; }
    }
}
