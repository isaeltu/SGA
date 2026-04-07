# Solution Architecture Document (SAD)
## Sistema de Gestión de Autobuses ITLA (SGA-ITLA)

### Información del documento
- **Asignatura:** Programación II  
- **Nombre:** Isael Fermin Capellan Valdez  
- **Matrícula:** 2023-1364  
- **Carrera:** Desarrollo de Software  
- **Tema:** Solution Architecture Document (SAD)  
- **Sistema:** Sistema de Gestión de Autobús ITLA (SGA-ITLA)  
- **Profesor:** Francis Ramirez  
- **Cuatrimestre:** C1 – 2026  

---

## 2. Introducción

### 2.1 Descripción del sistema
El ITLA transporta diariamente a más de **2,000 estudiantes** y **200 empleados** desde diferentes puntos de Santo Domingo hasta el campus. Actualmente, este servicio se gestiona de forma manual, lo que genera problemas de coordinación, falta de información en tiempo real y dificultad para hacer seguimiento.

El **Sistema de Gestión de Autobuses del ITLA (SGA-ITLA)** es la solución diseñada para **digitalizar completamente** este servicio. Con este sistema:

- **Estudiantes y empleados** pueden ver autobuses en tiempo real, hacer reservas y recibir notificaciones.
- **Operadores** pueden asignar viajes, monitorear la flota con GPS y gestionar incidencias desde una aplicación de escritorio.
- **Administradores** pueden acceder a reportes completos para tomar decisiones basadas en datos reales.

El sistema se compone de tres aplicaciones principales:

1. **Aplicación Web** (para estudiantes, empleados y administradores).
2. **Aplicación de Escritorio** (para operadores del servicio).
3. **API central** que conecta todo y maneja la lógica de negocio.

Además, integra servicios externos como:
- GPS (tracking en tiempo real)
- Google Maps (visualización de rutas)
- Notificaciones por Email y SMS

---

### 2.2 Alcance del proyecto

#### Lo que incluye el sistema
El sistema cubre el ciclo completo del servicio de transporte:

- Gestión completa de usuarios (estudiantes, empleados, conductores, operadores, administradores)
- Administración de la flota de autobuses con control de estado
- Configuración de rutas con paradas georreferenciadas
- Asignación de viajes a conductores
- Tracking GPS en tiempo real
- Sistema de reservas con validación de capacidad
- Control de autorizaciones digitales (tarjetas institucionales)
- Registro y seguimiento de incidencias
- Notificaciones automáticas por email, SMS y push
- Reportes de uso, ocupación y rendimiento del servicio

#### Usuarios del sistema (estimación)
- ~2,000 estudiantes
- ~200 empleados
- 10–15 conductores
- 3–5 operadores
- 2–3 administradores

#### Tecnología base
- **Frontend Web:** Blazor Server
- **Frontend Desktop:** Windows Forms
- **Backend:** ASP.NET Core 8 Web API
- **Base de datos:** SQL Server 2022
- **GPS:** Integración con dispositivos de tracking
- **Mapas:** Google Maps API

---

### 2.3 Objetivos

**Mejorar la operación diaria**  
Reducir el trabajo manual de asignación de viajes y permitir monitoreo en tiempo real: ubicación, ocupación y puntualidad. Los operadores podrán reaccionar rápidamente mediante alertas automáticas.

**Mejor experiencia para usuarios**  
Los estudiantes ya no dependen de suposiciones sobre cupo o llegada. Podrán ver rutas, reservar y recibir notificaciones por cambios.

**Tomar decisiones con datos**  
Reportes sobre uso por ruta/horario, demanda y rendimiento real del servicio, apoyando optimización basada en evidencia.

**Sistema bien diseñado y mantenible**  
Se aplica **Clean Architecture** para facilitar evolución. Cambios futuros (base de datos, app móvil, nuevos servicios) se podrán implementar sin reescribir todo el sistema.

---

## 3. Vista general del sistema

### 3.1 Diagrama de contexto
El SGA-ITLA es el sistema central. Los actores que interactúan son:

- Estudiantes/Empleados: usan la web para rutas, reservas, notificaciones.
- Conductores: reciben SMS cuando se asigna un viaje.
- Operadores: usan aplicación de escritorio para gestionar la operación.
- Administradores: configuran sistema y revisan reportes desde la web.

Sistemas externos:
- **Notification Hub:** emails, SMS y push notifications
- **Google Maps API:** mapas y geolocalización
- **GPS Services:** datos de ubicación en tiempo real

