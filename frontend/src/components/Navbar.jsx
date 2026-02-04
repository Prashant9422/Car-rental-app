import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../auth/useAuth';
import './Navbar.css';

export default function Navbar() {
    const { user, logout } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    return (
        <nav className="navbar">
            <div className="container navbar-content">
                <Link to="/" className="navbar-logo">
                    CarRental<span className="text-primary">.</span>
                </Link>

                <div className="navbar-links">
                    <Link to="/" className="nav-link">Home</Link>
                    <Link to="/cars" className="nav-link">Browse Cars</Link>
                    {user && <Link to="/dashboard" className="nav-link">Dashboard</Link>}
                    {user && user.role === 'Admin' && <Link to="/admin/add-car" className="nav-link">Add Car</Link>}
                </div>

                <div className="navbar-actions">
                    {user ? (
                        <div className="flex items-center gap-md">
                            <span className="user-greeting">Hi, {user.username || 'User'}</span>
                            <button onClick={handleLogout} className="btn btn-secondary btn-sm">
                                Logout
                            </button>
                        </div>
                    ) : (
                        <div className="flex items-center gap-sm">
                            <Link to="/login" className="btn btn-secondary btn-sm">Login</Link>
                            <Link to="/register" className="btn btn-primary btn-sm">Register</Link>
                        </div>
                    )}
                </div>
            </div>
        </nav>
    );
}
