import { useState, useRef } from 'react';
import Modal from './Modal';
import type { Workout } from '../types';
import { Calendar, Clock, Flame, Image as ImageIcon, Plus, Upload } from 'lucide-react';
import { workoutsApi, API_URL } from '../api';

interface WorkoutDetailsModalProps {
  workout: Workout | null;
  isOpen: boolean;
  onClose: () => void;
  onUpdate?: () => void;
}

export default function WorkoutDetailsModal({ workout, isOpen, onClose, onUpdate }: WorkoutDetailsModalProps) {
  const [newExerciseName, setNewExerciseName] = useState('');
  const [isAddingExercise, setIsAddingExercise] = useState(false);
  
  const [addingSetToExId, setAddingSetToExId] = useState<string | null>(null);
  const [newReps, setNewReps] = useState('');
  const [newWeight, setNewWeight] = useState('');
  
  const fileInputRef = useRef<HTMLInputElement>(null);
  const [isUploading, setIsUploading] = useState(false);

  if (!workout) return null;

  const handleAddExercise = async () => {
    if (!newExerciseName.trim()) return;
    try {
      await workoutsApi.addExercise(workout.id, newExerciseName);
      setNewExerciseName('');
      setIsAddingExercise(false);
      if (onUpdate) onUpdate();
    } catch (err) {
      console.error(err);
      alert("Failed to add exercise");
    }
  };

  const handleAddSet = async (exerciseId: string) => {
    if (!newReps || !newWeight) return;
    try {
      await workoutsApi.addSet(workout.id, exerciseId, parseInt(newReps), parseFloat(newWeight));
      setNewReps('');
      setNewWeight('');
      setAddingSetToExId(null);
      if (onUpdate) onUpdate();
    } catch (err) {
      console.error(err);
      alert("Failed to add set");
    }
  };

  const handleFileUpload = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;
    setIsUploading(true);
    try {
      await workoutsApi.uploadPhoto(workout.id, file);
      if (onUpdate) onUpdate();
    } catch (err) {
      console.error(err);
      alert("Failed to upload photo");
    } finally {
      setIsUploading(false);
      if (fileInputRef.current) fileInputRef.current.value = '';
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title="Workout Details">
      <div className="space-y-6">
        <div>
          <div className="flex items-center gap-3 mb-2">
            <h3 className="text-xl font-bold text-slate-800">{workout.title}</h3>
            <span className="text-xs px-2.5 py-1 bg-blue-50 text-blue-700 rounded-full font-semibold border border-blue-100">
              {workout.type}
            </span>
          </div>
          
          <div className="flex flex-wrap gap-4 text-sm text-slate-500 font-medium bg-slate-50 p-4 rounded-xl border border-slate-100">
            <span className="flex items-center gap-1.5"><Calendar className="w-4 h-4 text-slate-400" /> {new Date(workout.workoutDate).toLocaleDateString()}</span>
            <span className="flex items-center gap-1.5"><Clock className="w-4 h-4 text-slate-400" /> {workout.duration}</span>
            <span className="flex items-center gap-1.5"><Flame className="w-4 h-4 text-orange-400" /> {workout.caloriesBurned} kcal</span>
          </div>
        </div>

        <div>
          <div className="flex justify-between items-center mb-3 border-b border-slate-100 pb-2">
            <h4 className="font-semibold text-slate-800">Exercises</h4>
            <button 
              onClick={() => setIsAddingExercise(!isAddingExercise)}
              className="text-xs font-semibold text-primary hover:text-blue-700 flex items-center gap-1 transition-colors bg-blue-50 hover:bg-blue-100 px-2.5 py-1.5 rounded-lg"
            >
              <Plus className="w-3.5 h-3.5" /> Add Exercise
            </button>
          </div>

          {isAddingExercise && (
            <div className="bg-white p-3 rounded-xl border border-blue-100 shadow-sm mb-4 flex gap-2">
              <input 
                type="text"
                placeholder="Exercise name (e.g. Bench Press)"
                value={newExerciseName}
                onChange={(e) => setNewExerciseName(e.target.value)}
                className="flex-1 bg-slate-50 border border-slate-200 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-primary"
                autoFocus
              />
              <button onClick={handleAddExercise} className="btn-primary py-2 px-4 text-sm">Save</button>
            </div>
          )}

          {(!workout.exercises || workout.exercises.length === 0) ? (
            <p className="text-slate-500 text-sm italic bg-slate-50 p-4 rounded-xl border border-slate-100 text-center">No exercises recorded.</p>
          ) : (
            <div className="space-y-4">
              {workout.exercises.map((ex: any) => (
                <div key={ex.id} className="bg-slate-50 rounded-xl p-4 border border-slate-200 shadow-sm hover:shadow-md transition-shadow">
                  <div className="flex justify-between items-center mb-3">
                    <div className="font-bold text-slate-800">{ex.name || ex.eserciseName || ex.EserciseName}</div>
                    <button 
                      onClick={() => setAddingSetToExId(ex.id)}
                      className="text-xs font-medium text-slate-500 hover:text-primary flex items-center gap-1 bg-white border border-slate-200 hover:border-blue-200 px-2 py-1 rounded-md transition-colors"
                    >
                      <Plus className="w-3 h-3" /> Add Set
                    </button>
                  </div>
                  
                  {addingSetToExId === ex.id && (
                    <div className="flex gap-2 mb-3 bg-blue-50 p-2 rounded-lg border border-blue-100">
                      <input 
                        type="number" placeholder="Reps" value={newReps} onChange={(e) => setNewReps(e.target.value)}
                        className="w-20 bg-white border border-slate-200 rounded px-2 py-1 text-sm focus:outline-none focus:ring-2 focus:ring-primary"
                        autoFocus
                      />
                      <input 
                        type="number" placeholder="Weight (kg)" value={newWeight} onChange={(e) => setNewWeight(e.target.value)}
                        className="w-24 bg-white border border-slate-200 rounded px-2 py-1 text-sm focus:outline-none focus:ring-2 focus:ring-primary"
                      />
                      <button onClick={() => handleAddSet(ex.id)} className="bg-primary text-white text-xs font-medium px-3 rounded hover:bg-blue-700 transition-colors">Add</button>
                      <button onClick={() => setAddingSetToExId(null)} className="text-slate-400 hover:text-slate-600 text-xs px-2">Cancel</button>
                    </div>
                  )}

                  {ex.sets && ex.sets.length > 0 ? (
                    <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-2">
                      {ex.sets.map((set: any, j: number) => (
                        <div key={set.id || j} className="text-sm bg-white px-3 py-2 rounded-lg shadow-sm border border-slate-100 text-slate-700 flex flex-col justify-center items-center text-center">
                          <span className="text-xs text-slate-400 font-medium mb-0.5">Set {j + 1}</span>
                          <span className="font-semibold">{set.reps} <span className="text-slate-500 font-normal text-xs">reps</span> x {set.weight}</span>
                        </div>
                      ))}
                    </div>
                  ) : (
                    <div className="text-xs text-slate-400 italic">No sets added yet</div>
                  )}
                </div>
              ))}
            </div>
          )}
        </div>

        <div>
          <div className="flex justify-between items-center mb-3 border-b border-slate-100 pb-2">
            <h4 className="font-semibold text-slate-800 flex items-center gap-2">
              <ImageIcon className="w-4 h-4 text-slate-400" /> Progress Photos
            </h4>
            <button 
              onClick={() => fileInputRef.current?.click()}
              className="text-xs font-semibold text-primary hover:text-blue-700 flex items-center gap-1 transition-colors bg-blue-50 hover:bg-blue-100 px-2.5 py-1.5 rounded-lg"
              disabled={isUploading}
            >
              <Upload className="w-3.5 h-3.5" /> {isUploading ? "Uploading..." : "Upload Photo"}
            </button>
            <input 
              type="file" 
              ref={fileInputRef} 
              className="hidden" 
              accept="image/*"
              onChange={handleFileUpload} 
            />
          </div>
          
          {(!workout.photos || workout.photos.length === 0) ? (
            <p className="text-slate-500 text-sm italic bg-slate-50 p-4 rounded-xl border border-slate-100 text-center">No photos uploaded.</p>
          ) : (
            <div className="flex gap-3 overflow-x-auto pb-2">
              {workout.photos.map((photo, i) => (
                <a key={i} href={`${API_URL}/workouts/${workout.id}/photos/${photo.filePath}`} target="_blank" rel="noopener noreferrer" className="block flex-shrink-0 relative group">
                  <img 
                    src={`${API_URL}/workouts/${workout.id}/photos/${photo.filePath}`} 
                    alt="Progress" 
                    className="h-28 w-28 object-cover rounded-xl border border-slate-200 shadow-sm transition-transform group-hover:scale-105" 
                  />
                  <div className="absolute inset-0 bg-black/40 rounded-xl opacity-0 group-hover:opacity-100 transition-opacity flex items-center justify-center">
                    <span className="text-white text-xs font-semibold">View</span>
                  </div>
                </a>
              ))}
            </div>
          )}
        </div>

        <div className="flex justify-end pt-4 mt-6">
          <button onClick={onClose} className="btn-primary">
            Close
          </button>
        </div>
      </div>
    </Modal>
  );
}
