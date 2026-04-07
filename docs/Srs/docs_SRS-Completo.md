# SRS — Sistema de Gestión de Autobuses (SGA-ITLA)

## Información del documento

- **Plan de negocios**
- **Nombre:** Isael Fermin Capellan Valdez  
- **Matrícula:** 2023-1364  
- **Carrera:** Desarrollo de Software  
- **Tema:** SISTEMA DE GESTIÓN AUTOBUSES (SRS)  
- **Profesor:** Francis Ramirez  
- **Cuatrimestre:** C1 – 2026  

Este documento de requisitos de software (SRS) describe en detalle el propósito, las funciones, las interfaces y el comportamiento esperado del sistema de software. Está destinado a ser utilizado por las partes interesadas del proyecto, incluidos clientes, usuarios, analistas de negocios, diseñadores, desarrolladores y evaluadores.

---

## 1. Introducción

### 1.1 Propósito
El propósito de este documento es definir de manera clara y estructurada los requisitos del **Sistema de Gestión de Autobuses del ITLA (SGA-ITLA)**. Este documento sirve como referencia para los actores involucrados en el análisis, validación y control del sistema, estableciendo su alcance, así como los requerimientos funcionales y no funcionales necesarios para garantizar una comprensión común del sistema a desarrollar.

### 1.2 Público objetivo
Este documento está dirigido a los distintos actores involucrados en el análisis, validación, desarrollo y supervisión del Sistema de Gestión de Autobuses del ITLA (SGA-ITLA). El público objetivo incluye:

- Analistas de sistemas
- Desarrolladores
- Personal administrativo responsable del servicio de transporte
- Autoridades institucionales
- Usuarios finales del sistema
- Consultores que participen en la revisión o evaluación del proyecto

### 1.3 Alcance del producto
El Sistema de Gestión de Autobuses del ITLA (SGA-ITLA) abarca la gestión operativa y administrativa del servicio de transporte institucional. El sistema contempla:

- Validación de usuarios autorizados
- Control del uso del servicio
- Organización del abordaje de autobuses
- Gestión de reportes operativos generados por los conductores

Asimismo, permite centralizar la información relacionada con la operación del transporte con el fin de mejorar el control, la trazabilidad y la eficiencia del servicio dentro de la institución.

### 1.4 Definiciones y acrónimos

- **SGA-ITLA:** Sistema de Gestión de Autobuses del Instituto Tecnológico de las Américas (ITLA).
- **ITLA:** Instituto Tecnológico de las Américas.
- **Usuario:** Persona autorizada por la institución para utilizar el servicio de transporte, incluyendo estudiantes y empleados.
- **Conductor:** Empleado responsable de la operación de un autobús institucional asignado a un viaje.
- **Viaje:** Ejecución de una ruta de transporte en un horario definido, desde un punto de inicio hasta un punto de finalización.

---

## 2. Descripción general

### 2.1 Necesidades del usuario
Los usuarios del Sistema de Gestión de Autobuses SGA-ITLA necesitan:

- Gestionar consultas
- Comprar tickets
- Conocer el estado de los autobuses
- Verificar si son usuarios autorizados

Actualmente la gestión operativa y el control administrativo se realizan de manera manual, lo que retrasa los procesos en las operaciones diarias.

Los estudiantes y empleados requieren una plataforma que les permita:

- Consultar de forma rápida rutas, paradas y horarios disponibles
- Conocer el estado de los autobuses
- Planificar su llegada al campus adecuadamente

Por otra parte, los conductores deben poder:

- Consultar los viajes asignados
- Reportar incidencias ocurridas durante el recorrido

Adicionalmente, el sistema debe brindar información clara y confiable ante situaciones excepcionales:

- Retrasos
- Cancelaciones
- Cambios de rutas
- Limitaciones de capacidad

La ausencia de información oportuna genera confusión, pérdida de tiempo y conflictos entre usuarios, conductores y personal operativo.

También existe la necesidad de reducir la carga operativa del personal administrativo y de operaciones, quienes actualmente realizan validaciones, controles y registros manuales, incrementando el riesgo de errores y duplicidad de información. Se espera automatizar los procesos y garantizar reglas institucionales uniformes y transparentes.

### 2.2 Suposiciones y dependencias

#### Suposiciones
- Se asume que los estudiantes y empleados pertenecen a la comunidad del ITLA y cumplen las condiciones institucionales.
- Se asume que los conductores cuentan con responsabilidades previamente definidas por la institución.
- Se asume que pueden presentarse incidencias que afecten la operación normal de los viajes.
- Se asume que existe personal responsable de registrar, actualizar y mantener información del sistema (rutas, horarios, autobuses y usuarios).
- Se asume que la institución requiere información consolidada para supervisión, control y evaluación del servicio.

