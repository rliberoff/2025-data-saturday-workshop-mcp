# Taller MCP: Model Context Protocol en Azure

Taller práctico de 3 horas para aprender a construir servidores MCP que explotan datos desde diversas fuentes utilizando C# .NET 10.0 y Azure.

## 🎯 Descripción

Este taller te guía a través de la construcción de servidores **Model Context Protocol (MCP)** para integrar y explotar datos desde múltiples fuentes (Azure SQL, Cosmos DB, REST APIs). Aprenderás desde conceptos fundamentales hasta patrones empresariales de orquestación multi-fuente.

### ¿Qué es MCP?

Model Context Protocol es un protocolo estándar para exponer datos y capacidades a modelos de IA de manera estructurada, componible y segura.

## 📚 Contenido del Taller

**Duración**: 3 horas  
**Formato**: 11 bloques con teoría, demostraciones y ejercicios prácticos

### Bloques

1. **Apertura** (10 min) - Introducción y contexto
2. **Fundamentos** (25 min) - Conceptos MCP, arquitectura, casos de uso
3. **Anatomía de un Proveedor + Ejercicio 1** (30 min) - Live coding y recursos estáticos
4. **Ejercicio 2** (20 min) - Consultas paramétricas
5. **Ejercicio 3** (20 min) - Seguridad JWT y rate limiting
6. **Seguridad y Gobernanza** (15 min) - Patrones empresariales
7. **Ejercicio 4** (25 min) - Reto integrador: Analista virtual
8. **Orquestación Multi-Fuente** (15 min) - Patrones de integración
9. **Ejercicio 5** (30 min) - Agente de IA con Microsoft Agent Framework
10. **Hoja de Ruta y Casos B2B** (10 min) - Escenarios de negocio
11. **Cierre** (10 min) - Retrospectiva y siguientes pasos

## 🚀 Quick Start

### Prerequisitos

- **SDK**: .NET 10.0 o superior
- **IDE**: Visual Studio 2022 o VS Code con C# Dev Kit
- **Tools**: Git, PowerShell 7+
- **Azure** (opcional): Cuenta de Azure para ejercicios cloud

### Instalación

```powershell
# 1. Clonar el repositorio
git clone <repository-url>
cd mcp-workshop

# 2. Verificar entorno
.\scripts\verify-setup.ps1

# 3. Generar datos de ejemplo
.\scripts\create-sample-data.ps1

# 4. Construir solución
dotnet build McpWorkshop.sln
```

> **Nota**: Los datos de ejemplo se generan ejecutando el script `create-sample-data.ps1`, que crea archivos JSON dinámicos en la carpeta `data/` del repositorio (customers.json, products.json, orders.json, sessions.json, abandoned-carts.json, cart-events.json).

## 📖 Documentación

- **[Agenda Completa](docs/AGENDA.md)** - Cronograma detallado del taller
- **[Quick Reference](docs/QUICK_REFERENCE.md)** - Referencia rápida MCP y C#
- **[Instructor Handbook](docs/INSTRUCTOR_HANDBOOK.md)** - Guía de facilitación
- **[Troubleshooting](docs/TROUBLESHOOTING.md)** - Solución de problemas comunes
- **[Azure Deployment](docs/AZURE_DEPLOYMENT.md)** - Despliegue en Azure

### Módulos por Bloque

- [01 - Apertura](docs/modules/01b-apertura.md)
- [02 - Fundamentos](docs/modules/02b-fundamentos.md)
- [03 - Ejercicio 1: Anatomía de un Proveedor MCP](docs/modules/03b-ejercicio-1-anatomia-proveedor.md)
- [04 - Ejercicio 2: Consultas Paramétricas](docs/modules/04b-ejercicio-2-consultas-parametricas.md)
- [05 - Ejercicio 3: Seguridad](docs/modules/05b-ejercicio-3-seguridad.md)
- [06 - Seguridad & Gobernanza](docs/modules/06b-seguridad-gobernanza.md)
- [07 - Ejercicio 4: Analista Virtual](docs/modules/07b-ejercicio-4-analista-virtual.md)
- [08 - Orquestación Multi-Fuente](docs/modules/08-orquestacion-multifuente.md)
- [09 - Ejercicio 5: Agente con Microsoft Agent Framework](docs/modules/09b-ejercicio-5-agente-maf.md)
- [10 - Roadmap & Casos B2B](docs/modules/10-roadmap-casos-b2b.md)
- [11 - Cierre](docs/modules/11-cierre.md)

