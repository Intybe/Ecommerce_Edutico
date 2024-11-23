using System.ComponentModel.DataAnnotations;

namespace edutico.Models
{
    public class Produto
    {
        [Required(ErrorMessage = "O campo é obrigatório.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Apenas números são permitidos.")]
        [MinLength(14, ErrorMessage = "O campo deve ter no máximo 14 caracteres.")]
        public decimal codProd { get; set; }
        [Required(ErrorMessage = "O campo é obrigatório.")]
        [MaxLength(200)]
        public string nomeProd { get; set; }
        [Required(ErrorMessage = "O campo é obrigatório.")]
        [MaxLength(600)]
        public string descricao { get; set; }
        [Required(ErrorMessage = "Selecione uma opção")]
        public Classificacao classificacao { get; set; }
        [Required(ErrorMessage = "Selecione uma opção")]
        public Categoria categoria { get; set; }
        [Required(ErrorMessage = "Selecione uma opção")]
        public List<Habilidade> habilidades { get; set; }
        [Required(ErrorMessage = "O campo é obrigatório.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Apenas números são permitidos.")]

        public decimal valorUnit { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessage = "Apenas números são permitidos.")]
        [Required(ErrorMessage = "O campo é obrigatório.")]
        public int estoque { get; set; }
        [Required(ErrorMessage = "O campo é obrigatório.")]
        public bool statusProd { get; set; }
        [Required(ErrorMessage = "Selecione uma opção")]
        public bool lancamento { get; set; }

        public Imagem img { get; set; }
        [Required(ErrorMessage = "Selecione ao menos 2 imagens")]
        public List<Imagem> imgs { get; set; }
        public List<Avaliacao> avaliacoes { get; set; }
        public Dictionary<int, int> dadosAvaliacoes { get; set; }
        public int qtdAvaliacao { get; set; }
        public int somaAvaliacao { get; set; }
        public List<Produto> relacionados { get; set; }
        public int qtdVendas { get; set; }
        public decimal valorVendas { get; set; }
        public int qtdFavorito { get; set; }


        // Construtor genérico
        public Produto() { }

        // Construtor de prévia dos produtos (Geralmente utilizado na Index)
        public Produto(decimal codProd, string nomeProd, decimal valorUnit, int qtdAvaliacao, int somaAvaliacao, string imgsConcatenadas, bool statusProd)
        {
            this.codProd = codProd;
            this.nomeProd = nomeProd;
            this.valorUnit = valorUnit;
            this.qtdAvaliacao = qtdAvaliacao;
            this.somaAvaliacao = somaAvaliacao;
            this.imgs = ParseImagens(imgsConcatenadas);
            this.statusProd = statusProd;
        }

        // Construtor de prévia dos produtos (Carrinho)
        public Produto(decimal codProd, string nomeProd, decimal valorUnit, string imgsConcatenadas)
        {
            this.codProd = codProd;
            this.nomeProd = nomeProd;
            this.valorUnit = valorUnit;
            this.imgs = ParseImagens(imgsConcatenadas);
        }

        // Construtor para produtos favoritos (Fuuncionário)
        public Produto(decimal codProd, string nomeProd, decimal valorUnit, string imgsConcatenadas, int qtdFavorito)
        {
            this.codProd = codProd;
            this.nomeProd = nomeProd;
            this.valorUnit = valorUnit;
            this.imgs = ParseImagens(imgsConcatenadas);
            this.qtdFavorito = qtdFavorito;
        }

