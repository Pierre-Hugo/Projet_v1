import React, { useState } from 'react';

function ExportVideo({ ws }) {
  const [videoUrl, setVideoUrl] = useState('');

  const handleVideoChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = () => {
        const dataURL = reader.result;
        console.log('Video Data URL:', dataURL);
        setVideoUrl(dataURL);
      };
      reader.readAsDataURL(file);
    }
  };

  const handleVideoUrlChange = (e) => {
    setVideoUrl(e.target.value);
  };

  const handleLoadVideo = () => {
    console.log('Sending video URL:', videoUrl);
  };

  return (
    <>
      <h1>Choisissez une vidéo depuis votre appareil ou entrez une URL de vidéo:</h1>
      <form>
        <label>
          <input type="file" accept=".mp4, .mv4, .mov" onChange={handleVideoChange} />
        </label>
        <br />
      </form>
      {videoUrl && (
        <div>
          <h2>Votre vidéo:</h2>
          <video controls>
            <source src={videoUrl} type="video/mp4" />
            Your browser does not support the video tag.
          </video>
        </div>
      )}
    </>
  );
}

export default ExportVideo;