import React, { useState } from 'react';

function ExportImage({ ws }) {
  const [imageUrl, setImageUrl] = useState(null);

  const handleImageChange = async (e) => {
    const selectedImage = e.target.files[0];

    if (selectedImage) {
      const dataURL = await displayImage(selectedImage); // Afficher l'image dans la page web
      setImageUrl(dataURL); // Mettre à jour l'URL de l'image dans le state
      console.log(dataURL); // Afficher l'URL de l'image dans la console
      const dataToSend = {
        fileName: selectedImage.name,
        fileSize: selectedImage.size,
        fileType: selectedImage.type,
      };
      const jsonData = JSON.stringify(dataToSend);
      await sendNameFiles(jsonData);
    }
  };

  const displayImage = (file) => {
    return new Promise((resolve) => {
      const reader = new FileReader();
      reader.onload = () => {
        const img = new Image();
        img.onload = () => {
          const canvas = document.createElement('canvas');
          const ctx = canvas.getContext('2d');

          // Calculer les nouvelles dimensions de l'image tout en maintenant le ratio d'aspect
          let width = img.width;
          let height = img.height;
          if (width > 1920) {
            height = Math.round((1920 / width) * height);
            width = 1920;
          }
          if (height > 1080) {
            width = Math.round((1080 / height) * width);
            height = 1080;
          }

          // Redimensionner l'image sur le canevas
          canvas.width = width;
          canvas.height = height;
          ctx.drawImage(img, 0, 0, width, height);

          // Convertir le canevas en URL de données
          const dataURL = canvas.toDataURL('image/jpeg');
          console.log(dataURL); // Afficher l'URL de l'image dans la console
          resolve(dataURL);
        };
        img.src = URL.createObjectURL(file);
      };
      reader.readAsDataURL(file);
    });
  };

  const sendNameFiles = async (jsonData) => {
    ws.send(JSON.stringify({ data: jsonData }));
  };

  return (
    <>
      <h1>Choisissez une image depuis votre appareil!</h1>
      <form>
        <label>
          <input type="file" accept=".jpg, .png, .heic, .tiff" onChange={handleImageChange} />
        </label>
      </form>
      {imageUrl && <img src={imageUrl} alt="Uploaded Image" />} {/* Afficher l'image si l'URL est disponible */}
    </>
  );
}

export default ExportImage;
