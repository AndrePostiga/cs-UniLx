import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import mesa from '../../assets/mesa.jpg';
import livro from '../../assets/livro_useacabeca.png';
import aula from '../../assets/aula_mat.jpg';
import cadeira from '../../assets/cadeira.jpg';
import laptop from '../../assets/laptop.jpg';
import './Destaque.css';

const Destaque = () => {
  const anuncios = [
    { id: 1, nome: 'Livro Use a Cabeça! PMP', preco: 'R$ 50,00', imagem: livro },
    { id: 2, nome: 'Mesa de Estudos', preco: 'R$ 150,00', imagem: mesa },
    { id: 3, nome: 'Aulas de Matemática', preco: 'R$ 70,00', imagem: aula},
    { id: 4, nome: 'Cadeira Gamer', preco: 'R$ 500,00', imagem: cadeira },
    { id: 5, nome: 'Notebook Usado', preco: 'R$ 1200,00', imagem: laptop }
  ];

  return (
    <div className="container my-5">
      <h2>Anúncios em Destaque</h2>
      <div className="row row-cols-1 row-cols-md-5 g-4">
        {anuncios.map(anuncio => (
          <div className="col" key={anuncio.id}>
            <div className="card h-100">
              <img src={anuncio.imagem} className="card-img-top" alt={anuncio.nome} />
              <div className="card-body">
                <h5 className="card-title">{anuncio.nome}</h5>
                <p className="card-text">{anuncio.preco}</p>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Destaque;
