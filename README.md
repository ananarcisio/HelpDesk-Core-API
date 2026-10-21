# 🎧 HelpDesk Core API & Dashboard

O **HelpDesk Core** é uma solução Full-Stack completa para gerenciamento e controle inteligente de chamados de suporte técnico de TI. O sistema integra controle automatizado de **SLA (Service Level Agreement)**, transições dinâmicas do ciclo de vida dos tickets, métricas em tempo real e uma interface moderna desenvolvida com o conceito visual *Glassmorphism*.

![.NET 8](https://img.shields.io/badge/.NET%208-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![SQLite](https://img.shields.io/badge/SQLite-003B57?style=for-the-badge&logo=sqlite&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![Render](https://img.shields.io/badge/Render-46E3B7?style=for-the-badge&logo=render&logoColor=white)

---

## 🚀 Funcionalidades Principais

* **Gestão Automática de SLA:** Cálculo do prazo limite de resolução de acordo com o nível de prioridade do ticket (Baixa, Média, Alta e Crítico).
* **Ciclo de Vida do Chamado:** Fluxo estruturado de transição de estados (`Aberto` ➔ `Em Atendimento` ➔ `Resolvido` / `Fechado`).
* **Perfil Duplo de Acesso na Interface:**
  * **Visão Solicitante:** Formulário interativo para abertura de solicitações e acompanhamento de chamados ativos.
  * **Visão Técnico / N2:** Painel expandido focado em ações operacionais rápidas (atendimento, atribuição técnica e finalização).
* **Dashboard de Métricas:** Cards dinâmicos exibindo Total de Chamados, Atendimentos em Aberto, Resolvidos e Indicador de Estouro de SLA.

---

## 🛠️ Tecnologias e Arquitetura

### **Back-end**
* **.NET 8 Web API** — Arquitetura de controllers e endpoints RESTful performáticos.
* **Entity Framework Core 8** — Mapeamento objeto-relacional (ORM) e migrações de banco de dados.
* **SQLite** — Banco de dados relacional leve e portátil.
* **Swagger / OpenAPI** — Documentação e execução interativa das rotas da API.

### **Front-end**
* **HTML5, CSS3 & JavaScript (Vanilla)** — Renderização direta via arquivos estáticos integrados no pipeline Kestrel (`wwwroot`).
* **FontAwesome 6** — Conjunto de ícones para componentes visuais.

### **DevOps & Implantação**
* **Docker** — Containerização multi-stage build otimizada.
* **Render** — Deploy contínuo (CI/CD) automatizado a partir do repositório GitHub.

---

## 📌 Estrutura do Projeto

```text
HelpDesk-Core-API/
├── Dockerfile                   # Configuração de build e runtime Docker
├── HelpDesk-Core-API.sln        # Solução do .NET
└── HelpDeskCore.Api/
    ├── Controllers/             # Endpoints REST (Tickets, Relatórios)
    ├── Data/                    # AppDbContext e Migrations EF Core
    ├── Models/                  # Entidades e Enums (Ticket, SLA, Status)
    ├── Services/                # Regras de Negócio e Cálculos de SLA
    └── wwwroot/                 # Interface Web (HTML, CSS Glassmorphism, JS)
```

---

## 💻 Como Executar o Projeto Localmente

### Pré-requisitos
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) instalado na máquina.

### Passo a Passo

1. **Clonar o repositório:**
   ```bash
   git clone https://github.com/ananarcisio/HelpDesk-Core-API.git
   cd HelpDesk-Core-API/HelpDeskCore.Api
   ```

2. **Aplicar as Migrações do Banco de Dados (SQLite):**
   ```bash
   dotnet ef database update
   ```

3. **Iniciar o Servidor:**
   ```bash
   dotnet run
   ```

4. **Acessar no Navegador:**
   * **Dashboard Principal:** `http://localhost:5172`
   * **Documentação Swagger:** `http://localhost:5172/swagger`

---

## 🌐 Implantação em Nuvem (Render)

A aplicação está configurada com suporte nativo para containerização em nuvem via **Docker**, executando migrações automáticas de banco de dados no momento da inicialização do serviço.
