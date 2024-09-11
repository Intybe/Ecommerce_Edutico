namespace edutico.Models
{
    public class Login
    {
        // Criando os atributos da Classe Login
        public int codLogin { get; set; }
        public string? usuario { get; set; }
        public string? senha { get; set; }
        public int nivelAcesso { get; set; }
       
    }
}
