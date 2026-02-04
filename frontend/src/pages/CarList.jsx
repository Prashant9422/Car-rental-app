import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { carService } from '../services/api';
import { getCarImage } from '../utils/imageUtils';
import './Car.css';

export default function CarList() {
    const [cars, setCars] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const [searchTerm, setSearchTerm] = useState('');

    useEffect(() => {
        const fetchCars = async () => {
            try {
                const response = await carService.getAll();
                // Check if response has data property (standard API wrapper) or is array direct
                setCars(response.data?.items || response.data || []);
            } catch (err) {
                console.error('Failed to fetch cars', err);
                setError('Failed to load cars. Please try again later.');
            } finally {
                setLoading(false);
            }
        };

        fetchCars();
    }, []);

    const filteredCars = cars.filter(car =>
        car.make.toLowerCase().includes(searchTerm.toLowerCase()) ||
        car.model.toLowerCase().includes(searchTerm.toLowerCase())
    );

    if (loading) return <div className="loader">Loading cars...</div>;
    if (error) return <div className="container page-content text-center text-danger">{error}</div>;

    return (
        <div className="container page-content">
            <div className="page-header">
                <h2>Available Cars</h2>
                <p>Choose from our premium collection</p>
                <div className="search-container">
                    <div className="search-wrapper">
                        <span className="search-icon">🔍</span>
                        <input
                            type="text"
                            placeholder="Search by make or model..."
                            className="input search-input"
                            value={searchTerm}
                            onChange={(e) => setSearchTerm(e.target.value)}
                        />
                    </div>
                </div>
            </div>

            <div className="car-grid">
                {filteredCars.length > 0 ? (
                    filteredCars.map(car => (
                        <div key={car.id} className="card car-card">
                            <div className="car-image-wrapper">
                                <img
                                    src={getCarImage(car)}
                                    alt={`${car.make} ${car.model}`}
                                    className="car-image"
                                    onError={(e) => {
                                        e.target.onerror = null; // Prevent infinite loop
                                        e.target.src = 'https://images.unsplash.com/photo-1568605114967-8130f3a36994?auto=format&fit=crop&q=80&w=800';
                                    }}
                                />
                                <div className="car-badge-container">
                                    <span className="car-badge">{car.status}</span>
                                </div>
                            </div>
                            <div className="car-info">
                                <h3>{car.make} {car.model}</h3>
                                <div className="car-specs">
                                    <span>👤 {car.seatingCapacity} Seats</span>
                                    <span>⚡ {car.fuelType || 'Gasoline'}</span>
                                </div>
                                <div className="car-footer">
                                    <span className="car-price">${car.dailyRate}<small>/day</small></span>
                                    <Link to={`/cars/${car.id}`} className="btn btn-primary btn-sm">
                                        View Details
                                    </Link>
                                </div>
                            </div>
                        </div>
                    ))
                ) : (
                    <div className="col-span-full text-center text-secondary">
                        <p>No cars found matching "{searchTerm}"</p>
                    </div>
                )}
            </div>
        </div>
    );
}