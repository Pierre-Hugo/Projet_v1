import React from 'react';
import { Link } from 'react-router-dom';

const buttonStyle = {
  display: 'inline-block',
  padding: '10px 20px',
  backgroundColor: '#007bff',
  color: '#fff',
  textDecoration: 'none',
  borderRadius: '5px',
  margin: '5px',
  cursor: 'pointer',
};

function MediaSelect() {
  return (
    <>
      <h1>Choisissez un type de média pour commencer la partie !</h1>
      <div>
        <Link to="/exportMot" style={buttonStyle}>
          Mot
        </Link>
        <Link to="/exportImage" style={buttonStyle}>
          Image
        </Link>
        <Link to="/exportVideo" style={buttonStyle}>
          Video
        </Link>
        <Link to="/exportDessin" style={buttonStyle}>
          Dessin
        </Link>
        <Link to="/exportVote" style={buttonStyle}>
          Vote
        </Link>
      </div>
    </>
  );
}

export default MediaSelect;
