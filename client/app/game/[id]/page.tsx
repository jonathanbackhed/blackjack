"use client";

import React from "react";

export default async function Game({ params }: { params: Promise<{ id: string }> }) {
  const { id } = await params;
  return (
    <div className="flex min-h-screen items-center justify-center font-sans">
      <div className="flex h-[1000px] w-[1400px] flex-col items-center justify-around bg-red-400">
        <h1 className="text-t font-mono text-6xl">Game {id}</h1>
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
        <div className="space-x-4 font-mono">
          <button
            onClick={() => console.log("HIT")}
            className="inset-shadow-c-md hover:pointer-cursor rounded-2xl bg-green-500 px-3 py-2 hover:bg-green-400"
          >
            Hit
          </button>
          <button
            onClick={() => console.log("STAND")}
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
    </div>
  );
}
