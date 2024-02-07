import Yak from "./icons/Yak";
import './Header.css';
import DatePicker from "./DatePicker";
import { Link } from "react-router-dom";

export default function Header() {
  return (
    <header className="Header-main">
      <Link to="/" className="Header-link">
        <Yak />
        <span>Yakify</span>
        <span className="visually-hidden">Go to homepage</span>
      </Link>
      <DatePicker/>
    </header>
  );
}
