import { useState, useEffect } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import { carService, bookingService } from '../services/api';
import { useAuth } from '../auth/useAuth';
import { getCarImage } from '../utils/imageUtils';
import './Car.css';

// Helper to format date for input (YYYY-MM-DD)
const formatDate = (date) => {
    // Use local time components to avoid timezone offsets shifting the day
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
};

export default function CarDetails() {
    const { id } = useParams();
    const navigate = useNavigate();
    const { user } = useAuth();
    const [car, setCar] = useState(null);
    const [loading, setLoading] = useState(true);
    const [bookingLoading, setBookingLoading] = useState(false);
    const [error, setError] = useState(null);

    // Booking state
    const [startDate, setStartDate] = useState(formatDate(new Date()));
    const [endDate, setEndDate] = useState(formatDate(new Date(Date.now() + 86400000))); // Default +1 day

    useEffect(() => {
        const fetchCar = async () => {
            try {
                const response = await carService.getById(id);
                setCar(response.data || response);
            } catch (err) {
                console.error('Failed to fetch car details', err);
                setError('Failed to load car details.');
            } finally {
                setLoading(false);
            }
        };

        fetchCar();
    }, [id]);

    const calculateTotal = () => {
        if (!car) return 0;
        const start = new Date(startDate);
        const end = new Date(endDate);
        const diffTime = Math.abs(end - start);
        const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
        // Ensure at least 1 day
        const days = diffDays > 0 ? diffDays : 1;
        return (days * car.dailyRate).toFixed(2);
    };

    const handleBookNow = async () => {
        if (!user) {
            alert('Please log in to book a vehicle.');
            navigate('/login');
            return;
        }

        // Validate user session integrity
        if (!user.userId) {
            console.error('User session missing userId:', user);
            alert('Your session is invalid or expired. Please Log Out and Log In again to refresh your credentials.');
            // Optional: navigate('/login') or logout()
            return;
        }

        const totalCost = calculateTotal();
        const confirmBooking = window.confirm(`Confirm booking for ${car.make} ${car.model}?\n\nFrom: ${startDate}\nTo: ${endDate}\nTotal Cost: $${totalCost}`);
        if (!confirmBooking) return;

        setBookingLoading(true);
        const bookingPayload = {
            carId: id,
            customerId: user.userId,
            startDate: new Date(startDate).toISOString(),
            endDate: new Date(endDate).toISOString(),
            pickupLocation: "Main Office",
            returnLocation: "Main Office"
        };
        console.log('Sending Booking Payload:', bookingPayload);

        try {
            await bookingService.create(bookingPayload);
            alert('Booking created successfully! View it in your Dashboard.');
            navigate('/dashboard');
        } catch (err) {
            console.error('Booking failed', err);
            // Default to generic message
            let errorMessage = 'Failed to create booking.';

            if (err.response) {
                // If backend returns a message
                if (err.response.data?.message) {
                    errorMessage = err.response.data.message;
                }
                // If backend returns validation errors (standard ASP.NET format)
                if (err.response.data?.errors) {
                    const validationErrors = Object.values(err.response.data.errors).flat().join('\n');
                    errorMessage += `\n${validationErrors}`;
                }
            }

            setError(errorMessage);
        } finally {
            setBookingLoading(false);
        }
    };

    const handleContactSupport = () => {
        alert('Support contact: support@carrental.com\n\nCall us at: +1 (555) 123-4567');
    };

    if (loading) return <div className="loader">Loading details...</div>;
    if (error || !car) {
        return (
            <div className="container page-content text-center">
                <h2>{error || 'Car not found'}</h2>
                <Link to="/cars" className="btn btn-primary">Back to Cars</Link>
            </div>
        );
    }

    return (
        <div className="container page-content">
            <Link to="/cars" className="back-link">← Back to Cars</Link>

            <div className="car-details-grid">
                <div className="details-image">
                    <img src={getCarImage(car)} alt={`${car.make} ${car.model}`} />
                </div>

                <div className="details-info card">
                    <div className="details-header">
                        <h1>{car.make} {car.model}</h1>
                        <span className="details-price">${car.dailyRate} <small>/ day</small></span>
                    </div>

                    <div className="details-tags">
                        <span className="tag">{car.category}</span>
                        <span className="tag">{car.seatingCapacity} Seats</span>
                        <span className="tag">{car.transmission}</span>
                        {car.hasGPS && <span className="tag">GPS</span>}
                        {car.hasAirConditioning && <span className="tag">A/C</span>}
                    </div>

                    <p className="details-description">{car.description || 'No description available for this vehicle.'}</p>

                    {/* Booking Form - Hidden for Admins */}
                    {user?.role !== 'Admin' && (
                        <>
                            <div className="booking-form" style={{ margin: '1.5rem 0', padding: '1rem', background: '#f8fafc', borderRadius: '8px' }}>
                                <h3 style={{ fontSize: '1.1rem', marginBottom: '1rem' }}>Select Rental Dates</h3>
                                <div className="form-group" style={{ marginBottom: '1rem' }}>
                                    <label style={{ display: 'block', fontSize: '0.9rem', marginBottom: '0.5rem' }}>Pick-up Date</label>
                                    <input
                                        type="date"
                                        className="input"
                                        value={startDate}
                                        min={formatDate(new Date())}
                                        onChange={(e) => setStartDate(e.target.value)}
                                    />
                                </div>
                                <div className="form-group" style={{ marginBottom: '1rem' }}>
                                    <label style={{ display: 'block', fontSize: '0.9rem', marginBottom: '0.5rem' }}>Return Date</label>
                                    <input
                                        type="date"
                                        className="input"
                                        value={endDate}
                                        min={startDate}
                                        onChange={(e) => setEndDate(e.target.value)}
                                    />
                                </div>
                                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', fontWeight: 'bold', borderTop: '1px solid #e2e8f0', paddingTop: '0.5rem' }}>
                                    <span>Total Estimate:</span>
                                    <span style={{ color: '#2563eb', fontSize: '1.25rem' }}>${calculateTotal()}</span>
                                </div>
                            </div>

                            <div className="details-actions">
                                <button
                                    onClick={handleBookNow}
                                    disabled={bookingLoading || (car.status !== 0 && car.status !== 'Available')}
                                    className="btn btn-primary btn-block btn-lg"
                                >
                                    {bookingLoading ? 'Processing...' : (car.status === 0 || car.status === 'Available' ? 'Book Now' : 'Not Available')}
                                </button>
                                <button onClick={handleContactSupport} className="btn btn-secondary btn-block">
                                    Contact Support
                                </button>
                            </div>
                        </>
                    )}
                    {/* Admin Actions */}
                    {user?.role === 'Admin' && (
                        <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem' }}>
                            <button
                                className="btn btn-secondary"
                                onClick={() => {
                                    console.log('Navigating to edit car:', id);
                                    navigate(`/admin/edit-car/${id}`);
                                }}
                                style={{ width: '100%', cursor: 'pointer' }}
                            >
                                Edit Car
                            </button>
                            <button
                                className="btn btn-danger"
                                onClick={async () => {
                                    if (window.confirm('Are you sure you want to DELETE this vehicle? This action cannot be undone.')) {
                                        try {
                                            await carService.delete(id);
                                            alert('Car deleted successfully.');
                                            navigate('/cars');
                                        } catch (err) {
                                            console.error('Delete failed', err);
                                            alert('Failed to delete car. It might have active rentals.');
                                        }
                                    }
                                }}
                                style={{ width: '100%', cursor: 'pointer', backgroundColor: '#dc2626', color: 'white', border: 'none' }}
                            >
                                Delete Car
                            </button>
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
}