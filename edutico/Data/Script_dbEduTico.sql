/*						SUMÁRIO
	Para utilizar basta copiar o título da área que deseja ir, precionar
    Ctrl+F, colar na barra de pesquisa e dar um Enter.

	Criando Database
    Criado as Tabelas
    Adicionando as chaves estrangeiras
		FK tbCliente
        FK tbEndereco
        FK tbProduto
		FK tbHabilidade_Produto
        FK tbImagem
        FK tbFavorito
        FK tbAvaliacao
        FK tbPedido
        FK tbItemPedido
        FK tbCarrinho
        FK tbDevolucao
        FK tbPagamento
        FK tbCartaoCredito
	Procedures de Cadastro (Insert)
		Sessão Cadastro de Cliente
        Sessão Cadastro de Categoria de Produto
        Sessão Cadastro de Classificação Indicativa do Produto
        Sessão de Cadastro de Habilidade trabalhada do Produto
		Sessão de Cadastro da tabela Habilidade_Produto pois era relacionamento N:N
        Sessão de Cadastro de Produtos
        Sessão de Casdastro da Imagem do Produto
        Sessão de Cadastro dos Produtos favoritos
        Sessão de Cadastro de Produtos no Carrinho
        Sessão de Cadastro de Pedidos
        Sessão de Cadastro dos Itens do Pedido
        Sessão de Cadastro de Cartão de Crédito
        Sessão de Cadastro do Pagamento do Pedido
        Sessão de Cadastro de Avaliações
        Sessão de Cadastro de Devoluções
	Procedures Alterações (Update)
        Sessão de Atualização da Senha
        Sessão de Atualização da Quantidade Carrinho
        Sessão de Atualização dos Dados do Cliente (Informações Pessoais)
        Sessão de Atualização dos Dados do Cliente (Endereço)
        Sessão de Atualização dos Dados do Cliente (Dados Conta)
        Sessão de Atualização de Status do Pedido
        Sessão de Atualização de Produto
        Sessão de Atualização das Imagens do Produto
        Sessão de Atualização de Avaliações
        Sessão de Atualização do Status da devolução
		Sessão de Ativação do Produto
		Sessão de Atualização de Status do Produto
	Procedures de Exclusão (Delete)
		Sessão de Exclusão do Produto
		Sessão de Exclusão de Favoritos
        Sessão de Exclusão de Avaliação
        Sessão de Exclusão de Produtos do Carrinho
	Triggers
		Sessão de Redução de estoque
		Sessão de Deletar os produtos do carrinho após realização do pedido
	Views e Consultas
		View para unificar as informações do produto
        Procedure para Selecionar os Produtos do Funcionario
        Procedure para Selecionar os Produtos Relacionados
		Procedure para Selecionar os Produtos Lancamentos
		Procedure para Selecionar os Detalhes Produtos
        View para selecionar os produtos mais vendidos
        Consulta para obter as informações dos produtos mais vendidos
        View para selecionar os produtos mais favoritado
        Consulta para obter as informações dos produtos mais favoritado
        View para selecionar os produtos mais comentados
        Procedure para selecionar as avaliações do produto
        Procedure de validação do login
        View para unificar os dados do Cliente
        Procedure para selecionar os cartões de um cliente
        Procedure para filtrar produtos por classifcação
        Procedure para filtrar produto por categoria
        Procedure para filtrar os produtos por habilidade
        Procedure para Consultar os produtos no carrinho
        Procedure para Selecionar os produtos favoritados do usuário
        Procedure para selecionar as avaliações unicas do cliente
		View para unificar as informações do pedido
		Procedure para Selecionar todos os pedidos do usuário
		Procedure para Selecionar os detalhes do pedido
        Consulta para contar a quantidade de vendas no mês
        Consulta para contar a quantidade de vendas em R$ no mês
        Consulta para contar a quantidade de vendas em R$ no ano
        Consulta para contar a quantidade de vendas em R$ na semana
        Consulta da quantidade de devoluções
        Consulta da quantidade de pedidos pendentes
        Consulta da quantidade de produtos esgotados
        Consulta da quntidade de pedidos cancelados
        
	Inserção de Dados
		Inserção das Categorias
		Inserção das Classificação Indicativa
		Inserção da Habilidade do Produto
*/

-- Criando Database --
Drop database if exists dbEduTico;
Create database dbEduTico;
Use dbEduTico;

-- Criado as Tabelas --
Create table tbLogin(
	codLogin int primary key auto_increment,
	usuario varchar(200) unique not null,
    senha varchar(20) not null,
    nivelAcesso int not null
);

Create table tbCliente(
	codCli int primary key auto_increment,
    CPF decimal(11,0) unique not null,
    nomeCli varchar(200) not null,
    sobrenome varchar(200) not null,
    telefone decimal(11,0) not null,
    email varchar(200) not null,
    codLogin int not null,
    cepCli char(8),
    numEnd int,
    compEnd varchar(50)
);

Create table tbEndereco(
	cep char(8) primary key,
	logradouro varchar(200) not null,
	bairroId int not null,
	cidadeId int not null,
	ufId int not null
);

Create table tbEstado(
	ufId int primary key auto_increment,
	nomeUF char(2) not null
);

Create table tbCidade(
	cidadeId int primary key auto_increment,
	nomecidade varchar(200) not null
);

Create table tbBairro(
	bairroId int primary key auto_increment,
	nomeBairro varchar(200) not null
);

Create table tbProduto(
	codProd decimal(14,0) primary key,
    nomeProd varchar(200) not null,
    descricao varchar(600) not null,
    codClassificacao int not null,
    codCategoria int not null,
    lancamento boolean,
    valorUnit decimal(8,2) not null,
    estoque int not null,
    statusProd boolean not null
);
    
Create table tbCategoria(
	codCategoria int primary key auto_increment,
    nomeCategoria varchar(100) not null
);

Create table tbClassificacao(
	codClassificacao int primary key auto_increment,
    nomeClassificacao varchar(50) not null
);

Create table tbHabilidade(
	codHabilidade int primary key auto_increment,
    nomeHabilidade varchar(100) not null
);

Create table tbHabilidade_Produto(
	codProd decimal(14,0),
    codHabilidade int,
    Primary Key(codProd, codHabilidade)
);

Create table tbImagem(
	nomeImg varchar(200) not null primary key,
    enderecoImg varchar(300) not null,
    codProd decimal(14,0) not null
);

