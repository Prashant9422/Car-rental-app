import { useState, useEffect } from 'react';
import { useAuth } from '../auth/useAuth';
import { Link } from 'react-router-dom';
import { bookingService, userService } from '../services/api';
import './Dashboard.css';

export default function Dashboard() {
    const { user, updateUser } = useAuth();
    const [bookings, setBookings] = useState([]);
    const [loading, setLoading] = useState(true);
    const [isEditing, setIsEditing] = useState(false);
    const [editForm, setEditForm] = useState({
        firstName: '',
        lastName: '',
        email: '',
        phoneNumber: '',
        street: '',
        city: '',
        state: '',
        postalCode: '',
        country: '',
        driversLicenseNumber: ''
    });
    const [message, setMessage] = useState('');

    useEffect(() => {
        const fetchBookings = async () => {
            try {
                // Pass the userId to the service to fetch specific rentals
                const response = await bookingService.getMyBookings(user.userId);
                // Assuming response.data.items or response.data contains the list
                setBookings(response.data?.items || response.data || []);
            } catch (err) {
                console.error('Failed to fetch bookings', err);
            } finally {
                setLoading(false);
            }
        };

        if (user) {
            fetchBookings();
        }
    }, [user]);

    const handleEditClick = async () => {
        setIsEditing(true);
        setMessage('');
        try {
            const userData = await userService.getById(user.userId);
            // Result is wrapped in ApiResponse usually, check structure if needed.
            // Assuming userService.getById returns response.data which is the UserDto
            // based on api.js implementation:
            // getById: async (id) => { const response = await api.get(`/Users/${id}`); return response.data; },
            // If API returns { data: UserDto, success: true }, then we might need userData.data
            // Let's assume response.data IS the payload based on other service calls?
            // Wait, api.js returns response.data. 
            // Controllers return ApiResponse<T>. So response.data is ApiResponse<T>.
            // So we need userData.data to get the UserDto.

            const userDto = userData.data || userData; // Fallback if structure varies

            setEditForm({
                firstName: userDto.firstName || '',
                lastName: userDto.lastName || '',
                email: userDto.email || '',
                phoneNumber: userDto.phoneNumber || '',
                street: userDto.street || '',
                city: userDto.city || '',
                state: userDto.state || '',
                postalCode: userDto.postalCode || '',
                country: userDto.country || '',
                driversLicenseNumber: userDto.driversLicenseNumber || ''
            });
        } catch (err) {
            console.error('Failed to fetch user details', err);
            setMessage('Failed to load profile details.');
            setIsEditing(false);
        }
    };

    const handleCancel = () => {
        setIsEditing(false);
        setMessage('');
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        setEditForm(prev => ({ ...prev, [name]: value }));
    };

    const handleSave = async (e) => {
        e.preventDefault();
        setMessage('');
        try {
            // Prepare UpdateUserRequest
            // We need to match the DTO properties exactly? JS is case sensitive?
            // The DTO properties are PascalCase in C#, but typically JSON serialization makes them camelCase by default in .NET Core unless configured otherwise.
            // Let's assume camelCase for the JSON payload keys which the backend binds to the PascalCase DTO.

            const updateRequest = {
                firstName: editForm.firstName,
                lastName: editForm.lastName,
                phoneNumber: editForm.phoneNumber,
                street: editForm.street,
                city: editForm.city,
                state: editForm.state,
                postalCode: editForm.postalCode,
                country: editForm.country,
                driversLicenseNumber: editForm.driversLicenseNumber
            };

            await userService.update(user.userId, updateRequest);

            // Update local context
            const newFullName = `${editForm.firstName} ${editForm.lastName}`.trim();
            updateUser({
                ...updateRequest,
                fullName: newFullName,
                email: editForm.email // Email update might require backend support, DTO doesn't have Email! 
                // Checking UpdateUserRequest.cs: It does NOT have Email. 
                // So we cannot update email here.
            });

            setMessage('Profile updated successfully!');
            setTimeout(() => {
                setIsEditing(false);
                setMessage('');
            }, 1000);

        } catch (err) {
            console.error('Failed to update profile', err);
            setMessage('Failed to update profile. Please try again.');
        }
    };

    if (loading) return <div className="loader">Loading dashboard...</div>;

    return (
        <div className="container page-content">
            <div className="dashboard-header">
                <h1>Dashboard</h1>
                <p>Welcome back, <span className="text-primary">{user?.fullName || user?.username || 'Driver'}</span></p>
            </div>

            <div className="dashboard-grid">
                {user?.role !== 'Admin' && (
                    <div className="dashboard-section card">
                        <h3>Your Bookings</h3>
                        {bookings.length > 0 ? (
                            <div className="bookings-list">
                                {bookings.map(booking => (
                                    <div key={booking.id} className="booking-item">
                                        <div className="booking-info">
                                            <h4>Booking #{booking.id.substring(0, 8)}</h4>
                                            <div className="text-primary" style={{ fontWeight: 600, marginBottom: '0.25rem' }}>
                                                🚗 {booking.carInfo || 'Car Details Unavailable'}
                                            </div>
                                            <span className="booking-date">
                                                {new Date(booking.startDate).toLocaleDateString()} - {new Date(booking.endDate).toLocaleDateString()}
                                            </span>
                                        </div>
                                        <div className="booking-status">
                                            <span className={`status-badge status-${(booking.status === 0 || booking.status === 'Pending') ? 'pending' : (booking.status === 1 || booking.status === 'Active' || booking.status === 'Confirmed') ? 'active' : 'completed'}`}>
                                                {booking.status}
                                            </span>
                                            <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'flex-end', gap: '0.5rem' }}>
                                                <span className="booking-price">${booking.totalCost}</span>
                                                {(booking.status === 0 || booking.status === 'Pending') && (
                                                    <Link to={`/payment/${booking.id}`} className="btn btn-primary btn-sm">
                                                        Pay Now
                                                    </Link>
                                                )}
                                            </div>
                                        </div>
                                    </div>
                                ))}
                            </div>
                        ) : (
                            <div className="empty-state">
                                <p>No actual bookings found from API.</p>
                                <Link to="/cars" className="btn btn-primary btn-sm">Browse Cars</Link>
                            </div>
                        )}
                    </div>
                )}
                {user?.role === 'Admin' && (
                    <div className="dashboard-section card">
                        <h3>Admin Controls</h3>
                        <p>Welcome, Administrator. Use the navigation to manage vehicles.</p>
                        <div style={{ marginTop: '1rem', display: 'flex', gap: '1rem' }}>
                            <Link to="/cars" className="btn btn-primary">Manage Cars</Link>
                            <Link to="/admin/add-car" className="btn btn-secondary">Add New Car</Link>
                        </div>
                    </div>
                )}

                <div className="dashboard-sidebar">
                    <div className="card profile-card">
                        <h3>Profile</h3>
                        {message && <div className={`alert ${message.includes('success') ? 'alert-success' : 'alert-error'}`} style={{ padding: '0.5rem', marginBottom: '1rem', fontSize: '0.9rem' }}>{message}</div>}

                        {!isEditing ? (
                            <>
                                <div className="profile-details">
                                    <div className="profile-row">
                                        <span className="label">Name</span>
                                        <span className="value">{user?.fullName || 'N/A'}</span>
                                    </div>
                                    <div className="profile-row">
                                        <span className="label">Email</span>
                                        <span className="value">{user?.email || 'N/A'}</span>
                                    </div>
                                    <div className="profile-row">
                                        <span className="label">Role</span>
                                        <span className="value">{user?.role || 'User'}</span>
                                    </div>
                                </div>
                                <button
                                    onClick={handleEditClick}
                                    className="btn btn-secondary btn-block btn-sm"
                                    disabled={user?.role === 'Admin'}
                                    title={user?.role === 'Admin' ? 'Admins cannot edit their profile here' : ''}
                                >
                                    {user?.role === 'Admin' ? 'Edit Profile' : 'Edit Profile'}
                                </button>
                            </>
                        ) : (
                            <form onSubmit={handleSave} className="edit-profile-form">
                                <div className="form-grid-row">
                                    <div className="form-group">
                                        <label className="label">First Name</label>
                                        <input type="text" name="firstName" className="input" value={editForm.firstName} onChange={handleChange} required />
                                    </div>
                                    <div className="form-group">
                                        <label className="label">Last Name</label>
                                        <input type="text" name="lastName" className="input" value={editForm.lastName} onChange={handleChange} required />
                                    </div>
                                </div>
                                <div className="form-group">
                                    <label className="label">Phone</label>
                                    <input type="tel" name="phoneNumber" className="input" value={editForm.phoneNumber} onChange={handleChange} />
                                </div>
                                <div className="form-group">
                                    <label className="label">City</label>
                                    <input type="text" name="city" className="input" value={editForm.city} onChange={handleChange} />
                                </div>

                                <div className="form-actions" style={{ display: 'flex', gap: '0.75rem', marginTop: '1.5rem' }}>
                                    <button type="submit" className="btn btn-primary btn-sm" style={{ flex: 1 }}>Save Changes</button>
                                    <button type="button" onClick={handleCancel} className="btn btn-secondary btn-sm" style={{ flex: 1 }}>Cancel</button>
                                </div>
                            </form>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
}