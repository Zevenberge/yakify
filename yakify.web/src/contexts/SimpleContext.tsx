import { createContext, useContext, useState } from "react";

export function createSimpleContext<T>(value: T) {
  return createContext<{
    value: T;
    setValue: (value: T) => void;
  }>({ value, setValue: () => {} });
}

type ContextInstance<T> = ReturnType<typeof createSimpleContext<T>>;

export function SimpleContextProvider<T>(instance: ContextInstance<T>) {
  const Provider = (props: { value: T; children: React.ReactNode }) => {
    const [value, setValue] = useState(props.value);
    return (
      <instance.Provider value={{ value, setValue }}>
        {props.children}
      </instance.Provider>
    );
  };
  return Provider;
}

export function createContextWithHook<T>(initialValue: T) {
  const instance = createSimpleContext<T>(initialValue);
  const Provider = SimpleContextProvider(instance);
  const useValue = () => {
    const { value, setValue } = useContext(instance);
    return [value, setValue] as const;
  };
  return { Provider, useValue };
}
