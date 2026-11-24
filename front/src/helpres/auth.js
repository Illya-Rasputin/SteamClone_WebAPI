const tokenStorageKey = 'steamclone-token'
const roleClaim = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'

export const getToken = () => localStorage.getItem(tokenStorageKey)

export const isAdmin = () => {
  const token = getToken()
  if (!token) return false

  try {
    const encodedPayload = token.split('.')[1]
      .replace(/-/g, '+')
      .replace(/_/g, '/')
    const payload = JSON.parse(atob(encodedPayload))
    const roles = payload.role ?? payload.roles ?? payload[roleClaim] ?? []
    return (Array.isArray(roles) ? roles : [roles])
      .some((role) => typeof role === 'string' && role.toLowerCase() === 'admin')
  } catch {
    return false
  }
}
