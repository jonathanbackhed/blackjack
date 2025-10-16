import { HubConnectionBuilder, HubConnection } from "@microsoft/signalr";

let connection: HubConnection | null = null;

export function createSignalRClient() {
  if (!connection) {
    connection = new HubConnectionBuilder()
      .withUrl(`${process.env.NEXT_PUBLIC_SERVER_URL}/gamehub`)
      .withAutomaticReconnect()
      .build();
  }

  return connection;
}
