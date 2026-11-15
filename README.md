<p align="center">
  <img src="Assets/Sprites/IconoAplicacion.png" alt="Icono de Root of Evil" width="160">
</p>

# Root of Evil

Juego de plataformas y disparos en 2D con scroll lateral para Android, hecho en Unity 6 como proyecto en equipo del módulo PMD (2º DAM). Corres, haces doble salto y disparas a lo largo de tres niveles, y en el último te enfrentas a un jefe. Las vidas y el tiempo pasan de un nivel al siguiente.

## Jugabilidad

- **Tres niveles** que se juegan en orden: Nivel 1 → Nivel 2 → Nivel 3. También puedes empezar cualquier nivel desde el menú de selección de niveles.
- **Las vidas y el tiempo se conservan.** Empiezas con 3 vidas. Las vidas que te quedan al llegar a la bandera y el cronómetro pasan contigo al siguiente nivel.
- **Los enemigos** patrullan entre puntos de ruta. Tocar uno te quita una vida, y una bala basta para matarlo.
- **Objetos.** Ahora mismo los niveles solo tienen objetos de vida extra. La invencibilidad y el disparo rápido están programados, pero no hay ninguno colocado en los niveles.
  - Vida extra: +1 vida, sin límite
  - Invencibilidad: los enemigos no te hacen daño durante 5 segundos
  - Disparo rápido: disparas unas 5 veces más rápido durante 5 segundos
- **Las zonas ocultas** aparecen cuando entras en ellas.
- **Las plataformas móviles** te llevan mientras estás encima.
- **Combate contra el jefe** en el Nivel 3. Al entrar en la arena se cierra una pared detrás de ti y la cámara se queda fija en la arena. El jefe patrulla, y cuando te acercas se gira y lanza una llamarada que te quita una vida (la invencibilidad te protege). Tocarlo no hace daño. Necesita 3 disparos, y cuando muere la cámara vuelve a seguirte hasta la bandera.
- **Muerte:** mueres al perder tu última vida, al tocar una zona mortal (KillZone) o al caerte del nivel. Tras el Game Over vuelves a intentar el nivel con todas las vidas.
- **Menú de pausa** para continuar, reiniciar el nivel o volver al menú principal. El volumen de la música y de los efectos se ajusta en Opciones y se guarda entre partidas.

## Controles

| Acción | Teclado y ratón | Mando | Táctil (Android) |
| --- | --- | --- | --- |
| Moverse | `A` `D` / `←` `→` | Stick izquierdo | Joystick en pantalla |
| Saltar (pulsa otra vez en el aire para el doble salto) | `Espacio` | Botón inferior (A / ✕) | Botón de saltar |
| Disparar | Clic izquierdo / `Intro` | Botón izquierdo (X / □) | Botón de disparar |
| Pausa | Botón de pausa del HUD | — | Botón de pausa del HUD |

Los controles están definidos en [Assets/Settings/InputSystem_Actions.inputactions](Assets/Settings/InputSystem_Actions.inputactions) (paquete Input System).

## Primeros pasos

### Requisitos

- **Unity 6000.6.0f1**, instalado desde Unity Hub. Otras versiones pueden actualizar o romper el proyecto. Compruébalo en [ProjectSettings/ProjectVersion.txt](ProjectSettings/ProjectVersion.txt).
- Módulo **Android Build Support** (con OpenJDK y Android SDK & NDK), solo necesario para compilar para el móvil. La versión mínima es Android 10 (API 29).

### Abrir y jugar en el Editor

1. Clona el repositorio:

   ```sh
   git clone https://github.com/ibrohack/root-evil.git
   ```

2. En Unity Hub, elige **Add → Add project from disk** y selecciona la carpeta `root-evil`. La primera importación tarda unos minutos mientras Unity regenera `Library/`.
3. Abre [Assets/Scenes/MainMenu.unity](Assets/Scenes/MainMenu.unity) y pulsa **Play**.

También puedes abrir la escena de un nivel y pulsar Play para probarlo por separado.

### Compilar para Android

1. **File → Build Profiles → Android → Switch Platform.**
2. Comprueba que la lista de escenas está en este orden: `MainMenu`, `Level1`, `Level2`, `Level3`. El juego carga el siguiente nivel por su índice en la lista, así que el orden importa.
3. Conecta un móvil con la depuración USB activada y pulsa **Build And Run**, o pulsa **Build** para obtener un `.apk`.

El juego se ejecuta en horizontal. El nombre del paquete es `com.Mist.Rootevil`.

## Estructura del proyecto

```text
Assets/
├── Scenes/          MainMenu, Level1, Level2, Level3
├── Scripts/         Todo el código del juego (ver más abajo)
├── Prefabs/         Player, HUD, GameManager, AudioManager, enemigos, objetos, plataformas...
├── Animation/       Animator controllers y clips (Player, Enemy, PickUp, Flag, UI)
├── Audio/           Musica/ (música del menú y de los niveles), SFX/ (efectos de sonido)
├── Sprites/         Personajes, enemigos, objetos, fondos, tilesets
├── Tiles/           Paletas de tiles y tiles
├── Resources/       Fuente Upheaval TT y su asset de TextMesh Pro
├── Settings/        Ajustes de URP, acciones de entrada y build profiles
└── VirtualJoystick/ Paquete externo de joystick en pantalla
```

## Cómo encaja el código

