﻿namespace FondoXYZ.web.Models
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public string? NombreCompleto { get; set; }
    }

}
