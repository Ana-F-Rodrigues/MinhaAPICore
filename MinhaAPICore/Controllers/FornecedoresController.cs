using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinhaAPICore.Model;

namespace MinhaAPICore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedoresController : Controller
    {
        private readonly ApiDbContext _context;

        public FornecedoresController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: Fornecedores    
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Fornecedores.ToListAsync());
        }

        // GET: Fornecedores/Details/5
        [HttpGet(template:"detalhes")]        
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fornecedor = await _context.Fornecedores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fornecedor == null)
            {
                return NotFound();
            }

            return View(fornecedor);
        }

        // GET: Fornecedores/Create
        [HttpGet(template: "criar")]
        public IActionResult Create()
        {
            return Ok();
        }

        
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Documento,TipoFornecedor,Ativo")] Fornecedor fornecedor)
        {
            if (ModelState.IsValid)
            {
                fornecedor.Id = Guid.NewGuid();
                _context.Add(fornecedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fornecedor);
        }

        // GET: Fornecedores/Edit/5
        [HttpGet(template: "edit")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor == null)
            {
                return NotFound();
            }
            return View(fornecedor);
        }

        // POST: Fornecedores/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody]Fornecedor fornecedor)
        {
            //if (id != fornecedor.Id)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fornecedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FornecedorExists(fornecedor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok(fornecedor);
            }

            return BadRequest("Há erros nos dados passados");
        }


        // GET: Fornecedores/Delete/5
        [HttpGet(template: "delete")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fornecedor = await _context.Fornecedores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fornecedor == null)
            {
                return NotFound();
            }

            return View(fornecedor);
        }

        // POST: Fornecedores/Delete/5
        [HttpPost("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        [HttpPost("apagar")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);
            _context.Fornecedores.Remove(fornecedor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FornecedorExists(Guid id)
        {
            return _context.Fornecedores.Any(e => e.Id == id);
        }
    }
}