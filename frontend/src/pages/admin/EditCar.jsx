import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { carService } from '../../services/api';
import { getCarImage } from '../../utils/imageUtils';
import './AddCar.css'; // Reusing AddCar styles

export default function EditCar() {
    const { id } = useParams();
    const navigate = useNavigate();
    const [loading, setLoading] = useState(true);
    const [submitting, setSubmitting] = useState(false);
    const [error, setError] = useState(null);
    const [formData, setFormData] = useState({
        make: '',
        model: '',
        year: new Date().getFullYear(),
        licensePlate: '',
        color: '',
        mileage: 0,
        dailyRate: '',
        category: 0,
        description: '',
        seatingCapacity: 5,
        fuelType: 'Gasoline',
        transmission: 'Automatic',
        hasAirConditioning: true,
        hasGPS: false,
        imageUrl: '',
        status: 0 // Default 'Available'
    });

    useEffect(() => {
        const fetchCar = async () => {
            try {
                const response = await carService.getById(id);
                const car = response.data || response;

                // Map backend enum strings to integers if necessary, or just use values
                // Assuming backend returns strings for enums in getById, but expects ints/strings in update
                // Let's inspect the data during dev if needed. For now mapping straightforwardly.

                // Map backend enum strings to integers
                const statusMap = {
                    'Available': 0,
                    'Rented': 1,
                    'Maintenance': 2,
                    'Reserved': 3,
                    'OutOfService': 4
                };

                let loadedStatus = car.status;
                if (typeof loadedStatus === 'string' && statusMap[loadedStatus] !== undefined) {
                    loadedStatus = statusMap[loadedStatus];
                } else if (loadedStatus === undefined) {
                    loadedStatus = 0;
                }

                setFormData({
                    make: car.make,
                    model: car.model,
                    year: car.year,
                    licensePlate: car.licensePlate,
                    color: car.color,
                    mileage: car.mileage,
                    dailyRate: car.dailyRate,
                    category: car.category, // You might need to map string to int if backend returns string
                    description: car.description || '',
                    seatingCapacity: car.seatingCapacity,
                    fuelType: car.fuelType,
                    transmission: car.transmission,
                    hasAirConditioning: car.hasAirConditioning,
                    hasGPS: car.hasGPS,
                    imageUrl: car.imageUrl || '',
                    status: loadedStatus
                });
            } catch (err) {
                console.error('Failed to fetch car details', err);
                setError('Failed to load car details.');
            } finally {
                setLoading(false);
            }
        };
        fetchCar();
    }, [id]);

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: type === 'checkbox' ? checked : value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        console.log('Form Submit Triggered');
        // Debug check
        if (!formData.make || !formData.model) {
            alert('Validation Error: Make and Model are required');
            return;
        }

        setSubmitting(true);
        setError(null);

        try {
            console.log('Update initiated for ID:', id);
            if (!id) {
                alert('Error: Car ID is missing');
                return;
            }

            // Category Enum Mapping
            const categoryMap = {
                'Economy': 0, 'Compact': 1, 'Midsize': 2, 'Luxury': 3, 'SUV': 4, 'Minivan': 5
            };

            let categoryInt = parseInt(formData.category);
            if (isNaN(categoryInt)) {
                // Try to map from string
                if (categoryMap[formData.category] !== undefined) {
                    categoryInt = categoryMap[formData.category];
                } else {
                    categoryInt = 0; // Default to Economy if unknown
                }
            }

            const payload = {
                ...formData,
                year: parseInt(formData.year),
                mileage: parseInt(formData.mileage),
                dailyRate: parseFloat(formData.dailyRate),
                seatingCapacity: parseInt(formData.seatingCapacity),
                category: categoryInt,
                status: parseInt(formData.status) // Required by UpdateCarRequest
            };

            console.log('Sending Update Payload:', payload);

            await carService.update(id, payload);
            alert('Car updated successfully!');
            navigate(`/cars/${id}`);
        } catch (err) {
            console.error('Failed to update car', err);
            setError(err.response?.data?.message || 'Failed to update car. Please check inputs.');
        } finally {
            setSubmitting(false);
        }
    };

    if (loading) return <div className="loader">Loading car details...</div>;

    return (
        <div className="container page-content">
            <div className="add-car-container">
                <h2 className="section-title">Edit Vehicle</h2>

                {error && <div className="alert alert-danger">{error}</div>}

                <form onSubmit={handleSubmit} className="card add-car-form">
                    {/* Reuse form fields from AddCar - could extract to component but duplicating for speed now */}
                    <h3 className="form-section-title">Basic Information</h3>
                    <div className="form-grid">
                        <div className="form-group">
                            <label>Make</label>
                            <input name="make" value={formData.make} onChange={handleChange} required />
                        </div>
                        <div className="form-group">
                            <label>Model</label>
                            <input name="model" value={formData.model} onChange={handleChange} required />
                        </div>
                        <div className="form-group">
                            <label>Year</label>
                            <input type="number" name="year" value={formData.year} onChange={handleChange} required />
                        </div>
                        <div className="form-group">
                            <label>License Plate</label>
                            <input name="licensePlate" value={formData.licensePlate} onChange={handleChange} required />
                        </div>
                    </div>

                    <h3 className="form-section-title">Details & Pricing</h3>
                    <div className="form-grid">
                        <div className="form-group">
                            <label>Daily Rate ($)</label>
                            <input type="number" step="0.01" name="dailyRate" value={formData.dailyRate} onChange={handleChange} required />
                        </div>
                        <div className="form-group">
                            <label>Status</label>
                            <select name="status" value={formData.status} onChange={handleChange} className="input">
                                <option value="0">Available</option>
                                <option value="1">Rented</option>
                                <option value="2">Maintenance</option>
                                <option value="3">Reserved</option>
                                <option value="4">Out of Service</option>
                                {/* Handle string fallbacks from backend if needed */}
                                {['Available', 'Rented', 'Maintenance', 'Reserved', 'OutOfService'].map(s => (
                                    <option key={s} value={s}>{s}</option>
                                ))}
                            </select>
                        </div>
                        <div className="form-group">
                            <label>Category</label>
                            {/* Note: If backend returns "Compact" string, this select needs to handle string values too, or we map it */}
                            <select
                                name="category"
                                value={formData.category}
                                onChange={handleChange}
                                className="input"
                            >
                                <option value="0">Economy</option>
                                <option value="1">Compact</option>
                                <option value="2">Midsize</option>
                                <option value="3">Luxury</option>
                                <option value="4">SUV</option>
                                <option value="5">Minivan</option>
                                {/* Handle string fallbacks from backend */}
                                {['Economy', 'Compact', 'Midsize', 'Luxury', 'SUV', 'Minivan'].map(cat => (
                                    <option key={cat} value={cat}>{cat}</option>
                                ))}
                            </select>
                        </div>
                        <div className="form-group">
                            <label>Color</label>
                            <input name="color" value={formData.color} onChange={handleChange} required />
                        </div>
                        <div className="form-group">
                            <label>Mileage</label>
                            <input type="number" name="mileage" value={formData.mileage} onChange={handleChange} required />
                        </div>
                    </div>

                    <h3 className="form-section-title">Specifications</h3>
                    <div className="form-grid">
                        <div className="form-group">
                            <label>Transmission</label>
                            <select name="transmission" value={formData.transmission} onChange={handleChange}>
                                <option value="Automatic">Automatic</option>
                                <option value="Manual">Manual</option>
                            </select>
                        </div>
                        <div className="form-group">
                            <label>Fuel Type</label>
                            <select name="fuelType" value={formData.fuelType} onChange={handleChange}>
                                <option value="Gasoline">Gasoline</option>
                                <option value="Diesel">Diesel</option>
                                <option value="Electric">Electric</option>
                                <option value="Hybrid">Hybrid</option>
                            </select>
                        </div>
                        <div className="form-group">
                            <label>Seats</label>
                            <input type="number" name="seatingCapacity" value={formData.seatingCapacity} onChange={handleChange} required />
                        </div>
                        <div className="form-group">
                            <label>Image URL</label>
                            <input name="imageUrl" value={formData.imageUrl} onChange={handleChange} />
                            <div style={{ marginTop: '10px', height: '200px', overflow: 'hidden', borderRadius: '8px', border: '1px solid #ddd' }}>
                                <img
                                    src={getCarImage(formData)}
                                    alt="Car Preview"
                                    style={{ width: '100%', height: '100%', objectFit: 'cover' }}
                                    onError={(e) => e.target.style.display = 'none'}
                                />
                            </div>
                        </div>
                    </div>

                    <div className="form-row checkbox-row">
                        <label className="checkbox-label">
                            <input type="checkbox" name="hasAirConditioning" checked={formData.hasAirConditioning} onChange={handleChange} />
                            Has A/C
                        </label>
                        <label className="checkbox-label">
                            <input type="checkbox" name="hasGPS" checked={formData.hasGPS} onChange={handleChange} />
                            Has GPS
                        </label>
                    </div>

                    <div className="form-group">
                        <label>Description</label>
                        <textarea name="description" value={formData.description} onChange={handleChange} rows="3"></textarea>
                    </div>

                    <div className="form-actions">
                        <button type="button" className="btn btn-secondary" onClick={() => navigate(-1)}>Cancel</button>
                        <button
                            type="submit"
                            className="btn btn-primary"
                            disabled={submitting}
                            style={{ cursor: submitting ? 'not-allowed' : 'pointer' }}
                            onClick={() => console.log('Update button clicked')}
                        >
                            {submitting ? 'Updating...' : 'Update Car'}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}
