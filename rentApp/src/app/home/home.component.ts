import { Component, OnInit } from '@angular/core';
import { AppService } from '../../services/dao';
import { Cliente } from '../../models/cliente';
import * as dateFns from 'date-fns';


@Component({
  selector: 'app-home',
  standalone: false,
  
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  clientes: Cliente[] = [];
  clientesParaVencer: Cliente[] = [];
  currentIndex = 0;
  constructor(private services: AppService) { }


  ngOnInit(): void
  {
    this.services.getDados().subscribe(data => {
      this.clientes = data;
      this.pegaClientesParaVencer(this.clientes)
    })
  }

  pegaClientesParaVencer(clientes: Cliente[]): void
  {
    clientes.forEach((cliente) => {
      if (this.checkDiaVencimento(cliente))
        this.clientesParaVencer.push(cliente)
    })
  }

  checkDiaVencimento(cliente: Cliente): boolean {
    const hoje = new Date().getDate(); // Dia atual
    const diasNoMes = new Date(new Date().getFullYear(), new Date().getMonth() + 1, 0).getDate(); // Total de dias no mês atual
    const intervalo = 5; // Intervalo de dias

    const diaVencimento = cliente.diaVencimento;

    // Se o vencimento está no mesmo mês e dentro dos próximos 5 dias
    if (diaVencimento >= hoje && diaVencimento <= hoje + intervalo) {
      return true;
    }

    // Se houver virada do mês, considerar os dias do próximo mês
    if (hoje + intervalo > diasNoMes) {
      const diasRestantesEsteMes = diasNoMes - hoje;
      const diasNoProximoMes = intervalo - diasRestantesEsteMes;

      if (diaVencimento <= diasNoProximoMes) {
        return true;
      }
    }

    return false;

  }

  next() {
    if (this.currentIndex < this.clientesParaVencer.length - 1) {
      this.currentIndex++;
    } else {
      this.currentIndex = 0; // Retorna ao início
    }
  }

  previous() {
    if (this.currentIndex > 0) {
      this.currentIndex--;
    } else {
      this.currentIndex = this.clientesParaVencer.length - 1; // Vai ao último
    }
  }

  isActive(cliente: Cliente): boolean {
    return this.clientesParaVencer.indexOf(cliente) === this.currentIndex;
  }

}
