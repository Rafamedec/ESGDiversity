# ESG Diversity API - Docker

## ?? Como executar com Docker

### Pré-requisitos
- Docker Desktop instalado
- Docker Compose instalado

### Comandos para executar

#### 1. Build e Start dos containers
```bash
docker-compose up --build
```

#### 2. Executar em background
```bash
docker-compose up -d
```

#### 3. Parar os containers
```bash
docker-compose down
```

#### 4. Parar e remover volumes (limpar dados)
```bash
docker-compose down -v
```

### ?? Acessar a aplicação

Após executar o docker-compose, a API estará disponível em:
- **API**: http://localhost:5000
- **Swagger**: http://localhost:5000/swagger
- **SQL Server**: localhost:1433

### ?? Credenciais do SQL Server

- **Server**: localhost,1433
- **User**: sa
- **Password**: YourStrong@Passw0rd

### ?? Variáveis de Ambiente

Você pode modificar as variáveis de ambiente no arquivo `docker-compose.yml`:

- `SA_PASSWORD`: Senha do SQL Server (mínimo 8 caracteres, deve incluir maiúsculas, minúsculas, números e símbolos)
- `Jwt__Key`: Chave secreta para geração de tokens JWT
- `ConnectionStrings__DefaultConnection`: String de conexão do banco de dados

### ?? Build apenas da imagem da API

```bash
docker build -t esg-diversity-api .
```

### ?? Executar apenas a API (sem docker-compose)

```bash
docker run -p 5000:8080 -e ASPNETCORE_ENVIRONMENT=Development esg-diversity-api
```

### ?? Debug e Logs

Ver logs dos containers:
```bash
docker-compose logs -f
```

Ver logs apenas da API:
```bash
docker-compose logs -f api
```

Ver logs apenas do SQL Server:
```bash
docker-compose logs -f sqlserver
```

### ?? Rebuild após mudanças no código

```bash
docker-compose up --build
```

### ?? Dicas

1. **Primeira execução**: O SQL Server pode demorar alguns segundos para inicializar. A API aguardará automaticamente através do healthcheck.

2. **Persistência de dados**: Os dados do SQL Server são persistidos no volume `sqlserver_data`. Para limpar os dados, use `docker-compose down -v`.

3. **Desenvolvimento**: Para desenvolvimento local sem Docker, continue usando o SQL Server LocalDB com o perfil Development.

4. **Produção**: Altere as senhas e chaves secretas antes de fazer deploy em produção!
