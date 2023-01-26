using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.DTOs;
using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{

    private FilmeContext _context;
    private IMapper _mapper;

    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult IActionResult (
        [FromBody] CreateFilmeDto filmeDto)
    {

        // Poderia ser feito assim se não fosse usado o mapper

        //        Filme filme = new Filme()
        //        {
        //           Titulo = filmeDto.Titulo,
        //            //etc
        //        };


        // usando o automapper
        Filme filme = _mapper.Map<Filme>(filmeDto);

        // usando o o contexto filmes para a integração com o banco
        _context.Filmes.Add(filme);
        // salvando as inforamções
        _context.SaveChanges();

        //retorno
        return CreatedAtAction(nameof(RecuperaFilmePorId), 
                               new { id = filme.Id }, 
                               filme);
    }

    [HttpGet]
    public IEnumerable<Filme> RecuperaFilmes([FromQuery] int skip = 0, 
                                             [FromQuery] int take=50 )
    {
        return _context.Filmes.Skip(skip).Take(take);
    }

    // filme especifico
    [HttpGet("{Id}")]
    public IActionResult RecuperaFilmePorId(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        return Ok(filme);
    }
}