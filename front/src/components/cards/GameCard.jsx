import { Link } from 'react-router-dom'

const GameCard = ({game, canEdit = false}) => {
    const cover = game.images?.find((image) => image.isPreview)
    const imageUrl = cover ? `${import.meta.env.VITE_GAME_IMAGE_URL}${cover.name}` : ''

    return (
        <article className="game-card">
            <div className="game-card-link">
                <div className="game-card-image">
                    {imageUrl
                        ? <img src={imageUrl} alt={`${game.name} cover`} loading="lazy" />
                        : <div className="game-card-placeholder"><span>{game.name?.slice(0, 1)}</span></div>}
                </div>
                <div className="game-card-content">
                    <div className="game-card-meta">
                        <span>{game.developer?.name || 'STEAMCLONE LIBRARY'}</span>
                        <span>{game.release && new Date(game.release).getFullYear() > 1 ? new Date(game.release).getFullYear() : 'GAME'}</span>
                    </div>
                    <h3>{game.name}</h3>
                    <p className="game-card-description">{game.description || 'Discover more about this game in the library.'}</p>
                    <div className="game-card-footer">
                        <span className="genre-list">{game.genres?.slice(0, 2).map((genre) => genre.name).join(' · ') || 'Explore now'}</span>
                        <div className="game-card-actions">
                            <strong>{game.price > 0 ? `$${Number(game.price).toFixed(2)}` : 'Details'}</strong>
                            {canEdit && <Link className="game-edit-link" to={`/games/${game.id}/edit`}>Edit</Link>}
                        </div>
                    </div>
                </div>
            </div>
        </article>
    )
}

export default GameCard;