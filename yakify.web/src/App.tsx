import "./App.css";
import Header from "./Header";
import { Outlet } from "react-router-dom";

function App() {
  return (
    <div className="App">
      <Header />
      <div className="App-page">
        <Outlet />
      </div>
    </div>
  );
}

export default App;