#### Dependencias
- El sistema depende de la correcta participación de operadores, personal administrativo y conductores.
- Depende de que los procesos institucionales de registro y autorizaciones se ejecuten según políticas del ITLA.
- La confiabilidad depende del mantenimiento continuo y actualización de datos.
- El uso efectivo depende del cumplimiento de normativas internas de transporte institucional.
- La precisión de reportes depende de la veracidad y calidad de la información registrada.

---

## 3. Requerimientos

### 3.1 Requerimientos funcionales

#### 3.1.1 Gestión de usuarios

**RF001 – Registrar usuarios según su tipo**  
El sistema debe permitir registrar usuarios según su tipo:

- **Estudiantes:**
  - Capturar: primer nombre, primer apellido, cédula, correo electrónico, fecha de nacimiento y contraseña.
  - Generar automáticamente una matrícula.
  - Asignar un ID único y establecer estado.

- **Empleados:**
  - Capturar: primer nombre, primer apellido, cédula, correo electrónico, fecha de nacimiento, sueldo y estado.

- **Conductores:**
  - Capturar: primer nombre, primer apellido, cédula.
  - Asignar un ID único y establecer estado.

**RF002 – Consultar usuarios**  
El sistema debe permitir consultar usuarios mediante **ID**, **cédula** o **correo electrónico**.

**RF003 – Validar usuario**  
El sistema debe validar si un usuario está autorizado para utilizar el servicio de autobuses.

---

#### 3.1.2 Gestión de rutas, horarios y viajes

**RF004 – Consultar rutas y horarios**  
El sistema debe permitir a los usuarios consultar rutas y horarios disponibles.

**RF005 – Asignar y cancelar viajes**  
El sistema debe permitir que el operador asigne y cancele viajes a los conductores.

**RF006 – Iniciar viaje**  
El sistema debe permitir que el conductor inicie un viaje asignado.

**RF007 – Finalizar viaje**  
El sistema debe permitir que el conductor finalice el viaje asignado.

**RF008 – Monitorear viajes**  
El sistema debe permitir al operador monitorear el estado de los viajes: **programado**, **en curso**, **retrasado** o **cancelado**.

**RF009 – Consultar historial de viajes**  
El sistema debe permitir a usuarios autorizados consultar el historial de viajes.

**RF010 – Crear reservas de viaje**  
El sistema debe permitir que estudiantes y empleados creen reservas para gestionar y validar la ocupación del autobús.

**RF011 – Registrar viajes realizados**  
El sistema debe registrar:
- Fecha y hora de inicio y finalización
- Usuarios que abordaron
- Tipo de autorización utilizada

---

#### 3.1.3 Gestión de autobuses

**RF012 – Gestión de autobuses**  
El sistema debe permitir registrar, actualizar y consultar información de los autobuses.

**RF013 – Controlar capacidad del autobús**  
El sistema debe verificar que la capacidad máxima no sea excedida durante el abordaje.

---

#### 3.1.4 Gestión de incidencias

**RF014 – Registrar incidencias**  
El sistema debe permitir a los conductores registrar incidencias ocurridas durante el recorrido.

**RF015 – Registrar averías de autobús**  
El sistema debe permitir registrar averías y asociarlas al viaje y ruta correspondiente.

---

#### 3.1.5 Gestión de autorizaciones, pagos y acceso

**RF016 – Validar disponibilidad de saldo**  
El sistema debe validar que el saldo del usuario sea mayor al costo del viaje antes de permitir el acceso.

**RF017 – Crear tarjeta institucional**  
El sistema debe permitir que el operador cree tarjetas institucionales asociadas a un usuario.

**RF018 – Validar tarjeta digital**  
El sistema debe validar la tarjeta digital antes del acceso.

**RF019 – Automatizar el proceso de abordaje**  
El sistema debe automatizar el abordaje validando:
- Tarjeta
- Saldo
- Capacidad

**RF020 – Gestionar tipos de autorización**  
El sistema debe permitir gestionar distintos mecanismos de autorización.

**RF021 – Registrar autorizaciones de uso**  
El sistema debe registrar toda autorización otorgada.

**RF022 – Validar autorización antes del acceso**  
El sistema debe validar que el usuario tenga autorización válida.

**RF023 – Controlar vigencia de autorizaciones**  
El sistema debe verificar vigencia.

**RF024 – Controlar condiciones de consumo**  
Validar condiciones como saldo o número de usos disponibles.

**RF025 – Registrar transacciones de uso**  
Registrar todas las transacciones asociadas al uso del servicio.

