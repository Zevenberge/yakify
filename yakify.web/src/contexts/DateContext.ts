import { createContextWithHook } from "./SimpleContext";

export const { Provider: DateContextProvider, useValue: useDate } =
  createContextWithHook<number>(0);
