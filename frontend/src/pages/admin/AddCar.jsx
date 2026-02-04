import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { carService } from '../../services/api';
import './AddCar.css';

export default function AddCar() {
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [formData, setFormData] = useState({
        make: '',
        model: '',
        year: new Date().getFullYear(),
        licensePlate: '',
        color: '',
        mileage: 0,
        dailyRate: '',
        category: 0, // Default to Economy/Standard (enum)
        description: '',
        seatingCapacity: 5,
        fuelType: 'Gasoline',
        transmission: 'Automatic',
        hasAirConditioning: true,
        hasGPS: false,
        imageUrl: ''
    });

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: type === 'checkbox' ? checked : value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError(null);

        try {
            // Convert types as needed by backend (e.g. string to number)
            const payload = {
                ...formData,
                year: parseInt(formData.year),
                mileage: parseInt(formData.mileage),
                dailyRate: parseFloat(formData.dailyRate),
                seatingCapacity: parseInt(formData.seatingCapacity),
                category: parseInt(formData.category)
            };

            await carService.create(payload);
            alert('Car added successfully!');
            navigate('/cars');
        } catch (err) {
            console.error('Failed to add car', err);
            setError(err.response?.data?.message || 'Failed to add car. Please check inputs.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="container page-content">
            <div className="add-car-container">
                <h2 className="section-title">Add New Vehicle</h2>

                {error && <div className="alert alert-danger">{error}</div>}

                <form onSubmit={handleSubmit} className="card add-car-form">
                    {/* Basic Info */}
                    <h3 className="form-section-title">Basic Information</h3>
                    <div className="form-grid">
                        <div className="form-group">
                            <label>Make</label>
                            <input name="make" value={formData.make} onChange={handleChange} required placeholder="e.g. Toyota" />
                        </div>
                        <div className="form-group">
                            <label>Model</label>
                            <input name="model" value={formData.model} onChange={handleChange} required placeholder="e.g. Camry" />
                        </div>
                        <div className="form-group">
                            <label>Year</label>
                            <input type="number" name="year" value={formData.year} onChange={handleChange} required />
                        </div>
                        <div className="form-group">
                            <label>License Plate</label>
                            <input name="licensePlate" value={formData.licensePlate} onChange={handleChange} required placeholder="ABC-1234" />
                        </div>
                    </div>

                    {/* Details */}
                    <h3 className="form-section-title">Details & Pricing</h3>
                    <div className="form-grid">
                        <div className="form-group">
                            <label>Daily Rate ($)</label>
                            <input type="number" step="0.01" name="dailyRate" value={formData.dailyRate} onChange={handleChange} required />
                        </div>
                        <div className="form-group">
                            <label>Category</label>
                            <select name="category" value={formData.category} onChange={handleChange}>
                                <option value="0">Economy</option>
                                <option value="1">Compact</option>
                                <option value="2">Midsize</option>
                                <option value="3">Luxury</option>
                                <option value="4">SUV</option>
                                <option value="5">Minivan</option>
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

                    {/* Specs */}
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
                            <input name="imageUrl" value={formData.imageUrl} onChange={handleChange} placeholder="https://..." />
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
                        <button type="submit" className="btn btn-primary" disabled={loading}>
                            {loading ? 'Adding Vehicle...' : 'Add Vehicle'}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}
