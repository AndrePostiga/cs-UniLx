import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Navbar from './Components/Navbar/Navbar';
import Hero from './Components/Hero/Hero';
import Destaque from './Components/Destaque/Destaque';
import Categorias from './Components/Categorias/Categorias';
import Chamada from './Components/Chamada/Chamada';
import Rodape from './Components/Rodape/Rodape';
import Dashboard from './Components/Dashboard/Dashboard';
import Criacao from './Components/Criacao/Criacao';
import Perfil from './Components/Perfil/Perfil';
import BuscaAnuncios from './Components/BuscaAnuncios/BuscaAnuncios';
import MeusAnuncios from './Components/MeusAnuncios/MeusAnuncios';

const App = () => {
  return (
    <Router>
      <div>
        <Routes>
          <Route path="/" element={
            <>
              <Navbar />
              <Hero />
              <Destaque />
              <Categorias />
              <Chamada />
              <Rodape />
            </>
          } />
          <Route path="/dashboard" element={<Dashboard />} />
          <Route path="/criaranuncios" element={<Criacao/>} />
          <Route path="/perfil" element={<Perfil/>} />
          <Route path="/buscaanuncios" element={<BuscaAnuncios/>} />
          <Route path="/meusanuncios" element={<MeusAnuncios/>} />
        </Routes>
      </div>
    </Router>
  );
};

export default App;