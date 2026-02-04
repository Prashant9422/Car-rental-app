import './Footer.css';

export default function Footer() {
    return (
        <footer className="footer">
            <div className="container footer-content">
                <div className="footer-section">
                    <h3 className="footer-logo">CarRental.</h3>
                    <p>Premium car rental service for your journey.</p>
                </div>
                <div className="footer-section">
                    <h4>Links</h4>
                    <a href="/cars">Browse Cars</a>
                    <a href="/login">Login</a>
                    <a href="/register">Register</a>
                </div>
                <div className="footer-section">
                    <h4>Contact</h4>
                    <p>support@carrental.com</p>
                    <p>+1 (555) 123-4567</p>
                </div>
            </div>
            <div className="footer-bottom">
                <div className="container">
                    <p>&copy; {new Date().getFullYear()} CarRental Inc. All rights reserved.</p>
                </div>
            </div>
        </footer>
    );
}
