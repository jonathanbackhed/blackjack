async function apiClient(path: string) {
  const res = await fetch(`${process.env.NEXT_PUBLIC_SERVER_URL}${path}`);
  if (!res.ok) {
    // throw new Error(`API error: ${res.status}`);
    return null;
  }

  return res.json();
}

export const Api = {
  getServerIdFromCode: async (code: string) => await apiClient(`/server/getid/${code}`),
};
