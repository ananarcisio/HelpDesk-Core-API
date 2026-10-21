using HelpDeskCore.Api.Models;

namespace HelpDeskCore.Api.Services;

public class HelpDeskService
{
    // Lógica de Atribuição Automática por Categoria
    public string ObterTecnicoEspecialista(Categoria categoria)
    {
        return categoria switch
        {
            Categoria.Hardware => "Carlos (Suporte Hardware)",
            Categoria.Redes => "Ana Elisa (Surporte em Redes)",
            Categoria.Software => "Lucas (Suporte Sistemas)",
            _ => "Nenhum técnico atribuído"
        };
    }

    // Lógica de Cálculo de SLA baseado na Prioridade
    public DateTime CalcularDataLimiteSla(Prioridade prioridade)
    {
        var agora = DateTime.UtcNow;
        return prioridade switch
        {
            Prioridade.Critico => agora.AddHours(2),  // Resolução em até 2 horas
            Prioridade.Alta => agora.AddHours(8),     // Resolução em até 8 horas
            Prioridade.Media => agora.AddHours(24),   // Resolução em até 1 dia
            Prioridade.Baixa => agora.AddHours(48),   // Resolução em até 2 dias
            _ => agora.AddHours(24)
        };
    }
}