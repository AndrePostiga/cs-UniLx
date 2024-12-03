import React from 'react';
import { Card, Button, Container, Row, Col } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import './Dashboard.css';

const Dashboard = () => {
  const navigate = useNavigate();

  const handleNavigation = (path) => {
    navigate(path);
  };

  const irParaCriarAnuncios = () => {
    navigate('/criaranuncios');
  };

  return (
    <Container className="dashboard-container">
      <h2 className="mb-4">Bem-vindo!</h2>

      <Row>
        <Card className="dashboard-card" onClick={() => handleNavigation('/perfil')}>
            <Card.Body>
              <Card.Title>Perfil</Card.Title>
              <Card.Text>Veja suas informações pessoais.</Card.Text>
            </Card.Body>
          </Card>
      </Row>

      <Row>
        <Card className="dashboard-card" onClick={irParaCriarAnuncios}>
            <Card.Body>
              <Card.Title>Criação de Anúncios</Card.Title>
              <Card.Text>Crie anúncios para troca, venda ou doação.</Card.Text>
            </Card.Body>
          </Card>
      </Row>

      <Row>
          <Card className="dashboard-card" onClick={() => handleNavigation('/buscaanuncios')}>
            <Card.Body>
              <Card.Title>Buscar Anúncios</Card.Title>
              <Card.Text>Pesquise anúncios disponíveis na plataforma.</Card.Text>
            </Card.Body>
          </Card>
      </Row>

      <Row>
          <Card className="dashboard-card" onClick={() => handleNavigation('/meusanuncios')}>
            <Card.Body>
              <Card.Title>Meus Anúncios</Card.Title>
              <Card.Text>Gerencie e acompanhe seus anúncios.</Card.Text>
            </Card.Body>
          </Card>
      </Row>

      <Row>
          <Card className="dashboard-card" onClick={() => handleNavigation('/minhas-comunicacoes')}>
            <Card.Body>
              <Card.Title>Comunicação</Card.Title>
              <Card.Text>Ver todas as conversas.</Card.Text>
            </Card.Body>
          </Card>
      </Row>

    </Container>
  );
};

export default Dashboard;
