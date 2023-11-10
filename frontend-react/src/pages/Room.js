import React, { useState } from 'react';
import { Link } from 'react-router-dom'

function Room({ws}) {

  const [pseudo, setInputPseudo] = useState('');

  const handleInputChange = (e) => {
    setInputPseudo(e.target.value);
  };

  const handleSubmit = async (e) => {
    e.preventDefault(); // Prevent the default form submission behavior

    if (!pseudo) {
      // Handle the case when "mot" is empty, if needed
      return;
    }

    // Create a FormData object to collect form data
    const formData = new FormData();
    formData.append('pseudo', pseudo);

    try {
      // Send the FormData via WebSocket
      const dataToSend = {
        formData: formData,
        pseudo: pseudo,
      };

      ws.send(JSON.stringify(dataToSend));

      // Clear the form after sending
      setInputPseudo('');
    } catch (error) {
      console.error('Error sending data via WebSocket:', error);
    }
  };

    return (
      <>
        <h1>Entrez le numéro de la room</h1>
        <form>
          <label>
            PIN:
            <input type="number" value=""/>
            <br/>
            Pseudo:
            <input type="text" value={pseudo} onChange={handleInputChange}/>
            <br/>
          </label>
          <Link to="/export" onSubmit={handleSubmit}>Connexion</Link>
        </form>
      </>
        );
  
  }
  
  export default Room;