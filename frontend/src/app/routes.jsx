import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { lazy } from 'react';
import ProtectedRoute from './ProtectedRoute';
import AdminRoute from './AdminRoute';
import Layout from '../components/Layout';

const Home = lazy(() => import('../pages/Home'));
const Login = lazy(() => import('../pages/Login'));
const Register = lazy(() => import('../pages/Register'));
const Dashboard = lazy(() => import('../pages/Dashboard'));
const CarList = lazy(() => import('../pages/CarList'));
const CarDetails = lazy(() => import('../pages/CarDetails'));
const AddCar = lazy(() => import('../pages/admin/AddCar'));
const EditCar = lazy(() => import('../pages/admin/EditCar'));
const Payment = lazy(() => import('../pages/Payment'));

export default function AppRoutes() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<Layout />}>
          <Route path="/" element={<Home />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />

          <Route element={<ProtectedRoute />}> {/* Protected routes */}
            <Route path="/dashboard" element={<Dashboard />} />
            <Route path="/cars" element={<CarList />} />
            <Route path="/cars/:id" element={<CarDetails />} />
            <Route path="/payment/:bookingId" element={<Payment />} />

            {/* Admin only routes */}
            <Route element={<AdminRoute />}>
              <Route path="/admin/add-car" element={<AddCar />} />
              <Route path="/admin/edit-car/:id" element={<EditCar />} />
            </Route>
          </Route>
        </Route>
      </Routes>
    </BrowserRouter>
  );
}