---

### 3.2 Diagrama de containers
Dentro del boundary del sistema existen 5 containers principales:

1. **SGA.Web (Blazor Server)**: web para estudiantes/empleados/administradores.
2. **SGA.WinForms**: desktop para operadores.
3. **SGA.API**: lógica central y endpoints REST.
4. **Database (SQL Server)**: persistencia de datos.
5. **Notification Worker**: servicio en segundo plano para notificaciones.

Fuera del boundary:
- **Message Queue**: comunicación asíncrona con el worker
- **Google Maps API**
- **GPS Services**

---

### 3.3 Diagrama de componentes (interno de la API)
La API se organiza con **Clean Architecture**:

- **Capa API:** Controllers + Middleware (seguridad, manejo de errores, logging)
- **Capa Application:** Handlers (casos de uso), DTOs, Validators (FluentValidation), CQRS (MediatR)
- **Capa Domain:** lógica de negocio pura (reglas y entidades)
- **Capa Infrastructure:** Repositories (EF Core), servicios externos (GPS, email, SMS, maps), autenticación (JWT)

---

## 4. Vistas arquitectónicas (Modelo 4+1)

### 4.1 Vista lógica (módulos del sistema)

#### Autenticación y autorización
- Login, generación y validación de JWT.
- Control de acceso por rol.
- Token con duración estimada de **24 horas**.
- Middleware valida token y permisos en cada request.

#### Gestión de usuarios
- Registro/actualización de usuarios.
- Asignación de roles.
- Datos específicos por perfil:
  - Estudiante: carrera, período
  - Empleado: departamento
  - Conductor: licencia
  - Operador/Admin: datos internos

#### Gestión de autobuses
- Inventario (placa, modelo, año, capacidad, GPS).
- Estado del autobús:
  - Disponible / En mantenimiento / Fuera de servicio
- Integración con Viajes e Incidencias.

#### Gestión de rutas y horarios
- Rutas: origen, destino, paradas.
- Paradas con coordenadas GPS (para mapas).
- Horarios por día/semana.
- Consultado por usuarios y usado para crear viajes.

#### Gestión de viajes
- Viaje = ruta + fecha/hora + conductor + autobús.
- Estados: programado, en curso, finalizado, cancelado, retrasado.
- Interacciones:
  - Autobuses (disponibilidad)
  - Rutas (datos base)
  - GPS (ubicación)
  - Notificaciones (viaje asignado)

#### Gestión de reservas
- Permite reservar cupo.
- Validaciones:
  - que el viaje exista
  - capacidad disponible
  - autorización del usuario
- Crea reserva y reduce cupo.
- Soporta cancelación y registro de abordajes.
- Publica evento “ReservaConfirmada” para email.

#### Gestión de autorizaciones
- Control de acceso al servicio: tarjeta digital/institucional.
- Tipos: beca, pase mensual, prepago, empleado, etc.
- Reglas: vigencia, saldo disponible, consumo y transacciones.
- Usado por Reservas y por Abordaje.

#### Gestión de incidencias
- Incidencias reportadas por conductores o detectadas.
- Tipo, descripción, severidad, estado (registrada→resuelta).
- Asociadas a un viaje.
- Si es crítica: notifica operadores por email.

#### Tracking GPS (tiempo real)
- Recibe coordenadas cada ~30 segundos.
- Guarda en DB vinculado al viaje.
- Publica por **SignalR** a clientes conectados.
- Calcula ETA y guarda historial para reportes.

#### Notificaciones
- Recibe eventos: “ReservaConfirmada”, “ViajeAsignado”, “IncidenciaRegistrada”, etc.
- Decide canal:
  - Email (confirmaciones)
  - SMS (críticas / conductores)
  - Push (alertas rápidas)
- Registra auditoría: destinatario, canal, contenido, éxito/fallo.

#### Reportes y auditoría
- Reportes: uso por período, ocupación por ruta, puntualidad, incidencias frecuentes, costos.
- Logs: quién hizo qué y cuándo.
- Exportación: web + PDF + Excel.

#### Interacciones entre módulos
- Módulos maestros (Usuarios, Autobuses, Rutas): consultados por otros.
- Módulos operativos (Viajes, Reservas, Autorizaciones): coordinan el día a día.
- Módulos de soporte (Incidencias, GPS, Notificaciones, Reportes): servicios auxiliares.
- Comunicación por interfaces; eventos asíncronos para efectos secundarios (notificaciones).

