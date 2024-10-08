namespace Edutico.Models
{
    public class Pedido
    {
        public int codItem { get; set; }         
        public string NF { get; set; }           
        public int codProd { get; set; }          
        public int qtdItem { get; set; }          
        public decimal valorItem { get; set; }   
        public DateTime data { get; set; }        
        public int codLogin { get; set; }        
        public string statusPedido { get; set; }  
        public decimal valorTotal { get; set; }  
    }
}

