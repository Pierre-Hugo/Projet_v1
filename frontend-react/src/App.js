import React, { useEffect, useState } from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';

import Room from './pages/Room';
import Export from './pages/Export';

function App({ ws }) {
  const [userCreated, setUserCreated] = useState(false);
  const [usersList, setUsersList] = useState([]);
  const [isConnectionReady, setIsConnectionReady] = useState(false);

  useEffect(() => {
    ws.onopen = () => {
      setIsConnectionReady(true);
    };

    ws.onclose = () => {
      setIsConnectionReady(false);
    };
    
  }, [ws]);

  useEffect(() => {
    if (isConnectionReady && !userCreated) {
      let userID = localStorage.getItem('userID');

      if (!userID) {
        userID = generateRandomCode(8);
        localStorage.setItem('userID', userID);
      }

      if (ws && userID && !usersList.includes(userID)) {
        ws.send('USER' + userID);
        setUserCreated(true);
        setUsersList([...usersList, userID]);
      }
    }
  }, [isConnectionReady, userCreated, usersList, ws]);

  function generateRandomCode(length) {
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
    let code = '';

    for (let i = 0; i < length; i++) {
      const randomIndex = Math.floor(Math.random() * characters.length);
      code += characters.charAt(randomIndex);
    }

    return code;
  }

  return (
    <div className="App">
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Room ws={ws} />} />
          <Route path="/export" element={<Export ws={ws} />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
