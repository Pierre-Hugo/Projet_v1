import React from 'react';

function ExportImage({ ws }) {
  const handleImageChange = async (e) => {
    const selectedImage = e.target.files[0];

    if (selectedImage) {
      displayImageInConsole(selectedImage); // Affiche l'image dans la console
      const dataToSend = {
        fileName: selectedImage.name,
        fileSize: selectedImage.size,
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
