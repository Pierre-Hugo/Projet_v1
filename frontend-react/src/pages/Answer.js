import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
//import '../styles/ExportVote.css';

function Answer({ ws }) {
    const navigate = useNavigate();
    const [answerText, setAnswerText] = useState('');

    const handleInputChange = (event) => {
        setAnswerText(event.target.value);
    };

    const handleSubmit = () => {
        var unityID = localStorage.getItem('UNITY');
        ws.send(unityID + ":AN," + answerText); // Envoyer la réponse saisie
        navigate('/OnTV');
    };

    return (
        <div className="export-vote-container"> 
            <div className="canvas-container"> 
                <h2 className="title">Réponse:</h2>
                <input 
                    type="text" 
                    value={answerText} 
                    onChange={handleInputChange} 
                    placeholder="Entrez votre réponse ici..." 
                />
                <button onClick={handleSubmit}>Soumettre</button>
            </div>
        </div>
    );
}

export default Answer;
