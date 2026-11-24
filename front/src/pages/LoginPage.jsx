import { useState } from 'react'
import { Link, useLocation, useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify'
import { api } from '../api.js'

const LoginPage = () => {
  const [userName, setUserName] = useState('')
  const [password, setPassword] = useState('')
  const [submitting, setSubmitting] = useState(false)
  const [error, setError] = useState('')
  const navigate = useNavigate()
  const location = useLocation()

  const handleSubmit = async (event) => {
    event.preventDefault()
    setError('')
    setSubmitting(true)
    try {
      const { data } = await api.post('auth/login', { userName, password })
      if (!data.isSuccess || !data.payload) {
        throw new Error(data.message || 'Sign in failed.')
      }
      localStorage.setItem('steamclone-token', data.payload)
      window.dispatchEvent(new Event('auth-change'))
      toast.success(data.message || 'Signed in successfully.')
      navigate(location.state?.from?.pathname || '/')
    } catch (requestError) {
      const message = requestError.response?.data?.message ?? requestError.message ?? 'Could not sign in.'
      setError(message)
      toast.error(message)
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <section className="login-page">
      <div className="login-aside">
        <p className="eyebrow">WELCOME BACK</p>
        <h1>Your next<br /><span>great game</span><br />is waiting.</h1>
        <p>Sign in to manage games and get back to discovering something new.</p>
      </div>
      <form className="form-panel login-panel" onSubmit={handleSubmit}>
        <p className="eyebrow">YOUR ACCOUNT</p>
        <h2>Sign in</h2>
        <p className="form-intro">Enter your username and password to continue.</p>
        <label className="field">
          <span>Username</span>
          <input
            autoComplete="username"
            required
            value={userName}
            onChange={(event) => setUserName(event.target.value)}
            placeholder="Your username"
          />
        </label>
        <label className="field">
          <span>Password</span>
          <input
            autoComplete="current-password"
            required
            type="password"
            value={password}
            onChange={(event) => setPassword(event.target.value)}
            placeholder="Your password"
          />
        </label>
        {error && <p className="form-error" role="alert">{error}</p>}
        <button className="button button-primary button-wide" disabled={submitting} type="submit">
          {submitting ? 'Signing in…' : 'Sign in'}
        </button>
        <p className="form-footnote">New here? <Link to="/register">Create an account</Link></p>
      </form>
    </section>
  )
}

export default LoginPage