        // Construtor de detalhes do produto
        public Produto(
            decimal codProd,
            string nomeProd,
            string descricao,
            string classificacao,
            string categoria,
            string habilidadesConcatenadas,
            decimal valorUnit,
            int estoque,
            bool statusProd,
            bool lancamento,
            string imgsConcatenadas,
            string detalhesAvaliacaoConcatenados,
            string qtdPorEstrelaConcatenada,
            int qtdAvaliacao,
            int somaAvaliacao
        )
        {
            this.codProd = codProd;
            this.nomeProd = nomeProd;
            this.descricao = descricao;
            this.classificacao = ParseClassificacao(classificacao);
            this.categoria = ParseCategoria(categoria);
            this.habilidades = ParseHabilidades(habilidadesConcatenadas);
            this.valorUnit = valorUnit;
            this.estoque = estoque;
            this.statusProd = statusProd;
            this.lancamento = lancamento;
            this.imgs = ParseImagens(imgsConcatenadas);
            this.avaliacoes = ParseAvaliacoes(detalhesAvaliacaoConcatenados);
            this.dadosAvaliacoes = ParseDadosAvaliacoes(qtdPorEstrelaConcatenada);
            this.qtdAvaliacao = qtdAvaliacao;
            this.somaAvaliacao = somaAvaliacao;
        }


        // Construtor para criar um produto para cadastro
        public Produto(
            decimal codProd,
            string nomeProd,
            string descricao,
            string classificacao,
            string categoria,
            string habilidadesConcatenadas,
            decimal valorUnit,
            int estoque,
            bool lancamento
        )
        {
            this.codProd = codProd;
            this.nomeProd = nomeProd;
            this.descricao = descricao;
            this.classificacao = ParseClassificacao(classificacao);
            this.categoria = ParseCategoria(categoria);
            this.habilidades = ParseHabilidades(habilidadesConcatenadas);
            this.valorUnit = valorUnit;
            this.estoque = estoque;
            this.lancamento = lancamento;
        }

        // Construtor para prévia dos produtos no dashboard
        public Produto(decimal codProd, string nomeProd, decimal valorUnit, string imgsConcatenadas, int qtdVendas, decimal valorVendas)
        {
            this.codProd = codProd;
            this.nomeProd = nomeProd;
            this.valorUnit = valorUnit;
            this.qtdVendas = qtdVendas;
            this.valorVendas = valorVendas;
            this.imgs = ParseImagens(imgsConcatenadas);
        }


        // Método para converter a classificacao concatenada
        private Classificacao ParseClassificacao(string classificacaoBD)
        {
            // Verifica se a string está nula ou vazia
            if (!string.IsNullOrEmpty(classificacaoBD))
            {
                // Divide e coloca a classificação e seu código em um vetor
                var vClassificacao = classificacaoBD.Split(" - ");

                // Cria um objeto o tipo classificacao
                Classificacao classificacao = new Classificacao()
                {
                    // O codClassificacao fica no índice zero
                    codClassificacao = Convert.ToInt32(vClassificacao[0]),

                    // O nomeClassificacao fica no índice um
                    nomeClassificacao = vClassificacao[1]
                };
                return classificacao; // Encerra o método e retorna os dados
            }
            return null; // Se a string estiver vazia retorna null

        }


        // Método para converter a categoria concatenada
        private Categoria ParseCategoria(string categoriaBD)
        {
            // Veirifica se tem alguma categoria (Deveria ter)
            if (!string.IsNullOrEmpty(categoriaBD))
            {
                // Divide e coloca a habilidade e seu código em um vetor
                var vCategoria = categoriaBD.Split(" - ");

                // Cria um objeto do tipo categoria
                Categoria categoria = new Categoria()
                {
                    // O codCategoria fica no índice zero
                    codCategoria = Convert.ToInt32(vCategoria[0]),

                    // O nomeCategoria fica no índice um
                    nomeCategoria = vCategoria[1]
                };
                return categoria; // Encerra o método e retorna os dados
            }
            return null; // Se a categoria estiver vazia retorna null

        }


