using UnityEngine;

// Datos que pasan de un nivel al siguiente.
// Al ser static no pertenecen a ningun objeto de la escena, asi que sobreviven al cambio de escena.
public static class DatosPartida
{
	public static int vidas;      // 0 = empezar con el maximo de vidas del Player
	public static float tiempo;   // segundos acumulados al entrar en el nivel

	public static void Nueva()
	{
		vidas = 0;
		tiempo = 0f;
	}

	// El proyecto entra en Play sin recargar el dominio (Enter Play Mode Options),
	// por eso las variables static no se reinician solas entre una prueba y la siguiente.
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void ReiniciarAlEntrarEnPlay()
	{
		Nueva();
	}
}
