using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace RestauranteMvc.Dtos
{
    public class ReservaViewDto
    {
        [Required(ErrorMessage = "Data é obrigatória")]
        [DisplayName("Data")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }
        
        [Required(ErrorMessage = "Horário é obrigatório")]
        [DisplayName("Horário")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan Horario { get; set; }
        
        [Required(ErrorMessage = "Número de pessoas é obrigatório")]
        [DisplayName("Número de Pessoas")]
        [Range(1, 20, ErrorMessage = "Número de pessoas deve estar entre 1 e 20")]
        public int NumeroPessoas { get; set; }
        
        [DisplayName("Observações")]
        public string Observacoes { get; set; }
        
        // Propriedades adicionais que podem ser úteis na view
        [DisplayName("Nome")]
        public string NomeCliente { get; set; }
        
        [DisplayName("Telefone")]
        public string TelefoneCliente { get; set; }
        
        // Propriedade calculada para exibir data e hora juntas
        [DisplayName("Data e Hora")]
        public string DataHoraFormatada => $"{Data.ToShortDateString()} às {Horario.ToString(@"hh\:mm")}";
    }
}
