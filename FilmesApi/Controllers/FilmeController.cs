using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.DTOs;
using FilmesApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

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


        /// <summary>
    /// Adiciona um filme ao banco de dados
        /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaFilme(
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

    /// <summary>
    /// Retorna os filmes do banco de dados para leitura
    /// </summary>
    /// <param name="skip">Campo para selecionar a quantidade de filmes a serem pulados</param>
    /// <param name="take">Campo para selecionar a quantidade de filmes a serem retornados</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso a consulta seja feita com sucesso</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IEnumerable<ReadFilmeDto> RecuperaFilmes([FromQuery] int skip = 0,
                                             [FromQuery] int take = 50)
    {
        return _mapper.Map<List<ReadFilmeDto>>(
            _context.Filmes.Skip(skip).Take(take));
    }

    /// <summary>
    /// Retorna um filme em específico do banco de dados para leitura
    /// </summary>
    /// <param name="id">Campo para selecionar o ID do filme a ser consultado</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso a consulta seja feita com sucesso</response>
    [HttpGet("{id}")] // filme específico
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult RecuperaFilmePorId(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
        return Ok(filmeDto);
    }


    /// <summary>
    /// Atualiza um filme por completo no banco de dados
    /// </summary>
    /// <param name="id">Campo para selecionar o ID do filme a ser atualizado</param>
    /// <param name="filmeDto">Objeto com os campos necessários para atualizar um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso atualização seja feita com sucesso</response>
    [HttpPut("{id}")] // atualiza filme completo
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AtualizaFilme(int id,
    [FromBody] UpdateFilmeDto filmeDto)
    {
        var filme = _context.Filmes.FirstOrDefault(
            filme => filme.Id == id);
        if (filme == null) return NotFound();
        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Atualiza um filme parcial no banco de dados
    /// </summary>
    /// <param name="id">Campo para selecionar o ID do filme a ser atualizado</param>
    /// <param name="patch">Objeto com os campos necessários para atualizar um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso atualização seja feita com sucesso</response>
    [HttpPatch("{id}")] // atualiza filme parcial
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AtualizaFilmeParcial (int id, 
        JsonPatchDocument<UpdateFilmeDto> patch)
    {
        var filme = _context.Filmes.FirstOrDefault
            (filme => filme.Id == id);
        if (filme == null) return NotFound();

        //converter o filme do banco para o updatefilmedto para apliicar as regras de valçidação
        //caso o dto seja válido, converte de volta para o filme

        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);

        //válido?
        patch.ApplyTo(filmeParaAtualizar, ModelState); // se for válido essa aplicação então
                                                       // converte de volta para um filme

        //fazendo a validação
        if(!TryValidateModel(filmeParaAtualizar))
        {
            return ValidationProblem(ModelState); // não validado
        }
        //validado
        _mapper.Map(filmeParaAtualizar, filme);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Deleta o filme do banco de dados
    /// </summary>
    /// <param name="id">Campo para selecionar o ID do filme a ser deletado</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso o filme seja deltado com sucesso</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult DeletaFilme(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(
            filme => filme.Id == id);
        if (filme == null) return NotFound();
        _context.Remove(filme);
        _context.SaveChanges();
        return NoContent();
    }
}
