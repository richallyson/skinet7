let data: string | number;

data = '42';

data = 100;

interface ICar {
  color: string;
  model: string;
  topSpeed?: number;
}

const car1: ICar = {
  color: 'blue',
  model: 'ferrari'
}

const car2: ICar = {
  color: 'blue',
  model: 'ferrari',
  topSpeed: 100
};

// Aqui a gente ta criando uma função, onde a gente tem um x do tipo number e um y do tipo number, do qual retorna um number
// que é essa definição antes da seta
const mutiply = (x: number, y: number): number => {
  return x * y
}

// Aqui a gente consegue encapsular os objetos retornados em parenteses e transformar eles numa string
// Na verdade, é como se a gente tivesse armazenando esse x e y dentro de uma variavel. E ao botar o ponto na frente
// temos acesso a diversas funções
const mutiplyForString = (x: number, y: number): string => {
  return (x * y).toString();
}

