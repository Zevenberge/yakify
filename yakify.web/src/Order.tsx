import { GetResult } from "./hooks/useGet";
import { StockDto, useStock } from "./hooks/useStock";

export default function Order() {
  const stock = useStock();

  return <>
    <Stock stock={stock}/>
  </>
}

function Stock(props: { stock: GetResult<StockDto> }) {
  if (props.stock.success === null) {
    return (
      <StockCard>
        <p>Milking the yaks...</p>
      </StockCard>
    );
  }
  if(!props.stock.success) {
    return <StockCard>
        <p>The yaks ran away! An error occured.</p>
    </StockCard>
  }
  return (
    <StockCard>
      <dl className="Order-list">
        <dt>Milk</dt>
        <dd>{props.stock.result.milk}</dd>
        <dt>Milk</dt>
        <dd>{props.stock.result.milk}</dd>
      </dl>
    </StockCard>
  );
}

function StockCard(props: { children: React.ReactNode }) {
  return (
    <div className="card">
      <h2>Current stock</h2>
      {props.children}
    </div>
  );
}
