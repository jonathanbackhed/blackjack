"use client";

import { useSettingsStore } from "@/hooks/useSettingsStore";
import { useEffect, useState } from "react";
import { Api } from "@/lib/apiClient";
import { useRouter } from "next/navigation";
import { useSignalR } from "@/hooks/useSignalR";
import { ArrowBigLeft } from "lucide-react";
import { GameResponseDto } from "@/types/gameResponseDto";
import { PlayerAction } from "@/types/playerAction";
import { joinGame, leaveGame, startGame, PerformAction } from "@/lib/services/gameService";
import PlayerSeat from "./PlayerSeat";

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

  const player1 = serverState?.players[0]?.isDealer === false ? serverState.players[0] : null;
  const player2 = serverState?.players[1]?.isDealer === false ? serverState.players[1] : null;
  const player3 = serverState?.players[2]?.isDealer === false ? serverState.players[2] : null;

  const dealer = serverState?.players?.find((p) => p.isDealer) || null;

  const getServerId = async () => {
    const sid = await Api.getServerIdFromCode(id);
    if (!sid) {
      return router.replace("/");
    }
    setServerId(sid.serverId);
    return sid.serverId;
  };

  const handleLeaveGame = async () => {
    const success = await leaveGame(connection, isConnected, serverId!);
    if (success === true) router.replace("/");
  };

  const handlePerformAction = async (action: PlayerAction) => {
    if (serverId && playerId) {
      await PerformAction(connection, isConnected, serverId, playerId, action);
    }
  };

  useEffect(() => {
    if (isConnected) {
      (async () => {
        const sid = await getServerId();
        if (sid) {
          await joinGame(connection, isConnected, sid, username);
        }
      })();
    }
  }, [isConnected]);

  return (
    <div className="flex h-[1000px] w-[1400px] flex-col items-center justify-around bg-red-400">
      <h1 className="text-t text-center font-mono text-6xl">Game {id}</h1>
      {serverState?.winner && <h1 className="text-t text-center font-mono text-6xl">Winner {serverState?.winner}</h1>}
      {serverId && (
        <button
          onClick={() => startGame(connection, isConnected, serverId)}
          className="bg-b hover:bg-b-light rounded-2xl px-3 py-2 text-4xl"
        >
          Start game
        </button>
      )}
      {/* GAMEBOARD */}
      <div className="h-[600px] w-full rounded-full bg-neutral-700">
        <div className="grid h-full w-full grid-cols-5 grid-rows-5 gap-4 bg-blue-500/50">
          <div className="col-span-3 col-start-2 row-span-2 flex flex-1 items-center justify-center bg-fuchsia-500/40">
            {dealer ? <PlayerSeat player={dealer} /> : "DEALER"}
          </div>

          <div className="col-span-3 col-start-2 row-start-3 flex items-center justify-center bg-red-500/30 text-xl font-bold">
            Lorem ipsum dolor
          </div>

          <div className="col-start-2 row-span-2 flex flex-col items-center justify-center bg-green-500/40">
            {player3 ? <PlayerSeat player={player3} /> : "Empty seat"}
          </div>
          <div className="col-start-3 row-span-2 flex flex-col items-center justify-center bg-green-500/40">
            {player2 ? <PlayerSeat player={player2} /> : "Empty seat"}
          </div>
          <div className="col-start-4 row-span-2 flex flex-col items-center justify-center bg-green-500/40">
            {player1 ? <PlayerSeat player={player1} /> : "Empty seat"}
          </div>
        </div>
      </div>

      <div className="relative flex w-full items-center justify-center space-x-4">
        {isConnected && serverId && (
          <button
            onClick={handleLeaveGame}
            className="hover:bg-b-light absolute left-2 flex flex-row items-center rounded-2xl px-3 py-2 font-bold"
          >
            <ArrowBigLeft size={18} />
            Leave
          </button>
        )}
        <button
          onClick={() => {
            console.log("HIT");
            handlePerformAction(PlayerAction.Hit);
          }}
          className="inset-shadow-c-md hover:pointer-cursor rounded-2xl bg-green-500 px-3 py-2 hover:bg-green-400 disabled:cursor-not-allowed disabled:opacity-60"
        >
          Hit
        </button>
        <button
          onClick={() => {
            console.log("STAND");
            handlePerformAction(PlayerAction.Stand);
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
