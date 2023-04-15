﻿using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models.Dtos
{
    public class NationalParkDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";  // "" is space, which is not same as Null
        public string State { get; set; } = "";
        public DateTime Created { get; set; }
        public DateTime Established { get; set; }
    }
}