## 🏗️ Estructura del Proyecto

```
mcp-workshop/
├── docs/                          # Documentación del taller
│   ├── modules/                   # 11 módulos educativos (teoría + ejercicios)
│   │   ├── 01b-apertura.md
│   │   ├── 02b-fundamentos.md
│   │   ├── 03b-ejercicio-1-anatomia-proveedor.md
│   │   ├── 04b-ejercicio-2-consultas-parametricas.md
│   │   ├── 05b-ejercicio-3-seguridad.md
│   │   ├── 06b-seguridad-gobernanza.md
│   │   ├── 07b-ejercicio-4-analista-virtual.md
│   │   ├── 08-orquestacion-multifuente.md
│   │   ├── 09-roadmap-casos-b2b.md
│   │   └── 11-cierre.md
│   ├── AGENDA.md                  # Cronograma detallado 180 minutos
│   ├── INSTRUCTOR_HANDBOOK.md     # Guía para instructores
│   ├── QUICK_REFERENCE.md         # Cheat sheet de MCP y C#
│   └── TROUBLESHOOTING.md         # Solución de problemas
│
│
├── src/                           # Código fuente
│   └── McpWorkshop.Servers/
│       ├── Exercise1StaticResources/      # Puerto 5000: Recursos JSON estáticos
│       ├── Exercise2ParametricQuery/      # Puerto 5001: Herramientas con parámetros
│       ├── Exercise3SecureServer/         # Puerto 5002: JWT + Rate Limiting
│       ├── SqlMcpServer/         # Puerto 5009: Servidor MCP para SQL
│       ├── CosmosMcpServer/      # Puerto 5010: Servidor MCP para Cosmos
│       ├── RestApiMcpServer/     # Puerto 5011: Servidor MCP para REST APIs
│       ├── Exercise4VirtualAnalyst/       # Puerto 5012: Orquestador principal
│       ├── Exercise5AgentServer/          # Agente con Microsoft Agent Framework
│       └── McpWorkshop.Shared/            # Utilidades compartidas
│
├── tests/                         # Suite de pruebas
│   └── McpWorkshop.Tests/
│       ├── Integration/           # 50 integration tests (43 passing, 7 skipped)
│       │   ├── Exercise1IntegrationTests.cs
│       │   ├── Exercise2IntegrationTests.cs
│       │   ├── Exercise3IntegrationTests.cs
│       │   └── Exercise4IntegrationTests.cs
│       ├── Protocol/              # Validación de conformidad JSON-RPC
│       └── Performance/           # Benchmarks de rendimiento
│
│
├── scripts/                       # Scripts de automatización
│   ├── create-sample-data.ps1    # Generar o actualizar datos de ejemplo
│   ├── verify-setup.ps1          # Verificación de prerrequisitos
│   ├── verify-exercise1.ps1      # Validación Ejercicio 1
│   ├── verify-exercise2.ps1      # Validación Ejercicio 2
│   ├── verify-exercise3.ps1      # Validación Ejercicio 3
│   ├── verify-exercise4.ps1      # Validación Ejercicio 4
│   ├── verify-exercise5.ps1      # Validación Ejercicio 5
│   └── run-all-tests.ps1         # Ejecutar suite completa de tests
│
├── specs/                         # Especificaciones del proyecto
│   └── 001-mcp-workshop-course/
│       ├── spec.md               # Especificación completa
│       ├── plan.md               # Plan de implementación
│       ├── tasks.md              # 145 tareas (100% completas)
│       ├── contracts/            # Contratos JSON de cada ejercicio
│       └── research.md           # Investigación técnica
│
├── LICENSE                        # Licencia MIT
├── QUICKSTART.md                  # Guía de inicio rápido
├── README.md                      # Este archivo
└── McpWorkshop.sln               # Solución .NET
```

