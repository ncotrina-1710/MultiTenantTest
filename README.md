# MultiTenantTest
# Pasos para ejecutar proyecto
1. Ejecutar Migracion a BD Admin. (Revisar la cadena de conexión en appsettings.json -> ConnectionStrings -> Admin)
    1.1. update-database -context ApiDbContext
2. Al levantar el proyecto lo recomendable es ejecutar el endpoint: Post del controller de Organizacion
    2.1. Los parametros requeridos y necesarios son "name" y "slugTenant", al registrar la organizacion automaticamente se crea una BD para esa organizacion
	    con la tabla productos, la BD se generará dinamicamente segun vayan creando organizaciones. Se registrará con el sgte formato, indicado en
	    "appsettings.json -> ConnectionStrings -> Productos" donde "{0}" será remplazado por el valor ingresado en "slugTenant"
	    Server=.;Database=ApiMultiTenant_{0};Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True
	**Se recomienda que el valor de "slugTenant" no contenga espacios ni carácteres especiales.**
3. Ejecutar Endpoint HTTPOST de User, el cual permite crear usuarios con el "email","password" y "organizationId" esto con la finalidad de que al crear un usuario
   se relacione con una organizacion.
4. Ejecutar Endpoint de User/Login, permite iniciar sesion con las credenciales del usuario "email" y "password" con la finalidad de obtener el token y los tenants u
   organizaciones relacionadas al usuario
5. Con el token podemos acceder a los endpoints de Productos, previo a eso debemos agregar el token en la opcion de **Authorize** que brinda Swagger.
   Dichos endpoints necesitan del slugtenant para poder saber a que BD conectar y obtener informacion 
	
