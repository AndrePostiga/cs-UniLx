import React from 'react';
import './Chamada.css';

const Chamada = () => {
  return (
    <section className='container chamada'>
      <h2 className='chamada-titulo'>Crie sua conta no Balcão UFF e aproveite todas as vantagens</h2>
      <p>Descubra como o Balcão UFF facilita a troca, venda e doação de bens e serviços, e aproveite todos os benefícios de fazer parte da nossa comunidade!</p>

      <div className="how-it-works">
        <h3>Como Funciona</h3>
        <ol>
          <li><strong>Crie sua Conta:</strong> Autentique-se com sua conta UFF.</li>
          <li><strong>Crie Anúncios:</strong> Preencha um formulário com detalhes sobre o que você está oferecendo ou procurando.</li>
          <li><strong>Busque Anúncios:</strong> Use filtros para encontrar itens ou serviços que atendam às suas necessidades.</li>
          <li><strong>Converse com Usuários:</strong> Entre em contato com os anunciantes para negociar de forma segura.</li>
          <li><strong>Avalie as Transações:</strong> Após concluir uma transação, avalie os usuários para ajudar a construir uma reputação sólida.</li>
        </ol>
      </div>

      <div className="benefits">
        <h3>Benefícios do Balcão UFF</h3>
        <ul>
          <li><strong>Colaboração:</strong> Fomente a troca de conhecimentos e recursos dentro da comunidade UFF.</li>
          <li><strong>Sustentabilidade:</strong> Contribua para a economia circular, reduzindo o desperdício.</li>
          <li><strong>Segurança:</strong> Plataforma confiável com autenticação UFF e sistema de reputação.</li>
          <li><strong>Acessibilidade:</strong> Interface fácil de usar para todos os usuários.</li>
        </ul>
      </div>
    </section>
  );
};

export default Chamada;
