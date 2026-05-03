<h1>Sistema de Gestão de Restaurante - Sugestão de Compras</h1>

Este é um sistema desenvolvido em .NET para auxiliar na gestão de estoque de um restaurante. Com base no histórico de vendas de pratos dos últimos 7 dias e em suas respectivas receitas (ingredientes), o sistema calcula a demanda futura de insumos e gera uma lista de compras sugerida com uma margem de segurança de 20%.

<h2>🛠️ Tecnologias Utilizadas</h2>

- Linguagem: C# (.NET)
- Banco de Dados: Microsoft SQL Server (Containerizado via Docker)
- ORM: Entity Framework Core (EF Core) para persistência e mapeamento objeto-relacional
- Arquitetura: Domain-Driven Design (DDD)

<h2>🏛️ Arquitetura do Sistema (DDD)</h2>

O projeto foi construído seguindo os princípios do Domain-Driven Design (DDD) para garantir o desacoplamento entre a lógica de negócios e os detalhes de infraestrutura (banco de dados, frameworks).

A solução está dividida em 4 camadas principais:

1. Domínio (SistemaRestaurante.Domain)
O coração do sistema. Contém as Entidades que representam os conceitos do negócio, as Interfaces (contratos) para os repositórios e o Serviço de Domínio responsável por processar o cálculo de sugestão de compras. Não possui dependências de frameworks externos.

2. Aplicação (SistemaRestaurante.Application)
Atua como orquestradora dos casos de uso. Ela coordena a busca de dados na infraestrutura (através das interfaces do domínio), aciona a lógica de negócio no domínio e expõe os resultados utilizando DTOs (Data Transfer Objects).

3. Infraestrutura (SistemaRestaurante.Infrastructure)
Responsável pela comunicação com o mundo externo. Contém a implementação dos repositórios definidos no Domínio, o contexto do banco de dados (RestauranteDbContext) e as configurações do Entity Framework Core para o SQL Server.

4. Apresentação (SistemaRestaurante.Presentation.Console)
O ponto de entrada da aplicação. Configura a Injeção de Dependência, lê o arquivo appsettings.json, executa as Migrations para garantir que o banco esteja atualizado no Docker, faz a carga inicial de dados (Seed) e exibe o relatório final no console.

<h2>🧩 Conceitos de Engenharia de Software Aplicados</h2>
1. Separação de Responsabilidades (SoC)
Cada camada possui um propósito claro. Mudanças na camada de dados (como trocar o SQL Server por outro banco) não afetam as regras de negócio definidas no Domínio.

2. Inversão de Dependência (IoC / DI)
Os componentes de alto nível (Aplicação) não dependem diretamente de módulos de baixo nível (Infraestrutura). Ambos dependem de abstrações (Interfaces do Domínio), o que torna o código flexível e testável.

3. Migrations & Code-First
O banco de dados é modelado diretamente através de código C# (classes de domínio) e mapeado na Fluent API do EF Core. O controle de versão do esquema do banco de dados é feito por meio de Migrations do EF Core.

4. Containerização com Docker
O banco de dados Microsoft SQL Server roda de forma isolada dentro de um container Docker, facilitando a configuração rápida do ambiente de desenvolvimento em qualquer máquina.

<h2>🚀 Como Executar o Projeto</h2>
Pré-requisitos
.NET SDK instalado.

Docker instalado e rodando.

1. Subir o SQL Server no Docker

docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=SuaSenhaForte123!" \
   -p 1433:1433 --name sql_restaurante \
   -d mcr.microsoft.com/mssql/server:2022-latest

2. Clonar o repositório e restaurar dependências

git clone https://github.com/seu-usuario/SistemaRestaurante.git
cd SistemaRestaurante
dotnet restore

3. Criar o Banco e Aplicar as Migrations no Banco de DadosBash

dotnet ef migrations add InitialCreation --project src/SistemaRestaurante.Infrastructure/SistemaRestaurante.Infrastructure.csproj --startup-project src/SistemaRestaurante.Presentation.Console/SistemaRestaurante.Presentation.Console.csproj

dotnet ef database update --project src/SistemaRestaurante.Infrastructure/SistemaRestaurante.Infrastructure.csproj --startup-project src/SistemaRestaurante.Presentation.Console/SistemaRestaurante.Presentation.Console.csproj

4. Rodar a aplicação

cd src/SistemaRestaurante.Presentation.Console

dotnet run


