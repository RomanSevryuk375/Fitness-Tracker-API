import { useState } from 'react';
import Modal from './Modal';
import type { WorkoutType } from '../types';

interface NewWorkoutModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSave: (workoutData: any) => void;
}

export default function NewWorkoutModal({ isOpen, onClose, onSave }: NewWorkoutModalProps) {
  const [title, setTitle] = useState('');
  const [type, setType] = useState<WorkoutType>('Strength');
  const [duration, setDuration] = useState('01:00:00');
  const [caloriesBurned, setCaloriesBurned] = useState('0');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSave({
      id: Math.random().toString(36).substr(2, 9),
      title,
      type,
      duration,
      caloriesBurned: parseInt(caloriesBurned),
      workoutDate: new Date().toISOString(),
      exercises: [] 
    });
    setTitle('');
    onClose();
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title="Create New Workout">
      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label className="block text-sm font-medium text-slate-700 mb-1">Workout Title</label>
          <input 
            type="text" 
            required
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            className="w-full bg-slate-50 border border-slate-200 rounded-lg px-4 py-2.5 text-slate-800 placeholder-slate-400 focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent transition-all"
            placeholder="e.g. Morning Leg Day"
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-slate-700 mb-1">Workout Type</label>
          <select 
            value={type}
            onChange={(e) => setType(e.target.value as WorkoutType)}
            className="w-full bg-slate-50 border border-slate-200 rounded-lg px-4 py-2.5 text-slate-800 focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent transition-all"
          >
            <option value="Strength">Strength</option>
            <option value="Cardio">Cardio</option>
            <option value="Flexibility">Flexibility</option>
            <option value="HIIT">HIIT</option>
            <option value="CrossFit">CrossFit</option>
          </select>
        </div>

        <div className="grid grid-cols-2 gap-4">
          <div>
            <label className="block text-sm font-medium text-slate-700 mb-1">Duration</label>
            <input 
              type="text" 
              required
              value={duration}
              onChange={(e) => setDuration(e.target.value)}
              placeholder="HH:MM:SS"
              className="w-full bg-slate-50 border border-slate-200 rounded-lg px-4 py-2.5 text-slate-800 focus:outline-none focus:ring-2 focus:ring-primary transition-all"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-slate-700 mb-1">Calories Burned</label>
            <input 
              type="number" 
              required
              min="0"
              value={caloriesBurned}
              onChange={(e) => setCaloriesBurned(e.target.value)}
              className="w-full bg-slate-50 border border-slate-200 rounded-lg px-4 py-2.5 text-slate-800 focus:outline-none focus:ring-2 focus:ring-primary transition-all"
            />
          </div>
        </div>

        <div className="flex justify-end gap-3 pt-4 border-t border-slate-100 mt-6">
          <button type="button" onClick={onClose} className="btn-secondary">
            Cancel
          </button>
          <button type="submit" className="btn-primary">
            Save Workout
          </button>
        </div>
      </form>
    </Modal>
  );
}
