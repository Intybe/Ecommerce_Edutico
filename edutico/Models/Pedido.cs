using edutico.Models;

namespace Edutico.Models
{
    // Classe Pedido contendo informações do pedido em si
    public class Pedido
    {
        public int codLogin { get; set; }         
        public DateTime data { get; set; }      
        public string statusPedido { get; set; }  
        public decimal valorTotal { get; set; }   
        public List<ItemPedido> itensPedido { get; set; } 
    }

    
}


