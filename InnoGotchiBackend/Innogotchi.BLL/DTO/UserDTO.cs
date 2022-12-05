﻿using InnnoGotchi.DAL.Entities;

namespace InnoGotchi.BLL.DTO
{
    public class UserDTO
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? AvatarURL { get; set; }

        public List<ColoborationRequest> SentRequests { get; set; }
        public List<ColoborationRequest> ReceivedRequests { get; set; }
        public List<ColoborationRequestDTO> ColoborationRequests { get; set; } = new List<ColoborationRequestDTO>();

        public int FarmId { get; set; }
        public FarmDTO? Farm { get; set; }
    }
}
