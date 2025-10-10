"use client";

import Modal from "@/components/Modal";
import { useSettingsStore } from "@/lib/hooks/useSettingsStore";
import { Pencil } from "lucide-react";
import Link from "next/link";
import React, { useEffect, useState } from "react";

export default function Home() {
  const [showModal, setShowModal] = useState(false);
  const [gameCode, setGameCode] = useState("");

  const { username } = useSettingsStore();

  const joinGame = () => {
    console.log("JOIN GAME");
  };

  useEffect(() => {
    setTimeout(function () {
      if (!username || username.length < 3) setShowModal(true);
      else setShowModal(false);
    }, 700);
  }, [username]);

  return (
    <div className="flex min-h-screen flex-col items-center justify-center space-y-2 font-sans">
      <Modal show={showModal} close={() => setShowModal(false)} />

      <h1 className="text-t font-mono text-6xl">Blackjack for fun</h1>
      <div className="flex flex-row items-center justify-center">
        <p className="mr-1">Signed in as {username}</p>
        <button onClick={() => setShowModal(true)} className="hover:cursor-pointer">
          <Pencil size={12} />
        </button>
      </div>
      <input
        type="text"
        maxLength={8}
        placeholder="Enter game code"
        value={gameCode}
        onChange={(e) => setGameCode(e.target.value)}
        className="bg-b-dark inset-shadow-c-lg rounded-2xl px-5 py-3 text-center text-3xl"
      />
      <button onClick={joinGame} className="px-3 py-2 hover:cursor-pointer">
        Join
      </button>
      <Link href="/game/abc123" className="bg-b mt-10 rounded-2xl px-3 py-2 text-2xl">
        To game
      </Link>
    </div>
  );
}
