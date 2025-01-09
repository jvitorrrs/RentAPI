import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppService } from '../../services/dao';
import { Cliente } from '../../models/cliente';

@Component({
  selector: 'app-detail',
  standalone: false,
  
  templateUrl: './detail.component.html',
  styleUrl: './detail.component.css'
})
export class DetailComponent implements OnInit {
  id = ''
  cliente = new Cliente();
  clienteDetalhes: any[] = [];

  constructor(private route: ActivatedRoute, private service: AppService) { }

  ngOnInit(): void
  {
    this.id = this.route.snapshot.paramMap.get('id') || ''; // Pega o parâmetro da rota
    this.service.getDados().subscribe(data => {
      this.cliente = data.find(c => c.id.toString() == this.id)!

      console.log(this.cliente)

      this.clienteDetalhes = [
        { label: 'Nome', valor: this.cliente.nome },
        { label: 'Telefone', valor: this.cliente.telefone },
        { label: 'Endereço', valor: this.cliente.endereco },
        { label: 'Bairro', valor: this.cliente.bairro },
        { label: 'Cidade', valor: this.cliente.cidade },
        { label: 'Estado', valor: this.cliente.estado },
        { label: 'Email', valor: this.cliente.email },
        { label: 'Documento', valor: this.cliente.documento },
        { label: 'Valor do Contrato', valor: this.cliente.valorContrato.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }) },
        { label: 'Início do Contrato', valor: this.apropriarDatas(this.cliente.inicioContrato) },
        { label: 'Término do Contrato', valor: this.apropriarDatas(this.cliente.terminoContrato) },
        { label: 'Dia do Vencimento', valor: this.cliente.diaVencimento },
        { label: 'Último Pagamento', valor: this.cliente.ultimoPgto },  // Conversão para data
      ];
    })    
  }

  apropriarDatas(data: Date): string {
    return data.toString();
  }

}
