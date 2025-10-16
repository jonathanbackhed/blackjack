import { create } from "zustand";
import { persist, createJSONStorage } from "zustand/middleware";

interface StoreProps {
  username: string;
  setUsername: (username: string) => void;
  theme: "light" | "dark" | "auto";
  setTheme: (theme: "light" | "dark" | "auto") => void;
}

export const useSettingsStore = create<StoreProps>()(
  persist(
    (set) => ({
      username: "",
      setUsername: (username: string) => set({ username }),
      theme: "auto",
      setTheme: (theme: "light" | "dark" | "auto") => set({ theme }),
    }),
    {
      name: "store",
      storage: createJSONStorage(() => localStorage),
    },
  ),
);
