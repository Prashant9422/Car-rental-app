import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { bookingService, paymentService } from '../services/api';
import './Payment.css';

export default function Payment() {
    const { bookingId } = useParams();
    const navigate = useNavigate();

    const [booking, setBooking] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [processing, setProcessing] = useState(false);

    // Payment Method Enum: CreditCard=0, DebitCard=1, Cash=2, BankTransfer=3, PayPal=4, ApplePay=5, GooglePay=6
    const [paymentMethod, setPaymentMethod] = useState('0');
    const [cardDetails, setCardDetails] = useState({
        number: '',
        expiry: '',
        cvc: '',
        name: ''
    });

    useEffect(() => {
        const fetchBooking = async () => {
            try {
                const response = await bookingService.getById(bookingId);
                // Handle different response structures if needed, assuming response.data or direct
                const data = response.data || response;
                setBooking(data);

                // If booking is already paid/active, redirect
                if (data.status !== 0 && data.status !== 'Pending') {
                    setError("This booking is already processed or completed.");
                }
            } catch (err) {
                console.error('Failed to fetch booking', err);
                setError('Failed to load booking details.');
            } finally {
                setLoading(false);
            }
        };

        if (bookingId) {
            fetchBooking();
        }
    }, [bookingId]);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setCardDetails(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setProcessing(true);
        setError(null);

        try {
            // 1. Create Payment Record
            const paymentRequest = {
                rentalId: bookingId,
                amount: booking.totalCost,
                paymentMethod: parseInt(paymentMethod),
                description: `Payment for Booking #${bookingId.substring(0, 8)}`
            };

            const paymentResponse = await paymentService.create(paymentRequest);
            const paymentId = paymentResponse.data?.id || paymentResponse.id;

            // 2. Process Payment (Mock processing on server usually happens auto or via this endpoint)
            await paymentService.process(paymentId);

            // 3. Confirm Rental (if not auto-done by backend logic, usually payment confirms it)
            // Checking business logic: often payment success -> auto confirm or separate call.
            // Let's assume process payment is enough, or we might need rental confirm.
            // Safest: Try to confirm rental too if status doesn't change implicitly.
            try {
                await bookingService.confirm(bookingId);
            } catch (confirmError) {
                console.warn("Auto-confirm skipped or failed, might already be active", confirmError);
            }

            // Success Redirect
            navigate('/dashboard', { state: { message: 'Payment successful! Your booking is now active.' } });

        } catch (err) {
            console.error('Payment failed', err);
            setError('Payment failed. Please check your details and try again.');
            setProcessing(false);
        }
    };

    if (loading) return <div className="loader">Loading payment details...</div>;
    if (error && !booking) return <div className="container page-content text-center text-danger">{error}</div>;

    return (
        <div className="container page-content">
            <div className="payment-container">
                <div className="payment-header">
                    <h2>Secure Payment</h2>
                    <p>Complete your booking for <strong>{booking?.carInfo}</strong></p>
                </div>

                <div className="payment-grid">
                    <div className="order-summary card">
                        <h3>Order Summary</h3>
                        <div className="summary-row">
                            <span>Booking Reference</span>
                            <span>#{booking?.id.substring(0, 8)}</span>
                        </div>
                        <div className="summary-row">
                            <span>Car</span>
                            <span>{booking?.carInfo}</span>
                        </div>
                        <div className="summary-row">
                            <span>Dates</span>
                            <span>
                                {new Date(booking?.startDate).toLocaleDateString()} - {new Date(booking?.endDate).toLocaleDateString()}
                            </span>
                        </div>
                        <div className="summary-row">
                            <span>Duration</span>
                            <span>{booking?.rentalDays} Days</span>
                        </div>
                        <div className="summary-divider"></div>
                        <div className="summary-total">
                            <span>Total Amount</span>
                            <span>${booking?.totalCost}</span>
                        </div>
                    </div>

                    <div className="payment-form-card card">
                        <h3>Payment Details</h3>
                        {error && <div className="alert alert-error">{error}</div>}

                        <form onSubmit={handleSubmit}>
                            <div className="form-group">
                                <label className="label">Payment Method</label>
                                <select
                                    className="input"
                                    value={paymentMethod}
                                    onChange={(e) => setPaymentMethod(e.target.value)}
                                >
                                    <option value="0">Credit Card</option>
                                    <option value="1">Debit Card</option>
                                    <option value="2">Cash</option>
                                    <option value="3">Bank Transfer</option>
                                    <option value="4">PayPal</option>
                                    <option value="5">Apple Pay</option>
                                    <option value="6">Google Pay</option>
                                </select>
                            </div>

                            {(paymentMethod === '0' || paymentMethod === '1') && (
                                <>
                                    <div className="form-group">
                                        <label className="label">Card Number</label>
                                        <input
                                            type="text"
                                            name="number"
                                            className="input"
                                            placeholder="0000 0000 0000 0000"
                                            value={cardDetails.number}
                                            onChange={handleInputChange}
                                            required
                                        />
                                    </div>
                                    <div className="form-grid-row">
                                        <div className="form-group">
                                            <label className="label">Expiry Date</label>
                                            <input
                                                type="text"
                                                name="expiry"
                                                className="input"
                                                placeholder="MM/YY"
                                                value={cardDetails.expiry}
                                                onChange={handleInputChange}
                                                required
                                            />
                                        </div>
                                        <div className="form-group">
                                            <label className="label">CVC</label>
                                            <input
                                                type="text"
                                                name="cvc"
                                                className="input"
                                                placeholder="123"
                                                value={cardDetails.cvc}
                                                onChange={handleInputChange}
                                                required
                                            />
                                        </div>
                                    </div>
                                    <div className="form-group">
                                        <label className="label">Cardholder Name</label>
                                        <input
                                            type="text"
                                            name="name"
                                            className="input"
                                            placeholder="John Doe"
                                            value={cardDetails.name}
                                            onChange={handleInputChange}
                                            required
                                        />
                                    </div>
                                </>
                            )}

                            <button
                                type="submit"
                                className="btn btn-primary btn-block btn-lg"
                                style={{ marginTop: '1.5rem' }}
                                disabled={processing}
                            >
                                {processing ? 'Processing...' : `Pay $${booking?.totalCost}`}
                            </button>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    );
}
