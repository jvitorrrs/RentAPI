namespace RentAPI.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Email { get; set; }
        public string Documento { get; set; }
        public decimal ValorContrato { get; set; }
        public DateOnly InicioContrato { get; set; }
        public DateOnly TerminoContrato { get; set; }
        public int DiaVencimento { get; set; }
        public string UltimoPgto { get; set; }

        public Cliente() { }

    }

}
