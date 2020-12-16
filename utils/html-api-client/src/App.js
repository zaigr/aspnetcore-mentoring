import logo from './logo.svg';
import './App.css';
import Product from './Product';

function App() {

  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Northwind API client
        </p>
        <Product />
      </header>
    </div>
  );
}

export default App;
