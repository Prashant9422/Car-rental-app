import { Suspense } from 'react';
import AppRoutes from './routes';
import { AuthProvider } from '../auth/AuthContext';

export default function App() {
  return (
    <Suspense fallback={<div className="loader">Loading...</div>}>
      <AuthProvider>
        <AppRoutes />
      </AuthProvider>
    </Suspense>
  );
}
