using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Models;

public class Filme
{
    [Required(ErrorMessage = "O título do filme é obrigatório")]
    [MaxLength(50, ErrorMessage = "O título do filme não pode exceder 50 caracteres")]
    public string Titulo { get; set; }

    [Required(ErrorMessage = "O campo de duração é obrigatório")]
    [Range(1, 360, ErrorMessage = "A duração deve ter no mínimo 1 minuto e no máximo 360")]
    public int Duracao { get; set; }

    
    [MaxLength(200, ErrorMessage = "O Diretor não pode ultrapassar 400 caracteres")]
    public string Diretor { get; set; }

    [Required(ErrorMessage = "O genêro é obrigátório")]
    [MaxLength(200, ErrorMessage = "O genêro não pode ultrapassar 200 caracteres")]
    public string Genero { get; set; }
}