import logo from "./logo.svg";
import "./App.css";
import Header from "./Header";
import Home from "./Home";
import { Outlet } from "react-router-dom";

function App() {
  return (
    <div className="App">
      <Header />
      <div className="App-page">
        <Home />
        <Outlet />
      </div>
    </div>
  );
}

export default App;
