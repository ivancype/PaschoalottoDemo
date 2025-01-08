using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PaschoalottoDemo;
using PaschoalottoDemo.Models;
using PaschoalottoDemo.RandomUserGenerator;

namespace PaschoalottoDemo.Controllers
{
    /// <summary>
    /// Controlador que irá receber as requisições CRUD para visualização e manipulação dos dados de usuários.
    /// </summary>
    public class UsuariosController : Controller
    {
        private readonly DemoDbContext _context;

        private readonly IHttpClientFactory _clientFactory;

        private readonly JsonSerializerOptions _options;
        private RandomUserGeneratorData? RandomUserGeneratorData { get; set; }

        public UsuariosController(DemoDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _clientFactory = httpClientFactory;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PrimeiroNome,UltimoNome,Email,Telefone,DataNascimento,TipoDocumento,NumeroDocumento")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.DataNascimento = usuario.DataNascimento.ToUniversalTime();
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PrimeiroNome,UltimoNome,Email,Telefone,DataNascimento,TipoDocumento,NumeroDocumento")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    usuario.DataNascimento = usuario.DataNascimento.ToUniversalTime();
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
        /// <summary>
        /// Recebe requisição para criar 50 usuários novos automaticamente usando API da RandomUserGenerator. Faz uso do httpClient Factory
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> CreateBatch()
        {

            var client = _clientFactory.CreateClient("RandomUserGenerator");
            var response = await client.GetAsync("");

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();

                RandomUserGeneratorData = JsonSerializer.Deserialize<RandomUserGeneratorData>(apiResponse, _options);
            }

            if (RandomUserGeneratorData != null)
            {
                foreach (Result item in RandomUserGeneratorData.Results)
                {

                    _context.Add(new Usuario()
                    {
                        DataNascimento = item.Dob.Date.ToUniversalTime(),
                        Email = item.Email,
                        Telefone = item.Cell,
                        PrimeiroNome = item.Name.First,
                        UltimoNome = item.Name.Last,
                        NumeroDocumento = item.Id.Value,
                        TipoDocumento = item.Id.Name
                    });


                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
