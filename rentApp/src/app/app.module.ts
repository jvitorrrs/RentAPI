import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';

import { HomeComponent } from './home/home.component';
import { MatButtonModule } from '@angular/material/button';
import { HttpClientModule } from '@angular/common/http';
import { MatTableModule } from '@angular/material/table';  // Para a tabela
import { MatSortModule } from '@angular/material/sort';    // Para a ordenação das colunas
import { MatPaginatorModule } from '@angular/material/paginator'; // Para a paginação
import { MatCardModule } from '@angular/material/card';    // Para o Card
import { MatDatepickerModule } from '@angular/material/datepicker'; // Para o pipe de data (se você estiver usando Angular Material para manipulação de data)
import { MatNativeDateModule } from '@angular/material/core';  // Para o suporte de datas nativas
import { CommonModule } from '@angular/common';  // Para usar pipes como date e currency no template
import { MatIconModule } from '@angular/material/icon';
import { DetailComponent } from './detail/detail.component';
import { CadastroComponent } from './cadastro/cadastro.component';
import { SearchComponent } from './search/search.component';
import { VencidosComponent } from './vencidos/vencidos.component';
import { MatInputModule } from '@angular/material/input';
import { MatDividerModule } from '@angular/material/divider';
import { ReactiveFormsModule } from '@angular/forms';
import { NgxMaskDirective, NgxMaskPipe, provideNgxMask } from 'ngx-mask'; // Importe o módulo





@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    DetailComponent,
    CadastroComponent,
    SearchComponent,
    VencidosComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    MatSidenavModule,
    MatToolbarModule,
    MatListModule,
    MatButtonModule,
    HttpClientModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    MatCardModule,
    MatDatepickerModule,
    MatNativeDateModule,
    CommonModule,
    MatIconModule,
    MatInputModule,
    MatDividerModule,
    ReactiveFormsModule,
    NgxMaskDirective, NgxMaskPipe,    
  ],
  providers: [provideNgxMask()],
  bootstrap: [AppComponent],
})
export class AppModule { }
