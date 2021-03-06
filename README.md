# Event Sourcing - Pagamento

## Ideia do projeto
A ideia é simular como funcionar um sistema de criação/atualização/cancelamento de pedido e como tais ações são afetadas em um outro serviço de pagamento. É um projeto que visa praticar o uso de conceitos como Event Sourcing, comunicação de micro serviços usando Send/Receive, background services, testes unitários, onion architecture e testes.

## Tecnologias / Ferramentas / Práticas utilizadas
- SQL Server
- RabbitMq
- Send / Receive messages
- xUnit - Teste de unidade
- Onion Architecture
- BackgroundServices
- Event Sourcing

## Executar projeto

Para executar o projeto é necessário seguir os seguintes passos:

2. Configurar a Connection String do projeto
3. Rodar a migration
4. Subir a aplicação que irá ser executada na porta `6000`

## Observações
O projeto que complementa tal micro serviço é o Event Sourcing - Pedido que também esta em meu github! Sugestões e feedbacks são sempre bem vindos!