### Componentes Clave

**Servidores MCP (7 implementaciones)**:

- 3 servidores de ejercicios individuales (1-3)
- 4 servidores para ejercicio grupal (4)

**Documentación (28 archivos)**:

- 19 módulos educativos (con versiones para instructor)
- 9 guías de soporte (agenda, handbook, troubleshooting, etc.)

**Tests (83 tests totales)**:

- 50 integration tests (86% passing, 14% skipped con documentación)
- 25 protocol validation tests
- 8 performance tests

## 🎓 Ejercicios Prácticos

### Ejercicio 1: Recursos Estáticos (15 min)

**Objetivo**: Crear un servidor MCP que expone listas de clientes y productos como recursos estáticos.

**Conceptos clave**:

- Implementación de `resources/list` para descubrimiento
- Implementación de `resources/read` para acceso a datos
- Estructura de recursos MCP (URI, nombre, descripción)
- Serialización JSON de datos estáticos

**Servidor**: `Exercise1StaticResources` (Puerto 5000)

**Verificación**:

```powershell
.\scripts\verify-exercise1.ps1
```

**[📄 Guía completa →](docs/modules/03b-ejercicio-1-anatomia-proveedor.md)** _(Fusionado con demostración en vivo)_

---

### Ejercicio 2: Consultas Paramétricas (20 min)

**Objetivo**: Implementar herramientas MCP con parámetros para búsquedas y filtros dinámicos.

**Conceptos clave**:

- Implementación de `tools/list` para exponer capacidades
- Implementación de `tools/call` para ejecutar herramientas
- Esquemas de validación de parámetros (JSON Schema)
- Herramientas: `GetCustomers`, `SearchOrders`, `CalculateTotal`
- Paginación y filtros opcionales

**Servidor**: `Exercise2ParametricQuery` (Puerto 5001)

**Herramientas implementadas**:

1. **GetCustomers**: Filtrar clientes por país, ciudad, límite
2. **SearchOrders**: Buscar órdenes por cliente, fechas, estado
3. **CalculateTotal**: Calcular totales con aplicación de descuentos

**Verificación**:

```powershell
.\scripts\verify-exercise2.ps1
```

**[📄 Guía completa →](docs/modules/04b-ejercicio-2-consultas-parametricas.md)**

---

### Ejercicio 3: Servidor Seguro (20 min)

**Objetivo**: Agregar autenticación JWT, autorización por scopes, rate limiting y logging estructurado.

**Conceptos clave**:

- Autenticación con tokens JWT (JSON Web Tokens)
- Autorización basada en scopes (`read`, `write`, `admin`)
- Rate limiting por tier de usuario (Base: 10 req/min, Premium: 50 req/min)
- Middleware de seguridad en ASP.NET Core
- Logging estructurado de eventos de seguridad
- Respuestas HTTP 401 (Unauthorized) y 403 (Forbidden)

**Servidor**: `Exercise3SecureServer` (Puerto 5002)

**Scopes disponibles**:

- `read`: Solo lectura de recursos
- `write`: Lectura y modificación
- `admin`: Acceso completo incluyendo configuración

**Verificación**:

```powershell
.\scripts\verify-exercise3.ps1
```

**[📄 Guía completa →](docs/modules/05b-ejercicio-3-seguridad.md)**

---

### Ejercicio 4: Analista Virtual (25 min - Grupal)

**Objetivo**: Construir un orquestador MCP que coordina múltiples servidores para responder preguntas de negocio en español.

**Conceptos clave**:

- Arquitectura multi-servidor (3 servidores MCP independientes)
- Patrones de orquestación: paralelo, secuencial, fan-out
- Parser de lenguaje natural (español) para routing de consultas
- Caching con TTL para optimización
- Manejo de errores y fallbacks
- Síntesis de resultados de múltiples fuentes

