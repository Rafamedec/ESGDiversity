# ?? Guia de Autenticação e CRUD - ESG Diversity API

## ?? Índice
- [Autenticação JWT](#autenticação-jwt)
- [CRUD de Funcionários](#crud-de-funcionários-employees)
- [CRUD de Metas de Diversidade](#crud-de-metas-de-diversidade)
- [CRUD de Eventos de Inclusão](#crud-de-eventos-de-inclusão)
- [Exemplos com cURL](#exemplos-com-curl)
- [Exemplos com PowerShell](#exemplos-com-powershell)

---

## ?? Autenticação JWT

### Usuários Padrão Criados

| Username | Password | Role | Email |
|----------|----------|------|-------|
| `admin` | `password123` | Admin | admin@esgdiversity.com |
| `hr_manager` | `password123` | HR | hr@esgdiversity.com |
| `user` | `password123` | User | user@esgdiversity.com |

### 1. Fazer Login (POST /api/Auth/login)

**Request:**
```http
POST http://localhost:5000/api/Auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "password123"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "admin",
  "email": "admin@esgdiversity.com",
  "role": "Admin",
  "expiresAt": "2024-01-20T15:30:00Z"
}
```

### 2. Registrar Novo Usuário (POST /api/Auth/register)

**Request:**
```http
POST http://localhost:5000/api/Auth/register
Content-Type: application/json

{
  "username": "newuser",
  "password": "mypassword123",
  "email": "newuser@company.com",
  "role": "User"
}
```

**Response (201 Created):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "newuser",
  "email": "newuser@company.com",
  "role": "User",
  "expiresAt": "2024-01-20T15:30:00Z"
}
```

### Como Usar o Token

Adicione o token no header de Authorization:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

---

## ?? CRUD de Funcionários (Employees)

### Permissões
- **GET**: Qualquer usuário autenticado
- **POST**: Admin, HR
- **PUT**: Admin, HR
- **DELETE**: Apenas Admin

### 1. Listar Funcionários (GET /api/Employees)

**Request:**
```http
GET http://localhost:5000/api/Employees?page=1&pageSize=10&department=IT
Authorization: Bearer {seu_token}
```

**Response:**
```json
{
  "page": 1,
  "pageSize": 10,
  "totalCount": 120,
  "totalPages": 12,
  "hasPrevious": false,
  "hasNext": true,
  "data": [
    {
      "id": 1,
      "name": "Employee 1",
      "email": "employee1@company.com",
      "gender": "Female",
      "ethnicity": "Asian",
      "department": "IT",
      "position": "Senior",
      "salary": 95000.00,
      "hireDate": "2020-05-15T00:00:00Z",
      "isDisabled": false,
      "ageGroup": 35,
      "educationLevel": "Bachelor",
      "isActive": true
    }
  ]
}
```

### 2. Obter Funcionário por ID (GET /api/Employees/{id})

**Request:**
```http
GET http://localhost:5000/api/Employees/1
Authorization: Bearer {seu_token}
```

### 3. Criar Funcionário (POST /api/Employees)

**Request:**
```http
POST http://localhost:5000/api/Employees
Authorization: Bearer {seu_token}
Content-Type: application/json

{
  "name": "João Silva",
  "email": "joao.silva@company.com",
  "gender": "Male",
  "ethnicity": "Mixed",
  "department": "IT",
  "position": "Mid-Level",
  "salary": 75000.00,
  "hireDate": "2024-01-15T00:00:00Z",
  "isDisabled": false,
  "ageGroup": 28,
  "educationLevel": "Bachelor"
}
```

**Response (201 Created):**
```json
{
  "id": 501,
  "name": "João Silva",
  "email": "joao.silva@company.com",
  "gender": "Male",
  "ethnicity": "Mixed",
  "department": "IT",
  "position": "Mid-Level",
  "salary": 75000.00,
  "hireDate": "2024-01-15T00:00:00Z",
  "isDisabled": false,
  "ageGroup": 28,
  "educationLevel": "Bachelor",
  "isActive": true
}
```

### 4. Atualizar Funcionário (PUT /api/Employees/{id})

**Request:**
```http
PUT http://localhost:5000/api/Employees/501
Authorization: Bearer {seu_token}
Content-Type: application/json

{
  "position": "Senior",
  "salary": 95000.00
}
```

### 5. Deletar Funcionário (DELETE /api/Employees/{id})

**Request:**
```http
DELETE http://localhost:5000/api/Employees/501
Authorization: Bearer {seu_token}
```

**Response (204 No Content)**

---

## ?? CRUD de Metas de Diversidade

### Permissões
- **Todos os endpoints**: Admin, HR

### 1. Listar Metas (GET /api/DiversityGoals)

**Request:**
```http
GET http://localhost:5000/api/DiversityGoals?page=1&pageSize=10
Authorization: Bearer {seu_token}
```

### 2. Obter Meta por ID (GET /api/DiversityGoals/{id})

**Request:**
```http
GET http://localhost:5000/api/DiversityGoals/1
Authorization: Bearer {seu_token}
```

### 3. Criar Meta (POST /api/DiversityGoals)

**Request:**
```http
POST http://localhost:5000/api/DiversityGoals
Authorization: Bearer {seu_token}
Content-Type: application/json

{
  "department": "Engineering",
  "metricType": "Women in Leadership",
  "targetPercentage": 40.0,
  "currentPercentage": 25.0,
  "targetDate": "2025-12-31T00:00:00Z",
  "notes": "Increase women representation in leadership positions"
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "department": "Engineering",
  "metricType": "Women in Leadership",
  "targetPercentage": 40.0,
  "currentPercentage": 25.0,
  "targetDate": "2025-12-31T00:00:00Z",
  "createdAt": "2024-01-20T10:00:00Z",
  "status": "Active",
  "notes": "Increase women representation in leadership positions"
}
```

### 4. Atualizar Meta (PUT /api/DiversityGoals/{id})

**Request:**
```http
PUT http://localhost:5000/api/DiversityGoals/1
Authorization: Bearer {seu_token}
Content-Type: application/json

{
  "currentPercentage": 32.0,
  "notes": "Good progress made in Q1"
}
```

### 5. Deletar Meta (DELETE /api/DiversityGoals/{id})

**Request:**
```http
DELETE http://localhost:5000/api/DiversityGoals/1
Authorization: Bearer {seu_token}
```

---

## ?? CRUD de Eventos de Inclusão

### Permissões
- **GET**: Qualquer usuário autenticado
- **POST**: Admin, HR
- **PUT**: Admin, HR
- **DELETE**: Apenas Admin

### 1. Listar Eventos (GET /api/InclusionEvents)

**Request:**
```http
GET http://localhost:5000/api/InclusionEvents?page=1&pageSize=10&category=Training
Authorization: Bearer {seu_token}
```

### 2. Obter Evento por ID (GET /api/InclusionEvents/{id})

**Request:**
```http
GET http://localhost:5000/api/InclusionEvents/1
Authorization: Bearer {seu_token}
```

### 3. Criar Evento (POST /api/InclusionEvents)

**Request:**
```http
POST http://localhost:5000/api/InclusionEvents
Authorization: Bearer {seu_token}
Content-Type: application/json

{
  "title": "Diversity & Inclusion Workshop",
  "description": "Workshop focused on unconscious bias and inclusive practices",
  "eventDate": "2024-03-15T14:00:00Z",
  "category": "Training",
  "participantsCount": 85,
  "budget": 5000.00,
  "department": "HR"
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "title": "Diversity & Inclusion Workshop",
  "description": "Workshop focused on unconscious bias and inclusive practices",
  "eventDate": "2024-03-15T14:00:00Z",
  "category": "Training",
  "participantsCount": 85,
  "budget": 5000.00,
  "department": "HR",
  "status": "Scheduled",
  "createdAt": "2024-01-20T10:00:00Z"
}
```

### 4. Atualizar Evento (PUT /api/InclusionEvents/{id})

**Request:**
```http
PUT http://localhost:5000/api/InclusionEvents/1
Authorization: Bearer {seu_token}
Content-Type: application/json

{
  "participantsCount": 95,
  "status": "Completed"
}
```

### 5. Deletar Evento (DELETE /api/InclusionEvents/{id})

**Request:**
```http
DELETE http://localhost:5000/api/InclusionEvents/1
Authorization: Bearer {seu_token}
```

---

## ?? Exemplos com cURL

### Login
```bash
curl -X POST "http://localhost:5000/api/Auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "admin",
    "password": "password123"
  }'
```

### Criar Funcionário
```bash
TOKEN="seu_token_aqui"

curl -X POST "http://localhost:5000/api/Employees" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Maria Santos",
    "email": "maria.santos@company.com",
    "gender": "Female",
    "ethnicity": "Black",
    "department": "Engineering",
    "position": "Senior",
    "salary": 110000.00,
    "hireDate": "2024-01-20T00:00:00Z",
    "isDisabled": false,
    "ageGroup": 32,
    "educationLevel": "Master"
  }'
```

### Listar Funcionários
```bash
curl -X GET "http://localhost:5000/api/Employees?page=1&pageSize=5" \
  -H "Authorization: Bearer $TOKEN"
```

---

## ?? Exemplos com PowerShell

### Login
```powershell
$loginResponse = Invoke-RestMethod -Uri "http://localhost:5000/api/Auth/login" `
  -Method Post `
  -ContentType "application/json" `
  -Body (@{
    username = "admin"
    password = "password123"
  } | ConvertTo-Json)

$token = $loginResponse.token
Write-Host "Token: $token"
```

### Criar Funcionário
```powershell
$headers = @{
  "Authorization" = "Bearer $token"
  "Content-Type" = "application/json"
}

$employee = @{
  name = "Pedro Oliveira"
  email = "pedro.oliveira@company.com"
  gender = "Male"
  ethnicity = "Hispanic"
  department = "Marketing"
  position = "Mid-Level"
  salary = 70000.00
  hireDate = "2024-01-20T00:00:00Z"
  isDisabled = $false
  ageGroup = 29
  educationLevel = "Bachelor"
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5000/api/Employees" `
  -Method Post `
  -Headers $headers `
  -Body $employee
```

### Listar Funcionários
```powershell
Invoke-RestMethod -Uri "http://localhost:5000/api/Employees?page=1&pageSize=10" `
  -Method Get `
  -Headers @{ "Authorization" = "Bearer $token" }
```

### Criar Meta de Diversidade
```powershell
$goal = @{
  department = "IT"
  metricType = "Ethnic Diversity"
  targetPercentage = 50.0
  currentPercentage = 35.0
  targetDate = "2025-12-31T00:00:00Z"
  notes = "Increase ethnic diversity in IT department"
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5000/api/DiversityGoals" `
  -Method Post `
  -Headers $headers `
  -Body $goal
```

### Criar Evento de Inclusão
```powershell
$event = @{
  title = "Women in Tech Conference"
  description = "Annual conference celebrating women in technology"
  eventDate = "2024-06-15T09:00:00Z"
  category = "Conference"
  participantsCount = 200
  budget = 15000.00
  department = "IT"
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5000/api/InclusionEvents" `
  -Method Post `
  -Headers $headers `
  -Body $event
```

---

## ?? Matriz de Permissões

| Endpoint | GET | POST | PUT | DELETE |
|----------|-----|------|-----|--------|
| **Employees** | User, HR, Admin | HR, Admin | HR, Admin | Admin |
| **DiversityGoals** | HR, Admin | HR, Admin | HR, Admin | HR, Admin |
| **InclusionEvents** | User, HR, Admin | HR, Admin | HR, Admin | Admin |
| **DiversityMetrics** | Público | - | - | - |
| **GoalProgress** | Público | - | - | - |
| **SalaryEquity** | HR, Admin | - | - | - |

---

## ?? Fluxo Completo de Uso

1. **Autenticar**: `POST /api/Auth/login`
2. **Guardar o token** retornado
3. **Usar o token** em todas as requisições subsequentes
4. **Criar dados**: POST nos endpoints desejados
5. **Consultar dados**: GET para visualizar
6. **Atualizar dados**: PUT para modificar
7. **Deletar dados**: DELETE para remover

---

## ?? Códigos de Status HTTP

| Código | Significado |
|--------|-------------|
| 200 | OK - Sucesso |
| 201 | Created - Recurso criado |
| 204 | No Content - Sucesso sem conteúdo |
| 400 | Bad Request - Dados inválidos |
| 401 | Unauthorized - Não autenticado |
| 403 | Forbidden - Sem permissão |
| 404 | Not Found - Recurso não encontrado |
| 500 | Internal Server Error - Erro no servidor |

---

**Desenvolvido com ?? para promover diversidade e inclusão no ambiente corporativo**
