# Desafio Técnico — Cadastro de Beneficiários

Sistema para gerenciamento de beneficiários e planos de saúde, com funcionalidades de criação, listagem, edição e remoção (CRUD).

## Visão Geral

Este sistema foi desenvolvido para gerenciar beneficiários e planos de saúde, permitindo criar, listar, editar e remover registros de forma simples e organizada. Ele serve como base para aplicações que precisam controlar informações de usuários e seus respectivos planos.

## Stack Utilizada

- **Linguagem:** C#  
- **Frameworks:** .NET, Entity Framework  
- **Banco de Dados:** SQL Server  
- **Outros:** Swagger

## Como Rodar

### Localmente

```bash
# Clone o repositório 
git clone https://github.com/GustavoFhz/4t-desafios.git

# Acesse a pasta do projeto
cd 4t-desafios/Desafio_Tecnico_Cadastro_de_Beneficiarios

# Copie o exemplo de variáveis de ambiente para criar seu .env
.env.exemplo
Exemplo: 

DB_HOST=SeuServidor
DB_NAME=SeuBancoDeDados
DB_USER=SeuLogin
DB_PASS=SuaSenha

# Restaurar pacotes
dotnet restore

# Aplicar migration
dotnet ef database update

# Rode a aplicação
dotnet run


Como Rodar Testes

# Executar testes
dotnet test


## Decisões de Projeto

- **Banco de Dados:** SQL Server, pela compatibilidade com .NET.
- **Arquitetura:** Camadas (Controller, Service, Dto, Data) para melhor organização.
- **Configuração:** Connection string em `.env` para maior segurança.
- **Testes:** Uso de InMemory Database para testes unitários.


Exemplos de Requisições

Beneficiários (Criar Beneficiário)
http POST http://localhost:5000/api/beneficiario \
  nome="Gustavo Silva" \
  cpf="12345678901" \
  planoId:=1


Beneficiários (Listar Beneficiários)
http GET http://localhost:5000/api/beneficiario

Beneficiários (Buscar Beneficiário por ID)
http GET http://localhost:5000/api/beneficiario/1

Beneficiários (Editar Beneficiário)
http PUT http://localhost:5000/api/beneficiario/1 \
  id:=1 \
  nome="Gustavo S. Silva" \
  cpf="12345678901" \
  planoId:=2

Beneficiários (Deletar Beneficiário)
http DELETE http://localhost:5000/api/beneficiario/1

Planos (Criar Plano)
http POST http://localhost:5000/api/plano \
  nome="Plano Premium" \
  codigo_registro_ans ="uuid-do-plano"

Planos (Listar Planos)
http GET http://localhost:5000/api/plano

Planos (Buscar Plano por ID)
http GET http://localhost:5000/api/plano/1

Planos (Editar Plano)
http PUT http://localhost:5000/api/plano/1 \
  id:=1 \
  nome="Plano Básico" \
  codigo_registro_ans ="uuid-do-plano"

Planos (Deletar Plano)
http DELETE http://localhost:5000/api/plano/1

###Observações sobre o uso do Docker

Durante o desenvolvimento, realizei toda a configuração necessária para execução do projeto em containers Docker, incluindo:

Criação do Dockerfile e do docker-compose.yml

Implementação das variáveis de ambiente no projeto

Ajustes no Program.cs para permitir execução em ambiente Docker

No entanto, não foi possível executar o projeto via Docker na minha máquina devido a erros locais relacionados à inicialização do Docker.
Foram realizados diversos testes sem sucesso, mas ao executar o mesmo projeto em outro computador de um colega (macOS) que possui o docker, o ambiente Docker funcionou corretamente, indicando que a configuração está válida.

Por esse motivo, o projeto foi testado e validado localmente sem o uso do Docker, mas toda a estrutura necessária para containerização já está implementada.







