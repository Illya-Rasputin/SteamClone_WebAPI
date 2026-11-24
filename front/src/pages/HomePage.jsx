import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { api } from '../api.js'
import GameCard from '../components/cards/GameCard.jsx'

const HomePage = () => {
  const [games, setGames] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    api.get('game')
      .then(({ data }) => setGames(data.payload ?? []))
      .catch((requestError) => setError(requestError.response?.data?.message ?? 'Could not load games. Check that the API is running.'))
      .finally(() => setLoading(false))
  }, [])

  const featuredGame = games[0]
  const preview = featuredGame?.images?.find((image) => image.isPreview)

  return (
    <div className="home-page">
      <section className="hero-panel">
        <div className="hero-copy">
          <p className="eyebrow">DISCOVER YOUR NEXT ADVENTURE</p>
          <h1>Great games.<br /><span>Good times.</span></h1>
          <p className="hero-description">
            Find your next favorite from a hand-picked world of adventures, strategy, and everything in between.
          </p>
          <Link className="button button-primary" to="/games">Explore the library <span aria-hidden="true">→</span></Link>
        </div>
        <div className="hero-art" aria-label={featuredGame?.name ?? 'Featured games'}>
          {preview ? (
            <img src={`${import.meta.env.VITE_GAME_IMAGE_URL}${preview.name}`} alt={featuredGame.name} />
          ) : (
            <div className="hero-art-placeholder"><span>PLAY<br />SOMETHING<br />GREAT</span></div>
          )}
          {featuredGame && (
            <div className="hero-game-label">
              <span className="eyebrow">FEATURED GAME</span>
              <strong>{featuredGame.name}</strong>
            </div>
          )}
        </div>
        <div className="hero-orb hero-orb-one" />
        <div className="hero-orb hero-orb-two" />
      </section>

      <section className="section-block">
        <div className="section-heading">
          <div>
            <p className="eyebrow">THE LIBRARY</p>
            <h2>Popular right now</h2>
          </div>
          <Link className="text-link" to="/games">View all games <span aria-hidden="true">→</span></Link>
        </div>
        {loading && <p className="state-message">Loading the library…</p>}
        {error && <p className="state-message state-error" role="alert">{error}</p>}
        {!loading && !error && games.length === 0 && <p className="state-message">The library is quiet for now. Check back soon.</p>}
        {!loading && !error && games.length > 0 && (
          <div className="game-grid">
            {games.slice(0, 4).map((game) => <GameCard key={game.id} game={game} />)}
          </div>
        )}
      </section>

      <section className="community-banner">
        <div>
          <p className="eyebrow">YOUR COLLECTION STARTS HERE</p>
          <h2>Ready to find a new favorite?</h2>
        </div>
        <Link className="button button-secondary" to="/games">Browse all games</Link>
      </section>
    </div>
  )
}

export default HomePage
