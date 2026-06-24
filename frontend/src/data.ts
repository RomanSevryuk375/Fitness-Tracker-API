import type { Workout, Stats } from './types';

export const MOCK_WORKOUTS: Workout[] = [
  {
    id: '1',
    title: 'Morning Push Workout',
    type: 'Strength',
    duration: '01:15:00',
    caloriesBurned: 450,
    workoutDate: new Date(new Date().setDate(new Date().getDate() - 1)).toISOString(),
    exercises: [
      { name: 'Bench Press', sets: [{ reps: 10, weight: 80 }, { reps: 8, weight: 85 }] },
      { name: 'Shoulder Press', sets: [{ reps: 12, weight: 40 }, { reps: 10, weight: 45 }] }
    ]
  },
  {
    id: '2',
    title: 'HIIT Cardio Blast',
    type: 'HIIT',
    duration: '00:45:00',
    caloriesBurned: 520,
    workoutDate: new Date().toISOString(),
    exercises: [
      { name: 'Burpees', sets: [{ reps: 20, weight: 0 }, { reps: 20, weight: 0 }] },
      { name: 'Jump Rope', sets: [{ reps: 100, weight: 0 }, { reps: 100, weight: 0 }] }
    ]
  },
  {
    id: '3',
    title: 'Leg Day',
    type: 'Strength',
    duration: '01:30:00',
    caloriesBurned: 600,
    workoutDate: new Date(new Date().setDate(new Date().getDate() - 2)).toISOString(),
    exercises: [
      { name: 'Squats', sets: [{ reps: 8, weight: 100 }, { reps: 8, weight: 110 }, { reps: 6, weight: 120 }] },
      { name: 'Leg Press', sets: [{ reps: 12, weight: 200 }, { reps: 10, weight: 220 }] }
    ]
  }
];

export const MOCK_STATS: Stats = {
  totalWorkouts: 24,
  activeMinutes: 1420,
  caloriesBurned: 12450,
  streak: 5
};
