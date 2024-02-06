import logo from './logo.svg';
import './App.css';
import Header from './Header';

function App() {
  return (
    <div className="App">
      <Header/>
      <div className="App-page">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.tsx</code> and save to reload.
        </p>
        <a
          className="link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </div>
    </div>
  );
}

export default App;
