import { useState } from 'react'
import { Link } from 'react-router-dom'
import { toast } from 'react-toastify'
import { api } from '../api.js'

const RegisterPage = () => {
  const [userName, setUserName] = useState('')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [confirmPassword, setConfirmPassword] = useState('')
  const [submitting, setSubmitting] = useState(false)
  const [error, setError] = useState('')
  const [registeredEmail, setRegisteredEmail] = useState('')

  const handleSubmit = async (event) => {
    event.preventDefault()
    setError('')

    if (password !== confirmPassword) {
      setError('Passwords do not match.')
      return
    }

    setSubmitting(true)
    try {
      const { data } = await api.post('auth/register', {
        userName: userName.trim(),
        email: email.trim(),
        password,
      })

      if (!data.isSuccess) {
        throw new Error(data.message || 'Registration failed.')
      }

      setRegisteredEmail(email.trim())
      toast.success(data.message || 'Account created. Check your email to confirm it.')
    } catch (requestError) {
      const responseData = requestError.response?.data
      const validationErrors = responseData?.errors
        ? Object.values(responseData.errors).flat().join(' ')
        : ''
      const message = responseData?.message
        || validationErrors
        || responseData?.title
        || requestError.message
        || 'Could not create your account.'
      setError(message)
      toast.error(message)
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <section className="login-page">
      <div className="login-aside">
        <p className="eyebrow">JOIN THE LIBRARY</p>
        <h1>Make room<br />for your next<br /><span>favorite.</span></h1>
        <p>Create an account to join the SteamClone community and discover your next game.</p>
      </div>

      <div className="form-panel login-panel">
        {registeredEmail ? (
          <div className="registration-success" role="status">
            <p className="eyebrow">ONE LAST STEP</p>
            <h2>Check your inbox</h2>
            <p className="form-intro">
              We sent a confirmation link to <strong>{registeredEmail}</strong>. Confirm your email before signing in.
            </p>
            <Link className="button button-primary button-wide" to="/login">Go to sign in</Link>
            <p className="form-footnote">No message? Check your spam folder.</p>
          </div>
        ) : (
          <form onSubmit={handleSubmit}>
            <p className="eyebrow">CREATE AN ACCOUNT</p>
            <h2>Join SteamClone</h2>
            <p className="form-intro">Create your account with an email address you can access.</p>

            <label className="field">
              <span>Username</span>
              <input
                autoComplete="username"
                required
                minLength={3}
                maxLength={256}
                value={userName}
                onChange={(event) => setUserName(event.target.value)}
                placeholder="Choose a username"
              />
            </label>

            <label className="field">
              <span>Email address</span>
              <input
                autoComplete="email"
                required
                maxLength={256}
                type="email"
                value={email}
                onChange={(event) => setEmail(event.target.value)}
                placeholder="you@example.com"
              />
            </label>

            <label className="field">
              <span>Password</span>
              <input
                autoComplete="new-password"
                required
                minLength={6}
                type="password"
                value={password}
                onChange={(event) => setPassword(event.target.value)}
                placeholder="At least 6 characters"
              />
            </label>

            <label className="field">
              <span>Confirm password</span>
              <input
                autoComplete="new-password"
                required
                minLength={6}
                type="password"
                value={confirmPassword}
                onChange={(event) => setConfirmPassword(event.target.value)}
                placeholder="Enter your password again"
              />
            </label>

            {error && <p className="form-error" role="alert">{error}</p>}

            <button className="button button-primary button-wide" disabled={submitting} type="submit">
              {submitting ? 'Creating account…' : 'Create account'}
            </button>
            <p className="form-footnote">Already have an account? <Link to="/login">Sign in</Link></p>
          </form>
        )}
      </div>
    </section>
  )
}

export default RegisterPage
