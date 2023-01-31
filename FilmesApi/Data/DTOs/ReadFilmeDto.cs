using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Data.DTOs;

public class ReadFilmeDto
{
    public int Id { get; set; } 
    public string Titulo { get; set; }
    public int Duracao { get; set; }
    public string Genero { get; set; }
    public DateTime horaDaConsulta { get; set; } = DateTime.Now;
}