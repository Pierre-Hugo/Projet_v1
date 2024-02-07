import React, { useState } from 'react';

function ExportVideo({ ws }) {
  const [videoChunks, setVideoChunks] = useState([]);

  const handleVideoChange = async (e) => {
    const video = e.target.files[0];

    if (video) {
      setVideoChunks([]);
      const chunkSize = 400 * 1024; // 400 KB
      let offset = 0;

      while (offset < video.size) {
        const chunk = video.slice(offset, offset + chunkSize);
        setVideoChunks((prevChunks) => [...prevChunks, chunk]);
        offset += chunkSize;
      }

      const dataToSend = {
        fileName: video.name,
        fileSize: video.size,
        fileType: video.type,
      };
      const jsonData = JSON.stringify(dataToSend);
      displayVideoInConsole(video);
      await sendVideoChunks(jsonData);
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

  const sendVideoChunks = async (jsonData) => {
    ws.send(JSON.stringify({ data: jsonData }));

    // Send each video chunk
    for (const chunk of videoChunks) {
      const reader = new FileReader();
      reader.onload = () => {
        const arrayBuffer = reader.result;
        ws.send(arrayBuffer);
      };
      reader.readAsArrayBuffer(chunk);
    }
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
