# API Star Wars

## Instruções para executar o projeto
### Softwares necessários
- Visual Studio 2022 ou Vs Code
- .Net 6
- Docker

### Run
Executar através do Visual Studo selecionando Docker-compose.  
ou  
Executar o seguinte comando no PowerShell dentro da raiz do projeto. 
```
docker-compose  -f "docker-compose.yml" -f "docker-compose.override.yml" -p dockercompose-starwars-cg --ansi never up -d --force-recreate --remove-orphans
```
Após a execução do comando, acessar a API através da URL:  
http://localhost:59382/swagger/index.html  
ou  
https://localhost:59381/swagger/index.html

Se você precisar derrubar o ambiente, pode usar o seguinte comando:
```
docker-compose  -f "docker-compose.yml" -f "docker-compose.override.yml" -p dockercompose-starwars-cg down
```

## Testes

A aplicação possui testes integrados no projeto "tests\StarWars.RestAPI.IntegrationTests" e também possui testes unitários das principais camadas da aplicação dentro dos projetos:

 - tests\StarWars.RestAPI.Tests
 - tests\StarWars.Domain.Tests

### Cobertura de testes
É possível verificar a cobertura dos testes através do Visual Studio Enterprise. Uma outra alternativa caso não possua o Visual Studio Enterprise, é a utilização do Coverlet e ReportGenerator. 

**Segue orientação para utilização do Coverlet e ReportGenerator:**  
Executar o comando abaixo para execução dos testes e geração de arquivo xml com informações de cobertura.
```
dotnet test StarWars.sln --collect:"XPlat Code Coverage"
```
Após a execução desse comando, será gerada uma pasta "TestResults\\{guid}\coverage.cobertura.xml" dentro de cada projeto de teste. Sendo que o nome de uma das pastas será um guid, por exemplo: "TestResults\3e707ba4-02bf-4756-838b-0b09aefd12e9\coverage.cobertura.xml"

Em seguida, pegue o caminho completo de todos os arquivos de cobertura de testes gerados em cada projeto de teste para montar e executar o comando abaixo:
```
Reportgenerator -reports:"tests\StarWars.Domain.Tests\TestResults\{guid}\coverage.cobertura.xml;tests\StarWars.RestAPI.IntegrationTests\TestResults\{guid}\coverage.cobertura.xml;tests\StarWars.RestAPI.Tests\TestResults\{guid}\coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
```
> Obs: Não se esqueça de substituir "{guid} pelo nome da pasta correta gerada na sua máquina. 

Após executar o comando acima, acesse o arquivo "coveragereport\index.html" para visualização da cobertura dos testes.  
Mais informações podem ser obtidas nos links abaixo:  
https://learn.microsoft.com/pt-br/dotnet/core/testing/unit-testing-code-coverage?tabs=windows  
https://github.com/danielpalme/ReportGenerator#readme

# Documentação dos Endpoints

## Importar planeta
`POST api/Planets/{planetId}`

### Respostas 
Retorna `201 Created` se o planeta foi importado com sucesso. Retorna com o header 'location' com a URL do endpoint de GET /api/planet/{planetId}  
Retorna `404 Not Found` se o planeta não foi encontrado na API pública do Star Wars (https://swapi.dev/) para ser importado.  
Retorna `409 Conflict` se o planeta já foi importado anteriormente.

## Listar planetas / Pesquisar planeta
`GET api/Planets?search=a&page=2`

>Os parâmetros `search` e `page` são opcionais.  
>A busca é feita pelo nome do planeta.  
>O retorno será exibido com 10 itens por página.

### Respostas 
Retorna `200 Ok` com o json:

	{
	  "results": [
		{
		  "planetId": 0,
		  "name": "string",
		  "climate": "string",
		  "terrain": "string",
		  "films": [
			{
			  "filmId": 0,
			  "name": "string",
			  "director": "string",
			  "releaseDate": "2022-09-30T01:53:07.617Z"
			}
		  ]
		}
	  ],
	  "currentPage": 0,
	  "pageSize": 10
	}


## Buscar planeta pelo id
`GET api/Planets/{planetId}`

### Respostas:
Retorna `200 Ok` com o json:

	{
	  "planetId": 0,
	  "name": "string",
	  "climate": "string",
	  "terrain": "string",
	  "films": [
		{
		  "filmId": 0,
		  "name": "string",
		  "director": "string",
		  "releaseDate": "2022-09-30T01:50:31.407Z"
		}
	  ]
	}
Retorna `404 Not Found` se o planeta não foi encontrado. É possível que o planeta não existe porque ainda não foi importado.

## Deleta planeta pelo id
`DELETE api/Planets/{planetId}`

### Respostas 
Retorna `204 No Content` se o planeta foi excluído com sucesso.	  
Retorna `404 Not Found` se o planeta não foi encontrado.