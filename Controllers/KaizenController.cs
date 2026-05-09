using Kaizen.DTOs;
using Kaizen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class KaizenController : Controller
{
    private readonly AppDbContext _context;

    public KaizenController(AppDbContext context)
    {
        _context = context;
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

    public async Task<IActionResult> Details(int id)
    {
        var idea = await _context.KaizenIdeas
            .Include(k => k.Empleado)
            .FirstOrDefaultAsync(k => k.Id == id);

        if (idea == null)
            return NotFound();

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
}