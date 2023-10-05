import React, { useState } from 'react';


function Export({ws}) {

    const [mot, setInputMot] = useState('');
    const [image, setFileImage] = useState(null); 

    const handleInputChange = (e) => {
      setInputMot(e.target.value);

    };

    const handleImageChange = async (e) => {

        const image = e.target.files[0];



        if (image) {
          setFileImage(image);

                        // Créez un objet contenant les données que vous souhaitez envoyer
        const dataToSend = {
            fileName: image.name,
            fileSize: image.size,
            fileType: image.type,
        };

      // Convertissez l'objet en une chaîne JSON
      const jsonData = JSON.stringify(dataToSend);

        //ws.send(JSON.stringify({ data: jsonData }));

          await sendFileToWebSocket(image);
          await sendNameFiles(jsonData)
          console.log()

        } else {
          setFileImage(null);
        }
  
      };

        // Fonction pour envoyer le fichier via WebSocket
  const sendFileToWebSocket = async (file) => {

    ws.addEventListener('open', async () => {
      // Envoyez le nom du fichier en premier
      ws.send(file.name);

      // Ensuite, envoyez le contenu du fichier en morceaux
      const chunkSize = 1024; // Taille des morceaux en octets
      let offset = 0;

      while (offset < file.size) {
        const chunk = file.slice(offset, offset + chunkSize);
        ws.send(chunk);
        offset += chunkSize;
      }
      
      
    });

    ws.addEventListener('error', (error) => {
      console.error('Erreur WebSocket :', error);
    });

    ws.addEventListener("message", data => {
      console.log(data);
  });

  
  };

  const handleSubmit = async (e) => {
    e.preventDefault(); // Prevent the default form submission behavior

    if (!mot) {
      // Handle the case when "mot" is empty, if needed
      return;
    }

    // Create a FormData object to collect form data
    const formData = new FormData();
    formData.append('mot', mot);
    if (image) {
      formData.append('image', image);
    }

    try {
      // Send the FormData via WebSocket
      const dataToSend = {
        formData: formData,
        mot: mot,
      };

      ws.send(JSON.stringify(dataToSend));

      // Clear the form after sending
      setInputMot('');
      setFileImage(null);
    } catch (error) {
      console.error('Error sending data via WebSocket:', error);
    }
  };


    const send = async (mot) =>{
        ws.send(JSON.stringify({ data: mot }));
    }

    const sendNameFiles = async (jsonData) =>{
        ws.send(JSON.stringify({ data: jsonData }));
    }

    


    return (
      <>
            <h1>Choissisez un média pour commencer la partie !</h1>
              <form onSubmit={handleSubmit}>
                <label>
                    Mot:
                    <input type="text" value={mot} onChange={handleInputChange} />
                </label>
                <br />
                <br />
                <label>
                    Dessin:
                    <input type="file" value="" />
                </label>
                <br />
                <br />
                <label>
                    Image:
                    <input type="file" onChange={handleImageChange} />
                </label>
                <br />
                <br />
                <label>
                    Vidéo:
                    <input type="file" value="" />
                </label>
                <br />
                <br />
                <button onClick={() => send(mot)}>Sousmettre</button>
            </form>
      </>
        );
  
  }
  
  export default Export;