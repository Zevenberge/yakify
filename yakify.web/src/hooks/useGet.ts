import { useEffect, useState } from "react";
import { useConfiguration } from "./useConfiguration";

type FetchResult<T> = { success: false | null } | { success: true; result: T };

export type GetResult<T> = {
  reload: () => void;
} & FetchResult<T>;

export function useGet<T>(url: string): GetResult<T> {
  const configuration = useConfiguration();
  const [counter, setCounter] = useState(0);
  const [result, setResult] = useState<FetchResult<T>>({ success: null });

  useEffect(() => {
    async function getData() {
      try {
        const response = await fetch(`${configuration?.backEnd}${url}`);
        if (response.ok) {
          const body = (await response.json()) as T;
          setResult({ success: true, result: body });
        } else {
          setResult({ success: false });
        }
      } catch (error) {
        console.error(error);
        setResult({ success: false });
      }
    }
    if (!configuration) return;
    getData();
  }, [url, counter, configuration]);

  return {
    reload: () => setCounter(counter + 1),
    ...result,
  };
}
