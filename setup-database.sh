#!/bin/bash

# Script para configurar la base de datos de Mesa de Ayuda API
# Este script debe ejecutarse desde bash en Linux/macOS

# Colores para mensajes
COLOR_INFO="\033[0;36m"
COLOR_SUCCESS="\033[0;32m"
COLOR_ERROR="\033[0;31m"
COLOR_WARNING="\033[0;33m"
COLOR_RESET="\033[0m"

# Función para mostrar mensajes
function write_step {
    local message=$1
    local color=$2
    if [ -z "$color" ]; then
        color=$COLOR_INFO
    fi
    echo -e "\n${color}[$(date +%H:%M:%S)] $message${COLOR_RESET}"
}

# Verificar si estamos en la carpeta correcta
if [ ! -f "src/MesaApi.Api/appsettings.json" ]; then
    write_step "Error: Este script debe ejecutarse desde la carpeta raíz del proyecto MesaApi" $COLOR_ERROR
    exit 1
fi

# Verificar si dotnet está instalado
if ! command -v dotnet &> /dev/null; then
    write_step "Error: No se encontró .NET SDK. Por favor, instálalo desde https://dotnet.microsoft.com/download" $COLOR_ERROR
    exit 1
fi

dotnet_version=$(dotnet --version)
write_step "Usando .NET SDK versión: $dotnet_version" $COLOR_INFO

# Verificar si Entity Framework Core está instalado
if ! dotnet ef --version &> /dev/null; then
    write_step "Instalando Entity Framework Core CLI..." $COLOR_WARNING
    dotnet tool install --global dotnet-ef
    
    if [ $? -ne 0 ]; then
        write_step "Error al instalar Entity Framework Core CLI" $COLOR_ERROR
        exit 1
    fi
    
    ef_version=$(dotnet ef --version)
    write_step "Entity Framework Core CLI instalado correctamente: $ef_version" $COLOR_SUCCESS
else
    ef_version=$(dotnet ef --version)
    write_step "Usando Entity Framework Core versión: $ef_version" $COLOR_INFO
fi

# Menú de opciones
write_step "Configuración de Base de Datos para Mesa de Ayuda API" $COLOR_INFO
echo -e "\nSelecciona una opción:"
echo "1. Usar SQL Server Local"
echo "2. Usar Docker (recomendado para desarrollo)"
echo "3. Usar SQL Server en la nube (Azure SQL)"
echo "4. Salir"

read -p $'\nOpción: ' option

