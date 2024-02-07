import { useState } from "react";

export function useNumberInput(initialValue: number) {
  const [value, setValue] = useState(initialValue);
  return [
    value,
    (event: React.ChangeEvent<HTMLInputElement>) =>
      setValue(event.currentTarget.valueAsNumber),
  ] as const;
}
