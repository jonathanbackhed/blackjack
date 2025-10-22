"use client";

import { useSettingsStore } from "@/hooks/useSettingsStore";
import { useEffect, useState } from "react";
import { Api } from "@/lib/apiClient";
import { useRouter } from "next/navigation";
import { useSignalR } from "@/hooks/useSignalR";
import { ArrowBigLeft } from "lucide-react";
import { GameResponseDto } from "@/types/gameResponseDto";
import { PlayerAction } from "@/types/playerAction";
import { ActionRequestDto } from "@/types/actionRequestDto";

interface Props {
  id: string;
}

export default function GameBoard({ id }: Props) {
  const [serverId, setServerId] = useState<string | null>(null);
  const [serverState, setServerState] = useState<GameResponseDto | null>(null);

  const { connection, isConnected, playerId } = useSignalR((msg: GameResponseDto) => {
    console.log("Game update received:", msg);
    setServerState(msg);
  });
  const { username } = useSettingsStore();
  const router = useRouter();

  const getServerId = async () => {
    const sid = await Api.getServerIdFromCode(id);
    if (!sid) {
      return router.replace("/");
    }
    setServerId(sid.serverId);
    return sid.serverId;
  };

  const joinGame = async (sid: string) => {
    if (connection && isConnected && sid) {
      try {
        await connection.invoke("JoinServer", sid, username);
        console.log("Invoked JoinServer");
      } catch (err) {
        console.error("Error invoking JoinServer: ", err);
      }
    } else {
      console.warn("Connection or serverId not ready", connection, serverId);
    }
  };

  const leaveGame = async () => {
    if (connection && isConnected && serverId) {
      try {
        await connection.invoke("LeaveServer", serverId);
        console.log("Invoked LeaveServer");
        await connection.stop();
        router.replace("/");
      } catch (err) {
        console.error("Error invoking LeaveServer: ", err);
      }
    }
  };

  const startGame = async () => {
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

  const PerformAction = async (action: PlayerAction) => {
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

  useEffect(() => {
    if (isConnected) {
      (async () => {
        const sid = await getServerId();
        if (sid) {
          await joinGame(sid);
        }
      })();
    }
  }, [isConnected]);

  return (
    <div className="flex h-[1000px] w-[1400px] flex-col items-center justify-around bg-red-400">
      <h1 className="text-t text-center font-mono text-6xl">Game {id}</h1>
      <button onClick={startGame} className="bg-b hover:bg-b-light rounded-2xl px-3 py-2 text-4xl">
        Start game
      </button>
      {/* GAMEBOARD */}
      <div className="h-[600px] w-full rounded-full bg-neutral-700">
        <div className="grid h-full w-full grid-cols-5 grid-rows-5 gap-4 bg-blue-500/50">
          <div className="col-span-3 col-start-2 row-span-2 flex flex-1 items-center justify-center bg-fuchsia-500/40">
            DEALER
          </div>

          <div className="col-span-3 col-start-2 row-start-3 flex items-center justify-center bg-red-500/30 text-xl font-bold">
            Lorem ipsum dolor
          </div>

          <div className="col-start-2 row-span-2 flex items-center justify-center bg-green-500/40">PLAYER 3</div>
          <div className="col-start-3 row-span-2 flex items-center justify-center bg-green-500/40">PLAYER 2</div>
          <div className="col-start-4 row-span-2 flex items-center justify-center bg-green-500/40">PLAYER 1</div>
        </div>
      </div>

      <div className="relative flex w-full items-center justify-center space-x-4">
        {isConnected && serverId && (
          <button
            onClick={leaveGame}
            className="hover:bg-b-light absolute left-2 flex flex-row items-center rounded-2xl px-3 py-2 font-bold"
          >
            <ArrowBigLeft size={18} />
            Leave
          </button>
        )}
        <button
          onClick={() => {
            console.log("HIT");
            PerformAction(PlayerAction.Hit);
          }}
          className="inset-shadow-c-md hover:pointer-cursor rounded-2xl bg-green-500 px-3 py-2 hover:bg-green-400 disabled:cursor-not-allowed disabled:opacity-60"
        >
          Hit
        </button>
        <button
          onClick={() => {
            console.log("STAND");
            PerformAction(PlayerAction.Stand);
          }}
          className="inset-shadow-c-md hover:pointer-cursor rounded-2xl bg-red-500 px-3 py-2 hover:bg-red-400"
        >
          Stand
        </button>
        <button
          onClick={() => console.log("DOUBLE DOWN")}
          className="inset-shadow-c-md hover:pointer-cursor rounded-2xl bg-yellow-300 px-3 py-2 hover:bg-yellow-200"
        >
          Double down
        </button>
        <button
          onClick={() => console.log("SPLIT")}
          className="inset-shadow-c-md hover:pointer-cursor rounded-2xl bg-blue-600 px-3 py-2 hover:bg-blue-500"
        >
          Split
        </button>
      </div>
    </div>
  );
}
