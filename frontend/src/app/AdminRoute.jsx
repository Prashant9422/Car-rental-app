import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '../auth/useAuth';

export default function AdminRoute() {
    const { user } = useAuth();

    if (!user) {
        return <Navigate to="/login" />;
    }

    // Assuming role comes as string enum from backend
    if (user.role !== 'Admin') {
        return <Navigate to="/dashboard" />;
    }

    return <Outlet />;
}
