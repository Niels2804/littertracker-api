﻿using System.ComponentModel.DataAnnotations;

namespace trashtracker_api.Models
{
    public class User
    {
        public string Id { get; set; }
        public string IdentityUserId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        [StringLength(75)]
        public string? Username { get; set; }
        [StringLength(75)]
        public string? FirstName { get; set; }
        [StringLength(75)]
        public string? LastName { get; set; }
        public int Role { get; set; }
        public ICollection<FavoriteLocation>? FavoriteLocations { get; set; }
    }
}
