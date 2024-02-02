import React, { useState } from 'react';
import CanvasComponent from '../components/Canvas';

function Export({ ws }) {
  const [mot, setInputMot] = useState('');
  const [image, setFileImage] = useState(null);
  const [video, setFileVideo] = useState(null);

  const handleInputChange = (e) => {
    setInputMot(e.target.value);
  };

  const handleImageChange = async (e) => {
    const image = e.target.files[0];

    if (image) {
      setFileImage(image);

      const dataToSend = {
        fileName: image.name,
        fileSize: image.size,
        fileType: image.type,
      };

      const jsonData = JSON.stringify(dataToSend);

      await sendNameFiles(jsonData);
    } else {
      setFileImage(null);
    }
  };

  const handleVideoChange = async (e) => {
    const video = e.target.files[0];

    if (video) {
      setFileVideo(video);

      const dataToSend = {
        fileName: video.name,
        fileSize: video.size,
        fileType: video.type,
      };

      const jsonData = JSON.stringify(dataToSend);

      await sendNameFiles(jsonData);
    } else {
      setFileVideo(null);
    }
  };

  const sendFileToWebSocket = async (file) => {
    ws.addEventListener('open', async () => {
      ws.send(file.name);

      const chunkSize = 1024;
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

    ws.addEventListener('message', (data) => {
      console.log(data);
    });
  };

  const sendNameFiles = async (jsonData) => {
    ws.send(JSON.stringify({ data: jsonData }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!mot && !image && !video) {
      return;
    }

    const formData = new FormData();
    formData.append('mot', mot);

    if (image) {
      formData.append('image', image);
      await sendFileToWebSocket("USERd5c4bcf9e9bf170b2210d6cccb3025972607c20aff670678496167c68ef6165d:" + image);
    }

    if (video) {
      formData.append('video', video);
      await sendFileToWebSocket("USERd5c4bcf9e9bf170b2210d6cccb3025972607c20aff670678496167c68ef6165d:" + video);
    }

    try {
      const dataToSend = {
        formData: formData,
        mot: mot,
      };

      ws.send("unityjf:" + JSON.stringify(dataToSend));

      const downloadLink = document.createElement('a');
      downloadLink.href = URL.createObjectURL(image || video);
      downloadLink.download = image ? image.name : video.name;
      downloadLink.click();

      setInputMot('');
      setFileImage(null);
      setFileVideo(null);
    } catch (error) {
      console.error('Error sending data via WebSocket:', error);
    }
  };

  const send = async (mot) => {
    ws.send(JSON.stringify({ data: mot }));
  };

  return (
    <>
      
      <label>Dessin:</label>
      <CanvasComponent ws={ws} />
      <br />
      <br />
      <form onSubmit={handleSubmit}>
        <label>
          Mot:
          <input type="text" value={mot} onChange={handleInputChange} />
        </label>
        <br />
        <br />
        <label>
          Image:
          <input type="file" accept=".jpg, .png, .heic, .tiff" onChange={handleImageChange} />
        </label>
        <br />
        <br />
        <label>
          Vidéo:
          <input type="file" accept=".mp4, .mv4, .mov" onChange={handleVideoChange} />
        </label>
        <br />
        <br />
        <button onClick={() => send(mot)}>Soumettre</button>
      </form>
    </>
  );
}

export default Export;