Create table tbAvaliacao(
	codAvaliacao int primary key auto_increment,
    qtdEstrela decimal(1,0) not null,
    comentario varchar(700) not null,
    codLogin int not null,
    codProd decimal(14,0) not null
);

Create table tbFavorito(
	codLogin int,
    codProd decimal(14,0),
    Primary Key(codLogin, codProd)
);

Create table tbCarrinho(
    codLogin int not null,
    codProd decimal(14,0),
    qtdProd int not null,
    Primary Key(codLogin, codProd)
);

Create table tbPedido(
	NF int primary key auto_increment,
	data datetime not null,
	codLogin int not null,
	statusPedido decimal(1,0),
	valorTotal decimal(8,2) not null
);

Create table tbItemPedido(
	codItem int primary key auto_increment,
	NF int not null,
    codProd decimal(14,0) not null,
    qtdItem int not null,
    valorItem decimal(8,2) not null
);

Create table tbPagamento(
	codPag int primary key auto_increment,
    qtdParcela smallint not null,
    NF int not null,
    codPix varchar(200),
    codCartao int
);

Create table tbCartaoCredito(
	codCartao int primary key auto_increment,
    numCartao decimal(16,0),
    nomeTitular varchar(200),
    dataVali char(5),
    Bandeira varchar(100), 
    codLogin int
);

Create table tbDevolucao(
	codDevolucao int primary key auto_increment,
    dataSolicitacao datetime not null,
    motivo varchar(400) not null,
    statusDevolucao decimal(1,0) not null,
    codItem int not null
);

/*
	Adicionando as chaves estrangeiras
*/

-- FK tbCliente --
Alter table tbCliente Add foreign key(codLogin) references tbLogin(codLogin);
Alter table tbCliente Add foreign key(cepCli) references tbEndereco(cep);

-- FK tbEndereco --
Alter table tbEndereco Add foreign key(bairroId) references tbBairro(bairroId);
Alter table tbEndereco Add foreign key(cidadeId) references tbCidade(cidadeId);
Alter table tbEndereco Add foreign key(ufId) references tbEstado(ufId);

-- FK tbProduto --
Alter table tbProduto Add foreign key(codCategoria) references tbCategoria(codCategoria);
Alter table tbProduto Add foreign key(codClassificacao) references tbClassificacao(codClassificacao);

-- FK tbHabilidade_Produto --
Alter table tbHabilidade_Produto Add foreign key(codProd) references tbProduto(codProd);
Alter table tbHabilidade_Produto Add foreign key(codHabilidade) references tbHabilidade(codHabilidade);

-- FK tbImagem --
Alter table tbImagem Add foreign key(codProd) references tbProduto(codProd);

-- FK tbFavorito --
Alter table tbFavorito Add foreign key(codLogin) references tbLogin(codLogin);
Alter table tbFavorito Add foreign key(codProd) references tbProduto(codProd);

-- FK tbAvaliacao --
Alter table tbAvaliacao Add foreign key(codLogin) references tbLogin(codLogin);
Alter table tbAvaliacao Add foreign key(codProd) references tbProduto(codProd);

-- FK tbPedido --
Alter table tbPedido Add foreign key(codLogin) references tbLogin(codLogin);

-- FK tbItemPedido --
Alter table tbItemPedido Add foreign key(NF) references tbPedido(NF);

-- FK tbCarrinho --
Alter table tbCarrinho Add foreign key(codLogin) references tbLogin(codLogin);
Alter table tbCarrinho Add foreign key(codProd) references tbProduto(codProd);

-- FK tbDevolucao --
Alter table tbDevolucao Add foreign key(codItem) references tbItemPedido(codItem);

-- FK tbPagamento --
Alter table tbPagamento Add foreign key(NF) references tbPedido(NF);
Alter table tbPagamento Add foreign key(codCartao) references tbCartaoCredito(codCartao);

-- FK tbCartaoCredito --
Alter table tbCartaoCredito Add foreign key(CodLogin) references tbLogin(CodLogin);

/*
	Procedures de Cadastro (Insert)
*/

-- Sessão Cadastro de Cliente --
Delimiter $$
Create Procedure spInsertTbCliente(
	vCPF decimal(11,0), 
    vNome varchar(200), 
    vSobrenome varchar(200), 
    vTelefone decimal(11,0), 
    vEmail varchar(200), 
    vSenha varchar(20), 
    vLogradouro varchar(200), 
    vBairro varchar (200),  
    vCidade varchar (200) , 
    vUF char (2), 
    vCEP char(8), 
    vNumEnd int, 
    vCompEnd varchar(50)
)
Begin
	-- Criando variáveis para armazenar os Ids--
	Declare vBairroId int;
    Declare vCidadeId int;
    Declare vUfId int;
	Declare vCodLogin int; -- Armazena o código de Login do Cliente --
    
    If not exists(Select CPF from tbCliente where CPF = vCPF) then
		If not exists(Select email from tbCliente where email = vEmail) then
			-- Cadastro tbEndereco --
			If not exists (Select CEP from tbEndereco where CEP = vCEP) then 
				If not exists (Select nomeBairro from tbBairro where nomeBairro = vBairro) then 
					Insert into tbBairro(nomeBairro) 
							values(vBairro);
				End if;
				If not exists (Select nomeCidade from tbCidade where nomeCidade = vCidade) then 
					insert into tbCidade(nomeCidade)
							values(vCidade);
				End if;
				If not exists (Select nomeUF from tbEstado where nomeUF = vUf) then 
					Insert into tbEstado(nomeUF)
							values(vUF);
				End if;
				
				-- Atribuindo os Ids do Bairro, Cidade e Esatdo condizente --
				Set vBairroId = (Select BairroId from tbBairro where nomeBairro = vBairro);
				Set vCidadeId = (Select CidadeId from tbCidade where nomeCidade = vCidade);
				Set vUfId = (Select ufId from tbEstado where nomeUF = vUf);
				
				Insert into tbEndereco (Logradouro, BairroId, CidadeId, UFId, CEP)
						values(vLogradouro, vBairroId, vCidadeId, vUfId, vCEP);
			End if; -- Fechamneto cadastro tbEndereco --
        
			-- Cadastro Cliente Login --
			Insert into tbLogin(usuario, senha, nivelAcesso)
						values(vEmail, vSenha, 2);
			
            -- Atribuindo valor do Id de Login do Cliente --
			Set vCodLogin = (Select codLogin from tbLogin where usuario = vEmail);
			
			Insert into tbCliente(CPF, nomeCli, sobrenome, telefone, email, codLogin, cepCli, numEnd, compEnd)
							values(vCPF, vNome, vSobrenome, vTelefone, vEmail, vCodLogin, vCEP, vNumEnd, vCompEnd);
		Else
			Select('Esse email já está casdastrado!');
		End If;
	Else 
		Select('Esse CPF já está casdastrado! Tente fazer o Login');
	End if;
