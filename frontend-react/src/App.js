import { BrowserRouter, Routes, Route } from "react-router-dom";


import Room from "./pages/Room";
import Export from "./pages/Export";

function App({ws}) {

  return (
    <div className="App">
      <BrowserRouter>
      <Routes>
          <Route path="/" element={<Room />} />
          <Route path="/export" element={<Export ws = {ws} />} />
        </Routes>
      </BrowserRouter>
    </div>
      );

}

export default App;
