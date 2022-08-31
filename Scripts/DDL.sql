/*DDL - Data Definition Language*/

USE Master;
--DROP DATABASE Forum_Games; /* Apaga o banco de dados*/
GO

CREATE DATABASE Forum_Games;
	PRINT 'Banco de Dados Forum_Games foi criado com sucesso';
GO

USE Forum_Games;
GO

/* Cria a tabela TB_Jogadores*/
CREATE TABLE TB_Jogadores(
	Id INT IDENTITY(1,1) NOT NULL,
	Usuario NVARCHAR(200) NOT NULL,
	Senha NVARCHAR(200) NOT NULL,
	Nome NVARCHAR(200) NOT NULL,
	Email NVARCHAR(250) NOT NULL,
	Imagem NVARCHAR(1000) NULL

	CONSTRAINT [PK_Jogador] PRIMARY KEY (Id) /* Define a chave primária e seu respectivo nome*/
);
	PRINT 'A tabela TB_Jogadores foi criada com sucesso';
GO

/* Cria a tabela TB_Categorias_Grupos para indicar qual é o assunto do grupo */
CREATE TABLE TB_Categorias_Grupos(
	Id INT IDENTITY(1,1) NOT NULL,
	Categoria NVARCHAR(200) NOT NULL,

	CONSTRAINT [PK_Categoria_Grupo] PRIMARY KEY (Id)
);
	PRINT 'A tabela TB_Categorias_Grupos foi criada com sucesso';
GO

/* Cria a tabela TB_Categorias_Postagens para indicar qual é o assunto da postagem */
CREATE TABLE TB_Categorias_Postagens(
	Id INT IDENTITY(1,1) NOT NULL,
	Categoria NVARCHAR(200) NOT NULL,

	CONSTRAINT [PK_Categoria_Postagem] PRIMARY KEY (Id)
);
	PRINT 'A tabela TB_Categorias_Postagens foi criada com sucesso';
GO

/* Cria a tabela TB_Grupos para separar as postagens por grupos */
CREATE TABLE TB_Grupos(
	Id INT IDENTITY(1,1) NOT NULL,

	Descricao NVARCHAR(500) NOT NULL,
	CategoriaId INT NOT NULL,

	CONSTRAINT [PK_Grupo] PRIMARY KEY (Id),
	CONSTRAINT [FK_TB_Categoria_TB_Grupo] FOREIGN KEY (CategoriaId) REFERENCES TB_Categorias_Grupos(Id)  /* Define a chave estrangeira, seu nome e seu relacionamento*/
);
	PRINT 'A tabela TB_Grupos foi criada com sucesso';
GO

/* Cria a tabela TB_Postagens*/
CREATE TABLE TB_Postagens(
	Id INT IDENTITY(1,1) NOT NULL,

	Titulo NVARCHAR(200) NOT NULL,
	Texto NVARCHAR(500),
	Imagem NVARCHAR(500),
	DataHora DATETIME NOT NULL,
	GrupoId INT NOT NULL,
	CategoriaPostagemId INT NOT NULL,
	JogadorId INT NOT NULL,

	CONSTRAINT [PK_Postagem] PRIMARY KEY (Id),
	CONSTRAINT [FK_TB_Grupos_TB_Postagem] FOREIGN KEY (GrupoId) REFERENCES TB_Grupos(Id),
	CONSTRAINT [FK_TB_Categorias_Postagens_TB_Postagem] FOREIGN KEY (CategoriaPostagemId) REFERENCES TB_Categorias_Postagens(Id),
	CONSTRAINT [FK_TB_Jogador_TB_Postagem] FOREIGN KEY (JogadorId) REFERENCES TB_jogadores(Id),
);
	PRINT 'A tabela TB_Postagens foi criada com sucesso';
GO

/* Cria o relacionamento N:M entre jogadores e grupos*/
CREATE TABLE RL_Jogadores_Grupos(
	Id INT IDENTITY(1,1) NOT NULL,

	GrupoId INT NOT NULL,
	JogadorId INT NOT NULL,

	CONSTRAINT [PK_Jogadores_Grupos] PRIMARY KEY (Id),
	CONSTRAINT [FK_TB_Grupos] FOREIGN KEY (GrupoId) REFERENCES TB_Grupos(Id),
	CONSTRAINT [FK_TB_Jogadores] FOREIGN KEY (JogadorId) REFERENCES TB_Jogadores(Id),
);
	PRINT 'A tabela RL_Jogadores_Grupos foi criada com sucesso';
GO

USE Master;
GO

ALTER DATABASE Forum_Games MODIFY NAME = [Forum_Jogos_XPTO]; /*Altera o nome do banco de dados para Forum_Jogos_XPTO */
GO

ALTER DATABASE [Forum_Jogos_XPTO] MODIFY NAME = [Forum_Games]; /*Altera o nome do banco de dados para Forum_Games */
GO

USE Forum_Games;
GO
