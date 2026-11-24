import { useEffect, useState } from 'react'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { toast } from 'react-toastify'
import { api } from '../../api.js'

const getDateValue = (value) => value ? new Date(value).toISOString().slice(0, 10) : ''

const GameFormPage = () => {
  const { id } = useParams()
  const isEditing = Boolean(id)
  const navigate = useNavigate()
  const [form, setForm] = useState({
    name: '',
    price: '',
    release: new Date().toISOString().slice(0, 10),
    description: '',
    developerId: '',
    genreIds: [],
  })
  const [genres, setGenres] = useState([])
  const [developers, setDevelopers] = useState([])
  const [existingImages, setExistingImages] = useState([])
  const [previewImage, setPreviewImage] = useState(null)
  const [images, setImages] = useState([])
  const [loading, setLoading] = useState(isEditing)
  const [submitting, setSubmitting] = useState(false)
  const [error, setError] = useState('')
  const [developerError, setDeveloperError] = useState('')

  useEffect(() => {
    let active = true
    const loadFormData = async () => {
      const [genreResult, developerResult] = await Promise.allSettled([
        api.get('genre'),
        api.get('developer'),
      ])
      if (!active) return

      if (genreResult.status === 'fulfilled') {
        setGenres(genreResult.value.data.payload ?? [])
      } else {
        setError(genreResult.reason.response?.data?.message ?? 'Could not load genres.')
      }
      if (developerResult.status === 'fulfilled') {
        setDevelopers(developerResult.value.data.payload ?? [])
      } else {
        setDeveloperError('Sign in to load developer options.')
      }

      if (isEditing) {
        try {
          const { data } = await api.get(`game/${id}`)
          if (!active) return
          const game = data.payload
          setForm({
            name: game.name ?? '',
            price: game.price ?? '',
            release: getDateValue(game.release),
            description: game.description ?? '',
            developerId: game.developerId?.toString() ?? '',
            genreIds: game.genres?.map((genre) => genre.id) ?? [],
          })
          setExistingImages(game.images ?? [])
        } catch (requestError) {
          setError(requestError.response?.data?.message ?? 'Could not load this game.')
        }
      }
      if (active) setLoading(false)
    }
    loadFormData()
    return () => { active = false }
  }, [id, isEditing])

  const updateField = (event) => {
    const { name, value } = event.target
    setForm((current) => ({ ...current, [name]: value }))
  }

  const toggleGenre = (genreId) => {
    setForm((current) => ({
      ...current,
      genreIds: current.genreIds.includes(genreId)
        ? current.genreIds.filter((currentId) => currentId !== genreId)
        : [...current.genreIds, genreId],
    }))
  }

  const handleSubmit = async (event) => {
    event.preventDefault()
    setSubmitting(true)
    setError('')
    const formData = new FormData()
    formData.append('Name', form.name.trim())
    formData.append('Price', form.price)
    formData.append('Release', form.release)
    formData.append('Description', form.description)
    if (form.developerId) formData.append('DeveloperId', form.developerId)
    if (previewImage) formData.append('PreviewImage', previewImage)
    images.forEach((image) => formData.append('Images', image))

    if (isEditing) {
      formData.append('Id', id)
      form.genreIds.forEach((genreId, index) => {
        const genre = genres.find((item) => item.id === genreId)
        if (genre) {
          formData.append(`Genres[${index}].Id`, genre.id)
          formData.append(`Genres[${index}].Name`, genre.name)
        }
      })
    } else {
      form.genreIds.forEach((genreId) => {
        const genre = genres.find((item) => item.id === genreId)
        if (genre) formData.append('Genres', genre.name)
      })
    }

    try {
      const { data } = isEditing
        ? await api.put('game', formData)
        : await api.post('game', formData)
      toast.success(data.message || (isEditing ? 'Game updated.' : 'Game created.'))
      navigate('/games')
    } catch (requestError) {
      const message = requestError.response?.data?.message ?? 'Could not save this game.'
      setError(message)
      toast.error(message)
    } finally {
      setSubmitting(false)
    }
  }

  if (loading) return <p className="state-message">Loading game details…</p>

  return (
    <section className="game-form-page">
      <div className="form-page-heading">
        <Link className="back-link" to="/games">← Back to games</Link>
        <p className="eyebrow">{isEditing ? 'GAME MANAGEMENT' : 'ADD TO THE LIBRARY'}</p>
        <h1>{isEditing ? 'Update game' : 'Create a game'}</h1>
        <p className="page-intro">{isEditing ? 'Keep the game details up to date.' : 'Add a new title to the SteamClone library.'}</p>
      </div>

      <form className="game-form form-panel" onSubmit={handleSubmit}>
        {error && <p className="form-error" role="alert">{error}</p>}
        <div className="form-section-title">
          <span className="step-number">01</span>
          <div><h2>Game details</h2><p>The basics players want to know.</p></div>
        </div>
        <div className="form-grid">
          <label className="field field-span-two">
            <span>Game title <b>*</b></span>
            <input name="name" required maxLength="255" value={form.name} onChange={updateField} placeholder="e.g. Hades" />
          </label>
          <label className="field">
            <span>Price <b>*</b></span>
            <div className="input-with-prefix"><span>$</span><input name="price" type="number" min="0.01" step="0.01" required value={form.price} onChange={updateField} placeholder="19.99" /></div>
          </label>
          <label className="field">
            <span>Release date</span>
            <input name="release" type="date" value={form.release} onChange={updateField} />
          </label>
          <label className="field field-span-two">
            <span>Developer</span>
            <select name="developerId" value={form.developerId} onChange={updateField}>
              <option value="">Select a developer (optional)</option>
              {developers.map((developer) => <option key={developer.id} value={developer.id}>{developer.name}</option>)}
            </select>
            {developerError && <small className="field-hint">{developerError} <Link to="/login" state={{ from: { pathname: window.location.pathname } }}>Sign in</Link>, then return to this page.</small>}
          </label>
          <label className="field field-span-two">
            <span>Description</span>
            <textarea name="description" rows="5" value={form.description} onChange={updateField} placeholder="What makes this game special?" />
          </label>
        </div>

        <div className="form-section-title form-section-spaced">
          <span className="step-number">02</span>
          <div><h2>Genres</h2><p>Choose every genre that fits.</p></div>
        </div>
        {genres.length > 0 ? (
          <div className="genre-options">
            {genres.map((genre) => (
              <label className={`genre-option ${form.genreIds.includes(genre.id) ? 'selected' : ''}`} key={genre.id}>
                <input type="checkbox" checked={form.genreIds.includes(genre.id)} onChange={() => toggleGenre(genre.id)} />
                <span>{genre.name}</span>
              </label>
            ))}
          </div>
        ) : <p className="field-hint">No genres are available.</p>}

        <div className="form-section-title form-section-spaced">
          <span className="step-number">03</span>
          <div><h2>Artwork</h2><p>Add a cover and optional gallery images.</p></div>
        </div>
        {existingImages.length > 0 && (
          <div className="existing-images">
            <p className="field-hint">Current artwork (uploaded replacements are added when you save):</p>
            <div className="existing-image-list">
              {existingImages.map((image) => (
                <img key={image.name} src={`${import.meta.env.VITE_GAME_IMAGE_URL}${image.name}`} alt={image.isPreview ? 'Current cover' : 'Current gallery image'} />
              ))}
            </div>
          </div>
        )}
        <div className="form-grid artwork-fields">
          <label className="upload-field">
            <span>Cover image</span>
            <input type="file" accept="image/*" onChange={(event) => setPreviewImage(event.target.files?.[0] ?? null)} />
            <small>{previewImage?.name ?? (isEditing ? 'Choose a new cover to replace the current one' : 'Recommended: landscape image')}</small>
          </label>
          <label className="upload-field">
            <span>Gallery images</span>
            <input type="file" accept="image/*" multiple onChange={(event) => setImages(Array.from(event.target.files ?? []))} />
            <small>{images.length ? `${images.length} image${images.length === 1 ? '' : 's'} selected` : 'Optional screenshots or artwork'}</small>
          </label>
        </div>

        <div className="form-actions">
          <Link className="button button-outline" to="/games">Cancel</Link>
          <button className="button button-primary" type="submit" disabled={submitting}>
            {submitting ? 'Saving…' : isEditing ? 'Save changes' : 'Create game'}
          </button>
        </div>
      </form>
    </section>
  )
}

export default GameFormPage
