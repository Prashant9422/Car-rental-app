import { Link } from 'react-router-dom';
import { useAuth } from '../auth/useAuth';
import './Home.css';

export default function Home() {
    const { user } = useAuth();
    return (
        <div className="home-page">
            {/* Hero Section */}
            <section className="hero">
                <div className="container hero-content">
                    <h1>Find Your Perfect <span className="text-primary">Drive</span></h1>
                    <p className="hero-subtitle">
                        Experience the freedom of the road with our premium fleet.
                        Luxury, comfort, and performance at your fingertips.
                    </p>
                    <div className="hero-actions">
                        <Link to="/cars" className="btn btn-primary btn-lg">Browse Cars</Link>
                        {!user && (
                            <Link to="/register" className="btn btn-secondary btn-lg">Sign Up Now</Link>
                        )}
                    </div>
                </div>
                <div className="hero-background"></div>
            </section>

            {/* Features Section */}
            <section className="features container">
                <div className="feature-card">
                    <div className="feature-icon">🚀</div>
                    <h3>Fast Booking</h3>
                    <p>Book your dream car in minutes with our seamless process.</p>
                </div>
                <div className="feature-card">
                    <div className="feature-icon">💎</div>
                    <h3>Premium Quality</h3>
                    <p>Top-tier vehicles maintained to the highest standards.</p>
                </div>
                <div className="feature-card">
                    <div className="feature-icon">🛡️</div>
                    <h3>Secure & Safe</h3>
                    <p>Fully insured rides and 24/7 roadside assistance.</p>
                </div>
            </section>

            {/* Validated Stats Section - Mock */}
            <section className="stats container">
                <div className="stat-item">
                    <h2>50+</h2>
                    <p>Luxury Cars</p>
                </div>
                <div className="stat-item">
                    <h2>10k+</h2>
                    <p>Happy Customers</p>
                </div>
                <div className="stat-item">
                    <h2>24/7</h2>
                    <p>Support</p>
                </div>
            </section>
        </div>
    );
}