# Guion de capturas y video (SGA)

## Preparacion rapida
1. Inicia API en una terminal:
   - dotnet run --project /workspaces/SGA/SGA.Api/SGA.Api.csproj
2. Inicia Web en otra terminal:
   - dotnet run --project /workspaces/SGA/SGA.Web/SGA.Web.csproj
3. Abre la web en navegador (normalmente https://localhost:7xxx).
4. Abre Swagger API (normalmente https://localhost:7200/swagger).

## Capturas requeridas

### Captura 1 - Crear institucion y ver ID retornado (201)
Objetivo: demostrar POST exitoso.
Pasos:
1. En Swagger, endpoint POST /api/institutions.
2. Payload valido:
   {
     "code": "INST-EV-01",
     "name": "Institucion Evidencia 01",
     "createdBy": "evidencia-user"
   }
3. Ejecuta y captura pantalla mostrando:
   - Status 201
   - Response body con ID (>0)

Archivo sugerido: captura_01_crear_institucion_201.png

### Captura 2 - Listado de instituciones actualizado en UI
Objetivo: demostrar GET y renderizado UI.
Pasos:
1. En SGA.Web, abre la pagina de instituciones.
2. Refresca listado.
3. Captura donde se vea la institucion INST-EV-01 en tabla/lista.

Archivo sugerido: captura_02_listado_instituciones_ui.png

### Captura 3 - Crear bus y ver reflejo en consulta
Objetivo: demostrar ciclo POST + GET en buses.
Pasos:
1. Crea un bus desde UI o Swagger con InstitutionId existente.
2. Consulta buses (UI listado o GET /api/buses).
3. Captura con el bus recien creado visible.

Archivo sugerido: captura_03_bus_creado_y_consulta.png

### Captura 4 - Crear viaje y cambiar estado (start/complete/cancel)
Objetivo: demostrar comandos de estado de viajes.
Pasos:
1. Crea viaje en estado inicial (Scheduled).
2. Ejecuta start.
3. Ejecuta complete o cancel (uno de los dos para cerrar flujo).
4. Captura donde se vea el estado antes/despues.

Archivo sugerido: captura_04_viaje_cambio_estado.png

### Captura 5 - Reserva normal y reserva invitado
Objetivo: demostrar ambos flujos.
Pasos:
1. Reserva normal (con personId y authorizationId validos).
2. Reserva invitado desde portal cliente-estudiante.
3. Captura con confirmacion de ambos casos (IDs o mensaje de exito).

Archivo sugerido: captura_05_reserva_normal_e_invitado.png

### Captura 6 - Validacion de datos invalidos (400 visible en UI)
Objetivo: demostrar manejo de errores en frontend.
Pasos recomendados (mas simple):
1. En UI de crear institucion, coloca Code con mas de 20 caracteres.
   - Ejemplo: CODIGO-INSTITUCION-DEMASIADO-LARGO-001
2. Deja Name valido.
3. Enviar formulario.
4. Captura mostrando:
   - dato invalido ingresado
   - mensaje de error visible en UI (API error 400 o equivalente)

Archivo sugerido: captura_06_validacion_400_ui.png

### Captura 7 - Caso no encontrado (404)
Objetivo: demostrar manejo de recurso inexistente.
Pasos:
1. En Swagger, GET /api/institutions/999999
2. Ejecuta y captura mostrando Status 404.

Archivo sugerido: captura_07_not_found_404.png

## Guion del video (2 a 5 minutos)

### Estructura sugerida
1. Intro (15-20s)
   - "Este video muestra integracion UI-API en SGA con manejo de errores y validaciones".
2. CRUD corto de instituciones (45-60s)
   - Crear (201) -> listar en UI.
3. Operacion de viajes (30-45s)
   - Crear viaje y cambiar estado (start/complete o cancel).
4. Reservas (40-60s)
   - Normal + invitado.
5. Error controlado (20-30s)
   - Caso de validacion 400 en UI.
6. Cierre (10-15s)
   - "Arquitectura desacoplada, consumo por servicio y validaciones distribuidas".

## Checklist de cierre
- [ ] Captura 1 lista
- [ ] Captura 2 lista
- [ ] Captura 3 lista
- [ ] Captura 4 lista
- [ ] Captura 5 lista
- [ ] Captura 6 lista
- [ ] Captura 7 lista
- [ ] Video grabado (2-5 min)
- [ ] Evidencias guardadas en docs/evidencias/

## Recomendaciones de formato
- Resolucion: 1366x768 o 1920x1080.
- Nombra archivos exactamente como sugerido.
- Evita datos sensibles reales en pantalla.
- Si un endpoint falla por datos, corrige y repite para mantener narrativa limpia.
