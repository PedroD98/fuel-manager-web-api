using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace fuel_manager_web_api.Models;

[Table("VeiculoUsuarios")]
public class VeiculoUsuarios
{
    public int VeiculoId { get; set; }
    public Veiculo Veiculo { get; set; }
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }
}
