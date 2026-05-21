using BootStrapper.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BootStrapper.Core.Service;

internal class ProjectService
{
    void CreateProject(UserConfig config)
    {
        // Lógica para criar um projeto com base na configuração fornecida
        // Chama? a criação de arquivos, pastas, etc.
    }

    void DeleteProject(string projectPath)
    {
        // Lógica para excluir um projeto com base no caminho fornecido
    }

    void UpdateProject(string projectPath, UserConfig newConfig)
    {
        // Lógica para atualizar um projeto existente com base no caminho e na nova configuração fornecida
    }

    void GetProject(string projectPath)
    {
        // Lógica para obter as informações de um projeto com base no caminho fornecido
        // leitura de arquivos, pastas, etc.
    }

    void ListProjects(string projectFolderPath)
    {
        // Lógica para listar todos os projetos disponíveis
        // leitura de um diretório específico onde os projetos estão
    }

    void GetProjectHistory(string projectPath)
    {
        // Lógica para obter o histórico de mudanças de um projeto com base no caminho fornecido
        // consulta do banco de dados
    }
}
