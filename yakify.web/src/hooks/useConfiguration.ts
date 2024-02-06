import { useEffect, useState } from "react"

type Configuration = {
    backEnd: string;
}

export function useConfiguration() {
    const [configuration, setConfiguration] = useState<Configuration | null>(null);
    useEffect(() => {
        async function getConfiguration() {
            const result = await fetch("/appsettings.json");
            if(result.ok) {
                setConfiguration(await result.json());
            }
        }
        getConfiguration();
    }, []);
    return configuration;
}