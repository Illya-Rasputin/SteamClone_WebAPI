import { Outlet } from 'react-router-dom'
import Navbar from '../navbar/Navbar.jsx'

const DefaultLayout = () => {
    return (
        <div className="app-shell">
            <Navbar/>
            <main className="page-content">
                <Outlet/>
            </main>
            <footer className="site-footer">
                <span>STEAMCLONE</span>
                <span>A home for games worth playing.</span>
            </footer>
        </div>
    )
}

export default DefaultLayout;