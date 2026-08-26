using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fuel_manager_web_api.Models;

[Table("Usuarios")]
public class Usuario
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "O Nome é obrigatório.")]
    public string Nome { get; set; }
    [Required(ErrorMessage = "A Senha é obrigatória.")]
    public string Password { get; set; }
    [Required(ErrorMessage = "O Perfil é obrigatório.")]
    [Range(0, 2, ErrorMessage = "O Perfil deve ser um valor válido.")]
    public Perfil? Perfil { get; set; }
    public ICollection<VeiculoUsuarios> Veiculos { get; set; }
}

public enum Perfil
{
    [Display(Name = "Administrador")]
    Admin,
    [Display(Name = "Usuário")]
    Usuario
}