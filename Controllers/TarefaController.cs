using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var tarefa = _context.Tarefas
                .Where((x => x.Id == id))
                .Select(x => x.Id.ToString());

            return tarefa != null ? Ok(tarefa) :NotFound();
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            var tarefa = _context.Tarefas.ToList();
            return tarefa != null ? Ok(tarefa) : NotFound();
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            var tarefa = _context.Tarefas
                .Where((x => x.Titulo.ToUpper == titulo.ToUpper)).ToList();
            return tarefa != null ? Ok(tarefa) : NotFound();
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefa = _context.Tarefas.Where(x => x.Status == status).ToList();
            return tarefa != null ? Ok(tarefa) : NotFound();
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
            { 
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            }

            _context.Tarefas.Add(tarefa);
            _.cotta.SaveChanges();
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
            {
                return NotFound();
            }
            
            if (tarefa.Data == DateTime.MinValue)
            { 
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            }

            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Status = tarefa.Status;

            _contexto.SaveChanges();

            return Ok(tarefaBanco);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
            { 
                return NotFound();
            }

            _context.Remove(tarefaBanco);
            _context.SaveChanges(); 
            return NoContent();
        }
    }
}
