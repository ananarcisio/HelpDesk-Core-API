using HelpDeskCore.Api.Data;
using HelpDeskCore.Api.Models;
using HelpDeskCore.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskCore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly HelpDeskService _helpDeskService;

    public TicketsController(AppDbContext context, HelpDeskService helpDeskService)
    {
        _context = context;
        _helpDeskService = helpDeskService;
    }

    // POST: api/tickets (Abertura de Chamado com Atribuição e SLA Automáticos)
    [HttpPost]
    public async Task<ActionResult<Ticket>> CriarTicket(Ticket ticket)
    {
        // Aplica as regras de negócio automáticas
        ticket.TecnicoAtribuido = _helpDeskService.ObterTecnicoEspecialista(ticket.Categoria);
        ticket.DataLimiteSla = _helpDeskService.CalcularDataLimiteSla(ticket.Prioridade);
        ticket.DataCriacao = DateTime.UtcNow;
        ticket.Status = StatusTicket.Aberto;

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ObterPorId), new { id = ticket.Id }, ticket);
    }

    // GET: api/tickets (Listar todos os chamados)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Ticket>>> ObterTodos()
    {
        return await _context.Tickets.ToListAsync();
    }

    // GET: api/tickets/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Ticket>> ObterPorId(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null) return NotFound();
        return ticket;
    }

    // GET: api/tickets/relatorios/estatisticas (Painel com Relatórios/Métricas)
    [HttpGet("relatorios/estatisticas")]
    public async Task<IActionResult> ObterEstatisticas()
    {
        var total = await _context.Tickets.CountAsync();
        var abertos = await _context.Tickets.CountAsync(t => t.Status == StatusTicket.Aberto);
        var resolvidos = await _context.Tickets.CountAsync(t => t.Status == StatusTicket.Resolvido);
        var estouraramSla = await _context.Tickets.CountAsync(t =>
            t.Status != StatusTicket.Resolvido && DateTime.UtcNow > t.DataLimiteSla);

        return Ok(new
        {
            TotalChamados = total,
            Abertos = abertos,
            Resolvidos = resolvidos,
            ChamadosForaSla = estouraramSla
        });
    }
    // PUT: api/tickets/5/status
    [HttpPut("{id}/status")]
    public async Task<IActionResult> AtualizarStatus(int id, [FromBody] StatusTicket novoStatus)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null) return NotFound("Chamado não encontrado.");

        ticket.Status = novoStatus;

        // Se o status for alterado para Resolvido ou Fechado, grava o momento da resolução para o SLA
        if (novoStatus == StatusTicket.Resolvido || novoStatus == StatusTicket.Fechado)
        {
            ticket.DataResolucao = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // PUT: api/tickets/5/atribuir
    [HttpPut("{id}/atribuir")]
    public async Task<IActionResult> AtribuirTecnico(int id, [FromBody] string nomeTecnico)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null) return NotFound("Chamado não encontrado.");

        ticket.TecnicoAtribuido = nomeTecnico;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}