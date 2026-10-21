namespace HelpDeskCore.Api.Models;

public enum Prioridade
{
    Baixa = 1,
    Media = 2,
    Alta = 3,
    Critico = 4
}

public enum Categoria
{
    Hardware = 1,
    Redes = 2,
    Software = 3
}

public enum StatusTicket
{
    Aberto = 1,
    EmAtendimento = 2,
    Resolvido = 3,
    Fechado = 4
}

public class Ticket
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public Categoria Categoria { get; set; }
    public Prioridade Prioridade { get; set; }
    public StatusTicket Status { get; set; } = StatusTicket.Aberto;
    public string Solicitante { get; set; } = string.Empty;
    public string? TecnicoAtribuido { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime DataLimiteSla { get; set; }
    public DateTime? DataResolucao { get; set; }
}