End $$

-- Sessão Cadastro de Categoria de Produto --
Delimiter $$
Create Procedure spInsertTbCategoria(vNomeCategoria varchar(100))
Begin
	-- Verifica se já não existe essa categoria --
	If not exists (Select nomeCategoria from tbCategoria where nomeCategoria = vNomeCategoria) then
		Insert into tbCategoria(nomeCategoria)
						values(vNomeCategoria); -- Insere os dados --
    End if;
End $$

-- Sessão Cadastro de Classificação Indicativa do Produto --
Delimiter $$
Create Procedure spInsertTbClassificacao(vNomeClassificacao varchar(50))
Begin
	-- Verifica se já não existe essa classificacao --
	If not exists (Select nomeClassificacao from tbClassificacao where nomeClassificacao = vNomeClassificacao) then
		Insert into tbClassificacao(nomeClassificacao)
						values(vNomeClassificacao); -- Insere os dados --
    End if;
End $$

-- Sessão de Cadastro de Habilidade trabalhada do Produto --
Delimiter $$
Create Procedure spInsertTbHabilidade(vNomeHabilidade varchar(100))
Begin
	-- Verifica se já não existe essa habilidade registrada --
	If not exists (Select nomeHabilidade from tbHabilidade where nomeHabilidade = vNomeHabilidade) then
		Insert into tbHabilidade(nomeHabilidade)
						values(vNomeHabilidade); -- Insere os dados --
    End if;
End $$

-- Sessão de Cadastro da tabela Habilidade_Produto pois era relacionamento N:N --
Delimiter $$
Create Procedure spInsertTbHabilidade_Produto(
	vCodProd decimal(14,0), 
	vCodHabilidade int
)
Begin
	-- Verifica se existe esse código de produto e a códig da habilidade --
	If exists 
		(Select codProd from tbProduto where codProd = vCodProd) 
		And 
        (Select codHabilidade from tbHabilidade where codHabilidade = vCodHabilidade) then
        
		If not exists
			(Select codProd, codHabilidade from tbHabilidade_Produto 
            where codProd = vCodProd And codHabilidade = vCodHabilidade) then
            
            Insert into tbHabilidade_Produto(codProd, codHabilidade)
					values(vCodProd, vCodHabilidade);
		End if;
	End if;
End $$



-- Sessão de Cadastro de Produtos --
Delimiter $$
Create procedure spInsertTbProduto(
	vCodProd decimal(14,0), 
    vNomeProd varchar(200), 
    vDescricao varchar(600), 
    vCodClassificacao int, 
    vCodCategoria int, 
    vValorUnit decimal(8,2), 
    vEstoque int,
    vLancamento bool
)
Begin
    -- Verifica se esse código não está cadastrado --
    If not exists(Select codProd from tbProduto where codProd = vCodProd) then
        -- Insere os dados
		Insert into tbProduto(
			codProd, 
            nomeProd, 
            descricao, 
            codClassificacao, 
            codCategoria, 
            valorUnit, 
            estoque, 
            statusProd,
            lancamento)
		values(
			vCodProd, 
            vNomeProd, 
            vDescricao, 
            vCodClassificacao, 
            vCodCategoria, 
            vValorUnit, 
            vEstoque,
            1,
            vLancamento);
    
		Select('Produto Cadastrado com sucesso!');
	Else
		Select('Código de Barras já cadastrado!');
    End if;
End $$

-- Sessão de Cadastro da Imagem do Produto --
Delimiter $$
Create Procedure spInsertTbImagem(
	vCodProd decimal(14,0), 
    vNomeImg varchar(200), 
    vEnderecoImg varchar(300)
)
Begin
	-- Verifica se esse registro não existe
	If not exists(Select nomeImg from tbImagem where nomeImg = vNomeImg) then
		Insert into tbImagem(codProd, nomeImg, enderecoImg)
					values(vCodProd, vNomeImg, vEnderecoImg); -- Insere os dados --
	End if;
End $$

-- Sessão de Cadastro dos Produtos favoritos --
Delimiter $$
Create Procedure spInsertTbFavorito(
	vCodLogin int, 
    vCodProd decimal(14,0)
)
Begin
	If not exists(Select codProd, codLogin from tbFavorito where codLogin = vCodLogin And codProd = vCodProd) then
		-- Insere os valores --
		Insert into tbFavorito(codLogin, codProd)
						values(vCodLogin, vCodProd);
	End if;
End $$

-- Sessão de Cadastro de Produtos no Carrinho --
Delimiter $$
Create Procedure spInsertTbCarrinho(
	vCodLogin int, 
    vCodProd decimal(14,0), 
    vQtdProd int
)
Begin
	-- Verifica se esse registro não existe
	If not exists(Select codProd from tbCarrinho where codLogin = vCodLogin And codProd = vcodProd) then
		Insert into tbCarrinho(codLogin, codProd, qtdProd)
						values(vCodLogin, vCodProd, vQtdProd); -- Insere os dados --
                        
	Else -- Caso o registro exista, só atualiza a quantidade --
		Update tbCarrinho Set QtdProd = QtdProd + vQtdProd where codLogin = vCodLogin And codProd = vCodProd;
    End if;
End $$

-- Sessão de Cadastro de Pedidos --
Delimiter $$
Create Procedure spInsertTbPedido(vCodLogin int, vValorTotal decimal(8,2))
Begin
	Insert into tbPedido(data, codLogin, statusPedido, valorTotal)
				values(current_timestamp(), vCodLogin, 0, vValorTotal);
   SELECT LAST_INSERT_ID() AS NF;
End $$ 

-- Sessão de Cadastro dos Itens do Pedido --
Create Procedure spInsertTbItemPedido(
	vNF int, 
    vCodProd decimal(14,0), 
    vQtdItem int,
    vValorProd decimal(8,2)
)
Begin
    -- Verifica se o registro não existe --
    If not exists(Select * from tbItemPedido where NF = vNF And codProd = vCodProd) then
		Insert into tbItemPedido(NF, codProd, qtdItem, valorItem)
						values(vNF, vCodProd, vQtdItem, (vValorProd * vQtdItem)); -- Insere os dados --
    End if;
