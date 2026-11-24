import { Link, useLocation } from 'react-router-dom'
import { isAdmin } from '../../helpres/auth.js'

const AdminOnly = ({ children }) => {
  const location = useLocation()

  if (isAdmin()) return children

  const signedIn = Boolean(localStorage.getItem('steamclone-token'))
  return (
    <section className="empty-state">
      <p className="eyebrow">ADMIN AREA</p>
      <h2>Admin access required</h2>
      <p>Only administrators can access editor pages.</p>
      {signedIn
        ? <Link className="button button-outline" to="/games">Back to games</Link>
        : <Link className="button button-primary" to="/login" state={{ from: location }}>Sign in</Link>}
    </section>
  )
}

export default AdminOnly
