import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

function ExportMot({ ws }) {
  const [mot, setInputMot] = useState('');
  const navigate = useNavigate();

  const handleInputChange = (e) => {
    setInputMot(e.target.value);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!mot) {
      return;
    }

    try {

      var unityID = localStorage.getItem('UNITY');
      var pseudo = localStorage.getItem('pseudo');
      var pseudoColor = localStorage.getItem('pseudoColor');

      ws.send(unityID + ": NP," + pseudo + "," + pseudoColor + ",WRD," + mot);
      navigate('/WaitingState');


      setInputMot('');
    } catch (error) {
      console.error('Error sending data via WebSocket:', error);
    }
  };

  return (
    <>
      <h1>Inscrivez un Mot!</h1>
      <form onSubmit={handleSubmit}>
        Mot:
        <input type="text" value={mot} onChange={handleInputChange} />
        <button type="submit">Soumettre</button>
      </form>
    </>
  );
}

export default ExportMot;
