import { useDate } from "../contexts/DateContext";
import { useGet } from "./useGet";
import { useLiveUpdates } from "./useLiveUpdates";

export type StockDto = { milk: number; skins: number };

export function useStock() {
  const [date] = useDate();
  const stock = useGet<StockDto>(`/yak-shop/stock/${date}`);
  useLiveUpdates({
    url: "/ws/stock",
    reload: stock.reload,
  });
  return stock;
}
