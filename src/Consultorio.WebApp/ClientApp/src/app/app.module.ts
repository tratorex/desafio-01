import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { HttpModule } from '@angular/http'; 
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { ConsultaComponent } from './consulta/consulta.component';
import { ConsultaService } from './consulta/consulta.service';
import { ListaConsultaComponent } from './lista-consulta/lista-consulta.component';
import { DialogConfirmService } from './lista-consulta/dialogconfirm.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    ConsultaComponent,
    ListaConsultaComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    HttpModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: ListaConsultaComponent },
      { path: 'consulta', component: ConsultaComponent, pathMatch: 'full' },
      { path: 'consulta/:id', component: ConsultaComponent }
    ])
  ],
  providers: [DialogConfirmService, ConsultaService],
  bootstrap: [AppComponent]
})
export class AppModule { }
