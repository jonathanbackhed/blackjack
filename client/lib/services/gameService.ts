import { ActionRequestDto } from "@/types/actionRequestDto";
import { PlayerAction } from "@/types/playerAction";
import { HubConnection } from "@microsoft/signalr";
import router from "next/router";

export const joinGame = async (
  connection: HubConnection | null,
  isConnected: boolean,
  serverId: string,
  username: string,
) => {
  if (connection && isConnected && serverId) {
    try {
      await connection.invoke("JoinServer", serverId, username);
      console.log("Invoked JoinServer");
    } catch (err) {
      console.error("Error invoking JoinServer: ", err);
    }
  } else {
    console.warn("Connection not ready", connection);
  }
};

export const leaveGame = async (
  connection: HubConnection | null,
  isConnected: boolean,
  serverId: string,
): Promise<boolean> => {
  if (connection && isConnected && serverId) {
    try {
      await connection.invoke("LeaveServer", serverId);
      console.log("Invoked LeaveServer");
      await connection.stop();
      return true;
    } catch (err) {
      console.error("Error invoking LeaveServer: ", err);
    }
  }
  return false;
};

export const startGame = async (connection: HubConnection | null, isConnected: boolean, serverId: string) => {
  if (connection && isConnected && serverId) {
    try {
      await connection.invoke("StartGame", serverId);
      console.log("Invoked StartGame");
    } catch (err) {
      console.error("Error invoking StartGame: ", err);
    }
  } else {
    console.warn("Connection or serverId not ready", connection, serverId);
    console.warn("Connection:", connection);
    console.warn("ServerId:", serverId);
  }
};

export const PerformAction = async (
  connection: HubConnection | null,
  isConnected: boolean,
  serverId: string,
  playerId: string,
  action: PlayerAction,
) => {
  if (connection && isConnected && serverId && playerId) {
    try {
      const newAction: ActionRequestDto = {
        serverId: serverId,
        playerId: playerId,
        action: action,
      };
      await connection.invoke("PerformAction", newAction);
      console.log("Invoked PerformAction");
    } catch (err) {
      console.error("Error invoking PerformAction: ", err);
    }
  } else {
    console.warn("Connection or serverId not ready", connection, serverId);
    console.warn("Connection:", connection);
    console.warn("ServerId:", serverId);
  }
};
