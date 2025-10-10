"use client";

import React from "react";

interface Props {
  message?: string;
  status?: "info" | "error" | "success" | "neutral";
  onDismiss?: () => void;
}

export default function Toast({ message, status, onDismiss }: Props) {
  const [isVisible, setIsVisible] = React.useState(true);

  const handleDismiss = () => {
    setIsVisible(false);
    onDismiss?.();
  };

  return (
    <button
      onClick={handleDismiss}
      className={`bg-b shadow-c-lg animate-slide-in-top absolute z-50 rounded-2xl p-4 ${status === "success" ? "text-green-500" : status === "error" ? "text-red-500" : status === "info" ? "text-blue-500" : ""}`}
    >
      {message}
    </button>
  );
}
