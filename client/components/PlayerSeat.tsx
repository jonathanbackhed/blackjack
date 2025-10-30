import { PlayerStateDto } from "@/types/playerStateDto";
import React from "react";

interface Props {
  player: PlayerStateDto;
}

export default function PlayerSeat({ player }: Props) {
  return (
    <div className="space-y-2">
      <div className="text-center text-4xl">{player.handValue}</div>
      <h3>{player.name}</h3>
      {player.isBust ? (
        <p className="text-center">BUST</p>
      ) : player.isStanding ? (
        <p className="text-center">STANDING</p>
      ) : (
        ""
      )}
    </div>
  );
}
