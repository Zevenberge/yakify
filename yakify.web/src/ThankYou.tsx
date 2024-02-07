import { useNavigate } from "react-router-dom";
import { useLastOrder } from "./contexts/LastOrderContext";
import { useEffect } from "react";

export default function ThankYou() {
  const navigate = useNavigate();
  const [lastOrder] = useLastOrder();
  useEffect(() => {
    if (!lastOrder) navigate("/");
  }, [lastOrder, navigate]);
  if (!lastOrder) {
    return <></>;
  }
  return (
    <div className="card">
      <h2>Thank you for your support</h2>
      <p>You have bought:</p>
      {!!lastOrder.milk && <p>{lastOrder.milk} litres of milk</p>}
      {!!lastOrder.skins && <p>{lastOrder.skins} yak skins</p>}
      {!lastOrder.milk && !lastOrder.skins && (
        <p>nothing! But we still love you.</p>
      )}
      {lastOrder.status === 201 && <p>which is all you could have ever wanted.</p>}
      {lastOrder.status === 206 && <p>Unfortunately this is all we have.</p>}
      {lastOrder.status === 404 && <p>We could not meet your demands.</p>}
    </div>
  );
}
