/* Data modification language*/

USE Forum_Games;
GO

INSERT INTO TB_Jogadores (Nome, Email, Usuario, Senha) /* Jogadores e seus dados de cadastro*/
VALUES	


('Homer Simpson', 'homer@simpson', 'Homer', 'homer1234'),
('Marge Simpson', 'marge@simpson', 'Marge', 'marge1234'),
('Bart Simpson', 'bart@simpson', 'Bart', 'bart1234'),
('Lisa Simpson', 'lisa@simpson', 'Lisa', 'lisa1234'),
('Maggie Simpson', 'maggie@simpson', 'Maggie', 'maggie1234'),
('"Abe" Simpson', 'vovo@simpson', 'Vovô', 'vovo1234'),
('Ned Flanders', 'ned@simpson', 'Ned', 'ned1234'),
('Apu Nahasapeemapetilon', 'apu@simpson', 'Apu', 'apu1234'),
('Milhouse Van Houten', 'milhouse@simpson',	'Milhouse',	'milhouse1234'),
('Reverendo Timothy Lovejoy', 'reverendo@simpson', 'Reverendo',	'reverendo1234');
GO

INSERT INTO TB_Categorias_Grupos (Categoria) /* Categorias de grupos*/
VALUES	('Acao'),
		('RPG'),
		('FPS'),
		('MMORPG'),
		('Futebol'),
		('Corrida'),
		('Luta'),
		('Aventura'),
		('Tenis'),
		('Esportes gerais');
GO

INSERT INTO TB_Categorias_Postagens (Categoria) /* Categorias de postagens*/
VALUES	('Conquistas'),
		('Dicas'),
		('Evolucao'),
		('Falhas nos jogos'),
		('Experiencias'); 
GO

INSERT INTO TB_Grupos (Descricao, CategoriaId) /* Grupos de cada jogo diferente*/
VALUES	('God Of War', 1),
		('The Witcher', 2),
		('Call of Duty', 3),
		('League of Legends', 4),
		('Fifa 22', 5),
		('Gran Turismo', 6),
		('Street fighter 5', 7),
		('Uncharted 4', 8),
		('Tennis World Tour 2', 9),
		('Tony Hawks 1 + 2', 10),
		('Elden Ring', 2),
		('Fifa 23', 5),
		('Formula 1 2022', 6),
		('The King of Fighters', 7),
		('Mortal Kombat', 7); 
GO

INSERT INTO RL_Jogadores_Grupos (GrupoId, JogadorId) /* Relação N:M entre jogadores e grupos - Jogador pode participar de vários grupos*/
VALUES	(1, 1),
		(2, 2),
		(3, 3),
		(4, 4),
		(5, 5),
		(6, 6),
		(7, 7),
		(8, 8),
		(9, 9),
		(10, 10),
		(1, 2),
		(2, 1),
		(3, 4),
		(4, 3),
		(5, 6),
		(6, 5),
		(7, 8),
		(8, 9),
		(9, 10),
		(10, 9)
GO

INSERT INTO TB_Postagens (Titulo, Texto, Imagem, DataHora, GrupoId, CategoriaPostagemId, JogadorId) /* Postagens com seus campos preenchidos e com links para as imagens dos jogos*/
VALUES	
		('Como jogar God of War [Guia para iniciantes]', 'Veja como jogar God of War neste link', 'https://bit.ly/3JYBACB', GETDATE(), 1, 2, 1),
		('Habilitando a forja de itens de qualidade superior e mestre', 'Ferreiro mestre em Novigrad', 'https://bit.ly/3SL30Qm', GETDATE(), 2, 2, 2),
		('9 dicas essenciais de como jogar Call of Duty Mobile!', 'https://www.ligadosgames.com/call-of-duty-mobile-dicas/', 'https://bit.ly/3JWatIe', GETDATE(), 3, 2, 3),
		('Como jogar LoL - as melhores dicas para começar do jeito certo', 'https://www.ligadosgames.com/como-jogar-lol/', 'https://bit.ly/3dxtPaO', GETDATE(), 4, 2, 4),
		('Como atacar melhor', 'https://ge.globo.com/esports/fifa/noticia/2022/05/07/fifa-22-como-atacar-melhor-confira-dicas-para-ataque.ghtml', 'https://bit.ly/3AlkpIi', GETDATE(), 5, 2, 5),
		('Link com a lista de troféus', 'https://www.gameblast.com.br/2022/03/gran-turismo-7-guia-trofeus-conquistas.html', 'https://bit.ly/3drIBzD', GETDATE(), 6, 1, 6),
		('Capcom identifica bugs da nova temporada de Street Fighter V e diz que correções virão em março', 'https://bit.ly/3c0IHOw', 'https://bit.ly/3A5zwo2', GETDATE(), 7, 4, 7),
		('Caçador de relíquias', 'Encontre as relíquias estranhas - Em Uncharted 4, temos três dessas relíquias: uma de The Last of Us, Jax and Daxter e Crash Bandicoot.', 'https://bit.ly/3w3LxJq', GETDATE(), 8, 1, 8),
		('Cabeça da turma!', 'Derrote 20 inimigos com tiros na cabeça. Veja a conquista você sabe usar a cabeça.', 'https://bit.ly/3Prp3IU', GETDATE(), 8, 1, 9),
		('Dicas para mandar bem no jogo', 'https://ovicio.com.br/dicas-para-mandar-bem-em-street-fighter-v/', 'https://bit.ly/3SL3K86', GETDATE(), 7, 2, 8),
		('Onde vender pelo preço total os troféus de caça', 'Vender troféus em Novigrad', 'https://bit.ly/3JWMclq', GETDATE(), 2, 2, 1);
GO

BEGIN TRANSACTION /* Serve para começar uma alteração e não salvar automaticamente - Caso faça algo errado é só executar o ROLLBACK*/
/* Simulando como se tivesse esquecido uma informação na primeira postagem usando a função CONCAT*/
-- UPDATE TB_Postagens SET Texto = CONCAT(Texto, ': https://bit.ly/3wa41YK') WHERE ID = 1; 

/* Simulando como se tivesse esquecido uma informação na Primeira postagem usando concatenação simples*/
UPDATE TB_Postagens SET Texto = Texto + ': https://bit.ly/3wa41YK' WHERE ID = 1; 

SELECT * FROM TB_Postagens WHERE Id = 1; /* Conferir se a mudança foi feita corretamente*/

--ROLLBACK /* Retorna ao estado anterior caso seja feito algo errado*/

COMMIT /* Confirma a modificação se tudo estiver correto*/
GO