| Script | Función |
| --- | --- |
| [PlayerMovement.cs](Assets/Scripts/PlayerMovement.cs) | Movimiento, doble salto, disparo, daño (`RecibirDanio`, que también usa el jefe) y muerte |
| [GameManager.cs](Assets/Scripts/GameManager.cs) | Flujo del nivel: meta, siguiente nivel, pausa, Game Over, victoria |
| [UIManager.cs](Assets/Scripts/UIManager.cs) | HUD (vidas, cronómetro), paneles de pausa/Game Over/victoria, botones táctiles |
| [AudioManager.cs](Assets/Scripts/AudioManager.cs) | Música y efectos de sonido, volumen guardado |
| [DatosPartida.cs](Assets/Scripts/DatosPartida.cs) | Datos estáticos que pasan de un nivel a otro (vidas, tiempo) |
| [MenuPrincipal.cs](Assets/Scripts/MenuPrincipal.cs) | Menú principal: jugar, selección de niveles, opciones, salir |
| [AjustesVolumen.cs](Assets/Scripts/AjustesVolumen.cs) | Sliders de volumen del panel de Opciones |
| [EnemyMovement.cs](Assets/Scripts/EnemyMovement.cs) / [EnemyBoss.cs](Assets/Scripts/EnemyBoss.cs) | Enemigos que patrullan, y jefe del Nivel 3 (patrulla, ataque con llamarada y muerte) |
| [PickUp.cs](Assets/Scripts/PickUp.cs) | Objetos de vida, invencibilidad y disparo rápido |
| [BossZone.cs](Assets/Scripts/BossZone.cs), [ZonaOculta.cs](Assets/Scripts/ZonaOculta.cs), [PlatformMovement.cs](Assets/Scripts/PlatformMovement.cs), [Finish.cs](Assets/Scripts/Finish.cs) | Elementos del nivel: pared de la arena del jefe, zonas ocultas, plataformas móviles, transición entre niveles |
| [CameraFollow.cs](Assets/Scripts/CameraFollow.cs), [InfiniteBackground.cs](Assets/Scripts/InfiniteBackground.cs), [Bala.cs](Assets/Scripts/Bala.cs) | Cámara (se queda fija en la arena del jefe hasta que muere), fondo con scroll, balas |

Algunas cosas que conviene saber antes de tocar el código:

- **Cada nivel tiene su propio `GameManager`, `UIManager` y `Player`.** Asignan su `Instance` estática en `Awake` y se destruyen al cambiar de escena. Cada escena de nivel necesita los prefabs `GameManager`, `HUD`, `AudioManager` y `Player`.
- **`AudioManager` sobrevive al cambio de escena** (`DontDestroyOnLoad`). Si una escena tiene una segunda copia, esa copia se destruye sola, así que todas las escenas pueden incluir el prefab.
- **`DatosPartida` guarda los datos que pasan de un nivel a otro.** Solo se actualiza cuando el jugador llega a la bandera, así que al reiniciar un nivel recuperas las vidas y el tiempo que tenías al entrar.
- **Flujo del nivel:** el jugador toca la bandera (tag `Meta`) → `GameManager.ActualizarFinish()` guarda las vidas y el tiempo → se reproduce la animación de transición → su evento de animación llama a `Finish.CambiarEscena()` → se carga la siguiente escena de la lista. Después del último nivel aparece el panel de victoria.
- **Está activado entrar en Play Mode sin recargar el dominio.** Los campos estáticos *no* se reinician al volver a pulsar Play. Reinícialos con `[RuntimeInitializeOnLoadMethod]`, como hace `DatosPartida`.
- **Las colisiones se gestionan por tag:** `Suelo`, `Enemy`, `Bala`, `KillZone`, `Meta`, `ZonaOculta`, `TriggerCamara`, `TriggerBoss`.

### Añadir un nivel nuevo

1. Duplica una escena de nivel de `Assets/Scenes/` y construye el escenario con las paletas de tiles de `Assets/Tiles/`.
2. Mantén los prefabs `GameManager`, `HUD`, `AudioManager` y `Player`, y pon un prefab `Bandera` al final.
3. Añade la escena en **Build Profiles** después del último nivel actual.
4. En `MainMenu`, añade un botón al panel de selección de niveles. Su OnClick llama a `MenuPrincipal.CargarNivel` con el nombre de la escena.

## Cómo colaborar

- Trabaja en una rama propia (por ejemplo `nivel3` o `enemigos`) y fusiónala con `main` cuando esté lista.
- Los mensajes de commit siguen el formato `type(scope): Added ...`, por ejemplo `feat(UI): Added Level3 to the level select menu`.
- Sube siempre cada asset junto con su archivo `.meta`. Unity usa el GUID del `.meta` para mantener las referencias.
- Las escenas y los prefabs son difíciles de fusionar. Poneos de acuerdo antes de que dos personas editen la misma escena a la vez.

## Equipo

- Oihan Gonzalez ([OihanGonzalez](https://github.com/OihanGonzalez))
- Adam Rhouat ([AdamFixed](https://github.com/AdamFixed))
- Brayan Romero ([ibrohack](https://github.com/ibrohack))

## Recursos de terceros

- **Virtual Joystick Pack** para el joystick en pantalla ([Assets/VirtualJoystick/](Assets/VirtualJoystick/), incluye guía de usuario)
- Fuente **Upheaval TT** de Ænigma
- Sprites de objetos de *Metal Slug* (Neo Geo), © SNK
