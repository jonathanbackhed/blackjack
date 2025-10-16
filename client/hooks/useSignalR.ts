import { useState, useEffect } from "react";
import { createSignalRClient } from "@/lib/signalRClient";
import { HubConnection, HubConnectionState } from "@microsoft/signalr";
import { GameResponseDto } from "@/types/gameResponseDto";

export function useSignalR(onMessage: (msg: GameResponseDto) => void) {
  const [connection, setConnection] = useState<HubConnection | null>(null);
  const [isConnected, setIsConnected] = useState(false);

  const connect = async () => {
    if (connection && connection.state === HubConnectionState.Disconnected) {
      try {
        await connection.start();
        setIsConnected(true);
      } catch (error) {
        console.error("SignalR connection failed:", error);
        setIsConnected(false);
      }
    }
  };

  useEffect(() => {
    const conn = createSignalRClient();
    setConnection(conn);

    conn.on("GameUpdate", onMessage);

    return () => {
      conn.off("GameUpdate", onMessage);
      conn.stop();
    };
  }, []);

  useEffect(() => {
    if (connection && connection.state === HubConnectionState.Disconnected) {
      connect();
    }
  }, [connection]);

  return { connection, isConnected };
}
