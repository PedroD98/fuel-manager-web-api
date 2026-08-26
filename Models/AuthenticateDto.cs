using System;
using System.ComponentModel.DataAnnotations;

namespace fuel_manager_web_api.Models;

public class AuthenticateDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Password { get; set; }
}
