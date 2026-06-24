import { useState, useEffect } from 'react';
import { Clock, Flame, Dumbbell, TrendingUp, Award } from 'lucide-react';
import type { Workout } from './types';
import Header from './components/Header';
import StatCard from './components/StatCard';
import WorkoutCard from './components/WorkoutCard';
import NewWorkoutModal from './components/NewWorkoutModal';
import WorkoutDetailsModal from './components/WorkoutDetailsModal';
import AuthModal from './components/AuthModal';
import { workoutsApi } from './api';

function App() {
  const [workouts, setWorkouts] = useState<Workout[]>([]);
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(!!localStorage.getItem('token'));
  
  const [isNewWorkoutModalOpen, setIsNewWorkoutModalOpen] = useState(false);
  const [selectedWorkout, setSelectedWorkout] = useState<Workout | null>(null);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    if (isAuthenticated) {
      fetchWorkouts();
    }
  }, [isAuthenticated]);

  const fetchWorkouts = async () => {
    setIsLoading(true);
    try {
      const data = await workoutsApi.getAll();
      const workoutList = data.value || data.Value || data || [];
      setWorkouts(Array.isArray(workoutList) ? workoutList : []);
    } catch (err) {
      console.error("Failed to fetch workouts", err);
    } finally {
      setIsLoading(false);
    }
  };
  
  const handleNewWorkout = () => {
    setIsNewWorkoutModalOpen(true);
  };

  const handleSaveNewWorkout = async (newWorkoutData: any) => {
    try {
      const typeMap: Record<string, number> = {
        'Strength': 0,
        'Cardio': 1,
        'Flexibility': 2,
        'HIIT': 3,
        'CrossFit': 4
      };

      await workoutsApi.create({
        title: newWorkoutData.title,
        type: typeMap[newWorkoutData.type] ?? 0,
        duration: newWorkoutData.duration,
        caloriesBurned: newWorkoutData.caloriesBurned,
        workoutDate: newWorkoutData.workoutDate
      });
      fetchWorkouts();
    } catch (err) {
      console.error("Failed to save workout", err);
    }
  };

  const handleWorkoutClick = async (id: string) => {
    try {
      const data = await workoutsApi.getById(id);
      const detailedWorkout = data.value || data.Value || data;
      setSelectedWorkout(detailedWorkout);
    } catch (err) {
      console.error("Failed to fetch workout details", err);
    }
  };

  const handleLogout = () => {
    localStorage.removeItem('token');
    setIsAuthenticated(false);
    setWorkouts([]);
  };

  const totalWorkouts = workouts.length;
  const activeMinutes = workouts.reduce((total, w) => {
    if (!w.duration) return total;
    const parts = w.duration.split(':');
    if (parts.length === 3) {
      return total + parseInt(parts[0]) * 60 + parseInt(parts[1]);
    }
    return total;
  }, 0);
  const caloriesBurned = workouts.reduce((total, w) => total + (w.caloriesBurned || 0), 0);
  const streak = totalWorkouts > 0 ? 1 : 0; 

  return (
    <div className="min-h-screen bg-slate-50 text-slate-800 font-sans selection:bg-primary selection:text-white p-4 md:p-8">
      
      {!isAuthenticated && (
        <AuthModal 
          isOpen={true} 
          onSuccess={() => setIsAuthenticated(true)} 
        />
      )}

      {isAuthenticated && (
        <div className="max-w-6xl mx-auto relative z-10">
          
          <Header onNewWorkout={handleNewWorkout} onLogout={handleLogout} />

          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-5 mb-10 animate-slide-up" style={{ animationDelay: '0.1s' }}>
            <StatCard 
              title="Total Workouts" 
              value={totalWorkouts} 
              icon={<Dumbbell className="w-6 h-6 text-blue-500" />} 
              trend="+1 this week" 
            />
            <StatCard 
              title="Active Minutes" 
              value={activeMinutes} 
              icon={<Clock className="w-6 h-6 text-blue-500" />} 
              trend="+0 this week" 
            />
            <StatCard 
              title="Calories Burned" 
              value={caloriesBurned} 
              icon={<Flame className="w-6 h-6 text-orange-500" />} 
              trend="+0 this week" 
            />
            <StatCard 
              title="Current Streak" 
              value={`${streak} Days`} 
              icon={<Award className="w-6 h-6 text-yellow-500" />} 
              trend="Keep it up!" 
            />
          </div>

          <div>
            
            <div className="space-y-6 animate-slide-up" style={{ animationDelay: '0.2s' }}>
              <div className="flex justify-between items-center mb-4">
                <h2 className="text-xl font-bold flex items-center gap-2 text-slate-800">
                  <TrendingUp className="w-5 h-5 text-primary" />
                  Recent Workouts
                </h2>
              </div>
              
              <div className="space-y-4">
                {isLoading ? (
                  <div className="text-center py-10 text-slate-400">Loading workouts...</div>
                ) : workouts.length === 0 ? (
                  <div className="text-center py-10 text-slate-400 bg-surface rounded-xl border border-slate-200 border-dashed">
                    No workouts found. Create your first one!
                  </div>
                ) : (
                  workouts.map((workout) => (
                    <WorkoutCard 
                      key={workout.id} 
                      workout={{...workout, exercises: workout.exercises || []}} 
                      onClick={handleWorkoutClick} 
                    />
                  ))
                )}
              </div>
            </div>

          </div>
        </div>
      )}

      <NewWorkoutModal 
        isOpen={isNewWorkoutModalOpen}
        onClose={() => setIsNewWorkoutModalOpen(false)}
        onSave={handleSaveNewWorkout}
      />

      <WorkoutDetailsModal 
        isOpen={!!selectedWorkout}
        onClose={() => setSelectedWorkout(null)}
        workout={selectedWorkout}
        onUpdate={() => selectedWorkout && handleWorkoutClick(selectedWorkout.id)}
      />

    </div>
  );
}

export default App;
