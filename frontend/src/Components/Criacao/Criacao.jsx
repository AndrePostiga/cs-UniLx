import React, { useState, useCallback } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import { useLoadScript, GoogleMap, Marker } from "@react-google-maps/api";

function AdForm() {
  const [formData, setFormData] = useState({
    type: "",
    sub_category: "",
    expires_at: "",
    latitude: "",
    longitude: "",
    title: "",
    description: "",
    price: "",
    product_type: "",
    brand: "",
    skin_type: "",
    expiration_date: "",
    ingredients: "",
    is_organic: false,
    is_salary_disclosed: false,
    category: "",
  });

  const [toastVisible, setToastVisible] = useState(false); // Estado do toast
  const [subCategories, setSubCategories] = useState([]);

  const [mapCenter, setMapCenter] = useState({ lat: -22.8808, lng: -43.1043 });




  const { isLoaded } = useLoadScript({
    googleMapsApiKey: "AIzaSyDsSwjiKYS4BxAASXnYwNfZkSjaK1Eug3c", // Substitua pela sua chave
  });

  const [selectedLocation, setSelectedLocation] = useState(null);

  const handleMapClick = useCallback((event) => {
    const latitude = event.latLng.lat();
    const longitude = event.latLng.lng();
  
    setFormData((prev) => ({
      ...prev,
      latitude: latitude.toString(),
      longitude: longitude.toString(),
    }));
  
    setSelectedLocation({ lat: latitude, lng: longitude });
    setMapCenter({ lat: latitude, lng: longitude }); // Atualiza o centro do mapa
  }, []);
  

  if (!isLoaded) return <div>Carregando mapa...</div>;






  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
  
    // Atualizar o campo "type" automaticamente com base na categoria selecionada
    let updatedValue = type === "checkbox" ? checked : value;
  
    // Mapeamento de categoria para tipo de anúncio
    const categoryToTypeMap = {
      beleza: "beauty",
      eletronicos: "electronics",
      moda: "fashion",
      eventos: "events",
      empregos: "job_opportunities",
      animais: "pets",
    };
  
    // Mapeamento de subcategorias
    const subCategoryMap = {
      beleza: ["makeup", "fragrances", "haircare", "skincare"],
      eletronicos: ["smartphones", "laptops", "pcs", "videogames"],
      moda: ["mens_clothing", "womens_clothing", "kids_clothing", "unisex"],
      eventos: ["concerts", "workshops", "conferences", "parties_nightlife"],
      empregos: ["it_software", "healthcare", "education", "freelance"],
      animais: ["accessories"],
    };
  
    if (name === "category") {
      const subCategoriesForCategory = subCategoryMap[updatedValue] || [];
      setSubCategories(subCategoriesForCategory);
  
      setFormData((prev) => ({
        ...prev,
        [name]: updatedValue,
        type: categoryToTypeMap[updatedValue] || "", // Atualiza automaticamente o tipo
        sub_category: "", // Reseta o campo subcategoria ao mudar a categoria
      }));
    } else {
      setFormData((prev) => ({
        ...prev,
        [name]: updatedValue,
      }));
    }
  };
  
  
  const handleSubmit = async (e) => {
    e.preventDefault();

    const payload = {
      type: formData.type,
      sub_category: formData.sub_category,
      expires_at: new Date(formData.expires_at).toISOString(),
      address: {
        latitude: parseFloat(formData.latitude),
        longitude: parseFloat(formData.longitude),
      },
    };

    if (formData.category === "beleza") {
      payload.beauty_details = {
        title: formData.title,
        description: formData.description,
        price: parseFloat(formData.price),
        product_type: formData.product_type,
        brand: formData.brand,
        skin_type: formData.skin_type,
        expiration_date: new Date(formData.expiration_date).toISOString(),
        ingredients: formData.ingredients.split(",").map((item) => item.trim()),
        is_organic: formData.is_organic,
      };
    } else if (formData.category === "eletronicos") {
      payload.electronics_details = {
        title: formData.title,
        description: formData.description,
        price: parseFloat(formData.price),
        product_type: formData.product_type,
        brand: formData.brand,
        model: formData.model,
        storage_capacity: formData.storage_capacity,
        memory: formData.memory,
        processor: formData.processor,
        battery_life: parseFloat(formData.battery_life),
        warranty_until: new Date(formData.warranty_until).toISOString(),
        features: formData.features.split(",").map((item) => item.trim()),
        condition: formData.condition,
        includes_original_box: formData.includes_original_box,
        accessories: formData.accessories.split(",").map((item) => item.trim()),
      };
    } else if (formData.category === "moda") {
      payload.fashion_details = {
        title: formData.title,
        description: formData.description,
        price: parseFloat(formData.price),
        clothing_type: formData.clothing_type, 
        brand: formData.brand,
        sizes: formData.sizes.split(",").map((size) => size.trim()), 
        gender: formData.gender, 
        colors: formData.colors.split(",").map((color) => color.trim()), 
        materials: formData.materials.split(",").map((material) => material.trim()), 
        features: formData.features.split(",").map((feature) => feature.trim()), 
        designer: formData.designer,
        isHandmade: formData.is_handmade === "true", 
        releaseDate: new Date(formData.release_date).toISOString(),
        isSustainable: formData.is_sustainable === "true", 
      };
    } else if (formData.category === "eventos") {
      payload.events_details = {
          title: formData.title,
          description: formData.description,
          price: parseFloat(formData.price),
          event_type: formData.event_type, 
          event_date: new Date(formData.event_date).toISOString(),
          organizer: formData.organizer,
          age_restriction: formData.age_restriction,
          dress_code: formData.dress_code,
          highlights: formData.highlights.split(",").map((highlight) => highlight.trim()),
          is_online: formData.is_online === "true",
          contact_information: {
              phone: {
                  country_code: formData.phone_country_code,
                  area_code: formData.phone_area_code,
                  number: formData.phone_number
              },
              email: formData.contact_email,
              website: formData.contact_website
          }
      };
    } else if (formData.category === "empregos") {
      payload.job_opportunities_details = {
          title: formData.title,
          description: formData.description,
          position: formData.position,
          company: formData.company,
          salary: parseFloat(formData.salary),
          is_salary_disclosed: formData.is_salary_disclosed,
          work_location: formData.work_location,
          employment_type: formData.employment_type,
          experience_level: formData.experience_level,
          skills: formData.skills.split(",").map((skill) => skill.trim()),
          benefits: formData.benefits.split(",").map((benefit) => benefit.trim()),
          realocation_help: formData.realocation_help === "true",
          application_deadline: new Date(formData.application_deadline).toISOString(),
          contact_information: {
            phone: {
                country_code: formData.phone_country_code,
                area_code: formData.phone_area_code,
                number: formData.phone_number
            },
            email: formData.contact_email,
            website: formData.contact_website
          }
      };
    } else if (formData.category === "animais") {
      payload.pet_details = {
        title: formData.title,
        description: formData.description,
        price: parseFloat(formData.price),
        pet_type: formData.pet_type,
        animal_type: formData.animal_type,
        accessory_type: formData.accessory_type,
        materials: formData.materials.split(",").map((material) => material.trim()),
      };
    }
    
  
    console.log(JSON.stringify(payload, null, 2));
    try {
      const response = await fetch("http://localhost:5327/advertisements", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          "X-Impersonate": "account_01JE3PCSKS8S378F6CS9D3VGY2",
        },
        body: JSON.stringify(payload),
      });

      if (response.ok) {
        console.log("Anúncio cadastrado com sucesso!");
        setToastVisible(true); // Exibe o toast
        setTimeout(() => setToastVisible(false), 3000); // Esconde após 3 segundos
      } else {
        console.error("Erro ao cadastrar anúncio");
      }
    } catch (error) {
      console.error("Erro na requisição:", error);
    }
  };

  return (
    <div className="container mt-5">
      <h2 className="mb-4">Cadastrar Anúncio</h2>

      {/* Toast */}
      <div
        className={`toast-container position-fixed top-0 end-0 p-3`}
        style={{ zIndex: 1055 }}
      >
        <div
          className={`toast align-items-center text-bg-success ${
            toastVisible ? "show" : "hide"
          }`}
          role="alert"
          aria-live="assertive"
          aria-atomic="true"
        >
          <div className="d-flex">
            <div className="toast-body">Anúncio cadastrado com sucesso!</div>
            <button
              type="button"
              className="btn-close btn-close-white me-2 m-auto"
              data-bs-dismiss="toast"
              aria-label="Close"
              onClick={() => setToastVisible(false)}
            ></button>
          </div>
        </div>
      </div>

      <form onSubmit={handleSubmit}>
        
        <div className="mb-3">
          <label htmlFor="expires_at" className="form-label">
            Data de Expiração do Anúncio
          </label>
          <input
            type="datetime-local"
            className="form-control"
            id="expires_at"
            name="expires_at"
            onChange={handleChange}
          />
        </div>
        
        <div className="mb-3">
          <label htmlFor="latitude" className="form-label">
            Latitude
          </label>
          <input
            type="text"
            className="form-control"
            id="latitude"
            name="latitude"
            value={formData.latitude}
            readOnly
          />
        </div>
        <div className="mb-3">
          <label htmlFor="longitude" className="form-label">
            Longitude
          </label>
          <input
            type="text"
            className="form-control"
            id="longitude"
            name="longitude"
            value={formData.longitude}
            readOnly
          />
        </div>
        <div className="mb-3">
          <label>Escolha o local no mapa</label>
          <div style={{ height: "400px", width: "100%" }}>
          <GoogleMap
            zoom={10}
            center={mapCenter} // Usa o estado como centro do mapa
            mapContainerStyle={{ height: "100%", width: "100%" }}
            onClick={handleMapClick}
          >
            {selectedLocation && <Marker position={selectedLocation} />}
          </GoogleMap>

          </div>
        </div>
        
        <div className="mb-3">
          <label htmlFor="category" className="form-label">
            Categoria
          </label>
          <select
            className="form-select"
            id="category"
            name="category"
            onChange={handleChange}
          >
            <option value="">Selecione a Categoria</option>
            <option value="beleza">Beleza</option>
            <option value="eletronicos">Eletrônicos</option>
            <option value="moda">Moda</option>
            <option value="eventos">Eventos</option>
            <option value="empregos">Vagas de Emprego</option>
            <option value="animais">Animais</option>
          </select>
        </div>
        <div className="mb-3">
          <label htmlFor="sub_category" className="form-label">
            Subcategoria
          </label>
          <select
            className="form-select"
            id="sub_category"
            name="sub_category"
            value={formData.sub_category}
            onChange={handleChange}
            disabled={subCategories.length === 0} // Desativa o campo se não houver subcategorias disponíveis
          >
            <option value="">Selecione a Subcategoria</option>
            {subCategories.map((subCat) => (
              <option key={subCat} value={subCat}>
                {subCat}
              </option>
            ))}
          </select>
        </div>


        {formData.category === "beleza" && (
          <>
            <div className="mb-3">
              <label htmlFor="title" className="form-label">
                Título
              </label>
              <input
                type="text"
                className="form-control"
                id="title"
                name="title"
                placeholder="Título"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="description" className="form-label">
                Descrição
              </label>
              <textarea
                className="form-control"
                id="description"
                name="description"
                placeholder="Descrição"
                onChange={handleChange}
              ></textarea>
            </div>
            <div className="mb-3">
              <label htmlFor="price" className="form-label">
                Preço
              </label>
              <input
                type="number"
                className="form-control"
                id="price"
                name="price"
                placeholder="Preço"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="product_type" className="form-label">
                Tipo de Produto
              </label>
              <input
                type="text"
                className="form-control"
                id="product_type"
                name="product_type"
                placeholder="Tipo de Produto"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="brand" className="form-label">
                Marca
              </label>
              <input
                type="text"
                className="form-control"
                id="brand"
                name="brand"
                placeholder="Marca"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="skin_type" className="form-label">
                Tipo de Pele
              </label>
              <input
                type="text"
                className="form-control"
                id="skin_type"
                name="skin_type"
                placeholder="Tipo de Pele"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="expiration_date" className="form-label">
                Data de Validade do Produto
              </label>
              <input
                type="datetime-local"
                className="form-control"
                id="expiration_date"
                name="expiration_date"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="ingredients" className="form-label">
                Ingredientes (separados por vírgula)
              </label>
              <input
                type="text"
                className="form-control"
                id="ingredients"
                name="ingredients"
                placeholder="Ingredientes"
                onChange={handleChange}
              />
            </div>
            <div className="form-check mb-3">
              <input
                type="checkbox"
                className="form-check-input"
                id="is_organic"
                name="is_organic"
                onChange={handleChange}
              />
              <label className="form-check-label" htmlFor="is_organic">
                É orgânico?
              </label>
            </div>
          </>
        )}

        {formData.category === "eletronicos" && (
          <>
            <div className="mb-3">
              <label htmlFor="title" className="form-label">
                Título
              </label>
              <input
                type="text"
                className="form-control"
                id="title"
                name="title"
                placeholder="Título"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="description" className="form-label">
                Descrição
              </label>
              <textarea
                className="form-control"
                id="description"
                name="description"
                placeholder="Descrição"
                onChange={handleChange}
              ></textarea>
            </div>
            <div className="mb-3">
              <label htmlFor="price" className="form-label">
                Preço
              </label>
              <input
                type="number"
                className="form-control"
                id="price"
                name="price"
                placeholder="Preço"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="product_type" className="form-label">
                Tipo de Produto
              </label>
              <input
                type="text"
                className="form-control"
                id="product_type"
                name="product_type"
                placeholder="Tipo de Produto"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="brand" className="form-label">
                Marca
              </label>
              <input
                type="text"
                className="form-control"
                id="brand"
                name="brand"
                placeholder="Marca"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="model" className="form-label">
                Modelo
              </label>
              <input
                type="text"
                className="form-control"
                id="model"
                name="model"
                placeholder="Modelo"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="storage_capacity" className="form-label">
                Capacidade de Armazenamento
              </label>
              <input
                type="text"
                className="form-control"
                id="storage_capacity"
                name="storage_capacity"
                placeholder="Capacidade de Armazenamento"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="memory" className="form-label">
                Memória
              </label>
              <input
                type="text"
                className="form-control"
                id="memory"
                name="memory"
                placeholder="Memória"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="processor" className="form-label">
                Processador
              </label>
              <input
                type="text"
                className="form-control"
                id="processor"
                name="processor"
                placeholder="Processador"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="battery_life" className="form-label">
                Duração da Bateria (em percentual)
              </label>
              <input
                type="number"
                step="any"
                className="form-control"
                id="battery_life"
                name="battery_life"
                placeholder="Duração da Bateria"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="warranty_until" className="form-label">
                Garantia até
              </label>
              <input
                type="datetime-local"
                className="form-control"
                id="warranty_until"
                name="warranty_until"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="features" className="form-label">
                Características (separadas por vírgula)
              </label>
              <input
                type="text"
                className="form-control"
                id="features"
                name="features"
                placeholder="Características"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="condition" className="form-label">
                Condição do Produto
              </label>
              <input
                type="text"
                className="form-control"
                id="condition"
                name="condition"
                placeholder="new, used"
                onChange={handleChange}
              />
            </div>
            <div className="form-check mb-3">
              <input
                type="checkbox"
                className="form-check-input"
                id="includes_original_box"
                name="includes_original_box"
                onChange={handleChange}
              />
              <label
                className="form-check-label"
                htmlFor="includes_original_box"
              >
                Inclui caixa original?
              </label>
            </div>
            <div className="mb-3">
              <label htmlFor="accessories" className="form-label">
                Acessórios (separados por vírgula)
              </label>
              <input
                type="text"
                className="form-control"
                id="accessories"
                name="accessories"
                placeholder="Acessórios"
                onChange={handleChange}
              />
            </div>
          </>
        )}

        {formData.category === "moda" && (
          <>
            <div className="mb-3">
              <label htmlFor="title" className="form-label">
                Título
              </label>
              <input
                type="text"
                className="form-control"
                id="title"
                name="title"
                placeholder="Título"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="description" className="form-label">
                Descrição
              </label>
              <textarea
                className="form-control"
                id="description"
                name="description"
                placeholder="Descrição"
                onChange={handleChange}
              ></textarea>
            </div>
            <div className="mb-3">
              <label htmlFor="price" className="form-label">
                Preço
              </label>
              <input
                type="number"
                className="form-control"
                id="price"
                name="price"
                placeholder="Preço"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="clothing_type" className="form-label">
                Tipo de Roupa
              </label>
              <input
                type="text"
                className="form-control"
                id="clothing_type"
                name="clothing_type"
                placeholder="Tipo de Roupa"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="brand" className="form-label">
                Marca
              </label>
              <input
                type="text"
                className="form-control"
                id="brand"
                name="brand"
                placeholder="Marca"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="sizes" className="form-label">
                Tamanho
              </label>
              <input
                type="text"
                className="form-control"
                id="sizes"
                name="sizes"
                placeholder="Tamanhos (g, gg, m, p, plus_size, pp, xg, xxg, xxxg)"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="gender" className="form-label">
                Gênero
              </label>
              <input
                type="text"
                className="form-control"
                id="gender"
                name="gender"
                placeholder="Gênero (male, female, unisex)"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="colors" className="form-label">
                Cores (separadas por vírgula)
              </label>
              <input
                type="text"
                className="form-control"
                id="colors"
                name="colors"
                placeholder="Cores"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="materials" className="form-label">
                Materiais (separados por vírgula)
              </label>
              <input
                type="text"
                className="form-control"
                id="materials"
                name="materials"
                placeholder="Materiais"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="features" className="form-label">
                Características (separadas por vírgula)
              </label>
              <input
                type="text"
                className="form-control"
                id="features"
                name="features"
                placeholder="Características"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="designer" className="form-label">
                Designer
              </label>
              <input
                type="text"
                className="form-control"
                id="designer"
                name="designer"
                placeholder="Designer"
                onChange={handleChange}
              />
            </div>
            <div className="form-check mb-3">
              <input
                type="checkbox"
                className="form-check-input"
                id="is_handmade"
                name="is_handmade"
                onChange={handleChange}
              />
              <label className="form-check-label" htmlFor="is_handmade">
                É feito à mão?
              </label>
            </div>
            <div className="mb-3">
              <label htmlFor="release_date" className="form-label">
                Data de Lançamento
              </label>
              <input
                type="datetime-local"
                className="form-control"
                id="release_date"
                name="release_date"
                onChange={handleChange}
              />
            </div>
            <div className="form-check mb-3">
              <input
                type="checkbox"
                className="form-check-input"
                id="is_sustainable"
                name="is_sustainable"
                onChange={handleChange}
              />
              <label className="form-check-label" htmlFor="is_sustainable">
                É sustentável?
              </label>
            </div>
          </>
        )}

        {formData.category === "eventos" && (
            <>
              <div className="mb-3">
                <label htmlFor="title" className="form-label">
                  Título
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="title"
                  name="title"
                  placeholder="Título"
                  onChange={handleChange}
                />
              </div>
              <div className="mb-3">
                <label htmlFor="description" className="form-label">
                  Descrição
                </label>
                <textarea
                  className="form-control"
                  id="description"
                  name="description"
                  placeholder="Descrição"
                  onChange={handleChange}
                ></textarea>
              </div>
              <div className="mb-3">
                <label htmlFor="price" className="form-label">
                  Preço
                </label>
                <input
                  type="number"
                  className="form-control"
                  id="price"
                  name="price"
                  placeholder="Preço"
                  onChange={handleChange}
                />
              </div>
              <div className="mb-3">
                <label htmlFor="event_type" className="form-label">
                  Tipo de Evento
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="event_type"
                  name="event_type"
                  placeholder="Tipo de Evento"
                  onChange={handleChange}
                />
              </div>
              <div className="mb-3">
                <label htmlFor="event_date" className="form-label">
                  Data do Evento
                </label>
                <input
                  type="datetime-local"
                  className="form-control"
                  id="event_date"
                  name="event_date"
                  onChange={handleChange}
                />
              </div>
              <div className="mb-3">
                <label htmlFor="organizer" className="form-label">
                  Organizador
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="organizer"
                  name="organizer"
                  placeholder="Organizador"
                  onChange={handleChange}
                />
              </div>
              <div className="mb-3">
                <label htmlFor="age_restriction" className="form-label">
                  Restrição de Idade
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="age_restriction"
                  name="age_restriction"
                  placeholder="Restrição de Idade (age10, age12, age14, age16, age18, free)"
                  onChange={handleChange}
                />
              </div>
              <div className="mb-3">
                <label htmlFor="dress_code" className="form-label">
                  Dress Code
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="dress_code"
                  name="dress_code"
                  placeholder="Dress Code"
                  onChange={handleChange}
                />
              </div>
              <div className="mb-3">
                <label htmlFor="highlights" className="form-label">
                  Destaques (separados por vírgula)
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="highlights"
                  name="highlights"
                  placeholder="Destaques"
                  onChange={handleChange}
                />
              </div>
              <div className="form-check mb-3">
                <input
                  type="checkbox"
                  className="form-check-input"
                  id="is_online"
                  name="is_online"
                  onChange={handleChange}
                />
                <label className="form-check-label" htmlFor="is_online">
                  É online?
                </label>
              </div>
              <h5>Informações de Contato</h5>
              <div className="mb-3">
                <label htmlFor="phone_country_code" className="form-label">
                  Código do País
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="phone_country_code"
                  name="phone_country_code"
                  placeholder="Código do País"
                  onChange={handleChange}
                />
              </div>
              <div className="mb-3">
                <label htmlFor="phone_area_code" className="form-label">
                  Código de Área
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="phone_area_code"
                  name="phone_area_code"
                  placeholder="Código de Área"
                  onChange={handleChange}
                />
              </div>
              <div className="mb-3">
                <label htmlFor="phone_number" className="form-label">
                  Número de Telefone
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="phone_number"
                  name="phone_number"
                  placeholder="Número de Telefone"
                  onChange={handleChange}
                />
              </div>
              <div className="mb-3">
                <label htmlFor="contact_email" className="form-label">
                  E-mail de Contato
                </label>
                <input
                  type="email"
                  className="form-control"
                  id="contact_email"
                  name="contact_email"
                  placeholder="E-mail de Contato"
                  onChange={handleChange}
                />
              </div>
              <div className="mb-3">
                <label htmlFor="contact_website" className="form-label">
                  Website
                </label>
                <input
                  type="url"
                  className="form-control"
                  id="contact_website"
                  name="contact_website"
                  placeholder="Website"
                  onChange={handleChange}
                />
              </div>
            </>
        )}

        {formData.category === "empregos" && (
          <>
            <div className="mb-3">
              <label htmlFor="title" className="form-label">
                Título
              </label>
              <input
                type="text"
                className="form-control"
                id="title"
                name="title"
                placeholder="Título da vaga"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="description" className="form-label">
                Descrição
              </label>
              <textarea
                className="form-control"
                id="description"
                name="description"
                placeholder="Descrição da vaga"
                onChange={handleChange}
              ></textarea>
            </div>
            <div className="mb-3">
              <label htmlFor="position" className="form-label">
                Cargo
              </label>
              <input
                type="text"
                className="form-control"
                id="position"
                name="position"
                placeholder="Cargo"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="company" className="form-label">
                Empresa
              </label>
              <input
                type="text"
                className="form-control"
                id="company"
                name="company"
                placeholder="Nome da empresa"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="salary" className="form-label">
                Salário
              </label>
              <input
                type="number"
                className="form-control"
                id="salary"
                name="salary"
                placeholder="Salário (em reais)"
                onChange={handleChange}
              />
            </div>
            <div className="form-check mb-3">
              <input
                type="checkbox"
                className="form-check-input"
                id="is_salary_disclosed"
                name="is_salary_disclosed"
                onChange={handleChange}
              />
              <label className="form-check-label" htmlFor="is_salary_disclosed">
                Salário divulgado?
              </label>
            </div>
            <div className="mb-3">
              <label htmlFor="work_location" className="form-label">
                Local de Trabalho
              </label>
              <input
                type="text"
                className="form-control"
                id="work_location"
                name="work_location"
                placeholder="Ex.: hybrid, on_site, remote"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="employment_type" className="form-label">
                Tipo de Contrato
              </label>
              <input
                type="text"
                className="form-control"
                id="employment_type"
                name="employment_type"
                placeholder="Ex.: contract, full_time, part_time"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="experience_level" className="form-label">
                Nível de Experiência
              </label>
              <input
                type="text"
                className="form-control"
                id="experience_level"
                name="experience_level"
                placeholder="Ex.: Júnior, Pleno, Sênior"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="skills" className="form-label">
                Habilidades Requeridas (separadas por vírgula)
              </label>
              <input
                type="text"
                className="form-control"
                id="skills"
                name="skills"
                placeholder="Habilidades"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="benefits" className="form-label">
                Benefícios (separados por vírgula)
              </label>
              <input
                type="text"
                className="form-control"
                id="benefits"
                name="benefits"
                placeholder="Benefícios"
                onChange={handleChange}
              />
            </div>
            <div className="form-check mb-3">
              <input
                type="checkbox"
                className="form-check-input"
                id="relocation_help"
                name="relocation_help"
                onChange={handleChange}
              />
              <label className="form-check-label" htmlFor="relocation_help">
                Auxílio para mudança de local?
              </label>
            </div>
            <div className="mb-3">
              <label htmlFor="application_deadline" className="form-label">
                Prazo para candidatura
              </label>
              <input
                type="datetime-local"
                className="form-control"
                id="application_deadline"
                name="application_deadline"
                onChange={handleChange}
              />
            </div>
            <h5>Informações de Contato</h5>
            <div className="mb-3">
                <label htmlFor="phone_country_code" className="form-label">
                  Código do País
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="phone_country_code"
                  name="phone_country_code"
                  placeholder="Código do País"
                  onChange={handleChange}
                />
              </div>
              <div className="mb-3">
                <label htmlFor="phone_area_code" className="form-label">
                  Código de Área
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="phone_area_code"
                  name="phone_area_code"
                  placeholder="Código de Área"
                  onChange={handleChange}
                />
              </div>
              <div className="mb-3">
                <label htmlFor="phone_number" className="form-label">
                  Número de Telefone
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="phone_number"
                  name="phone_number"
                  placeholder="Número de Telefone"
                  onChange={handleChange}
                />
              </div>
            <div className="mb-3">
              <label htmlFor="contact_email" className="form-label">
                Email
              </label>
              <input
                type="email"
                className="form-control"
                id="contact_email"
                name="contact_email"
                placeholder="Email de contato"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="contact_website" className="form-label">
                Website
              </label>
              <input
                type="url"
                className="form-control"
                id="contact_website"
                name="contact_website"
                placeholder="Website"
                onChange={handleChange}
              />
            </div>
          </>
        )}

        {formData.category === "animais" && (
          <>
            <div className="mb-3">
              <label htmlFor="title" className="form-label">Título</label>
              <input
                type="text"
                className="form-control"
                id="title"
                name="title"
                placeholder="Título"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="description" className="form-label">Descrição</label>
              <textarea
                className="form-control"
                id="description"
                name="description"
                placeholder="Descrição"
                onChange={handleChange}
              ></textarea>
            </div>
            <div className="mb-3">
              <label htmlFor="price" className="form-label">Preço</label>
              <input
                type="number"
                className="form-control"
                id="price"
                name="price"
                placeholder="Preço"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="pet_type" className="form-label">Tipo de Anúncio</label>
              <input
                type="text"
                className="form-control"
                id="pet_type"
                name="pet_type"
                placeholder="Ex.: accessories, adoption"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="animal_type" className="form-label">Animal</label>
              <input
                type="text"
                className="form-control"
                id="animal_type"
                name="animal_type"
                placeholder="Ex.: Mamífero, Ave"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="accessory_type" className="form-label">Tipo de Acessório</label>
              <input
                type="text"
                className="form-control"
                id="accessory_type"
                name="accessory_type"
                placeholder="Ex.: Coleira, Brinquedo"
                onChange={handleChange}
              />
            </div>
            <div className="mb-3">
              <label htmlFor="materials" className="form-label">
                Materiais (separados por vírgula)
              </label>
              <input
                type="text"
                className="form-control"
                id="materials"
                name="materials"
                placeholder="Ex.: Couro, Nylon"
                onChange={handleChange}
              />
            </div>
          </>
        )}



        <button type="submit" className="btn btn-primary mb-4">
          Cadastrar Anúncio
        </button>
      </form>
    </div>
  );
}

export default AdForm;