using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fuel_manager_web_api.Models;

[Table("Consumos")]
public class Consumo
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "A Descrição é obrigatória.")]
    public string Descricao { get; set; }
    [Required(ErrorMessage = "A Data é obrigatória.")]
    public DateTime? Data { get; set; }
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "O Valor deve ser maior que zero.")]
    [Column(TypeName ="decimal(18,2)")]
    public decimal Valor { get; set; }
    [Required(ErrorMessage = "O Tipo é obrigatório.")]
    [Range(0, 2, ErrorMessage = "O Tipo deve ser um valor válido.")]
    public TipoCombustivel? Tipo { get; set; }
    [Required(ErrorMessage = "O Veículo é obrigatório.")]
    public int? VeiculoId { get; set; }
    public Veiculo Veiculo { get; set; }
}

public enum TipoCombustivel
{
    Diesel,
    Etanol,
    Gasolina
}
