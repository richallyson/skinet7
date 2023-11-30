import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

// Bem, a gente consegue usar propriedades de classes criadas nos nossos componentes. No caso dessa que é uma string simples, a gente
// pode simplesmente concatenar ela usando {{title}}. Ou seja, seria tipo: <h1>Seja bem vindo ao {{title}}</h1>
// Esse conceito de botar uma propriedade ou seja o que for dentro de duas chaves, se chama interpolação
export class AppComponent implements OnInit {
  title = 'Skinet';
  // Criamos essa propriedade para receber os dados da requisição, para ser printado na app.component.html
  // Essa propriedade faz uma herança da interface Product. Por incrivel que pareça, não usam o I antes do nome da interface
  // em angular. O que é uma doideira, pq né. Mas enfim, fazemos o contrato com a interface Product.
  // No Angular herança são um pouco diferente, elas são usadas mais como um contrato de forma para objetos ou classes
  products: Product[] = [];

  // E bem, quando o angular está no seu ciclo de vida que vem aqui nesse componente que é construido, o AppComponent
  // onde ele é construido. E por ser algo que é construido, temos acesso a um construtor, que é onde iremos injetar
  // o serviço que iremos precisar para fazer nossas requisições http. Ou seja, estamos injetando um serviço em um componente
  // E é dessa forma que injetamos serviços em componentes
  // E porquê private? Bem, isso significa que só podemos usar esse http, a requisição, dentro dessa classe, dentro desse comp
  // E bem, poderiamos pegar os dados diretamente do nosso construtor. Mas isso é considerado uma má, pratica, pois é muito
  // cedo no ciclo de vida para fazer isso. Dessa forma, primeiro iremos injetar o serviço na classe, e só depois, no ngOnInit
  // iremos fazer a requisição, que lá sim, é considerada uma boa prática.
  constructor(private http: HttpClient) { }

  // E bem, falando de ciclo de vida, uma vez que o ciclo começa, queremos trazer os nossos dados, e para isso usaremos um hook
  // O OnInit é um lyfecicle hook que é executado uma vez, apos a primeira inicialização das propriedades de entrada do componente
  // Ou seja, como queremos já trazer os dados do nosso back para o front, iremos fazer isso na inicialização da aplicação
  // Para mais infos: https://chat.openai.com/share/1fcc8baa-8e11-4701-8e29-c5b27418cf81
  // E aqui iremos fazer um get, usando o serviço que injetamos o nosso construtor. Basicamente, ele vai fazer um get nesse
  // endpoint e trazer os dados que desejamos manipular
  ngOnInit(): void {
    // Bem, e o que essa função get faz. Ela faz a requisição, pega o objeto json e retorna um observable
    // E o que diachos é observable? São stream de Dados: Um Observable é essencialmente um stream (fluxo) de dados ou eventos
    // Ele pode emitir múltiplos valores ao longo do tempo, ao contrário de uma chamada de função que retorna um único valor.
    // Observables são frequentemente usados para operações assíncronas, como chamadas de API ou eventos baseados em tempo.
    // Eles ajudam a lidar com operações que não têm resultados imediatos.
    // Para usar os dados emitidos por um Observable, você precisa se "inscrever" nele usando o subscribe().
    // Quando você se inscreve, você fornece uma função que será chamada sempre que o Observable emitir um novo valor.
    // Dessa forma, estaremos sempre observando as mudanças em um observable
    // Para mais info: https://chat.openai.com/share/645a5d82-cb25-482d-a9b8-0a107c19e199
    // Outra coisa que podemos fazer, é especificar o tipo de retorno que o get vai dar pra gente. No nosso caso, vamos retornar
    // Uma paginação do tipo Product que é uma array, como foi feito na API do csharp. Onde a paginação recebe os dados de paginação
    // assim como também o data, que no nosso caso, vai ser os dados do produto
    this.http.get<Pagination<Product[]>>('https://localhost:5001/api/products?pageSize=50').subscribe({
      // E dentro do subscribe iremos ditar o que vai acontecer com a resposta. No primeiro caso, caso dê bom, ela vai cair no next
      // Caso dê um erro, vai cair no error e fazer algo também. No nosso caso estamos definindo cenários simples, com console.log
      // Mas podemos manipular bem melhor, tipo, se der bom, faremos algo com os dados, se der error, poderemos emitir mensagens
      // E por fim, temos o complete, onde ele não recebe nenhum argumento, o next tem o response, e o error o error. Já o complete
      // não precisa, apenas uma função anonima, onde o retorno pode ser n coisas. Dessa forma, não temos mais acesso aos dados
      // vindos do observable, podemos apenas fazer tarefas realacionadas a compleção de algo que você deseja. Como fechar algo
      // liberar espaços. Etc. Para mais info: https://chat.openai.com/share/4c6b717e-2a78-4200-9929-39fe9e98272a
      // E o processo funciona assim, iremos fazer a requisição, nos inscrevemos no observable, pegamos os dados em next na
      // response, e depois de tudo, iremos pro complete. Caso aconteça um erro no trajeto, iremos para error, e ali mesmo será
      // finalizada a requisição, ela fica como "error complete". E no fim, tanto dando bom ou não, a sua inscrição será desfeita
      // E bem, esse é um exmplo do uso de observable com requisição http, ele pode ser usado em n diferentes tipos de coisas
      // next: response => console.log(response),
      next: response => this.products = response.data,
      error: error => console.log(error),
      complete: () => {
        console.log("request completed");
        console.log("extra statment");
        console.log("extra statment");
        console.log("extra statment");
      }
    });
  }
}
