export interface Set {
  reps: number;
  weight: number;
}

export interface Exercise {
  name: string;
  sets: Set[];
}

export type WorkoutType = 'Strength' | 'Cardio' | 'Flexibility' | 'HIIT' | 'CrossFit';

export interface Workout {
  id: string;
  title: string;
  type: WorkoutType;
  exercises?: Exercise[];
  exercisesCount?: number;
  duration: string;
  caloriesBurned: number;
  photos?: { filePath: string }[];
  workoutDate: string;
}

export interface Stats {
  totalWorkouts: number;
  activeMinutes: number;
  caloriesBurned: number;
  streak: number;
}
