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

    localStorage.setItem('pseudo', pseudo);
    

    if (!pin || !pseudo || !ws) {
      console.error('PIN, pseudo ou connexion WebSocket manquants.');
      return;
    }

    try {
      var ROOM = "UNITY" + pin
      //var pseu = localStorage.getItem('pseudo');

      ws.send(ROOM + ":CHECK")

      if(true){
        localStorage.setItem('UNITY', ROOM);
      }

      //ws.send(ROOM + ":" + pseu)

      //ws.send("unityjf:" +  ("NP", "pseudo", "BLUE", "pic", "URLDATA/reponse", "false"));

      setPin('');
      setPseudo('');
    } catch (error) {
      console.error('Erreur lors de l\'envoi des données via WebSocket :', error);
    }
  };

  const send = () => {
    handleSubmit({ preventDefault: () => {} }); // Appel à la fonction handleSubmit pour envoyer les données au WebSocket
  };

  return (
    <>
      <h1>Entrez le numéro de la room</h1>
      <form onSubmit={handleSubmit}>
        <label>
          PIN:
          <input type="text" value={pin} maxLength="4" onChange={handlePinChange} />
          <br />
          Pseudo:
          <input type="text" value={pseudo} maxLength="16" onChange={handlePseudoChange} />
          <br />
        </label>
        <Link to="/mediaSelect" onClick={send}>Connexion</Link>
      </form>
    </>
  );
}

export default Room;
