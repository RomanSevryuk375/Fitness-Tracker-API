import axios from 'axios';

export const API_URL = 'http://localhost:8090/api';

export const api = axios.create({
  baseURL: API_URL,
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export const auth = {
  login: async (credentials: any) => {
    const res = await api.post('/auth/login', credentials);
    return res.data;
  },
  register: async (data: any) => {
    const res = await api.post('/auth/register', data);
    return res.data;
  }
};

export const workoutsApi = {
  getAll: async () => {
    const res = await api.get('/workouts?take=50&skip=0');
    return res.data;
  },
  getById: async (id: string) => {
    const res = await api.get(`/workouts/${id}`);
    return res.data;
  },
  create: async (data: any) => {
    const res = await api.post('/workouts', data);
    return res.data;
  },
  addExercise: async (workoutId: string, name: string) => {
    const res = await api.post(`/workouts/${workoutId}/exercises`, { name });
    return res.data;
  },
  addSet: async (workoutId: string, exerciseId: string, reps: number, weight: number) => {
    const res = await api.post(`/workouts/${workoutId}/exercises/${exerciseId}/sets`, { reps, weight });
    return res.data;
  },
  uploadPhoto: async (workoutId: string, file: File) => {
    const formData = new FormData();
    formData.append('file', file);
    const res = await api.post(`/workouts/${workoutId}/photos`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });
    return res.data;
  }
};
