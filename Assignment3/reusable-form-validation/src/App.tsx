import { ReusableForm } from "./components/ReusableForm";
import "./App.css";

function App() {
  return (
    <>
      <header className="app-header">
        <div className="app-header-logo" />
        <span className="app-header-title">sana commerce</span>
      </header>
      <main className="app-container">
        <ReusableForm />
      </main>
    </>
  );
}

export default App;
