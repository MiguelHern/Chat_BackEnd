*NECESARIO* Crear las migraciones

**.NetCore CLI**

Comando para installar .NetCore CLI

```bash
dotnet tool install --global dotnet-ef
```

Para crear una nueva migración, el comando sugerido es el siguiente:

```bash
dotnet ef migrations add "MigracionInicial" --project Core --startup-project Presentation --output-dir Infraestructure\Persistance\Migrations
```

**Visual Studio**

---

```bash
Add-Migration "MigracionInicial" --project Core --startup-project Presentation --output-dir Infraestructure\Persistance\Migrations
```

Donde:

- `--project Core`: el proyecto que contiene el `dbcontext`.

- `--startup-project Presentation`: proyecto inicial de ejecución.

- `--output-dir Infraestructure\Persistance\Migrations`:  el directorio de destino que almacenará las migraciones.

### Aplicar las migraciones a la base de datos

En caso de que se quiera probar una versión en local, aplicar un nuevo parche o se migre la base de datos a otro servidor, el comando sugerido sería el siguiente:

**.NetCore CLI**
```bash
dotnet ef database update --project Core --startup-project Presentation
```

---

**Visual Studio**

```bash
Update-Database -Project Core -StartupProject Presentation
```

Donde:

- `--project Core`: se conoce como proyecto de destino porque es donde los comandos añaden o eliminan archivos. Por defecto, el proyecto en el directorio actual es el proyecto de destino. Puede especificar un proyecto diferente como proyecto de destino utilizando la opción `--project`.