End $$ 

-- Sessão de Cadastro de Cartão de Crédito --
Delimiter $$
Create Procedure spInsertTbCartaoCredito(
	vNumCartao decimal(16,0), 
    vNomeTitular varchar(200), 
    vDataVali char(5), 
    vBandeira varchar(100), 
    vCodLogin int
)
Begin
	-- Verifica se esse cartão não está registrado --
	If not exists(Select codCartao from tbCartaoCredito where codLogin = vCodLogin and numCartao = vNumCartao) then
		Insert into tbCartaoCredito(numCartao, nomeTitular, dataVali, bandeira, codLogin) -- Insere os dados --
							values(vNumCartao, vNomeTitular, vDataVali, vBandeira, vCodLogin);
	Else
		Select('Este cartão já está registrado!');
    End if;
End $$

-- Sessão de Cadastro do Pagamento do Pedido --
Delimiter $$
Create Procedure spInsertTbPagamento(
	vQtdParcela smallint, 
    vNF int, 
    vCodPix varchar(200), 
    vCodCartao int
)
Begin
	-- Verifica se o pedido não está pago --
	If not exists(Select NF from tbPagamento where NF = vNF) then
		-- Quando o codPix = 1, significa que o pagamento vai ser no cartão
		If (vCodPix = '1') then
			-- Verfica o cartão já está registrado --
			If exists(Select codCartao from tbCartaoCredito where codCartao = vCodCartao) then
				Insert Into tbPagamento(qtdParcela, NF, codCartao) -- Insere os dados --
								values(vQtdParcela, vNF, vCodCartao);
				
                Update tbPedido Set StatusPedido = 1 where NF = vNF; -- Atualiza os status do pedio --
                /* Status do Pedido:
                0 = Aguardando Pagamento;
                1 = Em andamento;
                2 = Concluído (Entregue);
                3 = Cancelado;
                4 = Devolvido;
                */
			Else  
				Select('Cartão inválido. Tente novamente.');
			End if;
		Else
			Insert Into tbPagamento(qtdParcela, NF, codPix)
							values(1, vNF, vCodPix);
                        
			Update tbPedido Set StatusPedido = 1 where NF = vNF;
		End If;
    End if;
End $$

-- Sessão de Cadastro de Avaliações --
Delimiter $$
Create Procedure spInsertTbAvaliacao(
	vQtdEstrela decimal(1,0), 
    vComentario varchar(700), 
    vCodLogin int, 
    vCodProd decimal(14,0)
)
Begin
	-- Verifica se o produto já foi comprado e entregue --
	If exists(
		Select codProd from tbItemPedido where codProd = vCodProd and 
        NF in (Select NF from tbPedido where codLogin = vCodLogin and statusPedido >= 2)) then
        
        If not exists(Select codProd, codLogin from tbavaliacao where codLogin = vCodLogin and codProd = vCodProd) then
		
			Insert into tbAvaliacao(qtdEstrela, comentario, codLogin, codProd) -- Insere os dados --
							values(vQtdEstrela, vComentario, vCodLogin, vCodProd);
			
			Select('Avaliação Registrada!');
		end if;
	else
		Select('É necessário comprar o produto para avaliar!');
		
	End if;
End $$

-- Sessão de Cadastro de Devoluções --
Delimiter $$
Create Procedure spInsertTbDevolucao(
	vMotivo varchar(400), 
    vCodItem int
)
Begin
	-- Declara a variável para receber a NF --
	Declare vNF int;
    
    -- Verifica se o Item existe --
	If exists(Select codItem from tbItemPedido where codItem = vCodItem) then
		-- Verifica se não existe uma devolução do Item doPedido --
		If not exists(Select codItem from tbDevolucao where codItem = vCodItem) then
			Insert into tbDevolucao(dataSolicitacao, motivo, statusDevolucao, codItem)
							Values(current_timestamp(), vMotivo, 0, vCodItem); -- Insere os dados --
			-- Atribui o número do pedido a variável -- 
			Set vNF = (Select NF from tbItemPedido where codItem = vCodItem);    
			
			Update tbPedido Set statusPedido = 4 where NF = vNF;
		End if;
	End if;
End $$

/*
	Procedures Alterações (Update)
*/

-- Sessão de Atualização da Senha -- 
Delimiter $$
Create Procedure spUptade_SenhaCli(vCodLogin int, vSenha varchar(20))
Begin
	Update tbLogin Set Senha = vSenha where codLogin = vCodLogin;
End $$

-- Sessão de Atualização da Quantidade Carrinho --
Delimiter $$
Create Procedure spUpdateTbCarrinho(
    vCodLogin int, 
    vCodProd decimal(14,0),
    vQtdProd int
)
Begin 
    Update tbCarrinho set qtdProd = vQtdProd where codProd = vCodProd and codLogin = vCodLogin;
End $$

-- Sessão de Atualização dos Dados do Cliente (Endereço) --
Delimiter $$
Create Procedure spUpdateTbClienteEndereco(
	-- Variáveis de Entrada --
	vCodLogin int, 
	vLogradouro varchar (200), 
	vBairro varchar (200),  
	vCidade varchar (200), 
	vUF char (2), 
	vCEP char(8), 
	vNumEnd int, 
	vCompEnd varchar(50)
)
Begin
	-- Criando variáveis para armazenar os Ids--
	Declare vBairroId int;
    Declare vCidadeId int;
    Declare vUfId int;

	-- Cadastro tbEndereco, caso ele não exista --
	If not exists (Select CEP from tbEndereco where CEP = vCEP) then 
		If not exists (Select nomeBairro from tbBairro where nomeBairro = vBairro) then 
			Insert into tbBairro(nomeBairro) 
					values(vBairro);
		End if;
		If not exists (Select nomeCidade from tbCidade where nomeCidade = vCidade) then 
			insert into tbCidade(nomeCidade)
					values(vCidade);
		End if;
		If not exists (Select nomeUF from tbEstado where nomeUF = vUf) then 
			Insert into tbEstado(nomeUF)
					values(vUF);
		End if;
		
		-- Atribuindo os Ids do Bairro, Cidade e Esatdo condizente --
		Set vBairroId = (Select BairroId from tbBairro where nomeBairro = vBairro);
		Set vCidadeId = (Select CidadeId from tbCidade where nomeCidade = vCidade);
		Set vUfId = (Select ufId from tbEstado where nomeUF = vUf);
		
		Insert into tbEndereco (Logradouro, BairroId, CidadeId, UFId, CEP)
				values(vLogradouro, vBairroId, vCidadeId, vUfId, vCEP);
	End if; -- Fechamento cadastro tbEndereco --
	
    -- Atuliza os dados do cliente --
	Update tbCliente Set 
		cepCli = vCEP, 
		numEnd = vNumEnd, 
		compEnd = vCompEnd  
	where codLogin = vCodLogin;
    
	
	Select('Dados Alterados com sucesso!');
