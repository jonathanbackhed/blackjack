"use client";

import { useSettingsStore } from "@/lib/hooks/useSettingsStore";
import { X } from "lucide-react";
import React, { useState } from "react";

interface Props {
  show: boolean;
  close: () => void;
}

export default function Modal({ show, close }: Props) {
  const [username, setUsername] = useState("");
  const [showError, setShowError] = useState(false);

  const { setUsername: saveName } = useSettingsStore();

  const handleSubmit = (e: any) => {
    e.preventDefault();
    if (username.length < 3) {
      setShowError(true);
    } else {
      saveName(username);
      setShowError(false);
      close();
    }
  };

  return (
    <div
      className={`${show ? "flex" : "hidden"} absolute top-0 left-0 z-40 h-screen w-screen items-center justify-center bg-black/60 backdrop-blur-sm`}
    >
      <div className={`bg-b relative flex flex-col items-center rounded-2xl p-8 ${showError && "pb-4"} font-mono`}>
        <button
          onClick={() => close()}
          className="hover:bg-b-dark absolute top-1.5 right-1.5 rounded-full p-1 hover:cursor-pointer"
        >
          <X size={20} />
        </button>
        <h2 className="text-t mb-2 text-xl">Set your name</h2>
        <form onSubmit={handleSubmit}>
          <input
            type="text"
            placeholder="JohnDoe"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            maxLength={20}
            minLength={3}
            className="text-default bg-b-dark inset-shadow-c-sm mr-2 rounded-2xl px-3 py-2"
          />
          <button
            onClick={handleSubmit}
            className="bg-b-light active:bg-b-dark active:inset-shadow-c-sm shadow-c-sm rounded-2xl px-3 py-2 hover:cursor-pointer active:shadow-none"
          >
            Save
          </button>
        </form>
        {showError && <p className="mt-4 text-red-500">Name must be at least 3 characters long</p>}
      </div>
    </div>
  );
}
