# Projeto de Serviços e Repositórios Genéricos

Este projeto fornece uma estrutura genérica robusta para interagir com diferentes bancos de dados, permitindo que as aplicações utilizem tanto bancos relacionais quanto não relacionais. Ele inclui suporte para **SQL** (via `Entity Framework`) e **MongoDB** (com MongoDB .NET Driver).

## Funcionalidades

- **Dois repositórios genéricos principais**:
  1. **`GenericRepository`**: Implementação voltada para bancos de dados SQL utilizando `Entity Framework`.
  2. **`GenericRepositoryMongoDb`**: Implementação voltada para bancos de dados MongoDB utilizando o driver nativo.
  
- **Dois serviços genéricos correspondentes**:
  1. **`GenericService`**: Serviço genérico para manipulação de dados usando o `GenericRepository`.
  2. **`MongoGenericService`**: Serviço genérico para operações com o `GenericRepositoryMongoDb`.

- **Operações comuns de repositório e serviço**:
  - `GetAllAsync()`: Retorna todas as entidades.
  - `GetByIdAsync(object id)`: Busca uma entidade por ID.
  - `AddAsync(TEntity entity)`: Adiciona uma nova entidade.
  - `UpdateAsync(TEntity entity)`: Atualiza uma entidade existente.
  - `RemoveAsync(object id)`: Remove uma entidade com base no identificador.
  - **Consultas flexíveis**: Métodos que aceitam expressões Lambda para manipulação dinâmica de dados.

## Arquitetura do Projeto

### Para Bancos de Dados SQL

- **Repositório**: `GenericRepository`
- **Serviço**: `GenericService`

**Configuração**:
- Configure um `DbContext` com seu banco relacional.
- Injete os serviços e repositórios genéricos via IoC (Inversão de Controle).

### Para Bancos de Dados MongoDB

- **Repositório**: `GenericRepositoryMongoDb`
- **Serviço**: `MongoGenericService`

**Configuração**:
- Configure a conexão com o banco MongoDB usando o cliente MongoDB.
- Injete os serviços e repositórios genéricos específicos via IoC.

## Exemplos de Configuração

### Para Bancos de Dados SQL

Adicione a configuração no `Program.cs`:

```csharp
builder.Services.AddDbContext<YourDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
