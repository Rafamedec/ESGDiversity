# ESG Diversity API - Testes Unitários

## ?? Resumo

Este projeto contém testes unitários para todos os controllers da API ESG Diversity & Inclusion, utilizando xUnit, Moq e .NET 8.

## ? Status dos Testes

**Total: 11 testes** - ? Todos passando!

### Controllers Testados

| Controller | Testes | Status |
|-----------|---------|--------|
| AuthController | 2 | ? Passou |
| DiversityMetricsController | 1 | ? Passou |
| EmployeesController | 2 | ? Passou |
| DiversityGoalsController | 2 | ? Passou |
| GoalProgressController | 1 | ? Passou |
| InclusionEventsController | 2 | ? Passou |
| SalaryEquityController | 1 | ? Passou |

## ?? Testes Implementados

### 1. AuthController
- ? `Login_ReturnsOk_WhenCredentialsAreValid` - Verifica se o login retorna status 200
- ? `Register_ReturnsCreated_WhenRegistrationIsSuccessful` - Verifica se o registro retorna status 201

### 2. DiversityMetricsController
- ? `GetDiversityMetrics_ReturnsOk_WhenParametersAreValid` - Verifica se retorna status 200

### 3. EmployeesController
- ? `GetAll_ReturnsOk_WhenParametersAreValid` - Verifica se listagem retorna status 200
- ? `GetById_ReturnsOk_WhenEmployeeExists` - Verifica se busca por ID retorna status 200

### 4. DiversityGoalsController
- ? `GetAll_ReturnsOk_WhenParametersAreValid` - Verifica se listagem retorna status 200
- ? `GetById_ReturnsOk_WhenGoalExists` - Verifica se busca por ID retorna status 200

### 5. GoalProgressController
- ? `GetGoalProgress_ReturnsOk_WhenParametersAreValid` - Verifica se retorna status 200

### 6. InclusionEventsController
- ? `GetInclusionEvents_ReturnsOk_WhenParametersAreValid` - Verifica se listagem retorna status 200
- ? `GetById_ReturnsOk_WhenEventExists` - Verifica se busca por ID retorna status 200

### 7. SalaryEquityController
- ? `GetSalaryEquityAnalysis_ReturnsOk_WhenParametersAreValid` - Verifica se retorna status 200

## ??? Tecnologias Utilizadas

- **Framework de Testes**: xUnit 2.5.3
- **Mocking**: Moq 4.20.72
- **Testing Tools**: Microsoft.NET.Test.Sdk 17.8.0
- **Integration Testing**: Microsoft.AspNetCore.Mvc.Testing 8.0.0
- **.NET**: 8.0

## ?? Como Executar os Testes

### Via Linha de Comando

```powershell
# Navegar para o diretório de testes
cd C:\Users\rafam\source\repos\ESGDiversity.API.Tests

# Executar todos os testes
dotnet test

# Executar com detalhes
dotnet test --verbosity detailed

# Executar com cobertura
dotnet test /p:CollectCoverage=true
```

### Via Visual Studio

1. Abrir **Test Explorer** (Test > Test Explorer)
2. Clicar em **Run All Tests**
3. Ver resultados em tempo real

### Via Visual Studio Code

1. Instalar extensão **C# Dev Kit**
2. Abrir painel de testes (Testing icon na barra lateral)
3. Clicar em **Run All Tests**

## ?? Resultado dos Testes

```
Test run for C:\Users\rafam\source\repos\ESGDiversity.API.Tests\bin\Debug\net8.0\ESGDiversity.API.Tests.dll (.NETCoreApp,Version=v8.0)
VSTest version 17.11.1 (x64)

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:    11, Skipped:     0, Total:    11, Duration: 8 ms
```

## ?? Estrutura do Projeto de Testes

```
ESGDiversity.API.Tests/
??? Controllers/
?   ??? AuthControllerTests.cs
?   ??? DiversityMetricsControllerTests.cs
?   ??? EmployeesControllerTests.cs
?   ??? DiversityGoalsControllerTests.cs
?   ??? GoalProgressControllerTests.cs
?   ??? InclusionEventsControllerTests.cs
?   ??? SalaryEquityControllerTests.cs
??? ESGDiversity.API.Tests.csproj
```

## ?? Configuração do Projeto

### global.json

```json
{
  "sdk": {
    "version": "8.0.416",
    "rollForward": "latestFeature"
  }
}
```

Este arquivo garante que o projeto use o .NET 8 SDK, evitando bugs conhecidos do .NET 10 preview com xUnit.

### Pacotes NuGet

```xml
<PackageReference Include="xunit" Version="2.5.3" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.5.3" />
<PackageReference Include="Moq" Version="4.20.72" />
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
<PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="8.0.0" />
```

## ?? Padrão de Testes

Todos os testes seguem o padrão **AAA (Arrange, Act, Assert)**:

```csharp
[Fact]
public async Task MethodName_ExpectedBehavior_WhenCondition()
{
    // Arrange - Configurar o cenário de teste
    var mockService = new Mock<IService>();
    var controller = new Controller(mockService.Object);
    
    // Act - Executar a ação
    var result = await controller.Method();
    
    // Assert - Verificar o resultado
    var okResult = Assert.IsType<ActionResult<Response>>(result);
    var objectResult = Assert.IsType<OkObjectResult>(okResult.Result);
    Assert.Equal(200, objectResult.StatusCode);
}
```

## ?? Notas Importantes

### Por que .NET 8?

O projeto originalmente estava configurado para .NET 10, mas havia bugs conhecidos com o xUnit. A solução foi:

1. Instalar o .NET 8 SDK: `winget install Microsoft.DotNet.SDK.8`
2. Criar arquivo `global.json` para forçar o uso do SDK 8.0.416
3. Recriar o projeto de testes com as dependências corretas

### Mocking com Moq

Todos os testes utilizam Moq para simular dependências:

```csharp
var mockService = new Mock<IService>();
mockService
    .Setup(s => s.MethodAsync(It.IsAny<int>()))
    .ReturnsAsync(expectedResult);
```

### Validação de Status Codes

Cada teste valida se o endpoint retorna o status HTTP correto (200 OK, 201 Created, etc.):

```csharp
var okResult = Assert.IsType<OkObjectResult>(result.Result);
Assert.Equal(200, okResult.StatusCode);
```

## ?? Próximos Passos

Possíveis melhorias futuras:

1. ? Adicionar testes para cenários de erro (400, 404, 401)
2. ? Adicionar testes para validação de dados
3. ? Adicionar testes de integração
4. ? Configurar cobertura de código (coverlet)
5. ? Adicionar testes de performance
6. ? Configurar CI/CD com GitHub Actions

## ?? Suporte

Para dúvidas sobre os testes, contate:
- **Email**: esg@company.com
- **Issue Tracker**: GitHub Issues

## ?? Licença

Este projeto de testes está sob a mesma licença MIT do projeto principal.

---

**Desenvolvido com ?? para garantir a qualidade do código**
