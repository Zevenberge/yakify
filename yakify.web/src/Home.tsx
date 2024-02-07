import { Link } from "react-router-dom";
import { Order } from "./icons/Order";
import "./Home.css";

export default function Home() {
    return <div className="Home">
        <div className="Home card">
            <Link to={"/order"}>
                <Order/>
                <span>Buy something</span>
            </Link>
        </div>
    </div>
}