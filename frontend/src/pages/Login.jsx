import { useState } from 'react';
import { useAuth } from '../auth/useAuth';
import { useNavigate, Link } from 'react-router-dom';
import './Auth.css';

export default function Login() {
    const [formData, setFormData] = useState({
        email: '',
        password: ''
    });
    const { login, loading, error } = useAuth(); // Assuming useAuth exposes loading/error from AuthContext
    const navigate = useNavigate();

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const success = await login(formData); // login determines success
        if (success) {
            navigate('/dashboard');
        }
    };

    return (
        <div className="auth-container">
            <div className="auth-card">
                <h2 className="auth-title">Welcome Back</h2>
                <p className="auth-subtitle">Sign in to continue your journey</p>

                {error && <div className="alert alert-danger">{error}</div>}

                <form onSubmit={handleSubmit} className="auth-form">
                    <div className="form-group">
                        <label htmlFor="email" className="label">Email Address</label>
                        <input
                            id="email"
                            name="email"
                            type="email"
                            className="input"
                            value={formData.email}
                            onChange={handleChange}
                            placeholder="Enter your email"
                            required
                        />
                    </div>

                    <div className="form-group">
                        <label htmlFor="password" className="label">Password</label>
                        <input
                            id="password"
                            name="password"
                            type="password"
                            className="input"
                            value={formData.password}
                            onChange={handleChange}
                            placeholder="Enter your password"
                            required
                        />
                    </div>

                    <button type="submit" className="btn btn-primary btn-block" disabled={loading}>
                        {loading ? 'Signing In...' : 'Sign In'}
                    </button>
                </form>

                <p className="auth-footer">
                    Don't have an account? <Link to="/register">Create one</Link>
                </p>
            </div>
        </div>
    );
}