**RF026 – Bloquear accesos no autorizados**  
Impedir el acceso si no existe autorización válida.

---

#### 3.1.6 Notificaciones, logs y reportes

**RF027 – Enviar notificaciones**  
Enviar notificaciones mediante:
- Correo electrónico
- SMS
- Notificaciones push

**RF028 – Notificar incidencias y averías**  
Notificar a operadores cuando ocurra una avería o incidencia.

**RF029 – Registrar logs del sistema**  
Registrar eventos relevantes: inicio/fin de viajes, accesos, autorizaciones, incidencias y notificaciones.

**RF030 – Consultar y filtrar logs**  
Permitir filtrar por fecha, tipo de evento y usuario.

**RF031 – Generar reportes**  
Permitir generar reportes de uso, ocupación, viajes e incidencias.

---

### 3.1.7 Requerimientos en formato EARS

- **RF001 – Registrar usuarios según su tipo**  
  Cuando un administrador registre un usuario, el sistema deberá permitir registrar usuarios según su tipo (estudiante, empleado o conductor) capturando la información correspondiente y asignando un identificador único.

- **RF002 – Generar matrícula para estudiantes**  
  Cuando se registre un estudiante nuevo, el sistema deberá generar automáticamente una matrícula única y asignar su estado inicial.

- **RF003 – Consultar usuarios**  
  Cuando un usuario autorizado realice una búsqueda, el sistema deberá permitir consultar usuarios mediante ID, cédula o correo electrónico.

- **RF004 – Consultar rutas y horarios**  
  Cuando un usuario acceda a la sección de rutas, el sistema deberá mostrar rutas y horarios disponibles.

- **RF005 – Enviar notificaciones**  
  Cuando ocurra un evento relevante, el sistema deberá enviar notificaciones por correo, SMS o push.

- **RF006 – Validar usuario autorizado**  
  Cuando un usuario intente utilizar el servicio, el sistema deberá validar que esté autorizado y activo.

- **RF007 – Iniciar viaje**  
  Cuando un conductor seleccione un viaje asignado, el sistema deberá permitir iniciarlo y cambiar su estado a “en curso”.

- **RF008 – Finalizar viaje**  
  Cuando un conductor complete el recorrido, el sistema deberá finalizar el viaje y registrar la hora de finalización.

- **RF009 – Asignar y cancelar viajes**  
  Cuando un operador gestione la programación, el sistema deberá permitir asignar o cancelar viajes.

- **RF010 – Monitorear viajes**  
  Cuando un operador consulte estados, el sistema deberá mostrar viajes programados, en curso, retrasados o cancelados.

- **RF011 – Validar disponibilidad de saldo**  
  Cuando un usuario intente abordar, el sistema deberá validar saldo mayor o igual al costo.

- **RF012 – Control de capacidad del autobús**  
  Cuando un usuario intente abordar, el sistema deberá verificar que la capacidad máxima no se haya alcanzado.

- **RF013 – Registrar incidencias**  
  Cuando ocurra una incidencia, el sistema deberá permitir registrarla con tipo y descripción.

- **RF014 – Generar reportes**  
  Cuando un usuario autorizado solicite estadísticas, el sistema deberá generar reportes de uso, ocupación, puntualidad e ingresos.

- **RF015 – Gestión de autobuses**  
  Cuando el personal administrativo gestione autobuses, el sistema deberá permitir registrar, actualizar y consultar información.

- **RF016 – Historial de viajes**  
  Cuando un usuario autorizado consulte su historial, el sistema deberá mostrar viajes realizados previamente.

---

### 3.2 Requerimientos no funcionales

#### 3.2.1 Rendimiento
- **RNF001:** Tiempo máximo de respuesta: **2 segundos** para operaciones normales.
- **RNF002:** Validación de abordaje (tarjeta, autorización y saldo) en **1 segundo**.
- **RNF003:** Procesamiento concurrente sin degradar rendimiento.
- **RNF004:** Notificaciones críticas en **máximo 5 segundos**.
- **RNF005:** Registro de logs sin afectar operaciones principales.
- **RNF006:** Escalabilidad sin cambios significativos de arquitectura.

#### 3.2.2 Disponibilidad
- **RNF007:** Disponible **24/7**.
- **RNF008:** Inactividad máxima: **10 minutos consecutivos**.
- **RNF009:** Actualizaciones sin afectar uso normal.
- **RNF010:** Recuperación: si falla un módulo, el resto opera parcialmente.

#### 3.2.3 Seguridad
- **RNF011:** Control de acceso por roles (estudiantes, empleados, conductores, operadores, administradores).
- **RNF012:** Cifrado y protección de datos.
- **RNF013:** Protección contra accesos no autorizados.
- **RNF014:** Notificación ante acceso denegado.

