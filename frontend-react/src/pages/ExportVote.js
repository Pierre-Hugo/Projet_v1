import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import '../styles/ExportVote.css';

function ExportVote({ ws }) {
    const navigate = useNavigate();
    const [selectedPlayer, setSelectedPlayer] = useState(null);
    const [playerResponses, setPlayerResponses] = useState({
        player1: 'Joueur 1',
        player2: 'Joueur 2',
        player3: 'Joueur 3',
        player4: 'Joueur 4'
    });

    const handleCheckboxChange = (player) => {
        setSelectedPlayer(player);
    };

    const handleSubmit = () => {
        // Do something with the selected player
        if (selectedPlayer !== null) {
            // Envoyer le joueur sélectionné au websocket
            try {
                var unityID = localStorage.getItem('UNITY');
                ws.send(unityID + ":VO," + selectedPlayer);
                //navigate('/WaitingState');
            } catch (error) {
                console.error('Error sending data via WebSocket:', error);
            }
        } else {
            // Gérer le cas où aucun joueur n'est sélectionné
            alert("Veuillez sélectionner un joueur avant d'envoyer !");
        }
    };

    useEffect(() => {
        // Mettre à jour les réponses des joueurs à chaque réception de données via WebSocket
        const handleWebSocketMessage = (event) => {
            const dataRecu = event.data;
            const messageComplet = dataRecu.split(":");
            const responseArray = messageComplet[1].split(",");
            const playerNumber = responseArray[0];
            const response = responseArray[1];

            setPlayerResponses(prevResponses => ({
                ...prevResponses,
                [playerNumber]: response
            }));
        };

        ws.onmessage = handleWebSocketMessage;

        return () => {
            ws.onmessage = null;
        };
    }, [ws]);

    return (
        <div className="export-vote-container">
            <h1 className="title">C'est à vous de voter !</h1>
            <div className="players-container">
                <div className="player-group">
                    <label>
                        <input
                            type="checkbox"
                            checked={selectedPlayer === 'player1'}
                            onChange={() => handleCheckboxChange('player1')}
                        />
                        {playerResponses.player1}
                    </label>
                    <label>
                        <input
                            type="checkbox"
                            checked={selectedPlayer === 'player2'}
                            onChange={() => handleCheckboxChange('player2')}
                        />
                        {playerResponses.player2}
                    </label>
                </div>
                <div className="player-group">
                    <label>
                        <input
                            type="checkbox"
                            checked={selectedPlayer === 'player3'}
                            onChange={() => handleCheckboxChange('player3')}
                        />
                        {playerResponses.player3}
                    </label>
                    <label>
                        <input
                            type="checkbox"
                            checked={selectedPlayer === 'player4'}
                            onChange={() => handleCheckboxChange('player4')}
                        />
                        {playerResponses.player4}
                    </label>
                </div>
            </div>
            <button onClick={handleSubmit}>Envoyer</button>
        </div>
    );
}

export default ExportVote;
