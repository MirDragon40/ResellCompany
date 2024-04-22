using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerMoveAbility))]
[RequireComponent(typeof(PlayerRotateAbility))]
[RequireComponent(typeof(PlayerAttackAbility))]
public class Player : MonoBehaviour, IDamaged
{
    public Stat Stat;

    public GameObject UI_AlmostDying;
    public GameObject UI_Damaged;

    public GameObject UI_GameOverPopup;

    public GameObject UI_CanvasHUD;

    public LobbyScene LobbyScene;

    private void Awake()
    {
        Stat.Init();
    }
    private void Start()
    {
        UI_AlmostDying.SetActive(false);
    }

    public void Damaged(int damage)
    {
        Stat.Health -= damage;

        if (Stat.Health <= 0 && Stat.Health > -5)
        {
            Death();
            StartCoroutine(Death_Coroutine());
        }
        else if (Stat.Health > 0)
        {
            StartCoroutine(Damaged_Coroutine());
        }
        else if (Stat.Health == 20)
        {
            StartCoroutine(DyingMessage_Coroutine());
        }
        
    }

    private void Death()
    {
        GetComponent<Animator>().SetTrigger("Death");
        GetComponent<PlayerAttackAbility>().InactiveCollider();

        Stat.MoveSpeed = 0;
        Stat.RunSpeed = 0;
        Stat.RotationSpeed = 0;

        UI_CanvasHUD.SetActive(false);

    }

    private IEnumerator DyingMessage_Coroutine()
    {
        UI_AlmostDying.SetActive(true);

        yield return new WaitForSeconds(0.4f);

        UI_AlmostDying.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        UI_AlmostDying.SetActive(true);

        yield return new WaitForSeconds(0.4f);

        UI_AlmostDying.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        UI_AlmostDying.SetActive(true);

        yield return new WaitForSeconds(0.8f);

        UI_AlmostDying.SetActive(false);
    }

    private IEnumerator Damaged_Coroutine()
    {
        UI_Damaged.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        UI_Damaged.SetActive(false);
    }
    private IEnumerator Death_Coroutine()
    {
        yield return new WaitForSeconds(2f);
        UI_GameOverPopup.SetActive(true);

        yield return new WaitForSeconds(4f);
        LobbyScene.RestartGame();
    }
}
