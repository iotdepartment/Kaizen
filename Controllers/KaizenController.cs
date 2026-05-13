using Kaizen.DTOs;
using Kaizen.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize] 
public class KaizenController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;


    public KaizenController(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }


    public async Task<IActionResult> Index()
    {
        var ideas = await _context.KaizenIdeas
            .Include(k => k.Empleado)
            .OrderByDescending(k => k.FechaRegistro)
            .ToListAsync();

        return View(ideas);
    }

    // GET: /Kaizen/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Kaizen/Create
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] KaizenIdeaDto dto)
    {
        if (dto == null)
            return BadRequest(new { message = "No se recibió información." });

        // Validar empleado
        var empleado = await _context.Empleados
            .FirstOrDefaultAsync(e => e.NoEmpleado == dto.NoEmpleado);

        if (empleado == null)
            return BadRequest(new { message = "El empleado no existe." });

        // Crear entidad
        var idea = new KaizenIdea
        {
            NoEmpleado = dto.NoEmpleado,
            Empleado = empleado,
            Supervisor = dto.Supervisor,
            Area = dto.Area,
            Turno = dto.Turno,
            AreaAplicacion = dto.AreaAplicacion,
            Descripcion = dto.Descripcion,
            ImagenActualBase64 = dto.ImagenActualBase64,
            ImagenMejoradaBase64 = dto.ImagenMejoradaBase64,
            FechaRegistro = DateTime.Now
        };

        _context.KaizenIdeas.Add(idea);
        await _context.SaveChangesAsync();

        return Json(new { message = "Idea registrada correctamente" });
    }

    // GET: /Kaizen/GetEmpleado/123
    [HttpGet]
    public async Task<IActionResult> GetEmpleado(int id)
    {
        var empleado = await _context.Empleados.FindAsync(id);

        if (empleado == null)
            return NotFound();

        return Json(empleado);
    }

    // Vista de detalles
    public async Task<IActionResult> Details(int id)
    {
        var idea = await _context.KaizenIdeas
            .Include(i => i.Comentarios) // solo comentarios
            .FirstOrDefaultAsync(i => i.Id == id);

        if (idea == null) return NotFound();
        return View(idea);
    }

    [HttpGet]
    public async Task<IActionResult> SearchEmpleados(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Json(new List<object>());

        var empleados = await _context.Empleados
            .Where(e =>
                e.NoEmpleado.ToString().Contains(query) ||
                e.Nombre.Contains(query))
            .OrderBy(e => e.NoEmpleado)
            .Take(10)
            .ToListAsync();

        return Json(empleados);
    }

    // Cambiar estado de la idea
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CambiarEstado(int id, string nuevoEstado, string? comentario)
    {
        var idea = await _context.KaizenIdeas.FindAsync(id);
        if (idea == null) return NotFound();

        idea.Estado = nuevoEstado;
        idea.EstadoFecha = DateTime.Now;

        // En vez de guardar el comentario aquí, lo mandamos a KaizenComentarios
        if (!string.IsNullOrEmpty(comentario))
        {
            var user = await _userManager.GetUserAsync(User);

            var ultimoNumero = await _context.KaizenComentarios
                .Where(c => c.IdeaId == id)
                .OrderByDescending(c => c.NumeroComentario)
                .Select(c => c.NumeroComentario)
                .FirstOrDefaultAsync();

            var nuevoComentario = new KaizenComentario
            {
                IdeaId = id,
                Usuario = user?.UserName,
                Comentario = comentario,
                FechaComentario = DateTime.Now,
                NumeroComentario = (ultimoNumero == 0 ? 1 : ultimoNumero + 1)
            };

            _context.KaizenComentarios.Add(nuevoComentario);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Details", new { id });
    }

    // Agregar comentario independiente
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AgregarComentario(int ideaId, string comentario)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        // Obtener el último número de comentario para esa idea
        var ultimoNumero = await _context.KaizenComentarios
            .Where(c => c.IdeaId == ideaId)
            .OrderByDescending(c => c.NumeroComentario)
            .Select(c => c.NumeroComentario)
            .FirstOrDefaultAsync();

        var nuevoComentario = new KaizenComentario
        {
            IdeaId = ideaId,
            Usuario = user.UserName,
            Comentario = comentario,
            FechaComentario = DateTime.Now,
            NumeroComentario = ultimoNumero + 1 // asigna consecutivo
        };

        _context.KaizenComentarios.Add(nuevoComentario);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", new { id = ideaId });
    }



}