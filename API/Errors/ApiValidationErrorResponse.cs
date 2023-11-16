using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    // Essa classe vai ser responsável por tratar os erros de validação. Tipo, caso eu digite uma string em um endpoint que aceita um int
    // o [ApiController] vai validar essa requisição, ver que o que foi passado foi algo não esperado e nos retornar uma msg de erro 400
    // E bem, o [ApiController] faz essa validação pra gente e retornar uma mensagem. Porém essa mensagem é ineficiente, pois ela pode
    // ter um erro, ou mais de um erro. E caso tenha mais de um, ela não vai apresentar ela pra gente
    // Então, primeiro precisamos fazer uma adição na Program.cs para dar um override no comportamento de validação da [ApiController]
    // Para que assim, possamos acessar a lista de erros que é retornado para gente na hora da validação desses campos
    // E essa lista está dentro de uma lista de erros que é guardada dentro do nosso ModelState (GetErrors)
    public class ApiValidationErrorResponse : ApiReponse
    {
        public ApiValidationErrorResponse() : base(400)
        {

        }

        public IEnumerable<string> Errors { get; set; }
    }
}