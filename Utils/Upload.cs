using System.IO;
using System;
using System.Drawing;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace ForumGames.Utils
{
    public static class Upload
    {
        public static string UploadFile(IFormFile arquivo)
        {
            string diretorio = "Images";
            string[] extensoesPermitidas = { "jpg", "jpeg", "png", "svg" };
            try
            {
                string ImageBase64 = "";
                var novoNome = "";
                // Onde será salvo o arquivo
                var pasta = Path.Combine("StaticFiles", diretorio);
                var caminho = Path.Combine(Directory.GetCurrentDirectory(), pasta);
                // Verifica se existe um arquivo para ser salvo
                if (arquivo.Length > 0)
                {
                    string nomeArquivo = ContentDispositionHeaderValue.Parse(arquivo.ContentDisposition).FileName.Trim('"');
                    if (ValidarExtensao(extensoesPermitidas, nomeArquivo))
                    {
                        var extensao = RetornarExtensao(nomeArquivo);
                        novoNome = $"{Guid.NewGuid()}.{extensao}";
                        var caminhoCompleto = Path.Combine(caminho, novoNome);
                        // Salvar o arquivo na aplicação
                        using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                        {
                            arquivo.CopyTo(stream);
                            
                        }
                        using (MemoryStream ms = new MemoryStream())
                        {
                            var fileStream = File.OpenRead(caminhoCompleto);
                            fileStream.CopyTo(ms);
                            byte[] imageBytes = ms.ToArray();
                            ImageBase64 = Convert.ToBase64String(imageBytes);
                        }
                    }
                }
                return novoNome;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        // Validar extensões

        // Validar extensão de arquivo
        public static bool ValidarExtensao(string[] extensoesPermitidas, string nomeArquivo)
        {
            var extensao = RetornarExtensao(nomeArquivo);
            foreach (var ext in extensoesPermitidas)
            {
                if (ext == extensao)
                {
                    return true;
                }
            }
            return false;
        }
        // Remover arquivo

        // Retornar Extensão
        public static string RetornarExtensao(string nomeArquivo)
        {
            // arquivo.jpeg arqui.vo.jpeg
            //      0   1       0   1   2
            // Por isso o dados.Length-1 - Para pegar sempre o último elemento do array após fazer o split
            string[] dados = nomeArquivo.Split('.');
            return dados[dados.Length - 1];
        }

        public static string ToBase64(IFormFile arquivo)
        {
            string nomeArquivo = ContentDispositionHeaderValue.Parse(arquivo.ContentDisposition).FileName.Trim('"');
            var fileStream = File.OpenRead(arquivo.ContentDisposition);
            using MemoryStream ms = new MemoryStream();
            fileStream.CopyTo(ms);
            byte[] imageBytes = ms.ToArray();
            var ImageBase64 = Convert.ToBase64String(imageBytes);
            return "";
        }
    }
}