---

### 4.2 Vista de procesos (flujos principales)
Procesos principales documentados:
1. Crear Reserva  
2. Asignar Viaje a Conductor  
3. Usuario Aborda Autobús  
4. Tracking GPS en Tiempo Real  
5. Gestión de Incidencia  

> Nota: estos procesos están soportados por diagramas de secuencia y flujos (ver sección 10).

---

### 4.3 Vista de desarrollo (estructura de carpetas y capas)

#### Estructura de solución
- `src/`
  - `SGA.Domain/`
  - `SGA.Application/`
  - `SGA.Infrastructure/`
  - `SGA.API/`
  - `SGA.Web/`
  - `SGA.WinForms/`
  - `SGA.NotificationWorker/`
- `docs/`
- `tests/`
  - `Unit/`
  - `Integration/`

#### Organización por capas

| Capa | Proyecto/Carpeta | Dependencias | Contenido |
|---|---|---|---|
| Domain | SGA.Domain | Ninguna | Entidades, Value Objects, Interfaces, Enums |
| Application | SGA.Application | Domain | Use Cases, DTOs, Validators, Handlers (CQRS) |
| Infrastructure | SGA.Infrastructure | Domain, Application | Repositorios, EF Core, servicios externos |
| API | SGA.API | Application, Infrastructure | Controllers, Middleware, Startup/Program |
| Web | SGA.Web | Application | Pages Blazor, Components, Services |
| Desktop | SGA.WinForms | Application | Forms, Presenters (MVP) |
| Worker | SGA.NotificationWorker | Application, Infrastructure | Background service, consumers |

---

## 5. Componentes del sistema

### 5.1 Componentes de presentación

#### SGA.Web — Aplicación Web (Blazor Server)
Aplicación principal para estudiantes, empleados y administradores.

**Capacidades:**
- Consultar rutas y horarios
- Crear reservas
- Ver mapa con GPS en tiempo real
- Historial de viajes
- Administración (usuarios, autobuses, rutas, reportes)

**Tiempo real:** SignalR actualiza mapas sin refrescar página.

**Justificación de Blazor Server:**
- Desarrollo rápido en C#
- Reutilización de conocimiento y código
- Evita complejidad de frameworks JS
- Requiere conexión constante, asumida estable (campus/WiFi)

##### Diseño de UI Web “bonita” (Blazor) — detalle arquitectónico
**Objetivo:** una interfaz moderna, consistente y rápida para diferentes roles.

**Framework UI recomendado:**
- **MudBlazor** (Material Design) *o* **Radzen** (estilo enterprise).

**Layout general:**
- **Sidebar** con módulos por rol.
- **Topbar** con: búsqueda rápida, notificaciones, perfil de usuario.
- **Diseño responsive**: móvil/tablet/desktop.
- **Sistema de temas**: modo claro/oscuro (opcional).

**Páginas por rol (propuesta):**
- **Estudiante/Empleado**
  - Dashboard (próximos viajes + estado general)
  - Rutas & Horarios (lista + mapa)
  - Reservas (crear/cancelar)
  - Mis Viajes (historial y detalles)
  - Autorización / Saldo (vigencia, saldo, tipo)
  - Notificaciones (historial)
- **Administrador**
  - Gestión de Usuarios (CRUD + roles)
  - Gestión de Autobuses (estado, capacidad, GPS)
  - Configurar Rutas/Paradas (con selección en mapa)
  - Gestión de Viajes (opcional desde web)
  - Reportes (gráficas + exportación PDF/Excel)
  - Logs/Auditoría (filtros avanzados)

**Componentes compartidos (Blazor):**
- Tablas con filtros/paginación/sort
- Modales de confirmación
- Formularios con validación (FluentValidation opcional vía API + validación UI)
- Badges de estado (`Programado`, `EnCurso`, `Cancelado`, etc.)
- Mapa (Google Maps) con markers y polylines de rutas
- Mapa en tiempo real (SignalR) para posiciones GPS

**Integración técnica:**
- `HttpClient` con handler para token JWT.
- Manejo de errores con `ProblemDetails`.
- SignalR:
  - `GpsHub`: actualizaciones de ubicación
  - `NotificationsHub` (opcional): eventos en tiempo real

---

#### SGA.WinForms — Aplicación de escritorio (Windows Forms)
Aplicación para operadores.