**Arquitectura**:

```
Usuario (español) → Orquestador → [SQL Server | Cosmos DB | REST API]
                         ↓
                    Cache (5 min TTL)
                         ↓
                   Respuesta sintetizada
```

**Servidores MCP implementados**:

1. **SqlMcpServer** (Puerto 5009): Datos transaccionales (clientes, órdenes)
2. **CosmosMcpServer** (Puerto 5010): Comportamiento de usuarios (sesiones, carritos)
3. **RestApiMcpServer** (Puerto 5011): APIs externas (inventario, envíos)
4. **Exercise4VirtualAnalyst** (Puerto 5012): Orquestador principal

**Preguntas de ejemplo**:

- "¿Cuántos clientes nuevos registrados en Madrid este mes?"
- "¿Qué usuarios abandonaron carritos en las últimas 24 horas?"
- "¿Cuál es el estado del pedido #1234 y su inventario asociado?"
- "Dame un resumen de ventas de la semana más productos más vendidos"

**Verificación**:

```powershell
.\scripts\verify-exercise4.ps1
```

**[📄 Guía completa →](docs/modules/07b-ejercicio-4-analista-virtual.md)**

### Ejercicio 5: Agente con Microsoft Agent Framework (30 min)

**Objetivo**: Crear un agente conversacional inteligente que integra los MCP servers creados en ejercicios anteriores.

**Conceptos clave**:

- Integración de múltiples servidores MCP (SQL, Cosmos, REST API)
- Descubrimiento automático de herramientas (`ListToolsAsync()`)
- Microsoft Agent Framework (MAF) para agentes conversacionales
- Comprensión de lenguaje natural en español
- Mantenimiento de contexto conversacional (multi-turno)

**Servidor**: `Exercise5AgentServer`

**Verificación**:

```powershell
./scripts/verify-exercise5.ps1
```

**[📄 Guía completa →](docs/modules/09b-ejercicio-5-agente-maf.md)**

## 🛠️ Tecnologías

### Stack Principal

- **Lenguaje**: C# .NET 10.0
- **Framework Web**: ASP.NET Core Minimal APIs
- **MCP Library**: ModelContextProtocol (NuGet prerelease)
- **Autenticación**: System.IdentityModel.Tokens.Jwt
- **Serialización**: System.Text.Json

### Azure Services (Opcionales - Ejercicio 4)

- **Hosting**: Azure Container Apps, Azure App Service
- **Datos**: Azure SQL Database, Azure Cosmos DB
- **Storage**: Azure Blob Storage
- **Monitoring**: Azure Log Analytics, Application Insights

### Infraestructura y Testing

- **Testing**: xUnit 3.1+, Microsoft.AspNetCore.Mvc.Testing
- **Scripting**: PowerShell 7+

### Puertos Utilizados

| Ejercicio                  | Puerto | Servidor         |
| -------------------------- | ------ | ---------------- |
| Ejercicio 1                | 5000   | Exercise1Server  |
| Ejercicio 2                | 5001   | Exercise2Server  |
| Ejercicio 3                | 5002   | Exercise3Server  |
| Ejercicio 4 - SQL          | 5010   | SqlMcpServer     |
| Ejercicio 4 - Cosmos       | 5011   | CosmosMcpServer  |
| Ejercicio 4 - REST         | 5012   | RestApiMcpServer |
| Ejercicio 4 - Orquestador  | 5003   | Exercise4Server  |
| Ejercicio 5 - Agente       | N/A    | Exercise5Agent   |

## 📄 Licencia

Este proyecto está bajo licencia MIT. Ver [LICENSE](LICENSE) para más detalles.

## 🌟 Créditos

Desarrollado como parte del Data Saturday Madrid Workshop 2025.

Este taller fue construido utilizando [GitHub Spec-Kit](https://github.com/github/spec-kit) - un framework de GitHub para desarrollo guiado por especificaciones.

---

## **¡Disfruta del taller y construye servidores MCP increíbles! 🚀**
