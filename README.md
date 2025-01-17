
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
```

### Para Bancos de Dados MongoDB

Adicione a configuração no `Program.cs`:

```csharp
var _mongoDbConfing = new MongoDBConfig
{
    ConnectionString = config["ConnectionStrings:MongoDb"],
    DataBaseName = config["ConnectionStrings:DataBaseName"]
};
builder.Services.AddScoped<MongoDbContext>(sp =>
{
    var client = new MongoClient(_mongoDbConfing.ConnectionString);
    var database = client.GetDatabase(_mongoDbConfing.DataBaseName);
    return new MongoDbContext(database); // O MongoDbContext já tem as coleções configuradas
});

// Registra as coleções dinamicamente no DI usando reflexão
var context = builder.Services.BuildServiceProvider().GetRequiredService<MongoDbContext>();

var properties = typeof(MongoDbContext)
    .GetProperties()
    .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IMongoCollection<>))
    .ToList();

// Registra as coleções no DI
foreach (var property in properties)
{
    var collectionType = property.PropertyType.GenericTypeArguments[0]; // Obtém o tipo genérico de IMongoCollection<T>
    var collectionServiceType = typeof(IMongoCollection<>).MakeGenericType(collectionType); // Cria o tipo de IMongoCollection<T>

    // Registra a coleção no DI
    builder.Services.AddScoped(collectionServiceType, sp =>
    {
        return property.GetValue(sp.GetRequiredService<MongoDbContext>());
    });
}


// Registra repositórios e serviços genéricos
builder.Services.AddScoped(typeof(IGenericRepositoryMongoDb<>), typeof(GenericRepositoryMongoDb<>));
builder.Services.AddScoped(typeof(IMongoGenericService<>), typeof(MongoGenericService<>));

```

Crie o Arquivo dbcontex do Mongo `MongoDbContext.cs`:
```csharp
public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        public MongoDbContext(IMongoDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public IMongoCollection<Usuario> Usuarios => _database.GetCollection<Usuario>("Usuarios");
        ```

### Uso nos Controladores

#### SQL:

```csharp
public class YourController : ControllerBase
{
    private readonly IGenericService<YourEntity> _service;

    public YourController(IGenericService<YourEntity> service)
    {
        _service = service;
    }
    
    public async Task<IActionResult> GetAll()
    {
        var entities = await _service.GetAllAsync();
        return Ok(entities);
    }
}
```

#### MongoDB:

```csharp
public class YourController : ControllerBase
{
    private readonly IMongoGenericService<YourEntity> _service;

    public YourController(IMongoGenericService<YourEntity> service)
    {
        _service = service;
    }
    
    public async Task<IActionResult> GetAll()
    {
        var entities = await _service.GetAllAsync();
        return Ok(entities);
    }
}
```

## Principais Diferenciais

- **Segregação clara**: A separação das implementações para SQL e MongoDB evita complexidades desnecessárias e garante um código coeso e especializado.
- **Reuso máximo de código**: Interfaces genéricas permitem o reuso das implementações padrão entre projetos.
- **Extensibilidade**: Adapte facilmente o comportamento padrão implementando novos métodos ou substituindo os existentes em serviços ou repositórios personalizados.

## Contribuições

Se você tiver sugestões, novas ideias ou melhorias, fique à vontade para abrir uma issue ou enviar um pull request.

## Licença

Este projeto está licenciado sob a [MIT License](LICENSE).
