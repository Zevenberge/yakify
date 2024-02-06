import { createContext, useContext, useState } from "react";

const DateContext = createContext<{
  value: number;
  setValue: (value: number) => void;
}>({ value: 0, setValue: () => {} });

export function DateContextProvider(props: {
  value: number;
  children: React.ReactNode;
}) {
  const [value, setValue] = useState(props.value);
  return (
    <DateContext.Provider value={{ value, setValue }}>
      {props.children}
    </DateContext.Provider>
  );
}

export function useDate(): [number, (newValue: number) => void] {
  const context = useContext(DateContext);
  return [context.value, context.setValue];
}
