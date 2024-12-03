import React from 'react';
import './Rodape.css'; 

const Rodape = () => {
  return (
    <footer className="footer mt-5 p-4 bg-dark text-light">
      <div className="container text-center">
        <div className="row">
          <div className="col-md-4">
            <h5>Contato</h5>
            <p>Email: contato@balcaouff.com</p>
            <p>Telefone: (21) 99999-9999</p>
          </div>
          <div className="col-md-4">
            <h5>Desenvolvido Por:</h5>
            <ul className="list-unstyled">
              <li><a href="https://github.com/oftheus" className="text-light">@oftheus</a></li>
              <li><a href="#" className="text-light">Gabriel</a></li>
              <li><a href="#" className="text-light">André</a></li>
              <li><a href="#" className="text-light">Rafael</a></li>
            </ul>
          </div>
          <div className="col md-4">
            <p>© 2024 Balcão UFF. Todos os direitos reservados.</p>
          </div>
        </div>
      </div>
    </footer>
  );
};

export default Rodape;