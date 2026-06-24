export default function Sidebar() {
  return (
    <div className="space-y-6 animate-slide-up" style={{ animationDelay: '0.3s' }}>
      <div className="simple-card p-6">
        <h3 className="font-bold text-slate-800 mb-4 text-lg">Activity This Week</h3>
        <div className="h-40 flex items-end gap-2 justify-between">
          {[40, 70, 45, 90, 60, 30, 85].map((height, i) => (
            <div key={i} className="w-full bg-slate-100 rounded-t-md relative group cursor-pointer hover:bg-slate-200 transition-colors h-full flex items-end">
              <div 
                className="w-full bg-primary rounded-t-md transition-all duration-300 group-hover:bg-blue-600"
                style={{ height: `${height}%` }}
              />
            </div>
          ))}
        </div>
        <div className="flex justify-between text-xs text-slate-400 mt-4 font-semibold">
          <span>Mon</span><span>Tue</span><span>Wed</span><span>Thu</span><span>Fri</span><span>Sat</span><span>Sun</span>
        </div>
      </div>

      <div className="simple-card p-6 border-t-4 border-t-blue-500">
        <h3 className="font-bold text-slate-800 mb-2">Upcoming Goal</h3>
        <p className="text-slate-500 text-sm mb-5">Run 5km under 25 minutes. You're almost there!</p>
        <div className="w-full bg-slate-100 rounded-full h-3 mb-2 overflow-hidden">
          <div className="bg-primary h-3 rounded-full w-[75%]"></div>
        </div>
        <span className="text-sm text-slate-600 font-semibold">75% Completed</span>
      </div>
    </div>
  );
}
