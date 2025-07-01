# 🏁 OpenRaceView

OpenRaceView is an open-source lap comparison and telemetry visualization platform — inspired by Serious-Racing.com.

Built to help racers upload, compare, and visualize their lap data using maps, animated ghost laps, and telemetry graphs.

---

## 🔧 Tech Stack

### Frontend
- [SvelteKit](https://kit.svelte.dev/) – reactive UI framework
- [Tailwind CSS](https://tailwindcss.com/) – styling
- [MapLibre GL JS](https://maplibre.org/) – interactive maps
- [Chart.js](https://www.chartjs.org/) – telemetry graphs

### Backend
- [ASP.NET Core](https://dotnet.microsoft.com/) – Web API
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) – Data access
- [ASP.NET Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity) – Auth system
- [MediatR](https://github.com/jbogard/MediatR) – CQRS pattern
- [PostgreSQL + PostGIS](https://postgis.net/) – Spatial data

---

## 🚀 Features

- Upload and store GPS-based lap data
- Visualize laps with animated dots on a map
- Compare multiple laps in real-time
- View speed/throttle/brake charts
- Associate laps with vehicles and users
- Auth system (login/register) with ASP.NET Identity

---

## 📦 Getting Started

### Requirements
- .NET 8 SDK
- Node.js 18+
- PostgreSQL with PostGIS enabled

### 1. Backend
```bash
cd backend/OpenRaceView.API
dotnet ef database update
dotnet run
````
### 2. Frontend
```bash
Copy
Edit
cd frontend
npm install
npm run dev
```

### 3. Access
Open the frontend: http://localhost:5173
API runs at: http://localhost:5000 (adjust as needed)

## 🛤️ Roadmap
 Track detection & map overlays

 Lap alignment via GPS or sector timing

 Video + telemetry sync

 Social sharing & lap comparison

## 🤝 Contributing
Contributions are welcome! Open an issue or submit a pull request.

## 📄 License
MIT License – see LICENSE

## 📸 Screenshots
Coming soon...
