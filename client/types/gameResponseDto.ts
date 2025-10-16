/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { ServerStatus } from "./serverStatus";
import { PlayerStateDto } from "./playerStateDto";

export interface GameResponseDto {
    serverId: string;
    status: ServerStatus;
    players: PlayerStateDto[];
    currentTurnPlayerId: string;
    winner: string;
    message: string;
}
