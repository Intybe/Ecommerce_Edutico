using System.ComponentModel.DataAnnotations;

namespace edutico.Models
{
    public class Cartao
    {
        public int codCartao { get; set; }
        [StringLength(9, MinimumLength = 9)]
        [Required(ErrorMessage = "Esse campo é obrigatório")]
        public decimal numCartao { get; set; }
        [StringLength(200)]
        [Required(ErrorMessage = "Esse campo é obrigatório")]
        public string nomeTitular { get; set; }
        [RegularExpression(@"^(0[1 - 9] | 1[0 - 2])\/\d{2}$", ErrorMessage = "Essa data não é válida")]
        [Required(ErrorMessage = "Esse campo é obrigatório")]
        public string dataVali { get; set; }
        [Required(ErrorMessage = "Esse campo é obrigatório")]
        public string bandeira { get; set; }
        [Required(ErrorMessage = "Esse campo é obrigatório")]
        [StringLength(5)]
        public int codLogin { get; set; }

    }
}
