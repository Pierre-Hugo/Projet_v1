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

      var unityID = localStorage.getItem('UNITY');
      var pseudo = localStorage.getItem('pseudo');

      ws.send(unityID + ": NP, " + pseudo + ", BLUE, WRD, " + mot);

      //ws.send("unityjf:" +  ("NP", "pseudo", "BLUE", "pic", "URLDATA/reponse", "false"));

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
