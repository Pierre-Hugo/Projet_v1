import React, { useState } from 'react';
import { Link } from 'react-router-dom';

function Room({ ws }) {

  const [pin, setPin] = useState('');
  const [pseudo, setPseudo] = useState('');

  const handlePinChange = (e) => {
    setPin(e.target.value);
  };

  const handlePseudoChange = (e) => {
    setPseudo(e.target.value);
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    if (!pin || !pseudo || !ws) {
      console.error('PIN, pseudo ou connexion WebSocket manquants.');
      return;
    }

    try {
      const dataToSend = {
        pin: pin,
        pseudo: pseudo,
      };

      ws.send(JSON.stringify(dataToSend));

      setPin('');
      setPseudo('');
    } catch (error) {
      console.error('Erreur lors de l\'envoi des données via WebSocket :', error);
    }
  };

 const send = () => {
    if (pin && pseudo && ws) {
      const dataToSend = {
        pin: pin,
        pseudo: pseudo,
      };
      ws.send(JSON.stringify(dataToSend));
    }
  };

  return (
    <>
      <h1>Entrez le numéro de la room</h1>
      <form onSubmit={handleSubmit}>
        <label>
          PIN:
          <input type="number" value={pin} onChange={handlePinChange} />
          <br />
          Pseudo:
          <input type="text" value={pseudo} onChange={handlePseudoChange} />
          <br />
        </label>
        <Link to="/export" onClick={send}>Connexion</Link>
      </form>
    </>
  );
}

export default Room;

