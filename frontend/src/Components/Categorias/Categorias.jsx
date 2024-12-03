import React from 'react';
import './Categorias.css'; 
import 'bootstrap/dist/css/bootstrap.min.css';
import roupas from '../../assets/roupas.png';
import aulas from '../../assets/aulas.png';
import eletro from '../../assets/eletro.png';
import servicos from '../../assets/servicos.png';
import animais from '../../assets/animal.png';
import outros from '../../assets/outros.png';
import beauty from '../../assets/beauty.png';

const Categorias = () => {
  const categories = [
    { name: 'Beleza', imageUrl: beauty },
    { name: 'Eletrônicos', imageUrl: eletro },
    { name: 'Moda', imageUrl: roupas },
    { name: 'Eventos', imageUrl: aulas },
    { name: 'Empregos', imageUrl: servicos },
    { name: 'Animais', imageUrl: animais },
    { name: 'Outros', imageUrl: outros },
  ];

  return (
    <div className="categorias-container container">
      <h2>Categorias</h2>
      <div className="d-flex flex-row flex-wrap justify-content-start">
        {categories.map((category, index) => (
          <div className="card m-1" key={index}>
            <img 
              src={category.imageUrl} 
              className="card-img-top mx-auto" // Centraliza a imagem
              alt={category.name} 
              style={{ height: '100px', objectFit: 'contain' }} // Ajusta a altura da imagem
            />
            <div className="card-body text-center">
              <h5 className="card-title">{category.name}</h5>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Categorias;