case $option in
    1)
        # SQL Server Local
        write_step "Configurando SQL Server Local..." $COLOR_INFO
        
        read -p "Servidor (por defecto: localhost): " server
        server=${server:-localhost}
        
        read -p "Nombre de la base de datos (por defecto: intranet): " database
        database=${database:-intranet}
        
        read -p "Usuario (por defecto: sa): " userId
        userId=${userId:-sa}
        
        read -p "Contraseña: " password
        
        # Actualizar appsettings.Development.json
        # Necesitamos jq para manipular JSON
        if ! command -v jq &> /dev/null; then
            write_step "Error: jq no está instalado. Por favor, instálalo con 'apt-get install jq' o 'brew install jq'" $COLOR_ERROR
            exit 1
        fi
        
        appsettingsPath="src/MesaApi.Api/appsettings.Development.json"
        connectionString="Server=$server;Database=$database;User Id=$userId;Password=$password;TrustServerCertificate=true;MultipleActiveResultSets=true"
        
        # Crear un archivo temporal con la nueva cadena de conexión
        jq --arg conn "$connectionString" '.ConnectionStrings.DefaultConnection = $conn' $appsettingsPath > temp.json && mv temp.json $appsettingsPath
        
        write_step "Cadena de conexión actualizada en appsettings.Development.json" $COLOR_SUCCESS
        
        # Ejecutar migraciones
        read -p "¿Deseas ejecutar las migraciones ahora? (S/N): " runMigrations
        if [[ $runMigrations == "S" || $runMigrations == "s" ]]; then
            write_step "Ejecutando migraciones..." $COLOR_INFO
            
            pushd src/MesaApi.Api > /dev/null
            dotnet ef database update
            
            if [ $? -eq 0 ]; then
                write_step "Migraciones ejecutadas correctamente" $COLOR_SUCCESS
            else
                write_step "Error al ejecutar las migraciones" $COLOR_ERROR
            fi
            popd > /dev/null
        fi
        ;;
    2)
        # Docker
        write_step "Configurando Docker..." $COLOR_INFO
        
        # Verificar si Docker está instalado
        if ! command -v docker &> /dev/null; then
            write_step "Error: Docker no está instalado. Por favor, instálalo desde https://www.docker.com/products/docker-desktop" $COLOR_ERROR
            exit 1
        fi
        
        # Iniciar contenedores
        write_step "Iniciando contenedores Docker..." $COLOR_INFO
        docker-compose up -d
        
        if [ $? -eq 0 ]; then
            write_step "Contenedores Docker iniciados correctamente" $COLOR_SUCCESS
            write_step "La API estará disponible en: http://localhost:8080 y https://localhost:8443" $COLOR_INFO
            write_step "La base de datos SQL Server estará disponible en: localhost,1433" $COLOR_INFO
            write_step "  - Usuario: sa" $COLOR_INFO
            write_step "  - Contraseña: YourStrong@Passw0rd" $COLOR_INFO
        else
            write_step "Error al iniciar los contenedores Docker" $COLOR_ERROR
        fi
        ;;
    3)
        # Azure SQL
        write_step "Configurando Azure SQL..." $COLOR_INFO
        
        read -p "Servidor Azure SQL (ejemplo: myserver.database.windows.net): " server
        
        read -p "Nombre de la base de datos (por defecto: intranet): " database
        database=${database:-intranet}
        
        read -p "Usuario: " userId
        
        read -p "Contraseña: " password
        
        # Actualizar appsettings.Development.json
        # Necesitamos jq para manipular JSON
        if ! command -v jq &> /dev/null; then
            write_step "Error: jq no está instalado. Por favor, instálalo con 'apt-get install jq' o 'brew install jq'" $COLOR_ERROR
            exit 1
        fi
        
        appsettingsPath="src/MesaApi.Api/appsettings.Development.json"
        connectionString="Server=$server;Database=$database;User Id=$userId;Password=$password;TrustServerCertificate=true;MultipleActiveResultSets=true"
        
        # Crear un archivo temporal con la nueva cadena de conexión
        jq --arg conn "$connectionString" '.ConnectionStrings.DefaultConnection = $conn' $appsettingsPath > temp.json && mv temp.json $appsettingsPath
        
        write_step "Cadena de conexión actualizada en appsettings.Development.json" $COLOR_SUCCESS
        
        # Ejecutar migraciones
        read -p "¿Deseas ejecutar las migraciones ahora? (S/N): " runMigrations
        if [[ $runMigrations == "S" || $runMigrations == "s" ]]; then
            write_step "Ejecutando migraciones..." $COLOR_INFO
            
            pushd src/MesaApi.Api > /dev/null
            dotnet ef database update
            
            if [ $? -eq 0 ]; then
                write_step "Migraciones ejecutadas correctamente" $COLOR_SUCCESS
            else
                write_step "Error al ejecutar las migraciones" $COLOR_ERROR
            fi
            popd > /dev/null
        fi
        ;;
    4)
        write_step "Saliendo..." $COLOR_INFO
        exit 0
        ;;
    *)
        write_step "Opción no válida" $COLOR_ERROR
        exit 1
        ;;
esac

# Instrucciones finales
write_step "Configuración completada" $COLOR_SUCCESS
write_step "Para ejecutar la API:" $COLOR_INFO
echo "  cd src/MesaApi.Api"
echo "  dotnet run"
write_step "Para probar la API, usa la colección de Postman incluida en el proyecto" $COLOR_INFO
echo "  Usuario: admin"
echo "  Contraseña: Admin123!"