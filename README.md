# UTN Conecta

Aplicación móvil que conecta a graduados de la UTN con oportunidades de trabajo y pasantías publicadas por organizaciones.

- **Autor:** Cabrera Fernando Sebastián
- **Institución:** UTN FRT, Tecnicatura Universitaria en Programación
- **Estado:** funcional de punta a punta para graduados y organizaciones (actualizado el 08/11/2026)

## Tecnologías

| Parte | Tecnología |
| --- | --- |
| API | ASP.NET Core Web API (.NET 10), Entity Framework Core |
| Base de datos | PostgreSQL |
| Autenticación | JWT con roles por tipo de usuario (graduado u organización) |
| App móvil | React Native con Expo, React Navigation, expo-secure-store |
| Pruebas | Postman |

## Estructura del repositorio

```
UTNConecta.API/     API: controladores, servicios, DTOs, modelos y migraciones
utn-conecta-app/    App móvil (Expo)
```

## Qué puede hacer cada usuario

**Graduado**
- Ingresar con email y contraseña.
- Ver el listado de oportunidades y filtrarlo.
- Ver el detalle de una oferta y postularse.
- Ver sus postulaciones en «Mis postulaciones».
- Ver y editar su perfil: nombre, apellido, año de egreso, carrera y enlace a su CV.

**Organización**
- Ingresar con email y contraseña.
- Publicar oportunidades (trabajo o pasantía) con todos los campos obligatorios.
- Ver sus oportunidades en «Mis oportunidades».
- Ver los postulantes de cada oferta: nombre, email, fecha y enlace al CV.

El registro de cuentas (graduados y organizaciones) está disponible en la API; la app no tiene pantalla de registro.

## Seguridad y validaciones

- La identidad del usuario sale siempre del token, nunca de la URL ni del cuerpo de la petición.
- Respuestas diferenciadas: 401 (sin sesión), 403 (sin permiso) y 404 (no existe).
- Solo la organización dueña de una oportunidad puede ver sus postulantes.
- El email es único por tipo de usuario (índice único en la base de datos y control en el servicio).
- Validación de campos obligatorios, año de egreso (entre 1950 y el año actual), carrera existente, tipo de oportunidad (`trabajo` o `pasantía`), contacto (email o teléfono) y enlace al CV.

## Cómo ejecutarlo

Requisitos: .NET 10 SDK, PostgreSQL, Node.js y la app Expo Go en el celular.

**1. API**

1. Crear en `UTNConecta.API/` un archivo `appsettings.json` con la cadena de conexión a PostgreSQL y la configuración JWT (el archivo real no se sube al repositorio porque contiene claves).
2. Aplicar las migraciones: `dotnet ef database update`
3. Iniciar la API: `dotnet run --launch-profile https` (escucha en `http://0.0.0.0:5226`)

**2. App móvil**

1. Entrar a `utn-conecta-app/` e instalar dependencias: `npm install`
2. En `src/config.js`, poner la IP de la computadora en la red Wi-Fi: `API_URL = "http://<IP-de-la-PC>:5226/api"`
3. Iniciar: `npx expo start -c` y escanear el QR con Expo Go. El celular y la PC deben estar en la misma red.

## Pruebas

Las pruebas funcionales y de seguridad se hicieron con Postman (planilla de 47 casos): validaciones de campos, respuestas 401/403/404, emails duplicados, carrera inexistente y acceso a postulantes. Para probar endpoints protegidos hay que iniciar sesión (login sin autenticación), copiar el `token` de la respuesta y usarlo en *Authorization → Bearer Token*.

## Alcance y trabajo futuro

Postergado a propósito para cumplir el plazo:

- Rol administrador: aprobación de organizaciones y validación de graduados por legajo.
- Subida del CV como archivo PDF (hoy es un enlace).
- Recomendaciones y búsqueda con inteligencia artificial.
- Panel web para organizaciones.
- Despliegue en la nube (la demostración corre en la computadora local).
- Editar o cerrar oportunidades y cambiar el estado de una postulación desde la app.
- Perfil con habilidades y experiencia.

## Documentación de gestión

El proyecto se gestionó con los trabajos prácticos de Gestión de Desarrollo de Software: propuesta, alcance e interesados, estimación, planificación y riesgos, y seguimiento y cierre (TP N.º 1 a 5).
