import { Calendar, Clock, Flame, ChevronRight } from 'lucide-react';
import type { Workout } from '../types';

interface WorkoutCardProps {
  workout: Workout;
  onClick: (id: string) => void;
}

export default function WorkoutCard({ workout, onClick }: WorkoutCardProps) {
  return (
    <div 
      onClick={() => onClick(workout.id)}
      className="simple-card p-5 hover:border-primary hover:shadow-md transition-all duration-200 group cursor-pointer"
    >
      <div className="flex justify-between items-start mb-4">
        <div>
          <div className="flex items-center gap-3 mb-2">
            <h3 className="text-lg font-bold text-slate-800 group-hover:text-primary transition-colors">{workout.title}</h3>
            <span className="text-xs px-2.5 py-1 bg-blue-50 text-blue-700 rounded-full font-semibold border border-blue-100">
              {workout.type}
            </span>
          </div>
          <div className="flex items-center gap-4 text-sm text-slate-500 font-medium">
            <span className="flex items-center gap-1.5"><Calendar className="w-4 h-4 text-slate-400" /> {new Date(workout.workoutDate).toLocaleDateString()}</span>
            <span className="flex items-center gap-1.5"><Clock className="w-4 h-4 text-slate-400" /> {workout.duration}</span>
            <span className="flex items-center gap-1.5"><Flame className="w-4 h-4 text-orange-400" /> {workout.caloriesBurned} kcal</span>
          </div>
        </div>
        <button className="p-2 bg-slate-50 rounded-full hover:bg-slate-100 text-slate-400 hover:text-primary transition-colors">
          <ChevronRight className="w-5 h-5" />
        </button>
      </div>
      
      <div className="pt-4 border-t border-slate-100">
        <p className="text-sm text-slate-500 font-medium mb-3">Exercises ({workout.exercisesCount ?? workout.exercises?.length ?? 0})</p>
        <div className="flex flex-wrap gap-2">
          {workout.exercises && workout.exercises.slice(0, 3).map((ex, i) => (
            <span key={i} className="text-xs bg-slate-50 px-3 py-1.5 rounded-lg border border-slate-200 text-slate-700 font-medium">
              {ex.name} <span className="text-slate-400 ml-1 font-normal">{ex.sets.length} sets</span>
            </span>
          ))}
          {workout.exercises && workout.exercises.length > 3 && (
            <span className="text-xs bg-slate-50 px-3 py-1.5 rounded-lg border border-slate-200 text-slate-500">
              +{workout.exercises.length - 3} more
            </span>
          )}
        </div>
      </div>
    </div>
  );
}
