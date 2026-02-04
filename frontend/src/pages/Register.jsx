import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../auth/useAuth';
import './Auth.css';

export default function Register() {
    const [formData, setFormData] = useState({
        firstName: '',
        lastName: '',
        email: '',
        phoneNumber: '',
        password: '',
        confirmPassword: ''
    });
    const { register, loading, error } = useAuth();
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

        if (formData.password !== formData.confirmPassword) {
            alert("Passwords do not match!");
            return;
        }

        const success = await register(formData);
        if (success) {
            navigate('/dashboard');
        }
    };

    return (
        <div className="auth-container">
            <div className="auth-card" style={{ maxWidth: '500px' }}>
                <h2 className="auth-title">Create Account</h2>
                <p className="auth-subtitle">Join us and start driving today</p>

                {error && <div className="alert alert-danger">{error}</div>}

                <form onSubmit={handleSubmit} className="auth-form">
                    <div className="form-row">
                        <div className="form-group half">
                            <label htmlFor="firstName" className="label">First Name</label>
                            <input
                                id="firstName"
                                name="firstName"
                                type="text"
                                className="input"
                                value={formData.firstName}
                                onChange={handleChange}
                                placeholder="First Name"
                                required
                            />
                        </div>
                        <div className="form-group half">
                            <label htmlFor="lastName" className="label">Last Name</label>
                            <input
                                id="lastName"
                                name="lastName"
                                type="text"
                                className="input"
                                value={formData.lastName}
                                onChange={handleChange}
                                placeholder="Last Name"
                                required
                            />
                        </div>
                    </div>

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
                        <label htmlFor="phoneNumber" className="label">Phone Number</label>
                        <input
                            id="phoneNumber"
                            name="phoneNumber"
                            type="tel"
                            className="input"
                            value={formData.phoneNumber}
                            onChange={handleChange}
                            placeholder="+1234567890"
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
                            placeholder="Create a strong password"
                            required
                        />
                        <small className="text-muted" style={{ fontSize: '0.8rem', marginTop: '0.25rem', display: 'block' }}>
                            Min 8 chars, 1 uppercase, 1 lowercase, 1 number.
                        </small>
                    </div>

                    <div className="form-group">
                        <label htmlFor="confirmPassword" className="label">Confirm Password</label>
                        <input
                            id="confirmPassword"
                            name="confirmPassword"
                            type="password"
                            className="input"
                            value={formData.confirmPassword}
                            onChange={handleChange}
                            placeholder="Confirm your password"
                            required
                        />
                    </div>

                    <button type="submit" className="btn btn-primary btn-block" disabled={loading}>
                        {loading ? 'Creating Account...' : 'Create Account'}
                    </button>
                </form>

                <p className="auth-footer">
                    Already have an account? <Link to="/login">Sign in</Link>
                </p>
            </div>
        </div>
    );
}