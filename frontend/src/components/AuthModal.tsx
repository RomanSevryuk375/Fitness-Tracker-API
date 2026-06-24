import { useState } from 'react';
import Modal from './Modal';
import { auth } from '../api';

interface AuthModalProps {
  isOpen: boolean;
  onSuccess: () => void;
}

export default function AuthModal({ isOpen, onSuccess }: AuthModalProps) {
  const [isLogin, setIsLogin] = useState(true);
  const [login, setLogin] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    try {
      if (isLogin) {
        const data = await auth.login({ login, password });
        localStorage.setItem('token', data.token || data.Token || data);
        onSuccess();
      } else {
        await auth.register({ login, password });
        // Auto login after register or ask to login
        const data = await auth.login({ login, password });
        localStorage.setItem('token', data.token || data.Token || data);
        onSuccess();
      }
    } catch (err: any) {
      setError(err.response?.data?.message || err.message || 'Authentication failed');
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={() => {}} title={isLogin ? "Welcome Back" : "Create Account"}>
      <form onSubmit={handleSubmit} className="space-y-4">
        {error && <div className="p-3 bg-red-50 text-red-600 rounded-lg text-sm border border-red-200">{error}</div>}
        
        <div>
          <label className="block text-sm font-medium text-slate-700 mb-1">Login</label>
          <input 
            type="text" 
            required
            value={login}
            onChange={(e) => setLogin(e.target.value)}
            className="w-full bg-slate-50 border border-slate-200 rounded-lg px-4 py-2.5 text-slate-800 focus:outline-none focus:ring-2 focus:ring-primary transition-all"
            placeholder="Username or email"
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-slate-700 mb-1">Password</label>
          <input 
            type="password" 
            required
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            className="w-full bg-slate-50 border border-slate-200 rounded-lg px-4 py-2.5 text-slate-800 focus:outline-none focus:ring-2 focus:ring-primary transition-all"
            placeholder="••••••••"
          />
        </div>

        <div className="pt-4">
          <button type="submit" className="w-full btn-primary py-3">
            {isLogin ? "Sign In" : "Sign Up"}
          </button>
        </div>
        
        <p className="text-center text-sm text-slate-500 mt-4">
          {isLogin ? "Don't have an account?" : "Already have an account?"}{' '}
          <button 
            type="button" 
            onClick={() => setIsLogin(!isLogin)} 
            className="text-primary font-medium hover:underline"
          >
            {isLogin ? "Sign Up" : "Sign In"}
          </button>
        </p>
      </form>
    </Modal>
  );
}
