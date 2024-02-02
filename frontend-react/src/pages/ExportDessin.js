//import React, { useState } from 'react';
//import { Link } from 'react-router-dom';
import CanvasComponent from '../components/Canvas';

function ExportDessin({ ws }) {
    return(
      <>
        <h1>Dessinez ce qu'il vous plaît!</h1>
        <CanvasComponent ws={ws} />
      </>
    );
}

export default ExportDessin;