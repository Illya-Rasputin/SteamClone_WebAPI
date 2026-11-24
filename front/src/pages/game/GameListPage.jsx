import { useEffect, useMemo, useState } from 'react'
import { Link } from 'react-router-dom'
import { api } from '../../api.js'
import GameCard from '../../components/cards/GameCard.jsx'
import { isAdmin } from '../../helpres/auth.js'

const GameListPage = () => {
  const [games, setGames] = useState([])
  const [search, setSearch] = useState('')
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const canAddGame = isAdmin()

  useEffect(() => {
    api.get('game')
      .then(({ data }) => setGames(data.payload ?? []))
      .catch((requestError) => setError(requestError.response?.data?.message ?? 'Could not load games. Check that the API is running.'))
      .finally(() => setLoading(false))
  }, [])

  const filteredGames = useMemo(() => {
    const query = search.trim().toLowerCase()
    return games.filter((game) =>
      game.name?.toLowerCase().includes(query)
      || game.developer?.name?.toLowerCase().includes(query)
      || game.genres?.some((genre) => genre.name?.toLowerCase().includes(query)),
    )
  }, [games, search])

  return (
    <section className="library-page">
      <div className="page-heading">
        <div>
          <p className="eyebrow">EXPLORE</p>
          <h1>Game library</h1>
          <p className="page-intro">Find your next favorite among {games.length} games.</p>
        </div>
        {canAddGame && <Link className="button button-primary" to="/games/new">＋ Add a game</Link>}
      </div>

      <label className="search-box">
        <span className="search-icon" aria-hidden="true">⌕</span>
        <span className="visually-hidden">Search games</span>
        <input
          type="search"
          value={search}
          onChange={(event) => setSearch(event.target.value)}
          placeholder="Search by title, developer, or genre"
        />
        <kbd>SEARCH</kbd>
      </label>

      {loading && <p className="state-message">Loading the library…</p>}
      {error && <p className="state-message state-error" role="alert">{error}</p>}
      {!loading && !error && filteredGames.length === 0 && (
        <div className="empty-state">
          <span className="empty-icon">⌕</span>
          <h2>No games found</h2>
          <p>{search ? 'Try another title or clear your search.' : 'There are no games in the library yet.'}</p>
        </div>
      )}
      {!loading && !error && filteredGames.length > 0 && (
        <>
          <div className="result-count">{filteredGames.length} {filteredGames.length === 1 ? 'GAME' : 'GAMES'}</div>
          <div className="game-grid">
            {filteredGames.map((game) => <GameCard key={game.id} game={game} canEdit={canAddGame} />)}
          </div>
        </>
      )}
    </section>
  )
}

export default GameListPage
