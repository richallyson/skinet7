import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

// Bem, a gente consegue usar propriedades de classes criadas nos nossos componentes. No caso dessa que é uma string simples, a gente
// pode simplesmente concatenar ela usando {{title}}. Ou seja, seria tipo: <h1>Seja bem vindo ao {{title}}</h1>
// Esse conceito de botar uma propriedade ou seja o que for dentro de duas chaves, se chama interpolação
export class AppComponent {
  title = 'Skinet';
}