End $$

-- Sessão de Atualização dos Dados do Cliente (Dados Conta) --
Delimiter $$
Create Procedure spUpdateTbClienteConta(
	-- Variáveis de Entrada --
	vCodLogin int, 
	vEmail varchar(200), 
	vSenha varchar(20)
)
Begin
	If not exists(Select usuario from tbLogin where usuario = vEmail and codLogin != vCodLogin) then
		if(vSenha is null) then
			if not exists (Select email from tbCliente where email = vEmail) then
				Update tbCliente Set email = vEmail where codLogin = vCodLogin;
				Update tbLogin Set usuario = vEmail where codLogin = vCodLogin;
                
                Select('Dados Alterados com sucesso!');
			end if;
		else
			-- Atuliza os dados do cliente --
			Update tbCliente Set email = vEmail where codLogin = vCodLogin;
			Update tbLogin Set usuario = vEmail, senha = vSenha where codLogin = vCodLogin;
		
			Select('Dados Alterados com sucesso!');
        end if;
	else
		Select('Esse email já está cadastrado, tente outro');
	end if;
End $$

-- Sessão de Atualização dos Dados do Cliente (Informações Pessoais) --
Delimiter $$
Create Procedure spUpdateTbClienteDados(
	-- Variáveis de Entrada --
	vCodLogin int, 
    vCPF decimal(11,0),
	vNome varchar(200), 
	vSobrenome varchar(200), 
	vTelefone decimal(11,0)
)
Begin
	If not exists(Select cpf from tbCliente where cpf = vCPF) then
		-- Atuliza os dados do cliente --
		Update tbCliente Set 
			cpf = vCPF,
			nomeCli = vNome, 
			sobrenome = vSobrenome, 
			telefone = vTelefone
		where codLogin = vCodLogin;
		
        Select('Dados atualizados com sucesso!');
        
	else
		Select('Este CPF já está cadastrado, por favor digite outro');
	End if;
End $$

-- Sessão de Atualização de Status do Pedido --
Delimiter $$
Create Procedure spUpdateTbPedido(
	vNF int, 
	vStatusPedido decimal(1,0)
)
Begin
	Update tbPedido Set statusPedido = vStatusPedido where NF = vNF;
    /* Status do Pedido:
	0 = Aguardando Pagamento;
	1 = Em andamento;
	2 = Concluído (Entregue);
	3 = Cancelado;
	4 = Devolvido;
	*/
End $$

-- Sessão de Atualização de Produto --
Delimiter $$
Create procedure spUpdateTbProduto(
	vCodProd decimal(14,0), 
    vNomeProd varchar(200), 
    vDescricao varchar(600), 
    vnomeClassificacao varchar(50), 
    vCategoria varchar(100), 
    vValorUnit decimal(8,2), 
    vLancamento boolean
)
Begin
	Declare vCodClassificacao int;
    Declare vCodCategoria int;
    
	-- Atribuindo valores --
	Set vCodClassificacao = (Select CodClassificacao from tbClassificacao where nomeClassificacao = vnomeClassificacao);
	Set vCodCategoria = (Select codCategoria from tbCategoria where nomeCategoria = vnomeCategoria);
	
    -- Atualizando dados do produto --
	Update tbProduto Set 
		nomeProd = vNomeProd, 
        descricao = vDescricao, 
        codClassificacao = vCodClassificacao, 
        codCategoria = vCodCategoria, 
        valorUnit = vValorUnit, 
        lancamento = vLancamento 
	where codProd = vCodProd;

	Select('Produto atualizado com sucesso!');
End $$

-- Sessão de Atualização das Imagens do Produto --
Delimiter $$
Create Procedure spUpdateTbImagem(
	vCodProd decimal(14,0), 
    vNomeImg varchar(200), 
    vEnderecoImg varchar(300)
)
Begin
	-- Verifica se o nome já está registrado -
	If not exists(Select nomeImg from tbImagem where nomeImg = vNomeImg) then
		Update tbImagem Set nomeImg = vNomeImg, enderecoImg = vEnderecoImg where codProd = vCodProd;
	End if;
End $$

-- Sessão de Atualização de Status do Produto --
Delimiter $$
create Procedure spUpdateStatusTbProduto(
	vcodProd decimal (14,0), 
	vstatusProd tinyint
)
Begin
	Update tbProduto Set statusProd = vstatusProd where codProd = vcodProd;
    /* Status do Pedido:
	0 = Ativo;
    1 = Desativado;
	*/
End $$

-- Sessão de Atualização de Avaliações --
Delimiter $$
Create Procedure spUpdateTbAvaliacao(
	vCodAvaliacao int, 
    vQtdEstrela decimal(1,0), 
    vComentario varchar(700)
)
Begin 
	-- Atualiza a avaliação --
	Update tbAvaliacao Set qtdEstrela = vQtdEstrela, comentario = vComentario where codAvaliacao = vCodAvaliacao;
End $$


-- Sessão de Atualização do Status da devolução --
Delimiter $$
Create Procedure spUpdateTbDevolucao(
	vCodDevolucao int, 
    vStatusDevolucao decimal(1,0)
)
Begin
	-- Atualiza o status --
	Update tbDevolucao Set statusDevolucao = vStatusDevolucao where codDevolucao = vCodDevolucao;
    /* Status da Devolução:
	0 = Em Análise;
    1 = Aprovada;
    2 = Negada;
    */
End $$


/*
	Procedures de Exclusão (Delete)
*/

-- Sessão de Exclusão de Favoritos --
Delimiter $$
Create Procedure spDeleteTbFavorito(
	vCodLogin int, 
    vCodProd decimal(14,0)
)
Begin
	Delete from tbFavorito where codLogin = vCodLogin and codProd = vCodProd;
End $$