**Capacidades:**
- Asignar viajes
- Monitorear operaciones
- Gestionar incidencias
- Visualizar autobuses en mapa

**Patrón:** MVP (Forms + Presenters).  
**Justificación:** rapidez, múltiples ventanas, estabilidad y productividad.

---

### 5.2 Componentes de backend

#### SGA.API — API REST (ASP.NET Core 8)
Centraliza lógica de negocio, expone endpoints REST y mantiene la arquitectura limpia.

- Middleware: JWT, excepciones, logging (Serilog)
- CQRS: MediatR
- Validación: FluentValidation
- Persistencia: EF Core

**Ventaja:** cambiar DB o servicios externos requiere cambios principalmente en Infrastructure.

#### SGA.NotificationWorker — Servicio de notificaciones
Worker en segundo plano que consume eventos de la cola:
- `ViajeAsignado`
- `ReservaConfirmada`
- `IncidenciaRegistrada` (crítica)

**Política de reintento:**
- Reintenta hasta 3 veces
- Luego envía a Dead-Letter Queue / cola de error

---

### 5.3 Componentes de infraestructura

#### Base de datos — SQL Server 2022
Contiene:
- Usuarios, Autobuses, Rutas, Paradas, Viajes, Reservas
- Autorizaciones, Transacciones, Abordajes
- Incidencias, UbicacionesGPS
- Notificaciones enviadas y Logs

Optimización:
- Índices para consultas frecuentes
- Auditoría de creación/modificación
- EF Core como acceso principal (SQL manual sólo para reportes complejos)

#### Message Queue — Azure Service Bus / RabbitMQ
Comunicación asíncrona API → Worker.
- En producción: Azure Service Bus (administrado)
- Local/dev: RabbitMQ con Docker

#### Servicios GPS
Dispositivos en autobuses envían ubicación cada 30s a un proveedor (ej. Traccar).
- API recibe/consulta y guarda ubicaciones
- Publica por SignalR a clientes
- Historial completo para reportes

#### Servicios externos
- Google Maps API: mapa interactivo y geolocalización
- SendGrid: emails
- Twilio: SMS

Encapsulados en Infrastructure para permitir reemplazo fácil.

---

### 5.4 Justificación de componentes
- **Web vs Desktop:** usuarios finales usan web; operadores requieren desktop más eficiente.
- **Clean Architecture:** separa responsabilidades, facilita cambios.
- **CQRS/MediatR:** lectura vs escritura; handlers testeables.
- **Worker de notificaciones:** no bloquea la API; procesamiento paralelo.
- **Docker:** facilita despliegue, consistencia y escalabilidad.

---

## 6. Decisiones arquitectónicas
- Clean Architecture (Domain/Application/Infrastructure/API)
- CQRS + MediatR
- Notificaciones asíncronas (cola + worker)
- Tracking GPS y SignalR (tiempo real)
- Desktop para operadores (WinForms + MVP)
- Docker para despliegue

---

## 7. Requisitos no funcionales (resumen)
- **Rendimiento:** <2s; GPS cada 30s; DB indexada.
- **Escalabilidad:** ~500 usuarios concurrentes; scaling horizontal con Docker.
- **Seguridad:** HTTPS, BCrypt, JWT 24h, roles.
- **Disponibilidad:** 99% (5:00 AM a 10:00 PM); backups diarios; auto-restart contenedores.
- **Usabilidad:** web responsive + desktop con atajos.
- **Compatibilidad:** navegadores modernos; Windows 10/11 + .NET 8 Runtime.
- **Mantenibilidad:** capas claras, pruebas, logging estructurado (Serilog).

---

## 8. Riesgos técnicos
- Fallo GPS: respaldo, alertas >5 min sin reportar, sistema sigue sin GPS.
- Caída BD: backups, plan recuperación, monitoreo.
- Fallo externos: reintentos, colas, fallback proveedores.
- Rendimiento: pruebas carga, caché, índices, escalamiento.
- Seguridad: updates, auditoría, HTTPS, JWT expira.
- Conexión GPS intermitente: última ubicación, alertas.
- Costos externos: monitoreo mensual y límites.

---

## 9. Conclusiones
SGA-ITLA aplica Clean Architecture, CQRS y mensajería asíncrona para una solución robusta que digitaliza el transporte del ITLA. Docker simplifica despliegue, SignalR habilita tracking en tiempo real y los reportes basados en datos permiten optimización continua. La arquitectura está lista para evolucionar sin reescribir el sistema.

