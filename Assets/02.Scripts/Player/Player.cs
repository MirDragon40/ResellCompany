using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamaged
{
    public Stat Stat;

    public GameObject UI_AlmostDying;



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
        if(Stat.Health <= 0)
        {
            Death();
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


}
