## Getting Started

### Prerequisites

Ensure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/News_Platform.git
   ```
2. Navigate to the project directory:
   ```bash
   cd News_Platform
   ```

### Build

To build the project, run:

```bash
dotnet build
```

### Start

To start the application, use:

```bash
dotnet run
```

### Development

For development, you can use the following command to run the application:

```bash
dotnet watch run
```

### Configuration

Update the `appsettings.json` file with the following configuration:

```json
{
  "JwtSettings": {
    "Issuer": "https://34.234.194.143:5239",
    "Audience": "http://3.93.27.238:5173"
  },
  "AppSettings": {
    "FrontendUrl": "http://3.93.27.238:5173"
  },
  "Cors": {
    "AllowedOrigins": ["http://3.93.27.238:5173"]
  }
}
```

Ensure the `FrontendUrl` and `AllowedOrigins` match your frontend's URL, and the `Issuer` corresponds to your backend's URL.
