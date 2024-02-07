import { useState } from "react";

export function useNumberInput(initialValue: number) {
  const [value, setValue] = useState(initialValue);
  return [
    value,
    (event: React.ChangeEvent<HTMLInputElement>) =>
      setValue(event.currentTarget.valueAsNumber),
  ] as const;
}

export function useTextInput(initialValue: string) {
  const [value, setValue] = useState(initialValue);
  return [
    value,
    (event: React.ChangeEvent<HTMLInputElement>) =>
      setValue(event.currentTarget.value),
  ] as const;
}
