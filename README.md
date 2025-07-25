# DotNetCoreApiStarterKit

A clean starter kit for building secure and scalable .NET 9 Web APIs with JWT Authentication, Identity, Design Patterns, and standardized API responses.

---

## ✅ Setup Steps

### 🏁 Project Initialization

- The project was created using the **ASP.NET Core Web API** template targeting **.NET 9 (Preview)**.

---

### 📘 Add Swagger for API Documentation

- **Installed Package:** `Swashbuckle.AspNetCore`
- **Configured** Swagger UI in `Program.cs`

```csharp
// Register Swagger services
builder.Services.AddSwaggerGen();

// Enable Swagger middleware
app.UseSwagger();
app.UseSwaggerUI();
```

---

### 🚀 Enable Swagger UI Auto Launch in Browser

- Updated `launchSettings.json` to auto-launch Swagger when the project runs.
- Enabled browser launch by setting `"launchBrowser": true`.
- Set `"launchUrl": "swagger"` to open the Swagger UI directly.
- Consolidated profiles into a single one for simplicity and clarity.
- **Commit:** `chore: Enable browser launch with Swagger UI`

**launchSettings.json change:**
```json
{
  "commandName": "Project",
  "dotnetRunMessages": true,
  "launchBrowser": true,
  "launchUrl": "swagger",
  "applicationUrl": "https://localhost:7246;http://localhost:5160",
  "environmentVariables": {
    "ASPNETCORE_ENVIRONMENT": "Development"
  }
}
