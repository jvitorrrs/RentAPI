// app.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Cliente } from '../models/cliente';

@Injectable({
  providedIn: 'root', // Serviço acessível em toda a aplicação
})
export class AppService {
  private apiUrl = 'http://localhost:5248/home/todos'; // URL da API
  private apiUrlSalvar = 'http://localhost:5248/home/salvar'; // URL da API

  constructor(private http: HttpClient) { }

  getDados(): Observable<Cliente[]> {
    const headers = new HttpHeaders()
      .set('Content-Type', 'application/json')
      .set('Access-Control-Allow-Origin', '*'); // Permitir todas as origens (não necessário na maioria dos casos, já que é controlado no backend)

    try {
      return this.http.get<Cliente[]>(this.apiUrl, { headers });
    }
    catch (ex) {
      return this.http.get<Cliente[]>(this.apiUrl, { headers });
    }

  }

  postDados(cliente: any): Observable<any> {

    let j = JSON.stringify(cliente)

    return this.http.post(this.apiUrlSalvar, cliente)

  }
}
