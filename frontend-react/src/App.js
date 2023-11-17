import React, { useEffect } from 'react';
import { BrowserRouter, Routes, Route } from "react-router-dom";



import Room from "./pages/Room";
import Export from "./pages/Export";

function App({ws}) {


  //const userID = generateRandomCode(8);

  useEffect(() => {
    const userID = generateRandomCode(8);
    if (ws && userID) {
      ws.send("USER" + userID);
    }
  }, [ws]);

  
  function generateRandomCode(length) {
    const characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    let code = "";
  
    for (let i = 0; i < length; i++) {
      const randomIndex = Math.floor(Math.random() * characters.length);
      code += characters.charAt(randomIndex);
    }
  
    return code;
  }

  //ws.send("USER" + userID);

  return (
    <div className="App">
      <BrowserRouter>
      <Routes>
          <Route path="/" element={<Room ws = {ws} />} />
          <Route path="/export" element={<Export ws = {ws} />} />
        </Routes>
      </BrowserRouter>
    </div>
      );

}

export default App;
