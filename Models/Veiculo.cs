using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fuel_manager_web_api.Models;

[Table("Veiculos")]
public class Veiculo : LinksHATEOS
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "A Marca é obrigatória.")]
    public string Marca { get; set; }
    [Required(ErrorMessage = "O Modelo é obrigatório.")]
    public string Modelo { get; set; }
    [Required(ErrorMessage = "A Placa é obrigatória.")]
    public string Placa { get; set; }
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "O Ano de Fabricação deve ser maior que zero.")]
    public int AnoFabricacao { get; set; }
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "O Ano do Modelo deve ser maior que zero.")]
    public int AnoModelo { get; set; }
    public ICollection<Consumo> Consumos { get; set; }
}
