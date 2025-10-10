import React from "react";

export default function Navbar() {
  return (
    <div className="bg-b m-2 flex h-min flex-row rounded-2xl p-2">
      <div className="text-t-default hover:bg-b-light rounded-2xl px-3 py-2">Item1</div>
      <div className="text-t-default hover:bg-b-light rounded-2xl px-3 py-2">Item2</div>
      <div className="text-t-default hover:bg-b-light rounded-2xl px-3 py-2 active:bg-indigo-500">Item3</div>
    </div>
  );
}
