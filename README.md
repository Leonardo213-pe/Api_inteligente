# API Inteligente de Tareas y Análisis

## Descripción
API RESTful para gestión de tareas internas con integración de API externa
y análisis de sentimiento usando ML.NET.

Stack: ASP.NET Core Web API .NET 8 + EF Core + SQLite + ML.NET

## Pasos para ejecutar localmente

### Requisitos
- .NET 8 SDK
- Git

### Instalación
git clone https://github.com/Leonardo213-pe/Api_inteligente.git
cd Api_inteligente/ApiInteligenteWeb
dotnet restore
dotnet run

Abrir en el navegador: http://localhost:5000/swagger

## Migraciones

dotnet tool install --global dotnet-ef --version 8.0.0
dotnet ef migrations add InitialCreate
dotnet ef database update

## Endpoints implementados

### Tareas
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | /api/tareas | Listar todas las tareas |
| GET | /api/tareas/{id} | Obtener tarea por ID |
| POST | /api/tareas | Crear nueva tarea |
| PUT | /api/tareas/{id} | Actualizar tarea |
| DELETE | /api/tareas/{id} | Eliminar tarea |

### Filtros
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | /api/tareas?estado=Pendiente | Filtrar por estado |
| GET | /api/tareas?prioridad=Alta | Filtrar por prioridad |
| GET | /api/tareas?fechaInicio=2026-05-01&fechaFin=2026-05-31 | Filtrar por rango de fechas |

### Tareas Externas
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | /api/tareas-externas | Listar tareas de API externa |
| GET | /api/tareas-externas/{id} | Obtener tarea externa por ID |

### ML.NET — Análisis de Sentimiento
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| POST | /api/ml/sentimiento | Analizar sentimiento de un comentario |

## Ejemplos de uso

### Crear tarea
POST /api/tareas
Content-Type: application/json

{
  "titulo": "Nueva tarea",
  "descripcion": "Descripción de la tarea",
  "estado": 0,
  "prioridad": 2,
  "fechaVencimiento": "2026-12-31"
}

Valores de estado:
- 0 = Pendiente
- 1 = EnProceso
- 2 = Completada

Valores de prioridad:
- 0 = Baja
- 1 = Media
- 2 = Alta

### Filtrar tareas
GET /api/tareas?estado=Pendiente
GET /api/tareas?prioridad=Alta
GET /api/tareas?fechaInicio=2026-05-01&fechaFin=2026-05-31

### Análisis de sentimiento
POST /api/ml/sentimiento
Content-Type: application/json

{
  "comentario": "La tarea fue completada correctamente y el sistema funciona bien"
}

Respuesta:
{
  "comentario": "La tarea fue completada correctamente y el sistema funciona bien",
  "sentimiento": "Positivo"
}

## API Externa utilizada

URL: https://jsonplaceholder.typicode.com/todos

Esta API provee tareas de prueba. El endpoint /api/tareas-externas
las consume y mapea al siguiente formato:

{
  "externalId": 1,
  "titulo": "delectus aut autem",
  "completado": false
}

## Modelo ML.NET — Análisis de Sentimiento

Tipo de modelo: Clasificación binaria (Binary Classification)
Algoritmo: SdcaLogisticRegression
Dataset: Creado manualmente con frases positivas y negativas en español

Ejemplos del dataset:
- "El sistema funciona perfecto" -> Positivo
- "Tarea completada exitosamente" -> Positivo
- "El sistema falla constantemente" -> Negativo
- "No funciona nada" -> Negativo

El modelo entrena al iniciar la aplicación con el dataset
incluido en la carpeta Data/dataset_sentimiento.csv

## Ramas del proyecto

| Rama | Descripción |
|------|-------------|
| feature/api-tareas | P1: CRUD de tareas |
| feature/filtros-tareas | P2: Filtros y búsqueda |
| feature/api-externa-todos | P3: Consumo API externa |
| feature/mlnet-basico | P4: Análisis de sentimiento ML.NET |

## Repositorio
https://github.com/Leonardo213-pe/Api_inteligente

## URL en Render
......