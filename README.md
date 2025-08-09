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

- ✅ Upload and store GPS-based lap data (RaceChrono JSON format)
- ✅ RESTful API for lap management (create, list, retrieve)
- ✅ Telemetry sample storage with validation and ordering
- ✅ Configurable sample limits per lap
- ✅ SQLite database with proper indexing
- 🔧 Visualize laps with animated dots on a map (frontend)
- 🔧 Compare multiple laps in real-time (frontend)
- 🔧 View speed/throttle/brake charts (frontend)
- 🔧 Associate laps with vehicles and users
- 🔧 Auth system (login/register) with ASP.NET Identity

---

## 📦 Getting Started

### Requirements
- .NET 9 SDK
- Node.js 18+
- (PostgreSQL optional - using SQLite by default)

### 1. Backend
```bash
cd backend/OpenRaceView.API
# Database will be auto-created on first run in development
dotnet run
```
### 2. Frontend
```bash
cd frontend
npm install
npm run dev
```

### 3. Access
- Frontend: http://localhost:5173
- Backend API: http://localhost:5000 (or port shown in console)
- Swagger UI: http://localhost:5000/swagger (in development)

---

## 📡 API Endpoints

### Lap Management

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/laps` | Create new lap with telemetry samples |
| `GET` | `/api/laps` | List all laps (summary data) |
| `GET` | `/api/laps/{id}` | Get specific lap details |
| `GET` | `/api/laps/{id}?includeSamples=true` | Get lap with telemetry samples |

### Sample Request (Create Lap)
```json
{
  "source": "RaceChronoV3",
  "trackName": "Nürburgring",
  "startTimeUtc": "2025-01-01T12:34:56Z",
  "durationMs": 48765,
  "samples": [
    { "t": 0, "lat": 50.3435412, "lon": 6.9599175, "elev": 0, "spd": 12.3 },
    { "t": 200, "lat": 50.3436000, "lon": 6.9599500, "spd": 13.1 }
  ]
}
```

**Configuration:**
- Max samples per lap: `Telemetry:MaxSamplesPerLap` (default: 50,000)
- Database: SQLite (`laps.db` in development)

---

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
