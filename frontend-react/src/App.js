import React, { useEffect, useState } from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';

import Room from './pages/Room';
import Export from './pages/Export';
import MediaSelect from './pages/MediaSelect';
import ExportImage from './pages/ExportImage';
import ExportMot from './pages/ExportMot';
import ExportVideo from './pages/ExportVideo';
import ExportDessin from './pages/ExportDessin';
import WaitingState from './pages/WaitingState';
import OnTv from './pages/OnTV';
import Answer from './pages/Answer';
import ExportVote from './pages/ExportVote';

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
          <Route path="/mediaSelect" element={<MediaSelect ws={ws} />} />
          <Route path="/exportImage" element={<ExportImage ws={ws} />} />
          <Route path="/exportMot" element={<ExportMot ws={ws} />} />
          <Route path="/exportVideo" element={<ExportVideo ws={ws} />} />
          <Route path="/exportDessin" element={<ExportDessin ws={ws} />} />
          <Route path="/waitingstate" element={<WaitingState ws={ws} />} />
          <Route path="/exportVote" element={<ExportVote ws={ws} />} />
          <Route path="/onTV" element={<OnTv ws={ws} />} />
          <Route path="/answer" element={<Answer ws={ws} />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
