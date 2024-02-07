import { useState } from "react";
import { useConfiguration } from "./useConfiguration";
export type PostProps<TOut> = {
  url: string;
  onSuccess: (result: TOut, status: number) => void;
  onFailure: (error: string) => void;
};

export function usePost<TIn, TOut = void>(props: PostProps<TOut>) {
  const configuration = useConfiguration();
  const [inProgress, setInProgress] = useState<boolean>(false);

  async function post(input: TIn) {
    setInProgress(true);
    try {
      const result = await fetch(`${configuration?.backEnd}${props.url}`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(input),
      });
      setInProgress(false);
      if (result.ok) {
        props.onSuccess((await result.json()) as TOut, result.status);
      } else {
        props.onFailure(result.statusText);
      }
    } catch (error: any) {
      console.log(error);
      setInProgress(false);
      props.onFailure(error.toString());
    }
  }

  return {
    inProgress,
    post,
  };
}
