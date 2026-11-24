import { Route, Routes } from 'react-router-dom'
import { ThemeProvider } from '@mui/material'
import { Bounce, ToastContainer } from 'react-toastify'
import { darkTheme } from './theming/darkTheme.js'
import AdminOnly from './components/auth/AdminOnly.jsx'
import DefaultLayout from './components/layouts/DefaultLayout.jsx'
import HomePage from './pages/HomePage.jsx'
import GameListPage from './pages/game/GameListPage.jsx'
import GameFormPage from './pages/game/GameFormPage.jsx'
import LoginPage from './pages/LoginPage.jsx'
import RegisterPage from './pages/RegisterPage.jsx'
import DevelopersHome from './pages/developer/DevelopersHome.jsx'
import CreateDeveloper from './pages/developer/CreateDeveloper.jsx'
import NotFound from './pages/NotFound.jsx'
import './App.css'

function App() {
  return (
    <>
      <ThemeProvider theme={darkTheme}>
        <Routes>
          <Route path="/" element={<DefaultLayout />}>
            <Route index element={<HomePage />} />
            <Route path="games" element={<GameListPage />} />
            <Route path="games/new" element={<AdminOnly><GameFormPage /></AdminOnly>} />
            <Route path="games/:id/edit" element={<AdminOnly><GameFormPage /></AdminOnly>} />
            <Route path="login" element={<LoginPage />} />
            <Route path="register" element={<RegisterPage />} />
            <Route path="developers" element={<DevelopersHome />} />
            <Route path="developers/new" element={<AdminOnly><CreateDeveloper /></AdminOnly>} />
            <Route path="create" element={<AdminOnly><CreateDeveloper /></AdminOnly>} />
            <Route path="*" element={<NotFound />} />
          </Route>
        </Routes>
        <ToastContainer
          position="top-right"
          autoClose={5000}
          newestOnTop
          closeOnClick
          pauseOnFocusLoss
          draggable
          pauseOnHover
          theme="dark"
          transition={Bounce}
        />
      </ThemeProvider>
    </>
  )
}

export default App