#### 3.2.4 Usabilidad, confiabilidad y cumplimiento

**Usabilidad**
- **RNF015:** Interfaz intuitiva, sin capacitación técnica previa.
- **RNF016:** Consistencia visual y de navegación.
- **RNF017:** Accesible desde móviles y navegadores web comunes.

**Confiabilidad**
- **RNF018:** Estabilidad durante la operación normal.
- **RNF019:** Recuperación ante fallos inesperados.
- **RNF020:** Integridad de la información.

**Cumplimiento**
- **RNF021:** Cumplimiento de políticas institucionales del ITLA.
- **RNF022:** Cumplimiento de normativas de protección de datos.
- **RNF023:** Soporte de auditoría y control institucional.

---

### 3.3 Requerimientos de interfaces externas

#### 3.3.1 Rendimiento
- Tiempo de respuesta por interfaces externas: **máximo 3 segundos** en condiciones normales.
- Soporte de múltiples solicitudes simultáneas.
- Intercambio de información eficiente y controlado.

#### 3.3.2 Seguridad
- Acceso restringido a usuarios y sistemas autorizados.
- Transmisión segura de información.
- Registro de intentos de acceso y operaciones realizadas.

#### 3.3.3 Controles adicionales de seguridad
- Prevención de accesos no autorizados.
- Registro de intentos de violación para auditoría.
- Cumplimiento de políticas institucionales de seguridad.

#### 3.3.4 Atributos de calidad del software
- **Mantenibilidad:** permitir mejoras sin afectar el funcionamiento general.
- **Escalabilidad:** adaptarse a aumento de usuarios y operaciones.
- **Confiabilidad:** operación estable y consistente.
- **Disponibilidad:** acceso según horarios establecidos.

#### 3.3.5 Reglas de negocio

El sistema debe implementar y aplicar de forma automatizada reglas institucionales del Servicio de Transporte (SGA-ITLA), garantizando cumplimiento uniforme, centralizado y verificable.

**Acceso al servicio**
- Acceso limitado a usuarios autorizados.
- Condiciones dependen del tipo de usuario.
- Depende de vigencia, saldo y cupo.
- Si no se cumplen condiciones, se rechaza el uso.
- Todo intento de uso debe registrarse.

**Pagos y autorizaciones**
- Uso asociado a autorización válida.
- No permitir acceso sin autorización vigente.
- Autorizaciones con vigencia y consumo.
- Transacciones registradas para verificación.

**Operación del servicio**
- Servicio organizado por rutas/horarios.
- Toda operación con planificación válida.
- Requiere asignación de recursos.
- No exceder capacidad física.
- Estados definidos.

**Ejecución y seguimiento**
- Inicio sólo bajo condiciones válidas.
- Inicio y fin registrados.
- Eventos relevantes reportables.
- Estado consistente durante todo el ciclo.

**Control, registro y auditoría**
- Toda acción relevante registrada.
- Registros útiles para supervisión y auditoría.
- Reglas uniformes, sin depender del canal.
- Información consistente y verificable.

---

### 3.4 Funcionalidades del sistema (alto nivel)

- Gestión de usuarios y roles.
- Validación de acceso al servicio de transporte.
- Administración de rutas, horarios y recursos.
- Registro y control de autorizaciones y pagos.
- Monitoreo y seguimiento de la operación del servicio.
- Generación de registros y reportes para auditoría.

---

## 4. Otros requerimientos

### 4.1 Requerimientos de base de datos
- Almacenamiento estructurado y seguro.
- Integridad y consistencia de datos.
- Respaldo y recuperación de información.

### 4.2 Requerimientos legales y regulatorios
- Cumplimiento de normativas vigentes relacionadas con datos.
- Respeto a políticas institucionales de privacidad y seguridad.

### 4.3 Internacionalización y localización
- Adaptación a formatos regionales de fecha/hora.
- Configuración del idioma según necesidades institucionales.

### 4.4 Gestión de riesgos (Matriz FMEA)
- Identificación de riesgos técnicos y operativos.
- Acciones preventivas y correctivas.

---

## 5. Apéndices

### 5.1 Glosario
Listado de términos relevantes utilizados en el documento y su definición.

### 5.2 Casos de uso y diagramas
Incluye diagramas y descripciones para comprender la interacción entre actores y el sistema.

**Actores del sistema**
- Estudiante
- Empleado
- Operador
- Administrador
- Sistema externo (si aplica)

### 5.3 Lista de pendientes (TBD — To Be Determined)
- Integración con sistemas externos específicos.
- Definición detallada de reportes finales.
- Ajustes finales en reglas operativas.