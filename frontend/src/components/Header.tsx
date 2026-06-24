import { Activity, Plus, LogOut } from 'lucide-react';

interface HeaderProps {
  onNewWorkout: () => void;
  onLogout?: () => void;
}

export default function Header({ onNewWorkout, onLogout }: HeaderProps) {
  return (
    <header className="flex justify-between items-center mb-8 animate-fade-in bg-surface p-4 rounded-xl border border-slate-200 shadow-sm">
      <div className="flex items-center gap-3">
        <div className="bg-primary p-2 rounded-lg text-white">
          <Activity className="w-6 h-6" />
        </div>
        <h1 className="text-2xl font-bold text-slate-800 tracking-tight">FitnessTracker</h1>
      </div>
      
      <div className="flex items-center gap-3">
        <button onClick={onNewWorkout} className="btn-primary flex items-center gap-2">
          <Plus className="w-5 h-5" />
          <span className="hidden sm:inline">New Workout</span>
        </button>
        {onLogout && (
          <button 
            onClick={onLogout}
            className="p-2 text-slate-400 hover:text-red-500 hover:bg-red-50 rounded-lg transition-colors flex items-center gap-2 border border-transparent hover:border-red-100"
            title="Log out"
          >
            <LogOut className="w-5 h-5" />
          </button>
        )}
      </div>
    </header>
  );
}
