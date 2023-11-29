import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

// Toda aplicação angular tem um NgModule, o mais. Ela é necessária para o funcionamento da aplicação
// Dentro dela temos primeiro o declarations, que é onde todo o componente criar na aplicação deve ser registrado no ng module através das declarations.
// E depois temos o imports. Qualquer outro modulo que iremos usar na nossa aplicação deve ser colocada no
// imports. Como você pode ver, no caso de ao criar uma aplicação temos dois Modulos registrados no imports
// Também temos a providers é onde registramos os nossos serviços. Creio eu que serviços seja o mesmo conceito de serviços em uma
// aplicação .NET
// E por fim temos o bootstrap, que é onde registramos os componentes que queremos que o bootstrap seja aplicado nele.
// O NgModule vê quem esta registrado no bootstrap, e injeta no componente ou componentes registrados
@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
