import React, { useEffect, useRef, useState } from "react";

function SelecionarLoc({ onLocationSelect }) {
  const mapRef = useRef(null);
  const [map, setMap] = useState(null);
  const [marker, setMarker] = useState(null);

  useEffect(() => {
    const google = window.google;

    // Inicializa o mapa
    const mapInstance = new google.maps.Map(mapRef.current, {
      center: { lat: -22.90278, lng: -43.2075 }, // Coordenadas iniciais (exemplo: Rio de Janeiro)
      zoom: 10,
    });

    // Adiciona um listener para capturar cliques no mapa
    mapInstance.addListener("click", (event) => {
      const { lat, lng } = event.latLng.toJSON();
      
      // Adiciona ou atualiza o marcador
      if (marker) {
        marker.setPosition(event.latLng);
      } else {
        const newMarker = new google.maps.Marker({
          position: event.latLng,
          map: mapInstance,
        });
        setMarker(newMarker);
      }

      // Envia as coordenadas para o componente pai
      onLocationSelect({ latitude: lat, longitude: lng });
    });

    setMap(mapInstance);
  }, [marker, onLocationSelect]);

  return <div ref={mapRef} style={{ height: "400px", width: "100%" }}></div>;
}

export default SelecionarLoc;
