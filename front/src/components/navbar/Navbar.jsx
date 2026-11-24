import { useEffect, useState } from 'react'
import { Link, NavLink, useNavigate } from 'react-router-dom'
import { isAdmin } from '../../helpres/auth.js'

const Navbar = () => {
  const [admin, setAdmin] = useState(isAdmin)
  const navigate = useNavigate()

  useEffect(() => {
    const updateAuth = () => setAdmin(isAdmin())
    window.addEventListener('storage', updateAuth)
    window.addEventListener('auth-change', updateAuth)
    return () => {
      window.removeEventListener('storage', updateAuth)
      window.removeEventListener('auth-change', updateAuth)
    }
  }, [])

  const signOut = () => {
    localStorage.removeItem('steamclone-token')
    window.dispatchEvent(new Event('auth-change'))
    navigate('/')
  }

  return (
    <header className="site-header">
      <div className="header-inner">
        <Link className="brand" to="/" aria-label="SteamClone home">
          <span className="brand-mark">S</span>
          <span>STEAM<span className="brand-light">CLONE</span></span>
        </Link>
        <nav className="main-nav" aria-label="Main navigation">
          <NavLink to="/" end>Home</NavLink>
          <NavLink to="/games">Games</NavLink>
          {admin && <NavLink to="/games/new">Add a game</NavLink>}
        </nav>
        <div className="header-actions">
          {localStorage.getItem('steamclone-token') ? (
            <button className="button button-outline button-small" onClick={signOut}>Sign out</button>
          ) : (
            <Link className="button button-primary button-small" to="/login">Sign in</Link>
          )}
        </div>
      </div>
    </header>
  )
}

export default Navbar
