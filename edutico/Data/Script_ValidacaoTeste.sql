-- Inserção dos Clientes --
Delimiter $$
 	Call spInsertTbCliente(10399833309, 'Cleia', 'Santana Alves', 11966163308, 'cleia@gmail.com', 'teste@123', 'Rua Angelina', 'Latrim', 'Rio de Janeiro','RJ', '12345058', 91, 'Residência');
	Call spInsertTbCliente(50780848859, 'Mario', 'Silva Cruz', 11966163308, 'mario@gmail.com', 'teste@123', 'Rua Angelina', 'Lapa', 'São Paulo','SP', '12345059', 60, 'Apartamento 309');
$$

-- Inserção do Login Intybe e Edutico --
Delimiter $$
	Insert into tbLogin(usuario, senha, nivelAcesso) values('manutencao.tech', '12345678', 0);
    Insert into tbLogin(usuario, senha, nivelAcesso) values('edutico.loja', '12345678', 1);
$$

/*

Select * from tbCarrinho Inner Join vwProduto On tbCarrinho.codProd = Código Left Join tbImagem On Código = tbImagem.codProd where codLogin = 1;

Select * from tbProduto;

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

Select * from tbPedido;

Update tbPedido set statusPedido = 2 where NF = 6;

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

Select * from tbProduto;

Update tbPedido Set StatusPedido = 2 where NF = 1;
Update tbPedido Set StatusPedido = 2 where NF = 7;

Delimiter $$
	Call spInsertTbAvaliacao(5, 'Meu filho amou', 1, 12345678910120);
    Call spInsertTbAvaliacao(4, 'Gostei, cumpre o que promete', 2, 12345678910120);
    Call spInsertTbAvaliacao(2, 'Horrível! Não funciona', 2, 12345678910121);
$$

Select * from tbPedido;

Select * from tbAvaliacao;

Delimiter $$
	Call spInsertTbDevolucao('Simples não funciona. Meu filho ficou frustrado por se tratar de uma propaganda enganosa', 3);
$$

Select * from tbDevolucao;
SET SQL_SAFE_UPDATES = 0;


Select * from tbImagem;

Update tbImagem Set enderecoImg = '~/imgs/253ae6ed-59da-4fd1-9eb8-350f0b794b1b.png';

Call spSelectTbCartoes(1);

Call spSelectTbAvaliacao(7896054025112);

Call spSelectTbProdutoClassificacao(3);

Call spSelectTbProdutoCategoria(5);

Call spSelectTbProdutoHabilidade(2);

Call spSelectHabilidadeProduto(7896054025112);

Select * from tbImagem;

Select * from tbPedido;
*/

