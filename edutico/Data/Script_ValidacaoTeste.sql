Use dbEdutico;

-- Inserção dos Clientes --
Delimiter $$
 	Call spInsertTbCliente(10399833309, 'Cleia', 'Santana Alves', 11988205608, 'cleia@gmail.com', 'teste@123', 'Rua Angelina', 'Latrim', 'Rio de Janeiro','RJ', '12345058', 91, 'Residência');
	Call spInsertTbCliente(50780848859, 'Mario', 'Silva Cruz', 11988205608, 'mario@gmail.com', 'teste@123', 'Rua Angelina', 'Lapa', 'São Paulo','SP', '12345059', 60, 'Apartamento 309');
	CALL spInsertTbCliente(20399833300, 'Ana', 'Lima Santos', 11988205609, 'ana@gmail.com', 'senha123', 'Av. Paulista', 'Bela Vista', 'São Paulo', 'SP', '12345060', 65, 'Residência');
CALL spInsertTbCliente(40390844850, 'Carlos', 'Pereira Souza', 21999204509, 'carlos@gmail.com', 'senha456', 'Rua das Flores', 'Botafogo', 'Rio de Janeiro', 'RJ', '12345061', 75, 'Apartamento 101');
CALL spInsertTbCliente(50780543877, 'Juliana', 'Ferreira Costa', 31988503458, 'juliana@gmail.com', 'teste123', 'Rua das Acácias', 'Centro', 'Belo Horizonte', 'MG', '12345062', 50, 'Residência');
CALL spInsertTbCliente(70190832211, 'Rafael', 'Almeida Rocha', 21998003467, 'rafael@gmail.com', 'senha789', 'Av. Atlântica', 'Copacabana', 'Rio de Janeiro', 'RJ', '12345063', 35, 'Apartamento 302');
CALL spInsertTbCliente(60190544859, 'Fernanda', 'Mendes Oliveira', 51988205600, 'fernanda@gmail.com', 'teste2023', 'Rua do Sol', 'Cidade Baixa', 'Porto Alegre', 'RS', '12345064', 90, 'Residência');
CALL spInsertTbCliente(80780848850, 'Eduardo', 'Vieira Martins', 41988205608, 'eduardo@gmail.com', 'senha2023', 'Rua XV de Novembro', 'Centro', 'Curitiba', 'PR', '12345065', 48, 'Residência');
CALL spInsertTbCliente(50790848859, 'Beatriz', 'Nascimento Lopes', 22988205609, 'beatriz@gmail.com', 'senha321', 'Rua da Paz', 'Centro', 'Niterói', 'RJ', '12345066', 30, 'Apartamento 203');
CALL spInsertTbCliente(30790544851, 'Marcelo', 'Silva Dias', 11988205607, 'marcelo@gmail.com', 'senha654', 'Rua Pedro II', 'Centro', 'Fortaleza', 'CE', '12345067', 80, 'Residência');
$$



-- Inserção do Login Edutico --
Delimiter $$
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

use dbEdutico;

Delimiter $$
	Call spInsertTbCartaoCredito(1234567890123456, 'Cleia Santana Alves', '06/27', 'Visa', 1);
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

Create Procedure spInsertTbCarrinho(
	vCodLogin int, 
    vCodProd decimal(14,0), 
    vQtdProd int
)

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


Select * from tbPedido;
