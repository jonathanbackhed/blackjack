import GameBoard from "@/components/GameBoard";

export default async function Game({ params }: { params: Promise<{ id: string }> }) {
  const { id } = await params;

  return (
    <div className="flex min-h-screen items-center justify-center font-sans">
      <GameBoard id={id} />
    </div>
  );
}
