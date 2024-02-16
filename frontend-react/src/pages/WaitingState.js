import React from 'react';
import '../styles/WaitingRoom.css';

function WaitingRoom() {
  return (
    <div className="container">
      <h1 className="title">En attente des joueurs<span className="dots"></span></h1>
      <div className="loader"></div>
    </div>
  );
}

export default WaitingRoom;
