export class Cliente {
  id: number;
  nome: string;
  telefone: string;
  endereco: string;
  bairro: string;
  cidade: string;
  estado: string;
  email: string;
  documento: string;
  valorContrato: number;
  inicioContrato: Date;
  terminoContrato: Date;
  diaVencimento: number;
  ultimoPgto: string;

  constructor(
    id: number = 0,
    nome: string = '',
    telefone: string = '',
    endereco: string = '',
    bairro: string = '',
    cidade: string = '',
    estado: string = '',
    email: string = '',
    documento: string = '',
    valorContrato: number = 0,
    inicioContrato: Date = new Date(),
    terminoContrato: Date = new Date(),
    diaVencimento: number = 0,
    ultimoPgto: string = ''
  ) {
    this.id = 0;
    this.nome = nome;
    this.telefone = telefone;
    this.endereco = endereco;
    this.bairro = bairro;
    this.cidade = cidade;
    this.estado = estado;
    this.email = email;
    this.documento = documento;
    this.valorContrato = valorContrato;
    this.inicioContrato = inicioContrato;
    this.terminoContrato = terminoContrato;
    this.diaVencimento = diaVencimento;
    this.ultimoPgto = ultimoPgto;
  }
}
