# Script para configurar la base de datos de Mesa de Ayuda API
# Este script debe ejecutarse desde PowerShell

# Colores para mensajes
$colorInfo = "Cyan"
$colorSuccess = "Green"
$colorError = "Red"
$colorWarning = "Yellow"

# Función para mostrar mensajes
function Write-Step {
    param (
        [string]$Message,
        [string]$Color = $colorInfo
    )
    Write-Host "`n[$(Get-Date -Format 'HH:mm:ss')] $Message" -ForegroundColor $Color
}

# Verificar si estamos en la carpeta correcta
if (-not (Test-Path "src\MesaApi.Api\appsettings.json")) {
    Write-Step "Error: Este script debe ejecutarse desde la carpeta raíz del proyecto MesaApi" -Color $colorError
    exit 1
}

# Verificar si dotnet está instalado
try {
    $dotnetVersion = dotnet --version
    Write-Step "Usando .NET SDK versión: $dotnetVersion" -Color $colorInfo
}
catch {
    Write-Step "Error: No se encontró .NET SDK. Por favor, instálalo desde https://dotnet.microsoft.com/download" -Color $colorError
    exit 1
}

# Verificar si Entity Framework Core está instalado
try {
    $efVersion = dotnet ef --version
    Write-Step "Usando Entity Framework Core versión: $efVersion" -Color $colorInfo
}
catch {
    Write-Step "Instalando Entity Framework Core CLI..." -Color $colorWarning
    dotnet tool install --global dotnet-ef
    
    if ($LASTEXITCODE -ne 0) {
        Write-Step "Error al instalar Entity Framework Core CLI" -Color $colorError
        exit 1
    }
    
    $efVersion = dotnet ef --version
    Write-Step "Entity Framework Core CLI instalado correctamente: $efVersion" -Color $colorSuccess
}

# Menú de opciones
Write-Step "Configuración de Base de Datos para Mesa de Ayuda API" -Color $colorInfo
Write-Host "`nSelecciona una opción:"
Write-Host "1. Usar SQL Server Local"
Write-Host "2. Usar Docker (recomendado para desarrollo)"
Write-Host "3. Usar SQL Server en la nube (Azure SQL)"
Write-Host "4. Salir"

$option = Read-Host "`nOpción"

