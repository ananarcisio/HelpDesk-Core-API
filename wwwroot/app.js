const API_URL = '/api/tickets';
let modoAtual = 'user'; // 'user' ou 'tech'

document.addEventListener('DOMContentLoaded', () => {
    carregarEstatisticas();
    carregarChamados();

    document.getElementById('ticket-form').addEventListener('submit', async (e) => {
        e.preventDefault();
        await criarChamado();
    });
});

function alternarVisao(visao) {
    modoAtual = visao;
    const btnUser = document.getElementById('btn-user-view');
    const btnTech = document.getElementById('btn-tech-view');
    const formSection = document.getElementById('form-section');
    const mainLayout = document.getElementById('main-layout');

    if (visao === 'tech') {
        btnTech.classList.add('active-view');
        btnUser.classList.remove('active-view');
        formSection.style.display = 'none';
        mainLayout.classList.add('full-width');
        document.body.classList.add('tech-mode');
    } else {
        btnUser.classList.add('active-view');
        btnTech.classList.remove('active-view');
        formSection.style.display = 'block';
        mainLayout.classList.remove('full-width');
        document.body.classList.remove('tech-mode');
    }

    carregarChamados();
}

async function carregarEstatisticas() {
    try {
        const response = await fetch(`${API_URL}/relatorios/estatisticas`);
        const stats = await response.json();

        document.getElementById('total-tickets').innerText = stats.totalChamados;
        document.getElementById('abertos-tickets').innerText = stats.abertos;
        document.getElementById('resolvidos-tickets').innerText = stats.resolvidos;
        document.getElementById('sla-tickets').innerText = stats.chamadosForaSla;
    } catch (err) {
        console.error('Erro ao carregar estatísticas:', err);
    }
}

async function carregarChamados() {
    try {
        const response = await fetch(API_URL);
        const tickets = await response.json();

        const list = document.getElementById('tickets-list');
        list.innerHTML = '';

        const categorias = { 1: 'Hardware', 2: 'Redes', 3: 'Software' };
        const prioridades = { 1: 'Baixa', 2: 'Média', 3: 'Alta', 4: 'Crítico' };
        const statusMap = { 1: 'Aberto', 2: 'Em Atendimento', 3: 'Resolvido', 4: 'Fechado' };

        tickets.forEach(ticket => {
            const tr = document.createElement('tr');

            const dataSla = new Date(ticket.dataLimiteSla).toLocaleString('pt-BR', {
                day: '2-digit', month: '2-digit', hour: '2-digit', minute: '2-digit'
            });

            // Verifica se estourou SLA
            const foraSla = ticket.status !== 3 && ticket.status !== 4 && new Date() > new Date(ticket.dataLimiteSla);
            const slaColor = foraSla ? 'style="color: #f87171; font-weight: bold;"' : '';

            tr.innerHTML = `
                <td>#${ticket.id}</td>
                <td>
                    <strong>${escapeHtml(ticket.titulo)}</strong><br>
                    <small style="color: var(--text-muted);">${escapeHtml(ticket.solicitante)}</small>
                </td>
                <td>
                    <span class="badge badge-${categorias[ticket.categoria].toLowerCase()}">${categorias[ticket.categoria]}</span><br>
                    <small class="priority-${ticket.prioridade}">${prioridades[ticket.prioridade]}</small>
                </td>
                <td><span class="badge status-${ticket.status}">${statusMap[ticket.status]}</span></td>
                <td><i class="fa-solid fa-user-gear"></i> ${escapeHtml(ticket.tecnicoAtribuido || 'Pendente')}</td>
                <td ${slaColor}><i class="fa-regular fa-clock"></i> ${dataSla} ${foraSla ? '🚨' : ''}</td>
                <td class="tech-only-col">
                    ${ticket.status === 1 ? `<button class="btn-action" onclick="alterarStatus(${ticket.id}, 2)"><i class="fa-solid fa-play"></i> Atender</button>` : ''}
                    ${ticket.status < 3 ? `<button class="btn-action success" onclick="alterarStatus(${ticket.id}, 3)"><i class="fa-solid fa-check"></i> Finalizar</button>` : '✔️ Concluído'}
                </td>
            `;
            list.appendChild(tr);
        });
    } catch (err) {
        console.error('Erro ao carregar chamados:', err);
    }
}

async function alterarStatus(id, novoStatus) {
    try {
        const response = await fetch(`${API_URL}/${id}/status`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(novoStatus)
        });

        if (response.ok) {
            carregarChamados();
            carregarEstatisticas();
        } else {
            alert('Erro ao atualizar status.');
        }
    } catch (err) {
        console.error('Erro ao alterar status:', err);
    }
}

async function criarChamado() {
    const payload = {
        solicitante: document.getElementById('solicitante').value,
        titulo: document.getElementById('titulo').value,
        categoria: parseInt(document.getElementById('categoria').value),
        prioridade: parseInt(document.getElementById('prioridade').value),
        descricao: document.getElementById('descricao').value
    };

    try {
        const response = await fetch(API_URL, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        });

        if (response.ok) {
            document.getElementById('ticket-form').reset();
            carregarChamados();
            carregarEstatisticas();
        }
    } catch (err) {
        console.error('Erro no envio:', err);
    }
}

function escapeHtml(str) {
    return String(str || '').replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
}