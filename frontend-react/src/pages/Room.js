import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

function Room({ ws }) {
  const [pin, setPin] = useState('');
  const [pseudo, setPseudo] = useState('');
  const [pinError, setPinError] = useState('');
  const [pseudoError, setPseudoError] = useState('');
  const [selectedColor, setSelectedColor] = useState('');
  const navigate = useNavigate();

  const handlePinChange = (e) => {
    let value = e.target.value.toUpperCase(); // Convertir en majuscules
    const alphanumericRegex = /^[a-zA-Z0-9]*$/; // Expression régulière pour vérifier l'alphanumérique

    // Limiter la longueur du PIN à 4 caractères
    if (value.length > 4) {
      value = value.slice(0, 4);
    }

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

  const handleColorChange = (e) => {
    setSelectedColor(e.target.value);
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
    
      ws.onmessage = function(event) {
        var message = event.data;
        console.log(message)
        if (message === ROOM + ":YES") {
          localStorage.setItem('UNITY', ROOM);
          localStorage.setItem('pseudoColor', selectedColor);
          ws.onmessage = null;
          navigate("/mediaSelect");
        }
      };

    } catch (error) {
      console.error('Erreur lors de l\'envoi des données via WebSocket :', error);
    }
    
  };

  const isButtonDisabled = !pin || !pseudo || pin.length !== 4 || !pseudo.trim() || !selectedColor;

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
          Couleur du pseudo:
          <div>
            <label>
              <input type="radio" name="color" value="BLUE" checked={selectedColor === 'BLUE'} onChange={handleColorChange} />
              Bleu
            </label>
            <br />
            <label>
              <input type="radio" name="color" value="RED" checked={selectedColor === 'RED'} onChange={handleColorChange} />
              Rouge
            </label>
            <br />
            <label>
              <input type="radio" name="color" value="WHITE" checked={selectedColor === 'WHITE'} onChange={handleColorChange} />
              Blanc
            </label>
            <br />
            <label>
              <input type="radio" name="color" value="BLACK" checked={selectedColor === 'BLACK'} onChange={handleColorChange} />
              Noir
            </label>
            <br />
            <label>
              <input type="radio" name="color" value="CYAN" checked={selectedColor === 'CYAN'} onChange={handleColorChange} />
              Cyan
            </label>
            <br />
            <label>
              <input type="radio" name="color" value="GRAY" checked={selectedColor === 'GRAY'} onChange={handleColorChange} />
              Gris
            </label>
            <br />
            <label>
              <input type="radio" name="color" value="GREEN" checked={selectedColor === 'GREEN'} onChange={handleColorChange} />
              Vert
            </label>
            <br />
            <label>
              <input type="radio" name="color" value="MAGENTA" checked={selectedColor === 'MAGENTA'} onChange={handleColorChange} />
              Magenta
            </label>
            <br />
            <label>
              <input type="radio" name="color" value="YELLOW" checked={selectedColor === 'YELLOW'} onChange={handleColorChange} />
              Jaune
            </label>
          </div>
          <br />
        </label>
        <button type="submit" disabled={isButtonDisabled}>Connexion</button>
      </form>
    </>
  );
}

export default Room;
