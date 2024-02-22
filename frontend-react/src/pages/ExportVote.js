import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
//import '../styles/ExportVote.css';

function ExportVote({ ws }) {
    const navigate = useNavigate();
    const [selectedPlayer, setSelectedPlayer] = useState(null);
    const [playerResponses, setPlayerResponses] = useState({});

    const handleCheckboxChange = (player) => {
        setSelectedPlayer(player);
    };

    const handleSubmit = () => {
        if (selectedPlayer !== null) {
            try {
                var unityID = localStorage.getItem('UNITY');
                ws.send(unityID + ":VO," + selectedPlayer);
                navigate('/onTV');
            } catch (error) {
                console.error('Error sending data via WebSocket:', error);
            }
        } else {
            alert("Veuillez sélectionner un joueur avant d'envoyer !");
        }
    };

    useEffect(() => {
        const handleWebSocketMessage = (event) => {
            const dataRecu = event.data;
            const messageComplet = dataRecu.split(":");
            const responseArray = messageComplet[1].split(",");
            
            for (let i = 0; i < responseArray.length; i += 2) {
                const player = responseArray[i];
                const response = responseArray[i + 1];
                
                setPlayerResponses(prevResponses => ({
                    ...prevResponses,
                    [player]: response
                }));
            }
        };

        ws.onmessage = handleWebSocketMessage;

        return () => {
            ws.onmessage = null;
        };
    }, [ws]);

    const checkboxes = Object.keys(playerResponses).map((player, index) => (
        <label key={player}>
            <input
                type="checkbox"
                checked={selectedPlayer === player}
                onChange={() => handleCheckboxChange(player)}
            />
            {playerResponses[player]}
        </label>
    ));

    return (
        <div className="export-vote-container">
            <h1 className="title">C'est à vous de voter !</h1>
            <div className="players-container">
                {checkboxes}
            </div>
            <button onClick={handleSubmit}>Envoyer</button>
        </div>
    );
}

export default ExportVote;
