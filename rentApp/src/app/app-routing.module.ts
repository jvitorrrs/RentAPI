import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { DetailComponent } from './detail/detail.component';
import { CadastroComponent } from './cadastro/cadastro.component';

const routes: Routes = [
  { path: '', component: HomeComponent }, // Rota inicial
  { path: 'detail/:id', component: DetailComponent }, // Rota para detalhes
  { path: 'cadastro', component: CadastroComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
