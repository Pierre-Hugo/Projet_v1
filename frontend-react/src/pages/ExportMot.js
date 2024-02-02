import React, { useState } from 'react';

function ExportMot({ ws }) {
  const [mot, setInputMot] = useState('');

  const handleInputChange = (e) => {
    setInputMot(e.target.value);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!mot) {
      return;
    }

    try {
      const dataToSend = {
        formData: null,  // On n'envoie pas de fichiers dans ce cas
        mot: mot,
      };

      ws.send("unityjf:" + JSON.stringify(dataToSend));

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
