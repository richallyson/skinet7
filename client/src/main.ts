// Explicação do funcionamento da main.ts: https://chat.openai.com/share/228de774-cb08-45c3-af1b-37011fdd39be
// Como eu n consigo comentar no angular.json, vou falar aqui sobre como usamos um certificado para ter nosso link como https
// A gente injetou o certificado ssl, para pode usar https. Usamos o mkcert para isso. Para ver melhor basta ir no angular.json
// E procurar por serve. Em options temos a injeção do certificado que foi gerado usando o mkcert

import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';


platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));
