# ?? Guia Docker - ESG Diversity API

## ?? Índice
- [Visão Geral](#visão-geral)
- [Pré-requisitos](#pré-requisitos)
- [Estrutura dos Containers](#estrutura-dos-containers)
- [Como Executar](#como-executar)
- [Comandos Úteis](#comandos-úteis)
- [Variáveis de Ambiente](#variáveis-de-ambiente)
- [Troubleshooting](#troubleshooting)
- [Backup e Restore](#backup-e-restore)

---

## ?? Visão Geral

A aplicação ESG Diversity API roda em containers Docker com:
- **API .NET 8.0** na porta `5000` (mapeada para `8080` interna)
- **SQL Server 2022** na porta `1433`
- **Rede isolada** para comunicação entre containers
- **Volume persistente** para dados do banco

---

## ?? Pré-requisitos

### Instalados e Funcionando:
- ? Docker Desktop 29.0.1+
- ? Docker Compose v2.40.3+

### Verificar Instalação:
```powershell
docker --version
docker-compose --version
```

---

## ??? Estrutura dos Containers

### Container 1: SQL Server (`esg-sqlserver`)
- **Imagem**: `mcr.microsoft.com/mssql/server:2022-latest`
- **Porta**: `1433:1433`
- **Usuário**: `sa`
- **Senha**: `YourStrong@Passw0rd`
- **Database**: `ESGDiversityDb` (criada automaticamente)
- **Volume**: `sqlserver_data` (persistente)

### Container 2: API (.NET) (`esg-diversity-api`)
- **Imagem**: Construída a partir do Dockerfile
- **Porta**: `5000:8080` (host:container)
- **Framework**: .NET 8.0
- **Ambiente**: Docker
- **Swagger**: Habilitado em `/`

### Rede (`esg-network`)
- **Driver**: bridge
- **Permite**: Comunicação isolada entre containers

---

## ?? Como Executar

### 1?? Clonar/Navegar até o Projeto

```powershell
cd C:\Users\rafam\source\repos\ESGDiversity.API
```

### 2?? Construir as Imagens

```powershell
docker-compose build
```

**Saída esperada:**
```
[+] Building 20.0s (17/17) FINISHED
 => [build] dotnet restore
 => [build] dotnet build
 => [publish] dotnet publish
 => [final] COPY
? esgdiversityapi-api Built
```

### 3?? Iniciar os Containers

```powershell
docker-compose up -d
```

**Saída esperada:**
```
[+] Running 4/4
 ? Network esgdiversityapi_esg-network
 ? Volume esgdiversityapi_sqlserver_data
 ? Container esg-sqlserver        Healthy
 ? Container esg-diversity-api    Started
```

### 4?? Verificar Status

```powershell
docker-compose ps
```

**Saída esperada:**
```
NAME                STATUS
esg-diversity-api   Up (healthy)
esg-sqlserver       Up (healthy)
```

### 5?? Acessar a API

**Swagger UI:**
```
http://localhost:5000
```

**Endpoint de teste:**
```
http://localhost:5000/api/DiversityMetrics
```

---

## ??? Comandos Úteis

### Gerenciamento de Containers

```powershell
# Ver logs da API
docker logs esg-diversity-api

# Ver logs em tempo real
docker logs -f esg-diversity-api

# Ver logs do SQL Server
docker logs esg-sqlserver

# Reiniciar containers
docker-compose restart

# Parar containers
docker-compose stop

# Parar e remover containers
docker-compose down

# Parar e remover TUDO (incluindo volumes)
docker-compose down -v

# Recriar containers
docker-compose up -d --force-recreate
```

### Acesso aos Containers

```powershell
# Acessar shell da API
docker exec -it esg-diversity-api /bin/bash

# Acessar SQL Server CLI
docker exec -it esg-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P YourStrong@Passw0rd -C

# Ver processos rodando no container
docker top esg-diversity-api
```

### Monitoramento

```powershell
# Ver uso de recursos
docker stats

# Inspecionar container
docker inspect esg-diversity-api

# Ver saúde do container
docker inspect --format='{{json .State.Health}}' esg-diversity-api

# Ver todas as redes
docker network ls

# Inspecionar rede
docker network inspect esgdiversityapi_esg-network
```

### Build e Deploy

```powershell
# Rebuild sem cache
docker-compose build --no-cache

# Build específico da API
docker-compose build api

# Pull de imagens atualizadas
docker-compose pull

# Up com rebuild forçado
docker-compose up -d --build
```

---

## ?? Variáveis de Ambiente

### SQL Server (`sqlserver`)

| Variável | Valor | Descrição |
|----------|-------|-----------|
| `ACCEPT_EULA` | Y | Aceita termos de uso |
| `SA_PASSWORD` | YourStrong@Passw0rd | Senha do SA |
| `MSSQL_PID` | Developer | Edição do SQL |

### API (`api`)

| Variável | Valor | Descrição |
|----------|-------|-----------|
| `ASPNETCORE_ENVIRONMENT` | Docker | Ambiente de execução |
| `ASPNETCORE_URLS` | http://+:8080 | URLs de binding |
| `ConnectionStrings__DefaultConnection` | Server=sqlserver;... | String de conexão |
| `Jwt__Key` | YourSecretKeyHere... | Chave JWT |
| `Jwt__Issuer` | ESGDiversityAPI | Emissor do token |
| `Jwt__Audience` | ESGDiversityAPI | Audiência do token |
| `Jwt__ExpiryInMinutes` | 60 | Tempo de expiração |

### Alterar Variáveis

Edite `docker-compose.yml` ou crie `.env`:

```env
SA_PASSWORD=MinhaSenhaForte123!
JWT_KEY=MinhaChaveSecretaSuperSegura123456789
```

---

## ?? Testes

### 1. Testar Conectividade

```powershell
# Ping na API
Invoke-WebRequest -Uri "http://localhost:5000" -UseBasicParsing

# Testar endpoint
Invoke-RestMethod -Uri "http://localhost:5000/api/DiversityMetrics?page=1&pageSize=2"
```

### 2. Testar Banco de Dados

```powershell
# Conectar ao SQL Server
docker exec -it esg-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P YourStrong@Passw0rd -C

# Verificar databases
SELECT name FROM sys.databases;
GO

# Verificar tabelas
USE ESGDiversityDb;
SELECT * FROM INFORMATION_SCHEMA.TABLES;
GO

# Contar funcionários
SELECT COUNT(*) FROM Employees;
GO
```

### 3. Testar Autenticação

```powershell
# Login
$body = '{"username":"admin","password":"password123"}'
$response = Invoke-RestMethod -Uri "http://localhost:5000/api/Auth/login" -Method Post -ContentType "application/json" -Body $body
$token = $response.token

# Usar token
$headers = @{ "Authorization" = "Bearer $token" }
Invoke-RestMethod -Uri "http://localhost:5000/api/Employees?page=1&pageSize=5" -Headers $headers
```

---

## ?? Troubleshooting

### Problema 1: Container não inicia

```powershell
# Ver logs detalhados
docker logs esg-diversity-api --tail 100

# Verificar se a porta está em uso
netstat -ano | findstr :5000
netstat -ano | findstr :1433

# Matar processo na porta
taskkill /F /PID [PID]
```

### Problema 2: Erro de conexão com banco

```powershell
# Verificar se SQL Server está healthy
docker-compose ps

# Reiniciar SQL Server
docker-compose restart sqlserver

# Ver logs do SQL Server
docker logs esg-sqlserver --tail 50
```

### Problema 3: API não responde

```powershell
# Verificar health check
docker inspect --format='{{json .State.Health}}' esg-diversity-api

# Reiniciar API
docker-compose restart api

# Rebuild completo
docker-compose down
docker-compose build --no-cache
docker-compose up -d
```

### Problema 4: Permissão negada

```powershell
# Executar PowerShell como Administrador
# Ou reiniciar Docker Desktop
```

### Problema 5: Volume corrompido

```powershell
# Remover volume e recriar
docker-compose down -v
docker volume rm esgdiversityapi_sqlserver_data
docker-compose up -d
```

---

## ?? Backup e Restore

### Backup do Banco de Dados

```powershell
# Criar backup
docker exec esg-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P YourStrong@Passw0rd -C -Q "BACKUP DATABASE ESGDiversityDb TO DISK='/var/opt/mssql/data/ESGDiversityDb.bak'"

# Copiar backup para host
docker cp esg-sqlserver:/var/opt/mssql/data/ESGDiversityDb.bak ./backup/
```

### Restore do Banco de Dados

```powershell
# Copiar backup para container
docker cp ./backup/ESGDiversityDb.bak esg-sqlserver:/var/opt/mssql/data/

# Restaurar
docker exec esg-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P YourStrong@Passw0rd -C -Q "RESTORE DATABASE ESGDiversityDb FROM DISK='/var/opt/mssql/data/ESGDiversityDb.bak' WITH REPLACE"
```

### Backup do Volume

```powershell
# Criar backup do volume
docker run --rm -v esgdiversityapi_sqlserver_data:/data -v ${PWD}:/backup alpine tar czf /backup/sqlserver_backup.tar.gz -C /data .

# Restaurar volume
docker run --rm -v esgdiversityapi_sqlserver_data:/data -v ${PWD}:/backup alpine sh -c "cd /data && tar xzf /backup/sqlserver_backup.tar.gz"
```

---

## ?? Healthchecks

### SQL Server
```bash
test: /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P YourStrong@Passw0rd -Q "SELECT 1" -C || exit 1
interval: 10s
timeout: 3s
retries: 10
start_period: 10s
```

### API
```bash
test: curl --fail http://localhost:8080/api/DiversityMetrics || exit 1
interval: 30s
timeout: 10s
retries: 3
start_period: 40s
```

---

## ?? Segurança em Produção

### ?? IMPORTANTE: Alterar Credenciais

Antes de deploy em produção:

1. **Alterar senha do SA**:
```yaml
SA_PASSWORD: ${SA_PASSWORD}  # Use variável de ambiente
```

2. **Alterar chave JWT**:
```yaml
Jwt__Key: ${JWT_SECRET_KEY}  # Use segredo forte
```

3. **Usar secrets do Docker**:
```yaml
secrets:
  db_password:
    external: true
  jwt_key:
    external: true
```

4. **Restringir portas**:
```yaml
ports:
  - "127.0.0.1:5000:8080"  # Apenas localhost
```

---

## ?? Monitoramento em Produção

### Logs Estruturados

```powershell
# Ver logs JSON
docker logs esg-diversity-api --since 1h | ConvertFrom-Json

# Filtrar erros
docker logs esg-diversity-api 2>&1 | Select-String "error"
```

### Métricas

```powershell
# Ver métricas em tempo real
docker stats esg-diversity-api
```

---

## ?? Deploy em Produção

### Docker Compose Production

Crie `docker-compose.prod.yml`:

```yaml
services:
  api:
    restart: always
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
    logging:
      driver: "json-file"
      options:
        max-size: "10m"
        max-file: "3"
```

Execute:
```powershell
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d
```

---

## ?? Checklist de Deploy

- [ ] Alterar senhas padrão
- [ ] Configurar variáveis de ambiente seguras
- [ ] Testar healthchecks
- [ ] Configurar backup automático
- [ ] Configurar logging centralizado
- [ ] Restringir acesso às portas
- [ ] Testar restore de backup
- [ ] Documentar configurações específicas

---

## ?? Links Úteis

- [Docker Documentation](https://docs.docker.com/)
- [Docker Compose Reference](https://docs.docker.com/compose/compose-file/)
- [SQL Server Docker](https://hub.docker.com/_/microsoft-mssql-server)
- [.NET Docker Images](https://hub.docker.com/_/microsoft-dotnet)

---

**?? Aplicação rodando com sucesso em Docker!**

---

## ?? Resumo de Portas

| Serviço | Porta Host | Porta Container | Protocolo |
|---------|------------|-----------------|-----------|
| API | 5000 | 8080 | HTTP |
| SQL Server | 1433 | 1433 | TCP |
| Swagger UI | 5000 | 8080 | HTTP |

---

**Criado com ?? para facilitar o deploy da ESG Diversity API**
