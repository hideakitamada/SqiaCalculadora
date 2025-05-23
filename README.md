# Calculadora de Investimentos - SQIA 📈

Esta API realiza o cálculo de investimentos com indexador pós-fixado, considerando dias úteis e cotações diárias. O sistema possui suporte a logs, testes automatizados e resiliência.

---

## 🚀 Tecnologias Utilizadas

### 🔧 Backend
- **.NET 8 / ASP.NET Core Web API** – Estrutura principal da aplicação
- **C#** – Linguagem da aplicação
- **Entity Framework Core** – ORM para acesso ao banco
- **InMemoryDatabase** – Banco de dados em memória para testes
- **FluentValidation** – Validação de entrada com regras claras
- **Polly (via EnableRetryOnFailure)** – Resiliência para EF Core
- **ILogger** – Logging estruturado
- Base de dados InMemory

### 📦 API & Documentação
- **Swagger (Swashbuckle)** – Interface para teste da API

### 🧪 Testes
- **xUnit** – Framework principal de testes
- **Moq** – Simulação de dependências (services, loggers, etc.)
- **FluentValidation.TestHelper** – Testes das validações
- **WebApplicationFactory** – Testes de integração com `HttpClient`

---

## 📁 Estrutura do Projeto

```bash
SqiaCalculadora/
├── Controllers/                # Controllers da API
├── Data/                       # DbContext e configuração EF Core
├── Models/                     # Models de entrada e saída
├── Repositories/               # Interface e implementação para Cotação
├── Services/                   # Interface e lógica de cálculo de investimentos
├── Settings/                   # Elementos de configuração do CircuitBreaker, Polly e Retry
├── Utils/                      # Helpers: dias úteis, truncamento
├── Validators/                 # FluentValidation para requests
├── Program.cs                  # Configuração principal da aplicação
SqiaCalculadora.Tests/          # Testes do Projeto
```


---

## 📞 Contato
Caso tenha dúvidas, sugestões ou precise de ajuda, fique à vontade para entrar em contato com o desenvolvedor responsável por este repositório. :)

---

> Projeto desenvolvido como solução técnica para simulação de investimentos com base em cotação diária indexada.
