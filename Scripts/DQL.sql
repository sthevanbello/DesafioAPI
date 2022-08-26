/* Data Query Language*/

USE Forum_Games;
GO

/* Faz a pesquisa e retorna todos os campos de todas as tabelas com o nome padrão que foram criados*/
SELECT * FROM TB_Categorias_Grupos;
SELECT * FROM TB_Categorias_Postagens;
SELECT * FROM TB_Jogadores;
SELECT * FROM RL_Jogadores_Grupos;
SELECT * FROM TB_Postagens;
SELECT * FROM TB_Grupos;
GO

/* Faz a pesquisa e retorna todos os campos com um nome personalizado utilizando 'Alias' com AS*/
SELECT J.Id AS 'Id jogador', J.Nome AS 'Nome Jogador', J.Usuario AS 'Nome de Usuário', J.Senha AS 'Senha do usuário' FROM TB_Jogadores AS J;
SELECT C.Id AS 'Id da categoria de cada grupo', C.Categoria AS 'Nome da categoria' FROM TB_Categorias_Grupos AS C;
SELECT CP.Id AS 'Id da categoria de postagens' FROM TB_Categorias_Postagens AS CP;
SELECT JG.Id AS 'Id do relacionamento jogador x grupo', JG.GrupoId AS 'Id do Grupo relacionado ao jogador', JG.JogadorId AS 'Id do Jogador relacionado ao grupo' FROM RL_Jogadores_Grupos AS JG;
SELECT	P.Id AS 'Id da postagem', P.Titulo AS 'Título da postagem', P.Texto AS 'Texto da postagem ou link para texto externo', 
		P.Imagem AS 'Link para a imagem', P.DataHora AS 'Data e hora da postagem', P.GrupoId AS 'Grupo da postagem', P.CategoriaPostagemId AS 'Categoria da postagem',
		P.JogadorId AS 'Jogador que fez a postagem'
		FROM TB_Postagens AS P;
GO


/* Verifica de quais grupos cada jogador participa */
SELECT J.Id AS 'Id Jogador', J.Nome, G.Descricao, G.Id AS 'Id do grupo' FROM TB_Jogadores AS J
	INNER JOIN	RL_Jogadores_Grupos AS RL ON J.Id = RL.JogadorId
	INNER JOIN TB_Grupos AS G ON G.Id = RL.GrupoId
	ORDER BY J.Id;
GO

/* Verifica de qual grupo o jogador 1 participa*/
SELECT J.Id AS 'Id Jogador', J.Nome, G.Descricao, G.Id AS 'Id do grupo' FROM TB_Jogadores AS J
	INNER JOIN	RL_Jogadores_Grupos AS RL ON J.Id = RL.JogadorId
	INNER JOIN TB_Grupos AS G ON G.Id = RL.GrupoId
	WHERE J.Id = 1
GO

/* Verfica quais postagens cada jogador fez e em qual grupo foram feitas ordenadas pelo Id do jogador*/
SELECT J.Nome, J.Usuario, G.Descricao AS 'Descrição Grupo', P.Titulo AS 'Título da postagem', p.Texto  AS 'Texto da postagem', p.Imagem FROM TB_Jogadores AS J
	INNER JOIN	RL_Jogadores_Grupos AS RL ON J.Id = RL.JogadorId
	INNER JOIN TB_Grupos AS G ON G.Id = RL.GrupoId
	INNER JOIN TB_Postagens AS P ON J.Id = P.JogadorId AND G.Id = P.GrupoId
	ORDER BY J.Id;
GO

/* Verfica quais postagens cada jogador fez e em qual grupo foram feitas ordenadas pelo Id do jogador e exibindo a categoria da postagem e do grupo */
SELECT J.Id, J.Nome, P.Titulo AS 'Título da postagem', P.Texto AS 'Texto da postagem', CP.Categoria, G.Descricao AS 'Descrição Grupo', CG.Categoria AS 'Categoria Grupo' FROM TB_Postagens AS P 
	INNER JOIN TB_Jogadores AS J ON P.JogadorId = J.Id
	INNER JOIN TB_Categorias_Postagens AS CP ON P.CategoriaPostagemId = CP.Id
	INNER JOIN TB_Grupos AS G ON G.Id = P.GrupoId 
	INNER JOIN TB_Categorias_Grupos AS CG ON G.CategoriaId = CG.Id
	
/* Exibir todos ao comentar o WHERE ou exibir um jogador específico*/
--WHERE P.JogadorId = 8;
GO

/* Verifica os jogadores e mostra se há postagens, caso não haja postagens mostra NULL na tabela da direita, pois retorna todos os jogadores da tabela da esquerda*/
SELECT * FROM TB_Jogadores AS J
	LEFT JOIN TB_Postagens AS P ON P.JogadorId = J.Id 
GO

/* Verifica as postagens (coluna da direita) e compara se todas tem jogadores (coluna da esquerda), caso haja, retorna null na coluna da esquerda*/
/* Como não é permitido haver uma postagem sem jogador, não retornará NULL em nenhum campo*/
SELECT * FROM TB_Jogadores AS J
	RIGHT JOIN TB_Postagens AS P ON P.JogadorId = J.Id 
GO

/* Confere se um jogador faz parte de um grupo e mostra se houve postagem. Se não houve retorna Null por conta do Left Join*/
SELECT * FROM TB_Jogadores AS J
	INNER JOIN RL_Jogadores_Grupos AS JG ON JG.JogadorId = J.Id
	LEFT JOIN TB_Grupos AS G ON G.Id = JG.Id
GO