using System.Net.Mail;
using System.Text.RegularExpressions;
using Bogus;
using Bogus.Extensions.Brazil;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentAPI.Models;

namespace RentAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        // GET: HomeController
        private readonly RentDbContext _context;

        // O DbContext é passado automaticamente via injeção de dependência
        public HomeController(RentDbContext context)
        {
            _context = context;
        }

        [HttpGet("todos", Name = "GetClientes")]
        public IEnumerable<Cliente> Get()
        {
            return _context.Cliente;
        }

        [HttpPost("salvar")]
        public IActionResult Post([FromBody]Cliente cliente)
        {
            Dictionary<string, List<string>> validacoes = new Dictionary<string, List<string>>();

            if (cliente == null)
            {
                return BadRequest(new { mensagem = "Cliente não pode ser nulo." });
            }
            else if (!Validacao(cliente, out validacoes))
            {
                return BadRequest(new
                {
                    mensagem = "Erro na validação dos dados do cliente.",
                    resultado = validacoes
                });
            }
            else
            {
                _context.Cliente.Add(cliente);
                int result = _context.SaveChanges();
                return Ok(new { mensagem = "Cliente válido!" });
            }
        }

        public bool Validacao(Cliente cliente, out Dictionary<string, List<string>> listaValidacoes)
        {
            listaValidacoes = new Dictionary<string, List<string>>();

            listaValidacoes.Add("Campos vazios", CamposVazios(cliente));
            listaValidacoes.Add("Numeros em campo de texto", ChecarNumero(cliente));
            listaValidacoes.Add("Formatos Inválidos", ChecarFormatos(cliente));
            listaValidacoes.Add("Datas Inválidas", ChecarDatas(cliente));

            foreach(var validacao in listaValidacoes)
            {
                foreach(var listaErros in validacao.Value)
                {
                    if(listaErros.Count() > 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public List<string> CamposVazios(Cliente cliente)
        {
            var properties = cliente.GetType().GetProperties();
            List<string> camposVazios = new List<string>();

            foreach (var property in properties)
            {
                if (string.IsNullOrEmpty(property.GetValue(cliente)!.ToString()))
                {
                    camposVazios.Add(property.Name);
                }
            }

            return camposVazios;
        }

        public List<string> ChecarNumero(Cliente cliente)
        {
            var propriedades = cliente.GetType().GetProperties();
            List<string> propriedadesParaChecar = new List<string>() { "Nome", "Bairro", "Cidade", "Estado" };
            List<string> propriedadesInvalidas = new List<string>();

            foreach (var propriedade in propriedades)
            {
                if (propriedadesParaChecar.Contains(propriedade.Name) && propriedade.GetValue(cliente)!.ToString()!.Any(char.IsDigit)
                    && !string.IsNullOrEmpty(propriedade.GetValue(cliente)!.ToString()))
                {
                    propriedadesInvalidas.Add(propriedade.Name);
                }
            }

            return propriedadesInvalidas;
        }

        public List<string> ChecarFormatos(Cliente cliente)
        {
            List<string> formatosInvalidos = new List<string>();

            if (!ValidarEmail(cliente.Email) && !string.IsNullOrEmpty(cliente.Email))
                formatosInvalidos.Add("Email");
            if (!ValidarTelefone(cliente.Telefone) && !string.IsNullOrEmpty(cliente.Telefone))
                formatosInvalidos.Add("Telefone");
            if (cliente.Documento.Length == 11 && !ValidarCPF(cliente.Documento) && !string.IsNullOrEmpty(cliente.Documento))
                formatosInvalidos.Add("Documento: CPF");
            if (cliente.Documento.Length == 14 && !ValidarCNPJ(cliente.Documento) && !string.IsNullOrEmpty(cliente.Documento))
                formatosInvalidos.Add("Documento: CNPJ");

            return formatosInvalidos;
        }

        public List<string> ChecarDatas(Cliente cliente)
        {
            List<string> datasInvalidas = new List<string>();

            if (cliente.TerminoContrato <= cliente.InicioContrato)
                datasInvalidas.Add("TerminoContrato");
            if (cliente.TerminoContrato < new DateOnly(2000, 1, 1) || cliente.InicioContrato < new DateOnly(2000, 1, 1))
                datasInvalidas.Add("Pós-2000");
            if (cliente.DiaVencimento < 0 || cliente.DiaVencimento > 31)
                datasInvalidas.Add("DiaVencimento");

            return datasInvalidas;
        }

        public bool ValidarEmail(string email)
        {
            try
            {
                var endereco = new MailAddress(email);
                return endereco.Address == email; // Verifica se o endereço é idêntico ao original
            }
            catch
            {
                return false; // Retorna falso em caso de exceção
            }
        }

        public bool ValidarTelefone(string telefone)
        {
            // Regex para validar os dois formatos
            string padrao = @"^\d{2}-\d{4,5}-\d{4}$";
            return Regex.IsMatch(telefone, padrao);
        }

        public bool ValidarCPF(string cpf)
        {
            // Remove caracteres não numéricos
            cpf = Regex.Replace(cpf, @"[^\d]", "");

            // Verifica o tamanho do CPF e se todos os dígitos são iguais
            if (cpf.Length != 11 || new string(cpf[0], 11) == cpf)
                return false;

            // Variáveis de cálculo
            int soma = 0, resto;

            // Cálculo do primeiro dígito verificador
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * (10 - i);

            resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            // Cálculo do segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * (11 - i);

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            // Verifica se os dígitos verificadores batem
            return cpf.EndsWith(digito1.ToString() + digito2.ToString());
        }
        public bool ValidarCNPJ(string cnpj)
        {
            // Remove caracteres não numéricos
            cnpj = Regex.Replace(cnpj, @"[^\d]", "");

            // Verifica se o tamanho é 14 e se todos os dígitos são iguais
            if (cnpj.Length != 14 || new string(cnpj[0], 14) == cnpj)
                return false;

            int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            tempCnpj += digito1;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            return cnpj.EndsWith(digito1.ToString() + digito2.ToString());
        }
    }
}
