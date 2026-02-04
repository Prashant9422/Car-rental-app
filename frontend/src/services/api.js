import axios from 'axios';

const api = axios.create({
    baseURL: '/api', // Proxied via Vite to http://localhost:5000
    headers: {
        'Content-Type': 'application/json',
    },
});

// Request interceptor to add token
api.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('token');
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => Promise.reject(error)
);

// Callback to trigger logout in the app (e.g., from AuthContext)
let logoutCallback = null;

export const registerLogout = (callback) => {
    logoutCallback = callback;
};

// Response interceptor to handle 401
api.interceptors.response.use(
    (response) => response,
    (error) => {
        if (error.response?.status === 401) {
            localStorage.removeItem('token');
            localStorage.removeItem('user');
            if (logoutCallback) {
                logoutCallback();
            }
        }
        return Promise.reject(error);
    }
);

export const authService = {
    login: async (credentials) => {
        const response = await api.post('/Auth/login', credentials);
        return response.data;
    },
    register: async (userData) => {
        const response = await api.post('/Auth/register', userData);
        return response.data;
    },
};

// Car Service - Expanded
export const carService = {
    getAll: async (params) => {
        const response = await api.get('/Cars', { params });
        return response.data;
    },
    getById: async (id) => {
        const response = await api.get(`/Cars/${id}`);
        return response.data;
    },
    getAvailable: async (params) => {
        const response = await api.get('/Cars/available', { params });
        return response.data;
    },
    search: async (params) => {
        const response = await api.get('/Cars/search', { params });
        return response.data;
    },
    create: async (carData) => {
        const response = await api.post('/Cars', carData);
        return response.data;
    },
    update: async (id, carData) => {
        const response = await api.put(`/Cars/${id}`, carData);
        return response.data;
    },
    delete: async (id) => {
        const response = await api.delete(`/Cars/${id}`);
        return response.data;
    },
    updateStatus: async (id, status) => {
        const response = await api.patch(`/Cars/${id}/status`, status);
        return response.data;
    }
};

// Rental/Booking Service - Expanded
export const bookingService = {
    getAll: async () => {
        const response = await api.get('/Rentals');
        return response.data;
    },
    getById: async (id) => {
        const response = await api.get(`/Rentals/${id}`);
        return response.data;
    },
    getMyBookings: async (userId) => {
        const endpoint = userId ? `/Rentals/customer/${userId}` : '/Rentals';
        const response = await api.get(endpoint);
        return response.data;
    },
    getByStatus: async (status) => {
        const response = await api.get(`/Rentals/status/${status}`);
        return response.data;
    },
    getActive: async () => {
        const response = await api.get('/Rentals/active');
        return response.data;
    },
    getOverdue: async () => {
        const response = await api.get('/Rentals/overdue');
        return response.data;
    },
    create: async (bookingData) => {
        const response = await api.post('/Rentals', bookingData);
        return response.data;
    },
    confirm: async (id) => {
        const response = await api.post(`/Rentals/${id}/confirm`);
        return response.data;
    },
    start: async (id, odometer) => {
        const params = odometer ? `?odometerAtPickup=${odometer}` : '';
        const response = await api.post(`/Rentals/${id}/start${params}`);
        return response.data;
    },
    complete: async (id, returnData) => {
        const response = await api.post(`/Rentals/${id}/complete`, returnData);
        return response.data;
    },
    cancel: async (id) => {
        const response = await api.post(`/Rentals/${id}/cancel`);
        return response.data;
    }
};

// User Service
export const userService = {
    getAllCustomers: async () => {
        const response = await api.get('/Users/customers');
        return response.data;
    },
    getById: async (id) => {
        const response = await api.get(`/Users/${id}`);
        return response.data;
    },
    update: async (id, userData) => {
        const response = await api.put(`/Users/${id}`, userData);
        return response.data;
    },
    delete: async (id) => {
        const response = await api.delete(`/Users/${id}`);
        return response.data;
    },
    activate: async (id) => {
        const response = await api.post(`/Users/${id}/activate`);
        return response.data;
    },
    deactivate: async (id) => {
        const response = await api.post(`/Users/${id}/deactivate`);
        return response.data;
    }
};

// Payment Service
export const paymentService = {
    getById: async (id) => {
        const response = await api.get(`/Payments/${id}`);
        return response.data;
    },
    getByRental: async (rentalId) => {
        const response = await api.get(`/Payments/rental/${rentalId}`);
        return response.data;
    },
    create: async (paymentData) => {
        const response = await api.post('/Payments', paymentData);
        return response.data;
    },
    process: async (id) => {
        const response = await api.post(`/Payments/${id}/process`);
        return response.data;
    },
    refund: async (id, amount) => {
        const params = amount ? `?amount=${amount}` : '';
        const response = await api.post(`/Payments/${id}/refund${params}`);
        return response.data;
    }
};

export default api;
