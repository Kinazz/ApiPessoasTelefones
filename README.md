# API de Pessoas e Telefones

## Objetivo

Esta API foi desenvolvida para gerenciar pessoas e seus telefones, permitindo cadastro, consulta, atualização, remoção e auditoria das operações realizadas. O sistema também oferece recursos de busca, exportação de dados e histórico de alterações.

---

## Principais Funcionalidades

- **Cadastro de Pessoas:** Permite criar, listar, buscar, atualizar e remover pessoas.
- **Cadastro de Telefones:** Permite adicionar, editar, listar e remover telefones associados a uma pessoa.
- **Validação de CPF:** Garante que o CPF informado seja válido e tenha 11 dígitos.
- **Busca Avançada:** Permite buscar pessoas por nome (busca parcial) e por número de telefone.
- **Exportação CSV:** Permite exportar pessoas e logs de auditoria em formato CSV.

---

## Estrutura dos Principais Arquivos

### 1. **Models**
- `PessoaModelo`: Representa uma pessoa, com propriedades como Id, Nome, Cpf, DataNascimento, Ativo e lista de Telefones.
- `TelefoneModelo`: Representa um telefone, com Id, Numero, Tipo e referência à pessoa.
- `PessoaAuditLog`: Registra logs de operações, indicando qual entidade foi alterada, ação, data/hora e dados antigos/novos.

### 2. **DTOs**
- `PessoaResponseDTO` e `TelefoneResponseDTO`: Usados para retornar dados ao cliente, evitando exposição direta das entidades do banco.

### 3. **Validations**
- `CpfAttribute`: Validador customizado para garantir que o CPF seja válido.

### 4. **Data**
- `PessoaContext`: Contexto do Entity Framework, define as tabelas e relacionamentos.

### 5. **Routes**
- `PessoaRoute`: Endpoints para operações com pessoas (CRUD, busca por telefone, busca por nome, exportação CSV).
- `TelefoneRoute`: Endpoints para operações com telefones (CRUD).
- `PessoaAuditLogRoute`: Endpoints para consultar e exportar logs de auditoria.

---

## Exemplos de Endpoints

### Pessoas
- `GET /people` – Lista todas as pessoas.
- `GET /people/{id}` – Busca pessoa por ID.
- `POST /people` – Cria uma nova pessoa.
- `PUT /people/{id}` – Atualiza uma pessoa.
- `DELETE /people/{id}` – Remove uma pessoa.
- `GET /people/by-phone/{numero}` – Busca pessoa pelo número do telefone.
- `GET /people/search/by-name/{nome}` – Busca pessoas por nome (parcial).
- `GET /people/export/csv` – Exporta pessoas em CSV.

### Telefones
- `GET /phones` – Lista todos os telefones.
- `POST /phones/{personId}` – Adiciona telefone para uma pessoa.
- `PUT /phones/{phoneId}` – Edita telefone.
- `DELETE /phones/{phoneId}` – Remove telefone.

### Auditoria
- `GET /people/logs` – Lista todos os logs de auditoria.
- `GET /people/logs/export/csv` – Exporta logs de auditoria em CSV.

---

## Fluxo de Auditoria

Cada operação de criação, atualização ou remoção em pessoas ou telefones gera um registro na tabela de auditoria (`PessoaAuditLog`), informando:
- Qual entidade foi alterada (Pessoa ou Telefone)
- Qual ação foi realizada (Criado, Atualizado, Removido)
- Data e hora da ação
- Dados antigos e novos (serializados em JSON)

---

## Validação de CPF

O campo CPF é validado para garantir:
- 11 dígitos numéricos
- Não pode ser uma sequência repetida (ex: 11111111111)
- Dígitos verificadores corretos

---


## Exportação CSV

Os endpoints de exportação retornam arquivos CSV com os dados de pessoas ou logs.


---

## Como Executar

1. Configure a string de conexão do banco de dados no `appsettings.json`.
2. `{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=NOME DB.db"
  }`  
3. Execute as migrações do Entity Framework para criar as tabelas.(Comando `dotnet ef migrations add`).
4. Rode o projeto e acesse o Swagger em `/swagger` para testar os endpoints.

---
