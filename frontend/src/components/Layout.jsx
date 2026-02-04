import Navbar from './Navbar';
import Footer from './Footer';
import { Outlet } from 'react-router-dom';

export default function Layout() {
    return (
        <div className="layout-root">
            <Navbar />
            <main className="layout-content">
                <Outlet />
            </main>
            <Footer />
        </div>
    );
}
