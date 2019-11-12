﻿namespace RX.Nyss.Web.Features.User.Dto
{
    public class GetNationalSocietyUsersResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string Project { get; set; }
        public bool IsHeadManager { get; set; }
        public bool IsPendingHeadManager { get; set; }
    }
}
