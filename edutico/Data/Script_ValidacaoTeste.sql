





Delimiter $$
	Call spInsertTbCliente(10399833309, 'Cleia', 'Santana Alves', 11966163308, 'cleia@gmail.com', 'teste@123', 'Rua Angelina', 'Latrim', 'Rio de Janeiro','RJ', '12345058', 91, 'Residência');
	Call spInsertTbCliente(50780848859, 'Mario', 'Silva Cruz', 11966163308, 'mario@gmail.com', 'teste@123', 'Rua Angelina', 'Lapa', 'São Paulo','SP', '12345059', 60, 'Apartamento 309');
$$

Delete from tbLogin where codLogin = 2;

Select * from vwCliente;

Call spSelectTbLogin('cleia@gmail.com', 'teste@123');

Delimiter $$
	Call spInsertTbCategoria('Jogos de Tabuleiro');
    Call spInsertTbCategoria('Jogos de Ação');
    Call spInsertTbCategoria('Jogos de Cartas');
    Call spInsertTbCategoria('Jogos de Eletrônicos');
    Call spInsertTbCategoria('Blocos de Montar');
    Call spInsertTbCategoria('Quebra-Cabeças');
    Call spInsertTbCategoria('Musicais');
$$

Delimiter $$
	Call spInsertTbClassificação('0 a 12 meses');
    Call spInsertTbClassificação('1 a 2 anos');
    Call spInsertTbClassificação('2 a 4 anos');
    Call spInsertTbClassificação('5 a 7 anos');
    Call spInsertTbClassificação('8 a 11 anos');
    Call spInsertTbClassificação('12 anos ou +');
$$

Delimiter $$
	Call spInsertTbHabilidade('Coordenação Motora');
    Call spInsertTbHabilidade('Cognição e Aprendizado');
    Call spInsertTbHabilidade('Raciocínio Lógico');
    Call spInsertTbHabilidade('Criatividade');
$$

Delimiter $$
	Call spInsertTbProduto(7896054025112, 'Blocos de Montar - Tand Kids - Baú com 120 Peças - Amarelo', 
		'A criança poderá se divertir com os Blocos de Montar - Tand Kids - Baú com 120 Peças da Toyster. Levando o seu baú para todos os lugares com a alça de transporte, a criança poderá se divertir muito e no final da brincadeira poder guardar as peças na caixa para manter organizado! Com os Blocos de Montar - Tand Kids, as crianças estimularão a imaginação enquanto brincam, unindo as peças e montando da melhor forma que desejarem, quando quiser é só desmontar e fazer outra coisa mega divertida.', 
        '2 a 4 anos', 
        'Blocos de Montar', 99.99, 100);
	Call spInsertTbProduto(7898578201263, 'Pianinho Banda da Selva', 
		'Estimule o desenvolvimento dos sentidos de seu pequeno artista com o Pianinho Banda da Selva da WinFun! Com simpáticos animaizinhos, cores vibrantes, luz e divertidos sons, o Pianinho Banda da Selva possui 2 modos diferentes de brincar: Um com diferentes músicas em cada uma das teclas e outro com notas musicais e efeitos sonoros. Os pequeninos vão se encantar!', 
        '2 a 4 anos', 
        'Musicais', 189.90, 230);
	Call spInsertTbProduto(7896027524796, 'Jogo Detetive com App', 
		'Jogo Detetive com App - Estrela! Um jogo de investigação acima de qualquer suspeita! Tudo começou na mansão de um rico industrial, Dr. Pessoa, a vítima do crime. Como um verdadeiro Sherlock, você está lá. Só que além de detetive, você também é um suspeito! Para chegar cada vez mais perto da solução deste mistério, vá entrando com seu peão nos possíveis locais do crime e dando palpites sobre o culpado e arma usada. Tire sua deduções e descubra a cada partida, um novo e emocionante mistério! As imagens são meramente ilustrativas, podendo sofrer alterações de cores na embalagem', 
        '8 a 11 anos', 
        'Jogos de Tabuleiro', 189.90, 50);
$$

Delimiter $$
	Call spInsertTbHabilidade_Produto(7896054025112, 2);
    Call spInsertTbHabilidade_Produto(7896054025112, 4);
    Call spInsertTbHabilidade_Produto(7898578201263, 4);
    Call spInsertTbHabilidade_Produto(7898578201263, 2);
    Call spInsertTbHabilidade_Produto(7896027524796, 3);
$$

Select * from tbImagem;
Select * from vwProduto;

Delimiter $$
	Call spInsertTbCarrinho(1, 7896054025112, 1);
    Call spInsertTbCarrinho(1, 7896054025112, 1);
    Call spInsertTbCarrinho(2, 7896054025112, 1);
    Call spInsertTbCarrinho(2, 7898578201263, 1);
    Call spInsertTbCarrinho(2, 7896027524796, 1);
$$

Select * from tbCarrinho;

Delimiter $$
	Call spInsertTbFavorito(1, 7896054025112);
    Call spInsertTbFavorito(1, 7898578201263);
    Call spInsertTbFavorito(2, 7896054025112);
$$

Select * from tbFavorito;

Delimiter $$
	Call spInsertTbPedido(1);
	Call spInsertTbPedido(2);
$$

Select * from tbPedido;

Delimiter $$
	Call spInsertTbItemPedido(1, 7896054025112, 2);
    Call spInsertTbItemPedido(2, 7896054025112, 1);
    Call spInsertTbItemPedido(2, 7898578201263, 1);
$$

Select * from tbItemPedido;

Delimiter $$
	Call spInsertTbCartaoCredito(1234567890123456, 'Cleia Santana Alves', '06/27', 6, 1);
$$

Select * from tbCartaoCredito;

Delimiter $$
	Call spInsertTbPagamento(2, 1, '1', 1);
    Call spInsertTbPagamento(1, 2, 'ABGF45HS679J', 0);
$$

Select * from tbPagamento;

Update tbPedido Set StatusPedido = 2 where NF = 1;
Update tbPedido Set StatusPedido = 2 where NF = 2;

Delimiter $$
	Call spInsertTbAvaliacao(5, 'Meu filho amou', 1, 7896054025112);
    Call spInsertTbAvaliacao(4, 'Gostei, cumpre o que promete', 2, 7896054025112);
    Call spInsertTbAvaliacao(2, 'Horrível! Não funciona', 2, 7898578201263);
$$

Select * from tbAvaliacao;

Delimiter $$
	Call spInsertTbDevolucao('Simples não funciona. Meu filho ficou frustrado por se tratar de uma propaganda enganosa', 3);
$$

Select * from tbDevolucao;


Call spSelectTbCartoes(1);

Call spSelectTbAvaliacao(7896054025112);

Call spSelectTbProdutoClassificacao(3);

Call spSelectTbProdutoCategoria(5);

Call spSelectTbProdutoHabilidade(2);

Call spSelectHabilidadeProduto(7896054025112);