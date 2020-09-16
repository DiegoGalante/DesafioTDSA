[![Build status](https://ci.appveyor.com/api/projects/status/o03t2w5lvqpigoap/branch/dev?svg=true)](https://ci.appveyor.com/project/DiegoGalante/desafiotdsa/branch/dev) [![codecov](https://codecov.io/gh/DiegoGalante/DesafioTDSA/branch/dev/graph/badge.svg)](https://codecov.io/gh/DiegoGalante/DesafioTDSA) *

# [Link do Desafio](https://gitlab.com/tdsasistemas/challenger/-/blob/master/desafio-backend.md)

## [Como Rodar o Projeto](https://github.com/DiegoGalante/DesafioTDSA/wiki/Rodar-o-Projeto) :rocket:

### [Resolução do Desafio](https://github.com/DiegoGalante/DesafioTDSA/wiki) :heart_eyes: :memo:

# O Desafio
Sua tarefa é construir uma aplicação SAAS. A aplicação é um simples repositório para gerenciar médicos com seus respectivos nomes, CPF's, crm's e especialidades. Utilize um repositório Git (público, de sua preferência) para versionamento e disponibilização do código.
A aplicação deve ser construída em .NET Core 3.1, utilizando EF Core com banco de dados PostgreSQL ou SQL SERVER, pode utilizar qualquer lib disponível no NuGET.
A API deverá ser documentada utilizando o formato OpenAPI (antigo Swagger).

# Critérios
## 1 - A Api deve responder na porta 3000
## 2 - Deve haver uma rota para listar todos os profissionais cadastrados: 
```
GET /medico
```

## 3 - Deve ser possível filtar profissionais utilizando uma busca por especialidade
```
GET /medico/ginicologista (ginicologista é a especialidade sendo buscada neste exemplo)
```
  
## 4 - Deve haver uma rota para cadastrar um novo médico

- O corpo da requisição deve conter as informações do médico a ser cadastro, sem o Id que deve ser gerado automaticamente pelo servidor. A resposta, em caso de sucesso, deve ser o novo Id gerado.

- Deve realizar as seguintes validações:

```
1. nome não pode ser vazio ou nulo;
2. nome não pode ser maior que 255 caracteres;
3. cpf deve ser válido;
4. crm não pode ser vazio ou nulo;
5. deve conter no minimo uma especialidade;
```
- Status: **200 Ok**

- Em caso de falha em qualquer uma das validações a cima deve retornar com o status 400 Bad Request e as respectivas mensagens.

- Exemplo Status: **400 Bad Request**
```json
{
      erros: [
          {
              campo: "nome",
              erro: "nome é obrigatorio"
          },
           {
              campo: "cpf",
              erro: "cpf inválido"
          },
      ]
  }

```

## 5 - O usuário deve poder remover um médico por id
```
DELETE /medico/:id
```
