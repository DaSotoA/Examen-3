using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TiendaWeb.Data;
using TiendaWeb.Models;

namespace TiendaWeb.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    [Area("Admin")]
    public class CervezasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnviroment;

        public CervezasController(ApplicationDbContext context, IWebHostEnvironment hostEnviroment)
        {
            _context = context;
            _hostEnviroment = hostEnviroment;
        }

        // GET: Admin/Cervezas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Cervezas.Include(c => c.Estilo);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Cervezas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cerveza = await _context.Cervezas
                .Include(c => c.Estilo)
                .FirstOrDefaultAsync(m => m.id == id);
            if (cerveza == null)
            {
                return NotFound();
            }

            return View(cerveza);
        }

        // GET: Admin/Cervezas/Create
        public IActionResult Create()
        {
            ViewData["idEstilo"] = new SelectList(_context.Estilos, "id", "name");
            return View();
        }

        // POST: Admin/Cervezas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,alcohol,idEstilo,precio,UrlImagen")] Cerveza cerveza)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostEnviroment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                if(archivos.Count > 0)
                {
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\cervezas");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    using (var fileStream = new FileStream(Path.Combine(subidas,nombreArchivo+extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStream);
                    }
                    cerveza.UrlImagen = @"imagenes\cervezas\" + nombreArchivo + extension;
                }
                _context.Add(cerveza);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["idEstilo"] = new SelectList(_context.Estilos, "id", "name", cerveza.idEstilo);
            return View(cerveza);
        }

        // GET: Admin/Cervezas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cerveza = await _context.Cervezas.FindAsync(id);
            if (cerveza == null)
            {
                return NotFound();
            }
            ViewData["idEstilo"] = new SelectList(_context.Estilos, "id", "name", cerveza.idEstilo);
            return View(cerveza);
        }

        // POST: Admin/Cervezas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,alcohol,idEstilo,precio,UrlImagen")] Cerveza cerveza)
        {
            if (id != cerveza.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string rutaPrincipal = _hostEnviroment.WebRootPath;
                    var archivos = HttpContext.Request.Form.Files;
                    if (archivos.Count > 0)
                    {
                        Cerveza? cervezabd = await _context.Cervezas.FindAsync(id);
                        if (cervezabd != null && cervezabd.UrlImagen != null)
                        {
                            var rutaImagenActual = Path.Combine(rutaPrincipal, cervezabd.UrlImagen);
                            if (System.IO.File.Exists(rutaImagenActual))
                            {
                                System.IO.File.Delete(rutaImagenActual);
                            }
                        }
                        _context.Entry(cervezabd).State = EntityState.Detached;
                        string nombreArchivo = Guid.NewGuid().ToString();
                        var subidas = Path.Combine(rutaPrincipal, @"imagenes\cervezas");
                        var extension = Path.GetExtension(archivos[0].FileName);
                        using (var fileStream = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                        {
                            archivos[0].CopyTo(fileStream);
                        }
                        cerveza.UrlImagen = @"imagenes\cervezas\" + nombreArchivo + extension;
                        _context.Entry(cerveza).State = EntityState.Modified;
                    }
                    _context.Update(cerveza);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CervezaExists(cerveza.id))
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
            ViewData["idEstilo"] = new SelectList(_context.Estilos, "id", "name", cerveza.idEstilo);
            return View(cerveza);
        }

        // GET: Admin/Cervezas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cerveza = await _context.Cervezas
                .Include(c => c.Estilo)
                .FirstOrDefaultAsync(m => m.id == id);
            if (cerveza == null)
            {
                return NotFound();
            }

            return View(cerveza);
        }

        // POST: Admin/Cervezas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cerveza = await _context.Cervezas.FindAsync(id);
            if (cerveza != null)
            {
                _context.Cervezas.Remove(cerveza);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CervezaExists(int id)
        {
            return _context.Cervezas.Any(e => e.id == id);
        }
    }
}
