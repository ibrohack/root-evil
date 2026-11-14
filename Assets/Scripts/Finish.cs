using UnityEngine;

public class Finish : MonoBehaviour
{
	Animator anim;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}
	void Start()
	{
		anim.SetTrigger("Change");
	}
	public void CambiarEscena()
	{
		GameManager.Instance.CambioDeEscena();
	}

	public void BloquearPlayer()
	{
		GameManager.Instance.BloquearPlayer();
	}
}
