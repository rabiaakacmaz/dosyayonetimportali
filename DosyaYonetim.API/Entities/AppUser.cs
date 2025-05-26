using DosyaYonetim.API.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;  
public class AppUser : IdentityUser
{
    public ICollection<UserFile> Files { get; set; }
}

