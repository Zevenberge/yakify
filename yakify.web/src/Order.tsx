import { GetResult } from "./hooks/useGet";
import { StockDto, useStock } from "./hooks/useStock";
import "./Order.css";
import { useNumberInput } from "./hooks/useInput";

export default function Order() {
  const stock = useStock();

  return (
    <div className="Order">
      <Stock stock={stock} />
      <OrderForm />
    </div>
  );
}

function Stock(props: { stock: GetResult<StockDto> }) {
  if (props.stock.success === null) {
    return (
      <StockCard>
        <p>Milking the yaks...</p>
      </StockCard>
    );
  }
  if (!props.stock.success) {
    return (
      <StockCard>
        <p>The yaks ran away! An error occured.</p>
      </StockCard>
    );
  }
  return (
    <StockCard>
      <dl className="Order-list">
        <dt>Milk</dt>
        <dd>{props.stock.result.milk.toFixed()}</dd>
        <dt>Skins</dt>
        <dd>{props.stock.result.skins.toFixed()}</dd>
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

function OrderForm() {
  const [milk, setMilk] = useNumberInput(0);
  const [skins, setSkins] = useNumberInput(0);

  async function submit(
    event: React.FormEvent<HTMLFormElement>
  ) {
    event.preventDefault();
    event.stopPropagation();
  }

  return (
    <div className="card">
      <h2>Place order</h2>
      <form className="Order-list" onSubmit={submit}>
        <label htmlFor="milk">Milk (l)</label>
        <input
          id="milk"
          type="number"
          step={1}
          min={0}
          required
          value={milk}
          onChange={setMilk}
        ></input>
        <label htmlFor="skins">Skins</label>
        <input
          id="skins"
          type="number"
          step={1}
          min={0}
          required
          value={skins}
          onChange={setSkins}
        ></input>
        <input type="submit" value="Go!" className="button primary"></input>
      </form>
    </div>
  );
}