switch ($option) {
    "1" {
        # SQL Server Local
        Write-Step "Configurando SQL Server Local..." -Color $colorInfo
        
        $server = Read-Host "Servidor (por defecto: localhost)"
        if ([string]::IsNullOrWhiteSpace($server)) { $server = "localhost" }
        
        $database = Read-Host "Nombre de la base de datos (por defecto: intranet)"
        if ([string]::IsNullOrWhiteSpace($database)) { $database = "intranet" }
        
        $userId = Read-Host "Usuario (por defecto: sa)"
        if ([string]::IsNullOrWhiteSpace($userId)) { $userId = "sa" }
        
        $password = Read-Host "Contraseña" -AsSecureString
        $bstr = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($password)
        $passwordText = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($bstr)
        
        # Actualizar appsettings.Development.json
        $appsettingsPath = "src\MesaApi.Api\appsettings.Development.json"
        $appsettings = Get-Content $appsettingsPath -Raw | ConvertFrom-Json
        $appsettings.ConnectionStrings.DefaultConnection = "Server=$server;Database=$database;User Id=$userId;Password=$passwordText;TrustServerCertificate=true;MultipleActiveResultSets=true"
        $appsettings | ConvertTo-Json -Depth 10 | Set-Content $appsettingsPath
        
        Write-Step "Cadena de conexión actualizada en appsettings.Development.json" -Color $colorSuccess
        
        # Ejecutar migraciones
        $runMigrations = Read-Host "¿Deseas ejecutar las migraciones ahora? (S/N)"
        if ($runMigrations -eq "S" -or $runMigrations -eq "s") {
            Write-Step "Ejecutando migraciones..." -Color $colorInfo
            
            try {
                Push-Location "src\MesaApi.Api"
                dotnet ef database update
                
                if ($LASTEXITCODE -eq 0) {
                    Write-Step "Migraciones ejecutadas correctamente" -Color $colorSuccess
                }
                else {
                    Write-Step "Error al ejecutar las migraciones" -Color $colorError
                }
            }
            finally {
                Pop-Location
            }
        }
    }
    "2" {
        # Docker
        Write-Step "Configurando Docker..." -Color $colorInfo
        
        # Verificar si Docker está instalado
        try {
            docker --version
        }
        catch {
            Write-Step "Error: Docker no está instalado. Por favor, instálalo desde https://www.docker.com/products/docker-desktop" -Color $colorError
            exit 1
        }
        
        # Iniciar contenedores
        Write-Step "Iniciando contenedores Docker..." -Color $colorInfo
        docker-compose up -d
        
        if ($LASTEXITCODE -eq 0) {
            Write-Step "Contenedores Docker iniciados correctamente" -Color $colorSuccess
            Write-Step "La API estará disponible en: http://localhost:8080 y https://localhost:8443" -Color $colorInfo
            Write-Step "La base de datos SQL Server estará disponible en: localhost,1433" -Color $colorInfo
            Write-Step "  - Usuario: sa" -Color $colorInfo
            Write-Step "  - Contraseña: YourStrong@Passw0rd" -Color $colorInfo
        }
        else {
            Write-Step "Error al iniciar los contenedores Docker" -Color $colorError
        }
    }
    "3" {
        # Azure SQL
        Write-Step "Configurando Azure SQL..." -Color $colorInfo
        
        $server = Read-Host "Servidor Azure SQL (ejemplo: myserver.database.windows.net)"
        $database = Read-Host "Nombre de la base de datos (por defecto: intranet)"
        if ([string]::IsNullOrWhiteSpace($database)) { $database = "intranet" }
        
        $userId = Read-Host "Usuario"
        
        $password = Read-Host "Contraseña" -AsSecureString
        $bstr = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($password)
        $passwordText = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($bstr)
        
        # Actualizar appsettings.Development.json
        $appsettingsPath = "src\MesaApi.Api\appsettings.Development.json"
        $appsettings = Get-Content $appsettingsPath -Raw | ConvertFrom-Json
        $appsettings.ConnectionStrings.DefaultConnection = "Server=$server;Database=$database;User Id=$userId;Password=$passwordText;TrustServerCertificate=true;MultipleActiveResultSets=true"
        $appsettings | ConvertTo-Json -Depth 10 | Set-Content $appsettingsPath
        
        Write-Step "Cadena de conexión actualizada en appsettings.Development.json" -Color $colorSuccess
        
        # Ejecutar migraciones
        $runMigrations = Read-Host "¿Deseas ejecutar las migraciones ahora? (S/N)"
        if ($runMigrations -eq "S" -or $runMigrations -eq "s") {
            Write-Step "Ejecutando migraciones..." -Color $colorInfo
            
            try {
                Push-Location "src\MesaApi.Api"
                dotnet ef database update
                
                if ($LASTEXITCODE -eq 0) {
                    Write-Step "Migraciones ejecutadas correctamente" -Color $colorSuccess
                }
                else {
                    Write-Step "Error al ejecutar las migraciones" -Color $colorError
                }
            }
            finally {
                Pop-Location
            }
        }
    }
    "4" {
        Write-Step "Saliendo..." -Color $colorInfo
        exit 0
    }
    default {
        Write-Step "Opción no válida" -Color $colorError
        exit 1
    }
}

# Instrucciones finales
Write-Step "Configuración completada" -Color $colorSuccess
Write-Step "Para ejecutar la API:" -Color $colorInfo
Write-Host "  cd src\MesaApi.Api"
Write-Host "  dotnet run"
Write-Step "Para probar la API, usa la colección de Postman incluida en el proyecto" -Color $colorInfo
Write-Host "  Usuario: admin"
Write-Host "  Contraseña: Admin123!"