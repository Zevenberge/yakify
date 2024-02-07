import { useEffect } from "react";
import { useConfiguration } from "./useConfiguration";
import { HubConnectionBuilder } from "@microsoft/signalr";

export function useLiveUpdates(props: { url: string; reload: () => void }) {
    const configuration = useConfiguration();
    useEffect(() => {
        const connection = new HubConnectionBuilder()
            .withUrl(`${configuration?.backEnd}${props.url}`)
            .build();
        connection.on("changed", props.reload);
        connection.start().catch(console.error);
        return () => {
            connection.stop();
        };
    }, [configuration?.backEnd, props.url]);
}
