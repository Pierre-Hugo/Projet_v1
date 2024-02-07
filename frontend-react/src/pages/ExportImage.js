import React from 'react';

function ExportImage({ ws }) {
  const handleImageChange = async (e) => {
    const selectedImage = e.target.files[0];

    if (selectedImage) {
      if (selectedImage.size > 5242880) { // Limite de 5 Mo (5242880 octets)
        alert('L\'image est trop grande. Veuillez sélectionner une image de taille inférieure à 5 Mo.');
        return;
      }

      const resizedImage = await resizeImage(selectedImage);

      displayImageInConsole(resizedImage);

      const dataToSend = {
        fileName: selectedImage.name,
        fileSize: resizedImage.size,
        fileType: selectedImage.type,
      };
      const jsonData = JSON.stringify(dataToSend);
      await sendNameFiles(jsonData);
    }
  };

  const displayImageInConsole = (file) => {
    const reader = new FileReader();
    reader.onload = () => {
      const dataURL = reader.result;
      console.log('Image Data URL:', dataURL);
    };
    reader.readAsDataURL(file);
  };

  const sendNameFiles = async (jsonData) => {
    ws.send(JSON.stringify({ data: jsonData }));
  };

  const resizeImage = (imageFile) => {
    return new Promise((resolve) => {
      const reader = new FileReader();
      reader.onload = (event) => {
        const img = new Image();
        img.onload = () => {
          const canvas = document.createElement('canvas');
          const ctx = canvas.getContext('2d');
          canvas.width = 800; // Largeur souhaitée
          canvas.height = (canvas.width / img.width) * img.height;
          ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
          canvas.toBlob((blob) => {
            resolve(new File([blob], imageFile.name, { type: 'image/jpeg', lastModified: Date.now() }));
          }, 'image/jpeg', 0.8); // Qualité de compression (0.8 = 80%)
        };
        img.src = event.target.result;
      };
      reader.readAsDataURL(imageFile);
    });
  };

  return (
    <>
      <h1>Choisissez une image depuis votre appareil!</h1>
      <form>
        <label>
          <input type="file" accept=".jpg, .png, .heic, .tiff" onChange={handleImageChange} />
        </label>
      </form>
    </>
  );
}

export default ExportImage;