---

## 10. Anexos — Descripción detallada de imágenes (1–11)

> Nota: En este chat, las imágenes fueron provistas como `image1`, `image2`, etc.  
> Cuando las subas al repositorio, se recomienda guardarlas en `docs/img/` con nombres claros y luego actualizar las referencias del documento.

### Imagen 1 — Error de Git al hacer Push (VS Code)
**Qué se ve:**
- Captura de pantalla del editor (probablemente VS Code) con un modal que dice **“Git - Push failed”**.
- El mensaje indica: *“Unable to push to the remote repository because your local branch is behind the remote branch. See the Output window for more details.”*
- Aparece botón **Cancel**.

**Qué significa:**
- Tu rama local no contiene los cambios más recientes que ya están en GitHub (remote).
- Git evita el push para no sobrescribir el historial.

**Cómo se resuelve (recomendación de flujo):**
1. `git pull --rebase`
2. Resolver conflictos si aparecen
3. `git push`

---

### Imagen 2 — Diagrama de secuencia: Validación de abordaje (QR/Tarjeta digital)
**Participantes:**
- Usuario
- Sistema de Validación (Lector QR/Tarjeta Digital)
- `SGA.API`
- `Database`

**Flujo:**
1. El usuario presenta tarjeta/QR al lector.
2. El lector invoca: `POST /api/abordajes/validar {autorizacionId, viajeId}`.
3. La API consulta datos de autorización en DB.
4. La API ejecuta validaciones:
   - Validar vigencia de la autorización.
   - Validar saldo disponible.
5. La API verifica capacidad del viaje en DB y recibe capacidad disponible.
6. Si pasa validación: registra abordaje en DB y recibe confirmación.

**Alternativas (alt):**
- **Validación exitosa** → respuesta **200 OK** con “Acceso permitido” → el usuario puede abordar.
- **Validación fallida** → respuesta **403 Forbidden** + motivo → acceso denegado (no autorizado / fecha vencida / saldo insuficiente / sin cupo).

**Aporte arquitectónico:**
- Aplica reglas de negocio antes de abordar: autorización, vigencia, saldo y capacidad.
- Deja evidencia en DB (registro de abordaje) para auditoría y reportes.

---

### Imagen 3 — Diagrama de secuencia: Asignación de viaje + SMS al conductor
**Participantes:**
- Operador
- `SGA.WinForms`
- `SGA.API`
- `Database`
- `Message Queue`
- `Notification Worker`
- `Twilio (SMS)`
- Conductor

**Flujo:**
1. Operador selecciona ruta, conductor y autobús desde WinForms.
2. WinForms llama: `POST /api/viajes/asignar {rutaId, conductorId, autobusId}`.
3. API valida disponibilidad del conductor y del autobús en DB.
4. API crea registro de viaje (ej. ID: 123).
5. API publica evento **“ViajeAsignado”** en la cola.
6. Se responde a WinForms con **201 Created** + datos del viaje.
7. Worker consume evento, construye mensaje SMS y lo envía via Twilio.
8. Conductor recibe SMS.
9. Se registra en DB que la notificación fue enviada.

**Aporte arquitectónico:**
- Uso de mensajería asíncrona para que la asignación de viaje no dependa del envío de SMS.
- Mejora rendimiento y resiliencia.

---

### Imagen 4 — Diagrama de secuencia: Reserva de viaje + Email de confirmación
**Participantes:**
- Usuario
- `SGA.Web`
- `SGA.API`
- `Database`
- `Message Queue`
- `Notification Worker`
- `Email Service`

**Flujo:**
1. Usuario selecciona viaje y crea reserva en la web.
2. Web llama: `POST /api/reservas {viajeId, usuarioId}`.
3. API valida token JWT.
4. API consulta viaje y capacidad.
5. API consulta autorización del usuario.
6. API valida saldo/vigencia.
7. API crea reserva en DB.
8. API publica evento **“ReservaConfirmada”**.
9. API responde **201 Created** y la web muestra confirmación en pantalla.
10. Worker consume evento y envía email de confirmación.
11. DB registra la notificación enviada y el usuario recibe el correo.

**Aporte arquitectónico:**
- Seguridad (JWT).
- Reserva transaccional con control de capacidad.
- Notificación asíncrona (no bloquea la respuesta de la API).

---

