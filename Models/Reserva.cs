using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestauranteMvc.Models
{
    public class Reserva
{
    [Key]
    public int ReservaId { get; set; }
    
    [Required(ErrorMessage = "Data é obrigatória")]
    [DisplayName("Data")]
    [DataType(DataType.Date)]
    public DateTime Data { get; set; }
    
    [Required(ErrorMessage = "Horário é obrigatório")]
    [DisplayName("Horário")]
    [DataType(DataType.Time)]
    public TimeSpan Horario { get; set; }
    
    [Required(ErrorMessage = "Número de pessoas é obrigatório")]
    [DisplayName("Número de Pessoas")]
    [Range(1, 20, ErrorMessage = "Número de pessoas deve estar entre 1 e 20")]
    public int NumeroPessoas { get; set; }
    
    [Required]
    public int UsuarioId { get; set; }
    
    public virtual Usuario Usuario { get; set; }
    
    [DisplayName("Observações")]
    public string Observacoes { get; set; }
    
    [DisplayName("Status")]
    public StatusReserva Status { get; set; }
}

public enum StatusReserva
{
    Pendente,
    Confirmada,
    Cancelada,
    Concluida
}

}