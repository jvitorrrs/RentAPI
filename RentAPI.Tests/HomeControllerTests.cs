using Moq;
using Xunit;
using System;
using RentAPI;
using System.Collections.Generic;
using RentAPI.Controllers;
using RentAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace RentAPI.Tests
{ 

    public class HomeControllerTests
    {
        private readonly Mock<RentDbContext> _mockContext;
        private readonly HomeController _clienteService;

        public HomeControllerTests()
        {
            _mockContext = new Mock<RentDbContext>();
            _clienteService = new HomeController(_mockContext.Object);
        }

        [Fact]
        public void IncluirCliente_NomeVazio_DeveLancarArgumentException()
        {
            // Arrange
            var cliente = new Cliente
            {
                Nome = "",
                Telefone = "11977777777",
                Endereco = "Rua das Flores, 123",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "SP",
                Email = "carlos.pereira@example.com",
                Documento = "43607703833", // CPF válido (exemplo)
                ValorContrato = 2500.00m,  // Valor do contrato em decimal
                InicioContrato = new DateOnly(2024, 01, 01),
                TerminoContrato = new DateOnly(2026, 01, 01),
                DiaVencimento = 5,
                UltimoPgto = "0"  // Representando que o cliente ainda não fez pagamento
            };


            // Act & Assert
            Assert.False(string.IsNullOrEmpty(cliente.Nome), "O campo não deveria estar vazio ou nulo.");
        }

        //public void Gerar()
        //{
        //    Random random = new Random();
        //    DateOnly[] inicioContrato = { new DateOnly(2020, 12, 15), new DateOnly(2024, 12, 15) };
        //    List<Cliente> clientes = new List<Cliente>();
        //    decimal min = 500, max = 5000;

        //    try
        //    {
        //        var faker = new Faker<Cliente>("pt_BR")
        //       .RuleFor(c => c.Nome, f => f.Name.FullName())
        //       .RuleFor(c => c.Telefone, f => f.Phone.PhoneNumber("###########"))
        //       .RuleFor(c => c.Endereco, f => f.Address.StreetName() + ", " + f.Address.BuildingNumber())
        //       .RuleFor(c => c.Bairro, f => f.Address.Direction())
        //       .RuleFor(c => c.Cidade, f => f.Address.City())
        //       .RuleFor(c => c.Estado, f => f.Address.State())
        //       .RuleFor(c => c.Email, f => f.Internet.Email())
        //       .RuleFor(c => c.Documento, f => f.Person.Cpf().Replace("-", "").Replace("/", ""))
        //       .RuleFor(c => c.ValorContrato, f => f.Finance.Amount(min, max))
        //       .RuleFor(c => c.InicioContrato, f => f.Date.BetweenDateOnly(inicioContrato[0], inicioContrato[1]))
        //       .RuleFor(c => c.DiaVencimento, f => f.Random.Int(1, 30));

        //        clientes = faker.Generate(10);

        //        for (int i = 0; i < clientes.Count; i++)
        //        {
        //            clientes[i].TerminoContrato = clientes[i].InicioContrato.AddYears(random.Next(1, 10));
        //            clientes[i].UltimoPgto = "0";
        //        }

        //        Post(clientes);
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        [Fact]
        public void Validacao_DeveRetornarErrosParaClienteInvalido()
        {
            // Arrange: Cria um cliente inválido
            var cliente = new Cliente
            {
                Nome = "",
                Telefone = "",
                Endereco = "Rua das Flores, 123",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "SP",
                Email = "carlos.pereira@example.com",
                Documento = "43607703833", // CPF válido (exemplo)
                ValorContrato = 2500.00m,  // Valor do contrato em decimal
                InicioContrato = new DateOnly(2024, 01, 01),
                TerminoContrato = new DateOnly(2026, 01, 01),
                DiaVencimento = 5,
                UltimoPgto = "0"  // Representando que o cliente ainda não fez pagamento
            };

            // Act: Executa a validação
            var resultado = Validacao(cliente, out var listaValidacoes);

            // Assert: Verifica se a validação falhou e os erros corretos foram retornados
            Assert.False(resultado, "A validação deveria falhar para o cliente inválido.");
            Assert.Contains("Campos vazios", listaValidacoes.Keys);

            //Assert.Contains("Datas Inválidas", listaValidacoes.Keys);
            //Assert.Contains("Data de nascimento não pode ser no futuro.", listaValidacoes["Datas Inválidas"],
            //    "A data de nascimento no futuro deveria ser listada como inválida.");

            //Assert.Contains("Formatos Inválidos", listaValidacoes.Keys);
            //Assert.Contains("CPF inválido.", listaValidacoes["Formatos Inválidos"],
            //    "Um CPF inválido deveria ser listado como erro de formato.");
            //Assert.Contains("Email inválido.", listaValidacoes["Formatos Inválidos"],
            //    "Um email inválido deveria ser listado como erro de formato.");
        }

        //[Fact]
        //public void Validacao_DeveRetornarSucessoParaClienteValido()
        //{
        //    // Arrange: Cria um cliente válido
        //    var cliente = new Cliente
        //    {
        //        Nome = "João Silva",
        //        DataNascimento = DateTime.Now.AddYears(-30), // Data válida
        //        Cpf = "123.456.789-09", // CPF válido
        //        Email = "joao.silva@email.com" // Email válido
        //    };

        //    var service = new ClienteService();

        //    // Act: Executa a validação
        //    var resultado = service.Validacao(cliente, out var listaValidacoes);

        //    // Assert: Verifica se a validação passou sem erros
        //    Assert.True(resultado, "A validação deveria passar para um cliente válido.");
        //    Assert.All(listaValidacoes.Values, erros => Assert.Empty(erros));
        //}

        //            if (listaErros.Count() > 0)
        //            {
        //                return false;
        //            }
        //        }
        //    }

        //    return true;
        //}

        public bool Validacao(Cliente cliente, out Dictionary<string, List<string>> listaValidacoes)
        {
            listaValidacoes = new Dictionary<string, List<string>>();

            listaValidacoes.Add("Campos vazios", CamposVazios(cliente));
            listaValidacoes.Add("Numeros em campo de texto", ChecarNumero(cliente));
            listaValidacoes.Add("Formatos Inválidos", ChecarFormatos(cliente));
            listaValidacoes.Add("Datas Inválidas", ChecarDatas(cliente));

            foreach (var validacao in listaValidacoes)
            {
                foreach (var listaErros in validacao.Value)
                {
                    if (listaErros.Count() > 0)
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