-- Sessão de Exclusão de Avaliação --
Delimiter $$
Create Procedure spDeleteTbAvaliacao(
	vCodLogin int, 
    vCodAvaliacao int
)
Begin
	-- Excluindo --
	Delete from tbAvaliacao where codLogin = vCodLogin and codAvaliacao = vCodAvaliacao;
End $$

-- Sessão de Exclusão de Produtos do Carrinho --
Delimiter $$
Create Procedure spDeleteTbCarrinho(
	vCodLogin int, 
    vCodProd decimal(14,0)
)
Begin 
	Delete from tbCarrinho where codLogin = vCodLogin and codProd = vCodProd;
End $$  


/*
	Triggers
*/

-- Sessão de Redução de estoque --
Delimiter $$
Create Trigger trgInsertTbPagamento After Insert On tbPagamento For Each Row
Begin
	-- Junta as tabelas através do codProduto --
	Update tbProduto Join tbItemPedido On tbProduto.codProd = tbItemPedido.codProd
    -- Subtrai o valor do Item do Pedido na tbProduto --
    Set tbProduto.estoque = tbProduto.estoque - tbItemPedido.qtdItem
    -- Chave de varicação é a NF --
    Where tbItemPedido.NF = New.NF;
End $$

-- Sessão de Deletar os produtos do carrinho após realização do pedido --
Delimiter $$
Create Trigger trgInsertTbItemPedido After Insert On tbItemPedido For Each Row
Begin
	Delete from tbCarrinho
    -- Delete onde o codLogin é igual ao codLogin disponível na Pedido --
    Where codLogin = (Select codLogin from tbPedido where NF = New.NF)
    And codProd = New.codProd;
End $$

/*
	Views e Consultas
*/

-- View para Selecionar as Estatisicas da Avaliações --
Delimiter $$
Create View vwEstatisticasAvaliacao As 
Select
    codProd,
    Count(*) AS totalAvaliacoes, 
    Sum(qtdEstrela) AS totalEstrelas,
    Sum(Case when qtdEstrela = 5 then 1 else 0 END) AS estrelas5,
    Sum(Case when qtdEstrela = 4 then 1 else 0 END) AS estrelas4,
    Sum(Case when qtdEstrela = 3 then 1 else 0 END) AS estrelas3,
    Sum(Case when qtdEstrela = 2 then 1 else 0 END) AS estrelas2,
    Sum(Case when qtdEstrela = 1 then 1 else 0 END) AS estrelas1
from tbAvaliacao
group by codProd;
$$

-- View para Selecionar Previa dos Produtos --
Delimiter $$
Create View vwPreviaProduto As
Select
    tbProduto.codProd,
    tbProduto.nomeProd,
    tbProduto.valorUnit,
    tbProduto.lancamento,
    tbProduto.statusProd,
    GROUP_CONCAT(CONCAT(tbImagem.nomeImg, ' -- ', tbImagem.enderecoImg) Separator ' | ') AS 'imgs',
    IFNULL(vwEstatisticasAvaliacao.totalAvaliacoes, 0) As 'qtdAvaliacao',
    IFNULL(vwEstatisticasAvaliacao.totalEstrelas, 0) As 'somaAvaliacao'
from tbProduto
    Left Join tbImagem On tbImagem.codProd = tbProduto.codProd
    Left Join tbAvaliacao On tbAvaliacao.codProd = tbProduto.codProd
    Left Join vwEstatisticasAvaliacao On vwEstatisticasAvaliacao.codProd = tbProduto.codProd
Group by tbProduto.codProd;
$$

-- View para unificar as informações do produto  --
Delimiter $$
Create View vwProdutoCompleto As
Select
    tbProduto.codProd,
    tbProduto.nomeProd,
    tbProduto.descricao,
    CONCAT(tbClassificacao.codClassificacao, ' - ', tbClassificacao.nomeClassificacao) As 'classificacao',
    CONCAT(tbCategoria.codCategoria, ' - ', tbCategoria.nomeCategoria) As 'categoria',
    GROUP_CONCAT(Distinct CONCAT(tbHabilidade.codHabilidade, ' - ', tbHabilidade.nomeHabilidade) Order By tbHabilidade.codHabilidade Separator ' | ') As 'habilidades',
    tbProduto.valorUnit,
    tbProduto.estoque,
    tbProduto.statusProd,
    tbProduto.lancamento,
    GROUP_CONCAT(Distinct CONCAT(tbImagem.nomeImg, ' -- ', tbImagem.enderecoImg) Separator ' | ') AS 'imgs',
    GROUP_CONCAT(CONCAT(
        tbAvaliacao.qtdEstrela, ' - ', 
        tbAvaliacao.comentario, ' - ',
        tbCliente.codLogin, ' - ', 
        SUBSTRING_INDEX(tbCliente.nomeCli, ' ', 1), ' ', 
        SUBSTRING_INDEX(tbCliente.sobrenome, ' ', -1)
    ) Separator ' | ') As 'detalhesAvaliacao',
    CONCAT_WS(', ',
        IFNULL(vwEstatisticasAvaliacao.estrelas5, 0),
        IFNULL(vwEstatisticasAvaliacao.estrelas4, 0),
        IFNULL(vwEstatisticasAvaliacao.estrelas3, 0),
        IFNULL(vwEstatisticasAvaliacao.estrelas2, 0),
        IFNULL(vwEstatisticasAvaliacao.estrelas1, 0)
    ) As 'qtdPorEstrela',
    IFNULL(vwEstatisticasAvaliacao.totalAvaliacoes, 0) As 'qtdAvaliacao',
    IFNULL(vwEstatisticasAvaliacao.totalEstrelas, 0) As 'somaAvaliacao'
from tbProduto
    Left Join tbCategoria On tbCategoria.codCategoria = tbProduto.codCategoria
    Left Join tbClassificacao On tbClassificacao.codClassificacao = tbProduto.codClassificacao
    Left Join tbHabilidade_Produto On tbProduto.codProd = tbHabilidade_Produto.codProd
    Left Join tbHabilidade On tbHabilidade.codHabilidade = tbHabilidade_Produto.codHabilidade
    Left Join tbImagem On tbImagem.codProd = tbProduto.codProd
    Left Join tbAvaliacao On tbAvaliacao.codProd = tbProduto.codProd
    Left Join vwEstatisticasAvaliacao On vwEstatisticasAvaliacao.codProd = tbProduto.codProd
    Left Join tbCliente On tbCliente.codLogin = tbAvaliacao.codLogin
Group by tbProduto.codProd;
$$

