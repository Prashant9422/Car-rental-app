// Native fetch implementation for Node 18+
const API_URL = 'http://localhost:5000/api';
// ID from previous Add Car test, or replace with a valid one if known.
// I will fetch all cars first to be safe.
const TARGET_CAR_ID = '24ac7f40-9df9-40e6-a2c7-f47df2f933f1';

async function testUpdateCar() {
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
            throw new Error(`Login failed: ${loginRes.status}`);
        }

        const loginData = await loginRes.json();
        const token = loginData.data.token;
        console.log('Login successful.');

        // 2. Try to Update Car
        // Providing payload similar to what EditCar.jsx constructs
        // Note: EditCar sends integers for Year, Mileage, DailyRate, SeatingCapacity, Status
        // But for Category, it tries int first, else string.

        // Let's try sending what I suspect is failing.
        const updatePayload = {
            make: 'TestMakeUpdated',
            model: 'TestModelUpdated',
            year: 2026,
            licensePlate: 'TEST-UPD',
            color: 'Red',
            mileage: 200,
            dailyRate: 150.50,
            category: 1, // Compact (as int)
            // category: "Compact", // Try this if int fails? But EditCar prefers int.
            description: 'Updated Description',
            seatingCapacity: 4,
            fuelType: 'Gasoline',
            transmission: 'Automatic',
            hasAirConditioning: true,
            hasGPS: true,
            imageUrl: 'http://example.com/update.jpg',
            status: 0 // Available
        };

        console.log(`Updating Car ${TARGET_CAR_ID}...`);
        const updateRes = await fetch(`${API_URL}/Cars/${TARGET_CAR_ID}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(updatePayload)
        });

        const resText = await updateRes.text();
        console.log('Update Status:', updateRes.status);
        console.log('Update Response Body:', resText);

        if (!updateRes.ok) {
            throw new Error('Update failed');
        }

        console.log('Update Successful!');

    } catch (error) {
        console.error('Test Failed:', error.message);
    }
}

testUpdateCar();
