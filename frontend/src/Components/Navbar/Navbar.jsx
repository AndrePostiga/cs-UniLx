import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom'; 
import './Navbar.css';
import brasao from '../../assets/brasao.png';

const Navbar = () => {
  const [sticky, setSticky] = useState(false);
  const navigate = useNavigate(); 

  useEffect(() => {
    window.addEventListener('scroll', () => {
      window.scrollY > 50 ? setSticky(true) : setSticky(false);
    });
  }, []);

  // Função para redirecionar ao Dashboard ao clicar em "Entrar" (Provisório)
  const irParaDashboard = () => {
    navigate('/dashboard');
  };

  return (
    <nav className={`container-fluid ${sticky ? 'navbar-escura' : ''}`}>
      <img src={brasao} alt="Brasão UFF" className='brasao' />
      <ul>
        <li onClick={irParaDashboard} style={{ cursor: 'pointer' }}>Entrar</li>
        <li><button className='botao2'>Anunciar grátis</button></li>
      </ul>
    </nav>
  );
};

export default Navbar;