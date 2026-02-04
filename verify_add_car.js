// Native fetch implementation for Node 18+
const API_URL = 'http://localhost:5000/api'; // Proxied to 5000 directly

async function testAddCar() {
    try {
        // 1. Login as Admin
        console.log('Logging in as Admin...');
        const loginRes = await fetch(`${API_URL}/Auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                email: 'admin@carrental.com',
                password: 'Admin@123'
            })
        });

        if (!loginRes.ok) {
            const err = await loginRes.text();
            throw new Error(`Login failed: ${loginRes.status} ${err}`);
        }

        const loginData = await loginRes.json();
        const token = loginData.data.token;
        console.log('Login successful. Token received.');

        // 2. Add Car
        console.log('Adding new car...');
        const carData = {
            make: 'TestMake',
            model: 'TestModel',
            year: 2025,
            licensePlate: 'TEST-' + Math.floor(Math.random() * 10000),
            color: 'TestColor',
            mileage: 100,
            dailyRate: 99.99,
            category: 1, // Compact
            description: 'Test Description',
            seatingCapacity: 4,
            fuelType: 'Gasoline',
            transmission: 'Automatic',
            hasAirConditioning: true,
            hasGPS: true,
            imageUrl: 'http://example.com/test.jpg'
        };

        const addCarRes = await fetch(`${API_URL}/Cars`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(carData)
        });

        if (!addCarRes.ok) {
            const err = await addCarRes.text();
            throw new Error(`Add Car failed: ${addCarRes.status} ${err}`);
        }

        const addCarData = await addCarRes.json();
        if (addCarData.success) {
            console.log('Car added successfully:', addCarData.data);
        } else {
            console.error('Failed to add car (API Success=false):', addCarData);
        }

    } catch (error) {
        console.error('Test Failed:', error.message);
    }
}

testAddCar();