-- Procedure para Selecionar os Produtos Relacionados --
Delimiter $$
Create Procedure spSelectRelacionados(vCategoria varchar(100))
Begin
    -- Tentar selecionar 5 produtos da categoria especificada
	If((Select COUNT(*) from vwProdutoCompleto where categoria = vCategoria order by RAND() limit 5) >= 5) then
		Select * from vwProdutoCompleto where categoria = vCategoria order by RAND() limit 5;
	Else
		-- Se não retornar resultados, selecionar produtos aleatórios
        Select * from vwProdutoCompleto order by RAND() limit 5;
    End if;
End $$

-- Procedure para Selecionar os Produtos do Funcionario --
Delimiter $$
Create Procedure spSelectPreviaProdutoF()
Begin
	Select * from vwPreviaProduto;
End $$

-- Procedure para Selecionar os Produtos Lancamentos --
Delimiter $$
Create Procedure spSelectPreviaProduto()
Begin
	Select * from vwPreviaProduto where statusProd = 1 and lancamento = 1;
End $$


-- Procedure para Selecionar os Detalhes Produtos --
Delimiter $$
Create Procedure spSelectDetalheProduto(vCodProd decimal(14,0))
Begin
	Select * from vwProdutoCompleto where codProd = vCodProd;
End $$

-- View para selecionar os produtos mais vendidos --
Delimiter $$
Create View vwProduto_MaisVendido As
	Select
		codProd, 
		COUNT(*) AS Total_Vendas
	From tbItemPedido Where NF IN (SELECT NF FROM tbPagamento)
	Group By codProd
	Order By Total_Vendas Desc Limit 10;
$$

-- Consulta para obter as informações dos produtos mais vendidos --
Select vwProduto.*, Total_Vendas from vwProduto Inner Join vwProduto_MaisVendido On vwProduto.Código = vwProduto_MaisVendido.codProd;

-- View para selecionar os produtos mais favoritados --
Delimiter $$
Create View vwProduto_MaisFavoritado As
	Select
		codProd
	From tbFavorito
	Group By codProd
	Order By COUNT(*) Desc Limit 3;
$$

-- Consulta para obter as informações dos produtos mais favoritado --
Select vwProduto.* from vwProduto Inner Join vwProduto_MaisFavoritado On vwProduto.Código = vwProduto_MaisFavoritado.codProd;

-- View para selecionar os produtos mais comentados --
Delimiter $$
Create View vwProduto_MaisComentado As
	Select
		codProd,
        COUNT(*) As Total_Comentarios
	From tbAvaliacao
	Group By codProd
	Order By Total_Comentarios Desc Limit 3;
$$

-- Procedure para selecionar as avaliações do produto --
Delimiter $$
Create Procedure spSelectTbAvaliacao(vCodProd decimal(14,0))
Begin
	Select * from tbAvaliacao where codProd = vCodProd;
End $$

-- Procedure para selecionar as avaliações unicas do cliente --
Delimiter $$
Create Procedure spSelectTbAvaliacaoUnica(vCodProd decimal(14,0), vcodLogin int)
Begin
	Select * from tbAvaliacao where codProd = vCodProd and codLogin= vcodLogin;
End $$


-- Procedure de validação do login --
Create Procedure spSelectTbLogin(vUsuario varchar(200), vSenha varchar(20))
Begin
	If exists(Select codLogin from tbLogin where usuario = vUsuario and senha = vSenha) then
		-- Retorna o nível de acesso e codigo do Login para ser manipulado pelo backend --
		Select codLogin, nivelAcesso from tbLogin where usuario = vUsuario;
    End if;
End $$    

-- View para unificar os dados do Cliente --
Delimiter $$
Create View vwCliente As
	Select
		tbCliente.codLogin,
		codCli As 'Código',
		CPF As 'CPF',
		nomeCli As 'Nome',
		sobrenome As 'Sobrenome',
		telefone As 'Telefone',
		email As 'Email',
		senha As 'Senha',
		cepCli As 'CEP',
        logradouro As 'Logradouro',
        nomeBairro As 'Bairro',
        nomeCidade As 'Cidade',
        nomeUf As 'Estado',
		numEnd As 'Número',
		compEnd As 'Complemento'
	from tbCliente
		Inner Join tbLogin On tbCliente.CodLogin = tbLogin.CodLogin
		Inner Join tbEndereco On tbCliente.CepCli = tbEndereco.CEP
        Inner Join tbBairro On tbEndereco.bairroId = tbBairro.BairroId
        Inner Join tbCidade On tbEndereco.cidadeId = tbCidade.CidadeId
        Inner Join tbEstado On tbEndereco.ufId = tbEstado.UfId;
$$

-- Procedure para selecionar os cartões de um cliente --
Delimiter $$
Create Procedure spSelectTbCartoes(vcodLogin int)
Begin
	Select * from tbCartaoCredito where codLogin = vCodLogin;
End $$

-- Procedure para filtrar produtos por classifcação --
Delimiter $$
Create Procedure spSelectTbProdutoClassificacao(vCodClassificacao int)
Begin
	Select vwProduto.* from tbProduto Inner Join vwProduto On codProd = Código
    where codClassificacao = vCodClassificacao and StatusProd = 0;
End $$

-- Procedure para filtrar produto por categoria --
Delimiter $$
Create Procedure spSelectTbProdutoCategoria(vCodCategoria int)
Begin
	Select vwProduto.* from tbProduto Inner Join vwProduto On codProd = Código
    where codCategoria = vCodCategoria and StatusProd = 0;
End $$

-- Procedure para filtrar os produtos por habilidade --
Delimiter $$
Create Procedure spSelectTbProdutoHabilidade(vCodHabilidade int)
Begin
	Select vwProduto.* from tbProduto 
		Inner Join vwProduto On tbProduto.codProd = Código
        Inner Join tbHabilidade_Produto On tbHabilidade_Produto.codProd = Código
    where codHabilidade = vCodHabilidade and StatusProd = 0;
End $$

-- Procedure para Consultar os produtos no carrinho --
Delimiter $$
Create Procedure spSelectCarrinho(vCodLogin int)
Begin
	Select tbCarrinho.codLogin, qtdProd, vwPreviaProduto.codProd, nomeProd, valorUnit, imgs from tbCarrinho Inner Join vwPreviaProduto On tbCarrinho.codProd = vwPreviaProduto.codProd where codLogin = 1;
End $$

