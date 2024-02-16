import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

function Room({ ws }) {
  const [pin, setPin] = useState('');
  const [pseudo, setPseudo] = useState('');
  const [pinError, setPinError] = useState('');
  const [pseudoError, setPseudoError] = useState('');
  const navigate = useNavigate();

  const handlePinChange = (e) => {
    const value = e.target.value.toUpperCase(); // Convertir en majuscules
    const alphanumericRegex = /^[a-zA-Z0-9]*$/; // Expression régulière pour vérifier l'alphanumérique

    if (!alphanumericRegex.test(value)) {
      setPinError('Le PIN doit contenir uniquement des lettres et des chiffres.');
    } else if (value.length !== 4) {
      setPinError('Le PIN doit contenir exactement 4 caractères.');
    } else {
      setPinError('');
    }
    setPin(value);
  };

  const handlePseudoChange = (e) => {
    const value = e.target.value;
    const lettersRegex = /^[a-zA-Z]*$/; // Expression régulière pour vérifier les lettres uniquement

    if (!lettersRegex.test(value)) {
      setPseudoError('Le pseudo doit contenir uniquement des lettres.');
    } else if (!value.trim()) {
      setPseudoError('Veuillez entrer un pseudo.');
    } else {
      setPseudoError('');
    }
    setPseudo(value);
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    localStorage.setItem('pseudo', pseudo);

    if (!pin || !pseudo || !ws || pin.length !== 4 || !pseudo.trim()) {
      console.error('PIN, pseudo ou connexion WebSocket manquants.');
      return;
    }

    try {
      const ROOM = "UNITY" + pin;
      ws.send(ROOM + ":CHECK");

      if (true) {
        localStorage.setItem('UNITY', ROOM);
        navigate("/mediaSelect"); // Naviguer vers la nouvelle route après validation
      }

      setPin('');
      setPseudo('');
    } catch (error) {
      console.error('Erreur lors de l\'envoi des données via WebSocket :', error);
    }
  };

  const isButtonDisabled = !pin || !pseudo || pin.length !== 4 || !pseudo.trim();

  return (
    <>
      <h1>Entrez le numéro de la room</h1>
      <form onSubmit={handleSubmit}>
        <label>
          PIN:
          <input type="text" value={pin} maxLength="4" onChange={handlePinChange} required />
          <br />
          {pinError && <span style={{ color: 'red' }}>{pinError}</span>}
          <br />
          Pseudo:
          <input type="text" value={pseudo} maxLength="16" onChange={handlePseudoChange} required />
          <br />
          {pseudoError && <span style={{ color: 'red' }}>{pseudoError}</span>}
          <br />
        </label>
        <button type="submit" disabled={isButtonDisabled}>Connexion</button>
      </form>
    </>
  );
}

export default Room;