# ?? ESG Diversity & Inclusion API

API REST para gestão de diversidade e inclusão corporativa com foco em métricas ESG (Environmental, Social and Governance).

## ?? Índice

- [Sobre o Projeto](#sobre-o-projeto)
- [Funcionalidades](#funcionalidades)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Pré-requisitos](#pré-requisitos)
- [Instalação e Configuração](#instalação-e-configuração)
- [Como Executar](#como-executar)
- [Endpoints da API](#endpoints-da-api)
- [Autenticação JWT](#autenticação-jwt)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Comandos Úteis](#comandos-úteis)
- [Banco de Dados](#banco-de-dados)
- [Exemplos de Uso](#exemplos-de-uso)
- [Contribuindo](#contribuindo)

---

## ?? Sobre o Projeto

Esta API foi desenvolvida para auxiliar empresas no gerenciamento e monitoramento de suas iniciativas de diversidade e inclusão, fornecendo:

- ?? Métricas detalhadas de diversidade por departamento
- ?? Acompanhamento de metas e progressos
- ?? Gestão de eventos de inclusão
- ?? Análise de equidade salarial
- ?? **CRUD completo de funcionários**
- ?? **CRUD completo de metas de diversidade**
- ?? **CRUD completo de eventos de inclusão**
- ?? **Autenticação JWT com controle de acesso baseado em roles**

---

## ? Funcionalidades

### ?? **Autenticação e Autorização**
- Login com JWT
- Registro de novos usuários
- Controle de acesso baseado em roles (Admin, HR, User)
- Tokens com expiração configurável

### ?? **Gestão de Funcionários (CRUD Completo)**
- Listar funcionários com paginação e filtros
- Criar novos funcionários
- Atualizar informações de funcionários
- Soft delete de funcionários
- **Permissões**: GET (Todos autenticados), POST/PUT (Admin, HR), DELETE (Admin)

### ?? **Gestão de Metas de Diversidade (CRUD Completo)**
- Criar metas de diversidade por departamento
- Acompanhar progresso das metas
- Atualizar percentuais e status
- Deletar metas concluídas
- **Permissões**: Apenas Admin e HR

### ?? **Gestão de Eventos de Inclusão (CRUD Completo)**
- Criar eventos de inclusão
- Listar eventos com análise de impacto
- Atualizar informações e status
- Deletar eventos
- **Permissões**: GET (Todos autenticados), POST/PUT (Admin, HR), DELETE (Admin)

### ?? **Relatórios e Análises (Apenas Leitura)**
- Diversity Metrics - Análise de diversidade por departamento
- Goal Progress - Acompanhamento de metas
- Salary Equity - Análise de equidade salarial (Requer Auth)

---

## ?? Tecnologias Utilizadas

- **Framework**: .NET 8.0
- **ORM**: Entity Framework Core 8.0
- **Banco de Dados**: SQL Server LocalDB
- **Autenticação**: JWT Bearer
- **Validação**: FluentValidation 11.3.0
- **Documentação**: Swagger/OpenAPI (Swashbuckle 6.5.0)
- **Logging**: Microsoft.Extensions.Logging

### Pacotes NuGet

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
<PackageReference Include="FluentValidation.AspNetCore" Version="11.3.0" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="7.0.3" />
```

---

## ?? Pré-requisitos

Antes de começar, certifique-se de ter instalado:

### Obrigatórios

- ?? [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) ou superior
- ??? [SQL Server LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) (geralmente incluído com Visual Studio)

### Recomendados

- ?? [Visual Studio 2022](https://visualstudio.microsoft.com/) (Community ou superior)
- ?? [Visual Studio Code](https://code.visualstudio.com/) (alternativa)
- ?? [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) (opcional, para gerenciar o banco)
- ?? [Postman](https://www.postman.com/downloads/) ou [Insomnia](https://insomnia.rest/download) (opcional, para testar APIs)

### Verificar Instalação

```powershell
# Verificar versão do .NET
dotnet --version

# Verificar SDKs instalados
dotnet --list-sdks

# Verificar runtimes instalados
dotnet --list-runtimes
```

---

## ?? Instalação e Configuração

### 1. Clonar o Repositório

```powershell
git clone <url-do-repositorio>
cd ESGDiversity.API
```

### 2. Restaurar Dependências

```powershell
dotnet restore
```

### 3. Configurar Connection String (Opcional)

O projeto já vem configurado com uma connection string padrão. Se necessário, edite o arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ESGDiversityDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### 4. Configurar JWT (Opcional)

Para ambientes de produção, altere as configurações JWT em `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "SuaChaveSecretaAqui_MinimoDe32Caracteres!",
    "Issuer": "ESGDiversityAPI",
    "Audience": "ESGDiversityAPI",
    "ExpiryInMinutes": 60
  }
}
```

---

## ?? Como Executar

### Método 1: Linha de Comando

```powershell
# Navegar até a pasta do projeto
cd C:\Users\rafam\source\repos\ESGDiversity.API

# Executar a aplicação
dotnet run
```

### Método 2: Com Hot Reload (Recomendado para Desenvolvimento)

```powershell
# Executa e recarrega automaticamente ao salvar alterações
dotnet watch run
```

### Método 3: Visual Studio

1. Abra o arquivo `ESGDiversity.API.sln`
2. Pressione **F5** (executar com debug) ou **Ctrl+F5** (executar sem debug)
3. Ou clique no botão verde ?? **ESGDiversity.API**

### Método 4: Visual Studio Code

1. Abra a pasta do projeto
2. Pressione **F5** ou vá em **Run > Start Debugging**
3. Selecione **.NET Core** se solicitado

---

## ?? Acessando a Aplicação

Após iniciar, a aplicação estará disponível em:

- **HTTPS**: `https://localhost:5001`
- **HTTP**: `http://localhost:5000`
- **Swagger UI**: `https://localhost:5001/` (página inicial)

O Swagger abrirá automaticamente no navegador mostrando toda a documentação interativa da API.

---

## ?? Endpoints da API

### Base URL
```
https://localhost:5001/api
```

### 1. Diversity Metrics

#### Listar Métricas de Diversidade
```http
GET /api/DiversityMetrics
```

**Query Parameters:**
- `page` (int, default: 1) - Número da página
- `pageSize` (int, default: 10) - Itens por página (máx: 100)
- `department` (string, optional) - Filtrar por departamento

**Exemplo de Request:**
```http
GET /api/DiversityMetrics?page=1&pageSize=5&department=IT
```

**Exemplo de Response:**
```json
{
  "page": 1,
  "pageSize": 5,
  "totalCount": 7,
  "totalPages": 2,
  "hasPrevious": false,
  "hasNext": true,
  "data": [
    {
      "department": "IT",
      "womenPercentage": 35.5,
      "minorityPercentage": 42.3,
      "disabledPercentage": 5.2,
      "totalEmployees": 120,
      "diversityScore": 27.67,
      "ethnicityDistribution": {
        "White": 69,
        "Asian": 25,
        "Black": 15,
        "Hispanic": 11
      },
      "ageGroupDistribution": {
        "20-29": 45,
        "30-39": 50,
        "40-49": 20,
        "50-59": 5
      }
    }
  ]
}
```

---

### 2. Goal Progress

#### Listar Progresso das Metas
```http
GET /api/GoalProgress
```

**Query Parameters:**
- `page` (int, default: 1)
- `pageSize` (int, default: 10)

**Exemplo de Request:**
```http
GET /api/GoalProgress?page=1&pageSize=10
```

**Exemplo de Response:**
```json
{
  "page": 1,
  "pageSize": 10,
  "totalCount": 15,
  "totalPages": 2,
  "data": [
    {
      "goalId": 1,
      "department": "Engineering",
      "metricType": "Women in Leadership",
      "targetPercentage": 40.0,
      "currentPercentage": 32.5,
      "progressPercentage": 81.25,
      "daysRemaining": 180,
      "status": "On Track",
      "isOnTrack": true,
      "recommendedActions": "Continue current strategy"
    }
  ]
}
```

---

### 3. Inclusion Events

#### Listar Eventos de Inclusão
```http
GET /api/InclusionEvents
```

**Query Parameters:**
- `page` (int, default: 1)
- `pageSize` (int, default: 10)
- `category` (string, optional) - Filtrar por categoria

**Exemplo de Request:**
```http
GET /api/InclusionEvents?page=1&pageSize=10&category=Training
```

**Exemplo de Response:**
```json
{
  "page": 1,
  "pageSize": 10,
  "totalCount": 25,
  "totalPages": 3,
  "data": [
    {
      "eventId": 1,
      "title": "Diversity & Inclusion Workshop",
      "eventDate": "2024-03-15T14:00:00Z",
      "category": "Training",
      "participantsCount": 85,
      "budget": 5000.00,
      "department": "HR",
      "status": "Completed",
      "costPerParticipant": 58.82,
      "impactLevel": "High"
    }
  ]
}
```

---

### 4. Salary Equity ?? (Requer Autenticação)

#### Análise de Equidade Salarial
```http
GET /api/SalaryEquity
```

**Headers Obrigatórios:**
```http
Authorization: Bearer {seu_token_jwt}
```

**Query Parameters:**
- `page` (int, default: 1)
- `pageSize` (int, default: 10)
- `department` (string, optional)

**Exemplo de Request:**
```http
GET /api/SalaryEquity?page=1&pageSize=10&department=IT
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Exemplo de Response:**
```json
{
  "page": 1,
  "pageSize": 10,
  "totalCount": 18,
  "totalPages": 2,
  "data": [
    {
      "department": "IT",
      "position": "Senior Developer",
      "maleAverageSalary": 95000.00,
      "femaleAverageSalary": 92000.00,
      "payGapPercentage": 3.16,
      "payGapAmount": 3000.00,
      "maleCount": 15,
      "femaleCount": 12,
      "equityStatus": "Equitable"
    }
  ]
}
```

---

## ?? Autenticação JWT

A API utiliza JWT (JSON Web Tokens) para autenticação. Todos os endpoints a partir de `/api/` requerem um token JWT válido no cabeçalho `Authorization`.

### 1. Gerar Token JWT

Endpoint para login e geração de token JWT:

```http
POST /api/auth/login
```

**Body Exemplo:**
```json
{
  "username": "admin",
  "password": "senha_forte"
}
```

**Response Exemplo:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2024-03-15T14:00:00Z"
}
```

### 2. Roles e Permissões

- **Admin**: Acesso completo a todos os endpoints (CRUD)
- **HR**: Acesso a gestão de funcionários e metas (semana)
- **User**: Acesso somente a leitura das métricas e eventos

### 3. Configuração Atual do JWT

```json
{
  "Jwt": {
    "Key": "YourSecretKeyHere12345678901234567890",
    "Issuer": "ESGDiversityAPI",
    "Audience": "ESGDiversityAPI",
    "ExpiryInMinutes": 60
  }
}
```

?? **IMPORTANTE**: Altere a chave secreta em ambientes de produção!

---

## ??? Estrutura do Projeto

```
ESGDiversity.API/
??? ?? Controllers/              # Controladores da API
?   ??? DiversityMetricsController.cs
?   ??? GoalProgressController.cs
?   ??? InclusionEventsController.cs
?   ??? SalaryEquityController.cs
?   ??? AuthController.cs         # Controlador de Autenticação
?
??? ?? Data/                     # Contexto do banco de dados
?   ??? ESGDiversityDbContext.cs
?
??? ?? Middleware/               # Middlewares customizados
?   ??? ExceptionHandlingMiddleware.cs
?
??? ?? Models/                   # Entidades do domínio
?   ??? DiversityGoal.cs
?   ??? Employee.cs
?   ??? InclusionEvent.cs
?   ??? SalaryEquityAnalysis.cs
?
??? ?? Services/                 # Lógica de negócio
?   ??? DiversityMetricsService.cs
?   ??? GoalProgressService.cs
?   ??? InclusionEventService.cs
?   ??? SalaryEquityService.cs
?   ??? AuthService.cs            # Serviço de Autenticação
?
??? ?? ViewModels/               # DTOs de resposta
?   ??? DiversityMetricsViewModel.cs
?   ??? GoalProgressViewModel.cs
?   ??? InclusionEventSummaryViewModel.cs
?   ??? PaginatedResponse.cs
?   ??? SalaryEquityViewModel.cs
?
??? ?? Validators/               # Validadores FluentValidation
?   ??? PaginationValidator.cs
?
??? ?? Program.cs                # Configuração da aplicação
??? ?? appsettings.json          # Configurações
??? ?? ESGDiversity.API.csproj   # Arquivo do projeto
```

---

## ?? Comandos Úteis

### Gerenciamento do Projeto

```powershell
# Restaurar pacotes NuGet
dotnet restore

# Compilar o projeto
dotnet build

# Compilar em modo Release
dotnet build --configuration Release

# Executar a aplicação
dotnet run

# Executar com hot reload (auto-restart)
dotnet watch run

# Limpar artefatos de build
dotnet clean

# Publicar para produção
dotnet publish -c Release -o ./publish
```

### Gerenciamento de Pacotes

```powershell
# Adicionar novo pacote
dotnet add package NomeDoPacote

# Adicionar pacote com versão específica
dotnet add package NomeDoPacote --version 1.0.0

# Remover pacote
dotnet remove package NomeDoPacote

# Listar pacotes instalados
dotnet list package

# Atualizar pacotes
dotnet add package NomeDoPacote
```

### Entity Framework Core

```powershell
# Criar nova migration
dotnet ef migrations add NomeDaMigration

# Aplicar migrations ao banco
dotnet ef database update

# Reverter última migration
dotnet ef migrations remove

# Listar migrations
dotnet ef migrations list

# Gerar script SQL
dotnet ef migrations script

# Dropar o banco de dados
dotnet ef database drop

# Ver informações do contexto
dotnet ef dbcontext info

# Gerar código do banco existente (scaffold)
dotnet ef dbcontext scaffold "ConnectionString" Microsoft.EntityFrameworkCore.SqlServer -o Models
```

### Informações e Diagnóstico

```powershell
# Verificar versão do .NET
dotnet --version

# Listar SDKs instalados
dotnet --list-sdks

# Listar runtimes instalados
dotnet --list-runtimes

# Ver informações do projeto
dotnet --info

# Verificar problemas no projeto
dotnet build /v:detailed
```

### Testes (quando implementados)

```powershell
# Executar todos os testes
dotnet test

# Executar testes com cobertura
dotnet test /p:CollectCoverage=true

# Executar testes específicos
dotnet test --filter "FullyQualifiedName~ClassName"
```

---

## ??? Banco de Dados

### Informações do Banco

- **Tipo**: SQL Server LocalDB
- **Nome**: `ESGDiversityDb`
- **Connection String**: `Server=(localdb)\\mssqllocaldb;Database=ESGDiversityDb;Trusted_Connection=True;MultipleActiveResultSets=true`

### Tabelas

1. **Employees** - 500 funcionários de exemplo
   - Informações demográficas
   - Dados salariais
   - Departamento e cargo

2. **DiversityGoals** - Metas de diversidade
   - Objetivos por departamento
   - Percentuais alvo e atual
   - Status e prazos

3. **InclusionEvents** - Eventos de inclusão
   - Descrição e categoria
   - Orçamento e participantes
   - Status do evento

4. **SalaryEquityAnalyses** - Análises de equidade
   - Médias salariais por gênero
   - Gap salarial
   - Métricas por departamento

### Criação Automática do Banco

O banco de dados é criado **automaticamente** na primeira execução da aplicação através do código:

```csharp
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ESGDiversityDbContext>();
    db.Database.EnsureCreated(); // ? Cria o banco automaticamente
}
```

### Seed Data

500 funcionários de exemplo são automaticamente gerados com:
- **Gêneros**: Male, Female, Non-Binary, Prefer not to say
- **Etnias**: White, Black, Asian, Hispanic, Indigenous, Mixed, Other
- **Departamentos**: IT, HR, Finance, Marketing, Sales, Operations, Engineering
- **Cargos**: Junior, Mid-Level, Senior, Lead, Manager, Director
- **Salários**: Entre $40,000 e $150,000

### Acessar o Banco de Dados

#### Via SQL Server Management Studio (SSMS)
1. Abrir SSMS
2. Conectar a: `(localdb)\mssqllocaldb`
3. Navegar até: Databases > ESGDiversityDb

#### Via Visual Studio
1. View > SQL Server Object Explorer
2. Expandir: (localdb)\mssqllocaldb
3. Databases > ESGDiversityDb

#### Via Azure Data Studio
1. Nova conexão
2. Server: `(localdb)\mssqllocaldb`
3. Authentication: Windows Authentication

### Recriar o Banco de Dados

```powershell
# Método 1: Via EF Core
dotnet ef database drop --force
dotnet run

# Método 2: Via SQL
sqlcmd -S "(localdb)\mssqllocaldb" -Q "DROP DATABASE ESGDiversityDb"
dotnet run
```

---

## ?? Autenticação JWT

A API utiliza JWT (JSON Web Tokens) para autenticação. Todos os endpoints a partir de `/api/` requerem um token JWT válido no cabeçalho `Authorization`.

### 1. Gerar Token JWT

Endpoint para login e geração de token JWT:

```http
POST /api/auth/login
```

**Body Exemplo:**
```json
{
  "username": "admin",
  "password": "senha_forte"
}
```

**Response Exemplo:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2024-03-15T14:00:00Z"
}
```

### 2. Roles e Permissões

- **Admin**: Acesso completo a todos os endpoints (CRUD)
- **HR**: Acesso a gestão de funcionários e metas (semana)
- **User**: Acesso somente a leitura das métricas e eventos

### 3. Configuração Atual do JWT

```json
{
  "Jwt": {
    "Key": "YourSecretKeyHere12345678901234567890",
    "Issuer": "ESGDiversityAPI",
    "Audience": "ESGDiversityAPI",
    "ExpiryInMinutes": 60
  }
}
```

?? **IMPORTANTE**: Altere a chave secreta em ambientes de produção!

---

## ?? Exemplos de Uso

### 1. Obter Token JWT

```bash
curl -X POST https://localhost:5001/api/auth/login -H "Content-Type: application/json" -d "{\"username\":\"admin\",\"password\":\"senha_forte\"}"
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2024-03-15T14:00:00Z"
}
```

### 2. Listar Funcionários (GET)

```bash
curl -X GET https://localhost:5001/api/Employees?page=1&pageSize=10 -H "Authorization: Bearer {seu_token_jwt}"
```

**Response:**
```json
{
  "page": 1,
  "pageSize": 10,
  "totalCount": 500,
  "totalPages": 50,
  "data": [
    {
      "id": 1,
      "name": "John Doe",
      "email": "john.doe@example.com",
      "gender": "Male",
      "ethnicity": "White",
      "department": "IT",
      "position": "Developer",
      "salary": 60000.00,
      "isActive": true
    }
  ]
}
```

### 3. Criar Novo Funcionário (POST)

```bash
curl -X POST https://localhost:5001/api/Employees -H "Content-Type: application/json" -H "Authorization: Bearer {seu_token_jwt}" -d "{\"name\":\"Jane Doe\",\"email\":\"jane.doe@example.com\",\"gender\":\"Female\",\"ethnicity\":\"Black\",\"department\":\"HR\",\"position\":\"Manager\",\"salary\":75000.00}"
```

**Response:**
```json
{
  "id": 201,
  "name": "Jane Doe",
  "email": "jane.doe@example.com",
  "gender": "Female",
  "ethnicity": "Black",
  "department": "HR",
  "position": "Manager",
  "salary": 75000.00,
  "isActive": true
}
```

### 4. Atualizar Funcionário (PUT)

```bash
curl -X PUT https://localhost:5001/api/Employees/201 -H "Content-Type: application/json" -H "Authorization: Bearer {seu_token_jwt}" -d "{\"salary\":80000.00}"
```

**Response:**
```json
{
  "id": 201,
  "name": "Jane Doe",
  "email": "jane.doe@example.com",
  "gender": "Female",
  "ethnicity": "Black",
  "department": "HR",
  "position": "Manager",
  "salary": 80000.00,
  "isActive": true
}
```

### 5. Deletar Funcionário (DELETE)

```bash
curl -X DELETE https://localhost:5001/api/Employees/201 -H "Authorization: Bearer {seu_token_jwt}"
```

**Response:**
```json
{
  "message": "Funcionário deletado com sucesso."
}
```

---

## ?? Contribuindo

Contribuições são bem-vindas! Siga os passos:

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

### Padrões de Código

- Use **C# Coding Conventions**
- Mantenha cobertura de testes acima de 80%
- Documente métodos públicos
- Siga os princípios SOLID
- Use async/await para operações I/O

---

## ?? Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.

---

## ?? Autores

- **ESG Diversity Team** - [esg@company.com](mailto:esg@company.com)

---

## ?? Suporte

Para suporte, envie um email para esg@company.com ou abra uma issue no repositório.

---

## ?? Agradecimentos

- Microsoft por fornecer excelente documentação do .NET
- Comunidade open source
- Todos os contribuidores do projeto

---

## ?? Recursos Adicionais

### Documentação Oficial

- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Swagger/OpenAPI](https://swagger.io/docs/)

### Tutoriais e Guias

- [REST API Best Practices](https://restfulapi.net/)
- [JWT Authentication](https://jwt.io/introduction)
- [EF Core Migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)

---

**Desenvolvido com ?? para promover diversidade e inclusão no ambiente corporativo**
