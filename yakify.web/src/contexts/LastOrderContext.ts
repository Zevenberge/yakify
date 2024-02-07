import { createContextWithHook } from "./SimpleContext";
import { OrderDto } from "../hooks/useOrder";

type LastOrder = Partial<OrderDto> & { status: number };

export const { Provider: LastOrderContextProvider, useValue: useLastOrder } =
  createContextWithHook<LastOrder | null>(null);