        // Método para converter habilidades concatenadas
        private List<Habilidade> ParseHabilidades(string habilidadesBD)
        {
            // Cria um lista de objeto do tipo Habilidade
            List<Habilidade> habilidades = new List<Habilidade>();

            // Verifica se há alguma habilidade na string
            if (!string.IsNullOrEmpty(habilidadesBD))
            {
                // Irá criar um objeto habilidade e adicioná-lo na lista para cada linha
                foreach (var campoHabilidade in (habilidadesBD.Split(" | ")))
                {
                    // Divide e coloca a habilidade e seu código em um vetor
                    var habilidade = campoHabilidade.Split(" - ");

                    // Cria e adiciona um objeto habilidade na lista
                    habilidades.Add(new Habilidade
                    {
                        // O código da habilidade fica no índice zero
                        codHabilidade = Convert.ToInt32(habilidade[0]),

                        // O nome da habilidade fica no índice um
                        nomeHabilidade = habilidade[1]
                    });
                }
                return habilidades; // Encerra o método e retorna os dados
            }
            return null; // Caso a string esteja vazia retorna null

        }


        // Método para converter imagens concatenadas para lista de objetos `Imagem`
        private List<Imagem> ParseImagens(string imgsBD)
        {
            // Cria um objeto do tipo Imagem
            List<Imagem> imagens = new List<Imagem>();

            // Veirifica se a string está vazia
            if (!string.IsNullOrEmpty(imgsBD))
            {
                // Pega a string e separa em "objeto" imagem
                foreach (var imagem in (imgsBD.Split(" | ")))
                {
                    // Separa as informações da string (objeto) e coloca em um vetor
                    var vImagem = imagem.Split(" -- ");

                    // Cria um objeto imagem e adiciona na lista
                    imagens.Add(new Imagem
                    {
                        nomeImg = vImagem[0],
                        enderecoImg = vImagem[1]
                    });
                }
                return imagens; // Encerra o método e retorna os dados
            }
            return null; // Se a string estiver vazia retorna null
        }


        // Método para converter avaliações concatenadas para lista de objetos `Avaliacao`
        private List<Avaliacao> ParseAvaliacoes(string avaliacoesBD)
        {
            //  Cria uma lista de objetos Avaliacao
            List<Avaliacao> avaliacoes = new List<Avaliacao>();

            // Verifica se tem alguma avaliação
            if (!string.IsNullOrEmpty(avaliacoesBD))
            {
                // A cada "|" transforma em uma linha para tratamento
                foreach (var avaliacao in (avaliacoesBD.Split(" | ")))
                {
                    // Seprara as informações de uma avaliação em um vetor
                    var vAvaliacao = avaliacao.Split(" - ");

                    // Cria um objeto avalição e adiciona na lista
                    avaliacoes.Add(new Avaliacao
                    {
                        qtdEstrela = int.Parse(vAvaliacao[0]),
                        comentario = vAvaliacao[1],
                        cliente = new Cliente
                        {
                            codLogin = int.Parse(vAvaliacao[2]),
                            nome = vAvaliacao[3]
                        }
                    });
                }
                return avaliacoes; // Encerra o método e retorna os dados
            }
            return null; // Se a string estiver vazia retorna null
        }


        // Método para converter estatísticas de avaliações para o dicionário
        private Dictionary<int, int> ParseDadosAvaliacoes(string dadosAvalicaoBD)
        {
            // Cria um dicionário de dados ints para guardar a quantidade de avalições por estrela
            // Precisa ser um diconário para utlizar no JS na tela detalhe produto (progress bar)
            Dictionary<int, int> dados = new Dictionary<int, int>();

            // Verifica se a string está nula ou vazia
            if (!string.IsNullOrEmpty(dadosAvalicaoBD))
            {
                // Separa as quantidades por estrela
                var qtdPorEstrela = dadosAvalicaoBD.Split(", ");

                // Cria um looping para repetir o processo a partir do 5
                for (int i = 5; i >= 1; i--)
                {
                    // Á primeira quantidade equivale a 5 estrelas e assim por diante
                    dados[i] = int.Parse(qtdPorEstrela[5 - i]);
                }

                return dados; // Encerra o método e retorna os dados
            }
            return null; // Se a string estiver vazia retorna null
        }
    }
}
