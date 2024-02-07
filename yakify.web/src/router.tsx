import {
  Route,
  createBrowserRouter,
  createRoutesFromElements,
} from "react-router-dom";
import App from "./App";
import Error from "./Error";
import Order from "./Order";
import Home from "./Home";

export default createBrowserRouter(
  createRoutesFromElements(
    <Route path="/" element={<App />} errorElement={<Error />}>
      <Route index element={<Home/>}/> 
      <Route path="order" element={<Order/>}/>
    </Route>
  )
);
