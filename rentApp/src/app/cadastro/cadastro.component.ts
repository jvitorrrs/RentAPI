import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Event } from '@angular/router';
import { AppService } from '../../services/dao';
import { Cliente } from '../../models/cliente';

@Component({
  selector: 'app-cadastro',
  templateUrl: './cadastro.component.html',
  standalone: false,
  styleUrls: ['./cadastro.component.css']
})
export class CadastroComponent implements OnInit {
  cadastroForm!: FormGroup;
  constructor(private fb: FormBuilder, private service: AppService) { }

  ngOnInit(): void {
    this.cadastroForm = this.fb.group({
      nome: ['', Validators.required],
      telefone: ['', Validators.required],
      endereco: ['', Validators.required],
      bairro: ['', Validators.required],
      cidade: ['', Validators.required],
      estado: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      documento: ['', Validators.required],
      valorContrato: [null, [Validators.required, Validators.min(0)]],
      inicioContrato: [null, Validators.required],
      terminoContrato: [null, [Validators.required, this.checarData]],
      diaVencimento: [null, [Validators.min(1), Validators.max(31)]],
      ultimoPgto: ['']
    });
  }

  onSubmit(): void {
    if (this.cadastroForm.valid) {
        let cliente = new Cliente(0,
        this.cadastroForm.get('nome')?.value,
        this.cadastroForm.get('telefone')?.value,
        this.cadastroForm.get('endereco')?.value,
        this.cadastroForm.get('bairro')?.value,
        this.cadastroForm.get('cidade')?.value,
        this.cadastroForm.get('estado')?.value,
        this.cadastroForm.get('email')?.value,
        this.cadastroForm.get('documento')?.value,
        this.cadastroForm.get('valorContrato')?.value,
        this.cadastroForm.get('inicioContrato')?.value,
        this.cadastroForm.get('terminoContrato')?.value,
        this.cadastroForm.get('diaVencimento')?.value,
        this.cadastroForm.get('ultimoPgto')?.value
      )

      this.service.postDados(cliente).subscribe({
        next: value => console.log('Observable emitted the next value: ' + value),
        error: err => console.error('Observable emitted an error: ' + err),
        complete: () => console.log('Observable emitted the complete notification')
      })
    }
  }

  onReset(): void {
    this.cadastroForm.reset();
  }

  validarCPF(cpf: string): boolean {
    // Remove caracteres não numéricos
    cpf = cpf.replace(/[^\d]+/g, '');

    // Valida se o CPF tem exatamente 11 dígitos ou é uma sequência inválida
    if (
      cpf.length !== 11 ||
      /^(\d)\1+$/.test(cpf) // Verifica se todos os dígitos são iguais
    ) {
      return false;
    }

    // Função para calcular os dígitos verificadores
    const calcularDigito = (cpfParcial: string, pesoInicial: number): number => {
      let soma = 0;

      for (let i = 0; i < cpfParcial.length; i++) {
        soma += parseInt(cpfParcial.charAt(i)) * pesoInicial--;
      }

      const resto = soma % 11;
      return resto < 2 ? 0 : 11 - resto;
    };

    // Calcula os dois dígitos verificadores
    const digito1 = calcularDigito(cpf.substring(0, 9), 10);
    const digito2 = calcularDigito(cpf.substring(0, 9) + digito1, 11);

    // Valida os dígitos calculados com os fornecidos
    return (
      digito1 === parseInt(cpf.charAt(9)) &&
      digito2 === parseInt(cpf.charAt(10))
    );
  }

  checarData(control: AbstractControl): ValidationErrors | null {
    let inicio = control.parent?.get('inicioContrato')!.value
    let termino = control.value

    if (termino <= inicio)
      return { erroData: true };
    else
      return null
  }
}


