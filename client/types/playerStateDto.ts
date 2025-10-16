/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { Card } from "./card";

export interface PlayerStateDto {
    playerId: string;
    name: string;
    cards: Card[];
    handValue: number;
    isDealer: boolean;
    isStanding: boolean;
    isBust: boolean;
}
