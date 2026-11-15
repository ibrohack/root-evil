using UnityEngine;
using UnityEngine.SceneManagement;

public enum Sonido
{
	Salto,
	DobleSalto,
	Disparo,
	Golpe,
	Muerte,
	EnemigoMuerto,
	PickUp,
	ZonaOculta,
	Meta,
	GameOver,
	Victoria,
	ClickUI
}

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance;

	private const string ClaveMusica = "VolumenMusica";
	private const string ClaveSFX = "VolumenSFX";

	[Header("Fuentes")]
	[SerializeField] private AudioSource fuenteMusica;
	[SerializeField] private AudioSource fuenteSFX;

	[Header("Musica")]
	[SerializeField] private AudioClip musicaMenu;
	[SerializeField] private AudioClip musicaNivel;
	[SerializeField] private string escenaMenu = "MainMenu";

	[Header("Efectos")]
	[SerializeField] private AudioClip salto;
	[SerializeField] private AudioClip dobleSalto;
	[SerializeField] private AudioClip disparo;
	[SerializeField] private AudioClip golpe;
	[SerializeField] private AudioClip muerte;
	[SerializeField] private AudioClip enemigoMuerto;
	[SerializeField] private AudioClip pickUp;
	[SerializeField] private AudioClip zonaOculta;
	[SerializeField] private AudioClip meta;
	[SerializeField] private AudioClip gameOver;
	[SerializeField] private AudioClip victoria;
	[SerializeField] private AudioClip clickUI;

	void Awake()
	{
		// Solo puede haber un AudioManager: el primero sobrevive entre escenas
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);

		if (fuenteMusica != null)
		{
			fuenteMusica.loop = true;
			fuenteMusica.playOnAwake = false;
			fuenteMusica.volume = VolumenMusica;
		}
		if (fuenteSFX != null)
		{
			fuenteSFX.playOnAwake = false;
			fuenteSFX.volume = VolumenSFX;
		}
	}

	void OnEnable()  { SceneManager.sceneLoaded += AlCargarEscena; }
	void OnDisable() { SceneManager.sceneLoaded -= AlCargarEscena; }

	void Start()
	{
		if (Instance != this) return;
		PonerMusicaDeEscena(SceneManager.GetActiveScene().name);
	}

	void AlCargarEscena(Scene escena, LoadSceneMode modo)
	{
		if (Instance != this) return;
		PonerMusicaDeEscena(escena.name);
	}

	void PonerMusicaDeEscena(string nombreEscena)
	{
		PonerMusica(nombreEscena == escenaMenu ? musicaMenu : musicaNivel);
	}

	void PonerMusica(AudioClip clip)
	{
		if (clip == null || fuenteMusica == null) return;

		// Si ya suena esa musica no se reinicia (por ejemplo al pasar de un nivel a otro)
		if (fuenteMusica.clip == clip && fuenteMusica.isPlaying) return;

		fuenteMusica.clip = clip;
		fuenteMusica.Play();
	}

	// Se puede llamar desde cualquier script. Si no hay AudioManager en la escena no hace nada.
	public static void Reproducir(Sonido sonido)
	{
		if (Instance == null || Instance.fuenteSFX == null) return;

		AudioClip clip = Instance.ClipDe(sonido);
		if (clip != null) Instance.fuenteSFX.PlayOneShot(clip);
	}

	public static void PausarMusica(bool pausar)
	{
		if (Instance == null || Instance.fuenteMusica == null) return;

		if (pausar) Instance.fuenteMusica.Pause();
		else Instance.fuenteMusica.UnPause();
	}

	public static void PararMusica()
	{
		if (Instance == null || Instance.fuenteMusica == null) return;
		Instance.fuenteMusica.Stop();
	}

	// Volumen de 0 a 1, guardado en PlayerPrefs para la siguiente partida
	public float VolumenMusica
	{
		get { return PlayerPrefs.GetFloat(ClaveMusica, 0.6f); }
		set
		{
			float volumen = Mathf.Clamp01(value);
			PlayerPrefs.SetFloat(ClaveMusica, volumen);
			if (fuenteMusica != null) fuenteMusica.volume = volumen;
		}
	}

	public float VolumenSFX
	{
		get { return PlayerPrefs.GetFloat(ClaveSFX, 1f); }
		set
		{
			float volumen = Mathf.Clamp01(value);
			PlayerPrefs.SetFloat(ClaveSFX, volumen);
			if (fuenteSFX != null) fuenteSFX.volume = volumen;
		}
	}

	public void GuardarVolumen()
	{
		PlayerPrefs.Save();
	}

	AudioClip ClipDe(Sonido sonido)
	{
		switch (sonido)
		{
			case Sonido.Salto: return salto;
			case Sonido.DobleSalto: return dobleSalto;
			case Sonido.Disparo: return disparo;
			case Sonido.Golpe: return golpe;
			case Sonido.Muerte: return muerte;
			case Sonido.EnemigoMuerto: return enemigoMuerto;
			case Sonido.PickUp: return pickUp;
			case Sonido.ZonaOculta: return zonaOculta;
			case Sonido.Meta: return meta;
			case Sonido.GameOver: return gameOver;
			case Sonido.Victoria: return victoria;
			case Sonido.ClickUI: return clickUI;
			default: return null;
		}
	}
}
