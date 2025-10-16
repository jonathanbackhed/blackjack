"use client";

import Modal from "@/components/Modal";
import { useSettingsStore } from "@/hooks/useSettingsStore";
import { Pencil } from "lucide-react";
import { useEffect, useState } from "react";
import isUsernameValid from "@/utils/validateUsername";
import { useRouter } from "next/navigation";

export default function Home() {
  const [showModal, setShowModal] = useState(false);
  const [gameCode, setGameCode] = useState("");

  const router = useRouter();

  const { username } = useSettingsStore();

  const joinGame = async () => {
    if (!isUsernameValid(username)) {
      setShowModal(true);
      return;
    }
    if (gameCode.length !== 8) return;

    router.push(`/game/${gameCode}`);
  };

  useEffect(() => {
    setTimeout(function () {
      if (!isUsernameValid(username)) setShowModal(true);
      else setShowModal(false);
    }, 700);
  }, [username]);

  return (
    <div className="flex min-h-screen flex-col items-center justify-center space-y-2 font-mono">
      <Modal show={showModal} close={() => setShowModal(false)} />
      <h1 className="text-t text-6xl">Blackjack for fun</h1>
      <div className="flex flex-row items-center justify-center">
        <p className="mr-1">Username is {username}</p>
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
        className="bg-b-dark inset-shadow-c-md rounded-2xl px-5 py-3 text-center text-3xl"
      />
      <button onClick={joinGame} className="px-3 py-2 hover:cursor-pointer">
        Join
      </button>
    </div>
  );
}
