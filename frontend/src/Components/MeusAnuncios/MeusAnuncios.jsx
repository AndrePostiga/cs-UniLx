import React, { useEffect, useState } from "react";
import axios from "axios";
import "bootstrap/dist/css/bootstrap.min.css";

const MeusAnuncios = () => {
  const [ads, setAds] = useState([]);
  const [selectedAd, setSelectedAd] = useState(null);

  useEffect(() => {
    const fetchAds = async () => {
      try {
        const response = await axios.get(
          "http://localhost:5327/accounts/account_01JE3PCSKS8S378F6CS9D3VGY2/advertisements"
        );
        setAds(response.data.data);
      } catch (error) {
        console.error("Erro ao buscar anúncios:", error);
      }
    };

    fetchAds();
  }, []);

  const handleDetails = (ad) => {
    setSelectedAd(ad);
  };

  const handleCloseModal = () => {
    setSelectedAd(null);
  };

  return (
    <div className="container mt-5">
      <h2>Meus Anúncios</h2>
      <table className="table table-striped">
        <thead>
          <tr>
            <th>ID</th>
            <th>Tipo</th>
            <th>Categoria</th>
            <th>Status</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {ads.map((ad) => (
            <tr key={ad.id}>
              <td>{ad.id}</td>
              <td>{ad.type}</td>
              <td>{ad.category}</td>
              <td>{ad.status}</td>
              <td>
                <button
                  className="btn btn-primary btn-sm"
                  onClick={() => handleDetails(ad)}
                >
                  Detalhes
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {/* Modal para exibir os detalhes do anúncio */}
      {selectedAd && (
        <div
          className="modal fade show d-block"
          tabIndex="-1"
          role="dialog"
          aria-labelledby="adDetailsModal"
          aria-hidden="true"
        >
          <div className="modal-dialog" role="document">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title" id="adDetailsModal">
                  Detalhes do Anúncio
                </h5>
                <button
                  type="button"
                  className="btn-close"
                  onClick={handleCloseModal}
                  aria-label="Close"
                />
              </div>
              <div className="modal-body">
                <p>
                  <strong>ID:</strong> {selectedAd.id}
                </p>
                <p>
                  <strong>Status:</strong> {selectedAd.status}
                </p>
                <p>
                  <strong>Tipo:</strong> {selectedAd.type}
                </p>
                <p>
                  <strong>Categoria:</strong> {selectedAd.category}
                </p>
                <p>
                  <strong>Expira em:</strong> {new Date(selectedAd.expires_at).toLocaleString()}
                </p>
                <p>
                  <strong>Endereço:</strong> {`${selectedAd.address.neighborhood}, ${selectedAd.address.city}, ${selectedAd.address.state}, ${selectedAd.address.country}`}
                </p>
                <p>
                  <strong>CEP:</strong> {selectedAd.address.zip_code}
                </p>
                <p>
                  <strong>Criado em:</strong> {new Date(selectedAd.created_at).toLocaleString()}
                </p>
                <p>
                  <strong>Atualizado em:</strong> {new Date(selectedAd.updated_at).toLocaleString()}
                </p>
                {selectedAd.beauty_details && (
                  <>
                    <h6>Detalhes de Beleza:</h6>
                    <p>
                      <strong>Marca:</strong> {selectedAd.beauty_details.brand}
                    </p>
                    <p>
                      <strong>Descrição:</strong> {selectedAd.beauty_details.description}
                    </p>
                    <p>
                      <strong>Preço:</strong> R$ {selectedAd.beauty_details.price}
                    </p>
                  </>
                )}
                {/* Adicionar mais detalhes específicos de cada tipo de anúncio conforme necessário */}
              </div>
              <div className="modal-footer">
                <button
                  type="button"
                  className="btn btn-secondary"
                  onClick={handleCloseModal}
                >
                  Fechar
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default MeusAnuncios;
