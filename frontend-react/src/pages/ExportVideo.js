import React from 'react';

function ExportVideo({ ws }) {
  const handleVideoChange = async (e) => {
    const video = e.target.files[0];

    if (video) {
      displayVideoInConsole(video); // Affiche la vidéo dans la console
      const dataToSend = {
        fileName: video.name,
        fileSize: video.size,
        fileType: video.type,
      };
      const jsonData = JSON.stringify(dataToSend);
      await sendNameFiles(jsonData);
    }
  };

  const displayVideoInConsole = (file) => {
    const reader = new FileReader();
    reader.onload = () => {
      const dataURL = reader.result;
      console.log('Video Data URL:', dataURL);
    };
    reader.readAsDataURL(file);
  };

  const sendNameFiles = async (jsonData) => {
    ws.send(JSON.stringify({ data: jsonData }));
  };

  return (
    <>
      <h1>Choisissez une vidéo depuis votre appareil!</h1>
      <form>
        <label>
          <input type="file" accept=".mp4, .mv4, .mov" onChange={handleVideoChange} />
        </label>
      </form>
    </>
  );
}

export default ExportVideo;
