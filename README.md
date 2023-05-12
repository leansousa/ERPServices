# ERPServices
O objetivo desde projeto é realizar uma prova de conceito da utilização de MicroServiço.

Neste projeto, foram adotadas tecnologias para atender a diferentes propósitos, com a finalidade de desenvolver uma arquitetura técnica que atendesse os requisitos do cliente.
# Escopo Solicitado
![escopo](https://github.com/leansousa/ERPServices/blob/main/Documentation/escopo.jpg?raw=true)

## Descrição da Solução
Para o desenvolvimento dos requisitos funcionais solicitados pelo cliente, a visão de implementação esta descrita a seguir:

O cliente irá realizar diariamente os lançamentos do fluxo de caixa no sistema. Para tanto, irá informar a data, a descrição do lançamento, o tipo (crédito e débito) e o valor.  Vale destacar que o usuário que está inserindo o lançamento no sistema tem a possibilidade de realizar as cinco operações básicas: Consulta, Listagem, Inclusão, Edição e Exclusão. Para as operações de Inclusão, Edição e Exclusão. Uma vez realizado o lançamento com sucesso, o sistema irá gravar no Banco de dados e enviará uma mensagem (mensageria) com os dados do lançamento. Há um serviço em segundo plano (JOB) responsável por receber a mensagem e processar, de forma a consolidar os dados para que seja possível a emissão do relatório diário de lançamento. O processamento da mensagem no serviço de mensageria prevê que a mensagem somente seja removida da fila caso haja sucesso no processamento. No caso de falha, a mensagem é colocada em uma fila de "mensagens não processadas" (Dead letter exchanges - DLXs). Essa fila controla o reenvio da mensagem para a fila principal com intervalo de 10 minutos. Essa emissão de relatório é realizar através de seriço específico de emissão de relatório. 

### Fora de Escopo
- Não foi considerado o desenvolvimento de frontend para essa prova de conceito. O foco foi no desenvolvimento de uma arquitetura voltada para microserviços;
- Não foi considerado o envio de e-mail para transações que o cliente queira ser informado.
- Não foi considerado o desenvolvimento do cadastro de usuário para autorização e autenticação.

### Sugestão de Melhoria
- Inclusão de microserviço para dupla checagem da consolidação diária de dados, visando a integridade e atomicidade das informações;

## Desenho da Solução
Nesta seção, são apresentados os diagramas que compõem a arquitetura de aplicação, bem como o diagrama do modelo de dados de forma simplificada

### Arquitetura
![diagrama1](https://github.com/leansousa/ERPServices/blob/main/Documentation/arquitetura.png?raw=true)

### Banco de dados
![diagrama2](https://github.com/leansousa/ERPServices/blob/main/Documentation/bancodados.png?raw=true)

### Camadas
A arquitetura da aplicação está diretamente baseada no Domain-Driven Design (DDD)

![diagrama3](https://github.com/leansousa/ERPServices/blob/main/Documentation/camada.png?raw=true)

### Microserviços
| Nome | Objetivo | 
| ------ | ------ | 
| erpservices.cashflow.api | Responsável pela gestão dos lançamentos de fluxo de caixa diários no sistema |
| erpservices.processcashflow.api | Responsável pelo consolidação dos dados para o relatório, através da filha de mensagem |
| erpservices.report.api | Responsável pela emissão do relatório consolidado de lançamentos diarário | 
| erpservices.identity | Responsável pela autenticação e autorização utilizando JWT | 
| erpservices.apigateway | Gateway responsável pela orquestração e centralização dos serviços | 


### Frameworks e Ferramentas utilizadas
Nesta seção, são listadas todas as ferramentas e frameworks utilizados no desenvolvimento da solução.

| Descrição | Objetivo | Versão |
| ------ | ------ | ------ |
| RabbitMQ | Serviço de Mensageria | 3.0 |
| MySQL | Sistema de Banco de Dados relacional | 8.0 |
| .Net Core | Desenvolvimento | 7.0 |
| Entity Framework | ORM | 7.0 |
| Swashbuckle | Documentação | 7.5 |
| AutoMapper | Mapeamento de objetos | 12.0 |
| Ocelot | API Gateway | 19.0 |
| Docker Windows | Implantação de Container | 20.10 |
| Docker Compose | Implantação de Container | 2.10 |

### Autenticação e Autorização
Os dados de usuário para autorização e autenticação devem ser considerados conforme a tabela a seguir:

| E-mail | Senha | Role | Serviços |
| ------ | ------ | ------ | ------ |
| admin@erpservices.com | admin | ADMIN | erpservices.cashflow.api
| admin@erpservices.com | admin | ADMIN | erpservices.cashflow.api
| report@erpservices.com | report | REPORT | erpservices.report.api


# Implantação e Execução
Para implantação e execução da aplicação, é necessário ter o ambiente Docker, bem como o Docker Compose instalado e configurado.
Depois disso, os seguintes comandos devem ser executados no diretório /src/. 
```sh
docker-compose build
docker-compose up
```
Uma vez executados os comandos com sucesso, será possivel acessar a aplicação através das seguintes URLs:

| Descrição | URL Direta | URL Gateway
| ------ | ------ | ------ |
| erpservices.cashflow.api | http://localhost:5100/swagger | http://localhost:5000/gateway/cashflow |
| erpservices.processcashflow.api | http://localhost:5200/swagger | Não aplicável |
| erpservices.report.api | http://localhost:5300/swagger | http://localhost:5000/gateway/reportcashflow |
| erpservices.apigateway | http://localhost:5400/swagger | http://localhost:5000/gateway/auth |
| erpservices.apigateway | http://localhost:5000 | http://localhost:5000 |


