import { useDate } from "../contexts/DateContext";
import { useGet } from "./useGet";

export type StockDto = { milk: number; skins: number };

export function useStock() {
  const [date] = useDate();
  const stock = useGet<StockDto>(`/yak-shop/stock/${date}`);
  return stock;
}