### Imagen 5 — Diagrama de secuencia: Reserva de viaje (duplicado/variante)
**Qué se ve:**
- Es el mismo flujo general de la imagen 4 (reserva + JWT + evento + email).
- Se puede conservar como variante o eliminar por redundancia.

**Recomendación:**
- Mantener solo una versión en el repositorio para evitar confusión (a menos que muestre alguna diferencia importante).

---

### Imagen 6 — Diagrama de casos de uso: SGA‑ITLA
**Actores:**
- Usuario (Estudiante/Empleado)
- Operador
- Conductor
- Administrador

**Casos de uso principales:**
- Consultar rutas y horarios
- Ver ubicación GPS en tiempo real
- Consultar historial de viajes
- Crear reserva (incluye consultar rutas/horarios)
- Monitorear viajes en tiempo real (operador)
- Asignar viaje a conductor (operador)
- Gestionar incidencias (operador)
- Reportar incidencia (conductor)
- Gestionar usuarios (admin)
- Gestionar autobuses (admin)
- Configurar rutas (admin)
- Generar reportes (admin)

**Aporte arquitectónico:**
- Define alcance funcional por rol y los módulos UI que deben existir.

---

### Imagen 7 — Estructura de solución (Clean Architecture + apps)
**Qué se ve:**
- Carpeta `src/` con proyectos:
  - `SGA.Domain` (Entities, ValueObjects, Interfaces)
  - `SGA.Application` (Features, Validators, Common)
  - `SGA.Infrastructure` (Persistence, Repositories, Services: GPS/Email/Maps)
  - `SGA.API` (Controllers, Middleware)
  - `SGA.Web` (Pages, Components)
  - `SGA.WinForms` (Forms, Presenters)
  - `SGA.NotificationWorker`
- `docs/` para documentación
- `tests/` con Unit e Integration

**Aporte arquitectónico:**
- Evidencia separación de responsabilidades y soporte para pruebas.

---

### Imagen 8 — Flujo (actividad): Registro de incidencia + decisión de criticidad
**Pasos:**
1. Conductor detecta problema.
2. Conductor llama a centro de operaciones.
3. Operador abre aplicación desktop.
4. Operador registra: tipo, descripción, severidad.
5. API recibe: `POST /api/incidencias`.
6. Guardar en base de datos.
7. Publicar evento **IncidenciaRegistrada**.
8. Decisión **¿Es crítica?**
   - Sí → sugerir cancelar viaje.
   - No → registrar para seguimiento.
9. Fin.

**Aporte arquitectónico:**
- Flujo claro de operación.
- Eventos para disparar acciones (notificaciones / escalamiento).

---

### Imagen 9 — Flujo (actividad): GPS en tiempo real + SignalR
**Pasos:**
1. Dispositivo GPS en autobús.
2. Envía ubicación cada 30 segundos.
3. API recibe `POST /api/gps/update`.
4. Decisión: **¿Viaje activo?**
   - No → ignorar datos.
   - Sí → guardar en DB + publicar via SignalR a clientes conectados.
5. Clientes actualizan mapas:
   - Usuarios en app web.
   - Operadores en mapa de monitoreo.
6. Fin.

**Aporte arquitectónico:**
- Tiempo real con SignalR.
- Persistencia + broadcast (histórico y live view).

---

### Imagen 10 — Modelo de dominio (UML) con IDs `int`
**Entidades destacadas:**
- Autorizacion, Tarjeta, Transaccion
- Viaje, Ruta, Parada, RutaParada
- Autobus
- Reserva, Abordaje
- UbicacionGPS
- Incidencia
- Usuario + Estudiante/Empleado/Conductor

**Aporte arquitectónico:**
- Base conceptual del modelo relacional y de las entidades del Domain.
- Relaciona reservas/abordajes con viaje, autorizaciones y usuarios.

---

### Imagen 11 — Modelo de dominio refinado con `Guid` + Enums + roles extra
**Mejoras vs imagen 10:**
- IDs `Guid` (mejor para integraciones y sistemas distribuidos).
- Enums para estados/tipos:
  - EstadoViaje, EstadoAutobus, TipoAutorizacion, EstadoReserva, TipoIncidencia, SeveridadIncidencia.
- Roles adicionales: Operador y Administrador como entidades/perfiles.
- Métodos extra (validación/vigencia/capacidad/cálculos).

**Aporte arquitectónico:**
- Modelo más fuerte y tipado para implementación real.
- Reduce errores al estandarizar estados y tipos con enums.