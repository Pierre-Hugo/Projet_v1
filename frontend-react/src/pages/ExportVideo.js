import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

function ExportVideo({ ws }) {
  const [videoUrl, setVideoUrl] = useState('');
  const navigate = useNavigate();

  const handleVideoChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      var unityID = localStorage.getItem('UNITY');
      var pseudo = localStorage.getItem('pseudo');
      var pseudoColor = localStorage.getItem('pseudoColor');
      const reader = new FileReader();

      reader.onload = () => {
        const dataURL = reader.result;
        setVideoUrl(dataURL);
        ws.send(unityID + ": NP," + pseudo + "," + pseudoColor + ",VID," + dataURL);
      };
      reader.readAsDataURL(file);

    }
    navigate('/WaitingState');
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