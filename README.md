Este proyecto tiene 2 fases: Autenticación y Autorización.

Se crea un Token JWT que almacenará información del usuario, cifrada y confiable. Se puede usar para autenticar un usuario en el sistema, intercambiar información y realizar la autorización a los resursos que defina.
El sistema de autenticación utiliza unas credenciales enviadas por el cliente al servidor, que incluyen información como usuario y contraseña. Son validadas y el sistema crea una serie de atributos que son asociados al identificador de usuario y se cifran con un tiempo de caducidad, generando de esta forma un token JWT.
Este Token JWT es enviado al cliente, donde será almacenado.  Una vez el cliente dispone de un token válido, está listo para solicitar recursos (fase de autorización).
El cliente encapsula el token y lo envía a la API.
Un middleware de autorización valida el token antes del acceso al controlador, para identificar al usuario y comprobar su caducidad, permitiendo que consuma el controlador que el usuario solicita y, por tanto, el recurso, obteniendo como respuesta el recurso deseado.
En caso de que el token no fuera válido, la respuesta sería 401 (usuario no autorizado).

En la parte de Autorización muestro un método de acceso anónimo, el cual no necesita de autorización para ejecutarse pese a estar dentro de una clase con [Authorize].

Y por último, también hay un método de autorización basado en Roles (user, admin...etc).
