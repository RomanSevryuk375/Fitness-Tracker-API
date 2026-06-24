import type { ReactNode } from 'react';

interface StatCardProps {
  title: string;
  value: string | number;
  icon: ReactNode;
  trend: string;
}

export default function StatCard({ title, value, icon, trend }: StatCardProps) {
  return (
    <div className="simple-card p-5 hover:shadow-md transition-shadow">
      <div className="flex justify-between items-start mb-3">
        <div className="bg-slate-50 p-2.5 rounded-lg border border-slate-100 text-primary">
          {icon}
        </div>
        <span className="text-xs font-medium px-2 py-1 bg-green-50 text-green-700 rounded-md">{trend}</span>
      </div>
      <div>
        <p className="text-2xl font-bold text-slate-800">{value}</p>
        <h3 className="text-slate-500 text-sm font-medium mt-1">{title}</h3>
      </div>
    </div>
  );
}
