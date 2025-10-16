export default function isUsernameValid(username: string): boolean {
  if (!username || username.length < 3 || username.length > 20) return false;

  return true;
}