-- Procedure para Selecionar os produtos favoritados do usuário --
Delimiter $$
Create Procedure spSelectFavoritos(vCodLogin int)
Begin
	Select tbFavorito.codLogin, vwPreviaProduto.* from tbFavorito Inner Join vwPreviaProduto On tbFavorito.codProd = vwPreviaProduto.codProd where codLogin = vCodLogin;
End $$

-- View para unificar as informações do pedido --
Delimiter $$
Create View vwPedido As
Select
	tbPedido.NF,
    tbPedido.data,
    tbPedido.codLogin,
    tbPedido.statusPedido,
    tbPedido.valorTotal,
	GROUP_CONCAT(CONCAT(tbItemPedido.codItem, ' -- ', tbProduto.nomeProd, ' -- ', tbItemPedido.codProd, ' -- ', tbItemPedido.qtdItem, ' -- ', tbItemPedido.valorItem, ' -- ', (Select nomeImg from tbImagem where codProd = tbItemPedido.codProd limit 1), ' -- ', (Select enderecoImg from tbImagem where codProd = tbItemPedido.codProd limit 1)) Order By tbItemPedido.codItem Separator ' | ') As 'itensPedido'
from tbPedido
	Inner Join tbItemPedido On tbPedido.NF = tbItemPedido.NF
    Left Join tbProduto On tbProduto.codProd = tbItemPedido.codProd
Group By tbPedido.NF;
$$
    
-- Procedure para Selecionar todos os pedidos do usuário --
Delimiter $$
Create Procedure spSelectMeusPedidos(vCodLogin int)
Begin
	Select tbPedido.*, codItem, codprod, qtdItem, valorItem  from tbPedido Join tbItemPedido On tbPedido.NF = tbItemPedido.NF where codLogin = vCodLogin;
End $$

-- Procedure para Selecionar os detalhes do Pedido --
Delimiter $$
Create Procedure spSelectDetalhesPedido(vNF int)
Begin
	Select * from vwPedido where NF = vNF;
End $$

/* Inserção de Dados */

-- Inserção das Categorias --
Delimiter $$
	Call spInsertTbCategoria('Brinquedos de Montar');
	Call spInsertTbCategoria('Brinquedos Sustentáveis');
	Call spInsertTbCategoria('Brinquedos Científicos');
	Call spInsertTbCategoria('Quebra-Cabeças');
	Call spInsertTbCategoria('Livros Interativos');
	Call spInsertTbCategoria('Brinquedos Sensoriais');
	Call spInsertTbCategoria('Instrumentos Musicais');
	Call spInsertTbCategoria('Jogos de Tabuleiro');
	Call spInsertTbCategoria('Jogos de Desafio');
	Call spInsertTbCategoria('Jogos de Ação');
	Call spInsertTbCategoria('Jogos de Cartas');
	Call spInsertTbCategoria('Jogos Eletrônicos');
$$

-- Inserção das Classificação Indicativa --
Delimiter $$
	Call spInsertTbClassificação('Até 2 Anos');
    Call spInsertTbClassificação('3+ Anos');
    Call spInsertTbClassificação('6+ Anos');
    Call spInsertTbClassificação('10+ Anos');
    Call spInsertTbClassificação('12+ Anos');  
$$                

-- Inserção da Habilidade do Produto --
Delimiter $$
	Call spInsertTbHabilidade('Brinquedos de Montar');
    Call spInsertTbHabilidade('Brinquedos Sustentáveis');
    Call spInsertTbHabilidade('Brinquedos Científicos');
    Call spInsertTbHabilidade('Quebra-Cabeças');
    Call spInsertTbHabilidade('Livros Interativos');
    Call spInsertTbHabilidade('Brinquedos Sensoriais');
    Call spInsertTbHabilidade('Instrumentos Musicais');
    Call spInsertTbHabilidade('Jogos de Tabuleiro');
    Call spInsertTbHabilidade('Jogos de Desafio');
    Call spInsertTbHabilidade('Jogos de Ação');
    Call spInsertTbHabilidade('Jogos de Cartas');
    Call spInsertTbHabilidade('Jogos Eletrônicos');  
$$


/* REVISAR 

-- Consulta para obter as informações dos produtos mais favoritado --
Select vwProduto.* from vwProduto Inner Join vwProduto_MaisFavoritado On vwProduto.Código = vwProduto_MaisFavoritado.codProd;

-- Consulta para contar a quantidade de vendas no mês --
Select Count(*) As 'Vendas Realizadas esse mês' from tbPagamento 
	Inner Join tbPedido On tbPagamento.NF = tbPedido.NF 
where month(data) = Month(Curdate()) 
	And Year(data) = YEAR(Curdate());

-- Consulta para contar a quantidade de vendas em R$ no mês --
Select Sum(ValorTotal) As 'Total Vendas esse mês' from tbPagamento 
	Inner Join tbPedido On tbPagamento.NF = tbPedido.NF 
where month(data) = Month(Curdate()) 
	And Year(data) = YEAR(Curdate());
    
-- Consulta para contar a quantidade de vendas em R$ no ano --
Select Sum(ValorTotal) As 'Total Vendas essa ano' from tbPagamento 
	Inner Join tbPedido On tbPagamento.NF = tbPedido.NF 
where Year(data) = YEAR(Curdate());

-- Consulta para contar a quantidade de vendas em R$ na semana --
Select Sum(ValorTotal) As 'Total Vendas essa semana' from tbPagamento 
	Inner Join tbPedido On tbPagamento.NF = tbPedido.NF 
where YEARWEEK(data, 1) = YEARWEEK(CURDATE(), 1); 
-- O número 1 indica que a semana começa na Segunda, conforme o padrão ISO --

-- Consulta da quantidade de devoluções --
Select Count(*) As 'Quantidade de devoluções' from tbDevolucao;

-- Consulta da quantidade de pedidos pendentes --
Select Count(*) As 'Quantidade de pedido pendentes' from tbPedido where statusPedido = 1;

-- Consulta da quantidade de produtos esgotados --
Select Count(*) As 'Quantidade de produtos esgotados' from vwProduto where estoque = 0;

-- Consulta da quntidade de pedidos cancelados --
Select Count(*) As 'Quantidade de pedidos cancelados' from tbPedido where statusPedido = 3;

-- Consulta para obter as infromações dos produtos mais comentados --
Select vwProduto.* from vwProduto Inner Join vwProduto_MaisComentado On vwProduto.Código = vwProduto_MaisComentado.codProd;

*/