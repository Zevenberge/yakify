import Yak from "./icons/Yak";
import './Header.css';
import DatePicker from "./DatePicker";

export default function Header() {
  return (
    <header className="Header-main">
      <a className="Header-link" href="/">
        <Yak />
        <span>Yakify</span>
        <span className="visually-hidden">Go to homepage</span>
      </a>
      <DatePicker/>
    </header>
  );
}
