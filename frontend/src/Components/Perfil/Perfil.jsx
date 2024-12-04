import './Perfil.css';
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Container, Row, Col, Card, Spinner } from 'react-bootstrap';

const Perfil = () => {
  const [profile, setProfile] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const hardcodedId = 'account_01JE3PCSKS8S378F6CS9D3VGY2';
  const API_URL = `http://localhost:5327/accounts/${hardcodedId}`;

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const response = await axios.get(API_URL);
        setProfile(response.data);
      } catch (err) {
        setError('Erro ao carregar os dados do perfil.');
      } finally {
        setLoading(false);
      }
    };

    fetchProfile();
  }, [API_URL]);

  if (loading) {
    return (
      <Container className="text-center mt-5">
        <Spinner animation="border" />
        <p>Carregando...</p>
      </Container>
    );
  }

  if (error) {
    return (
      <Container className="text-center mt-5">
        <p className="text-danger">{error}</p>
      </Container>
    );
  }

  return (
    <Container className="mt-5">
      <Row className="justify-content-center">
        <Col md={6}>
          <Card>
            <Card.Body>
              <Card.Title>Perfil</Card.Title>
              <Card.Text>
                <strong>Nome:</strong> {profile.name}
              </Card.Text>
              <Card.Text>
                <strong>CPF:</strong> {profile.cpf}
              </Card.Text>
              <Card.Text>
                <strong>Email:</strong> {profile.email}
              </Card.Text>
              <Card.Text>
                <strong>Descrição:</strong> {profile.description}
              </Card.Text>
              <Card.Text>
                <strong>Reputação:</strong> {profile.rating}
              </Card.Text>
              <Card.Text>
                <strong>Criado em:</strong> {new Date(profile.created_at).toLocaleString()}
              </Card.Text>
            </Card.Body>
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default Perfil;
