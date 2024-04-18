using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class Terminal : MonoBehaviour
{
    public static Terminal Instance { get; private set; }

    private TypingDisplay typingDisplay;

    public GameObject Interaction_UI;
    public TextMeshProUGUI Interaction_Text;

    public bool IsUsingTerminal = false;

    public Text Terminal_text1;
    public Text Terminal_text2;
    public Text Money_text;
    public Text Notify_text;

    public Player Player;

    [Header("아이템 오브젝트")]
    public GameObject Axe_Item;
    public GameObject Flashlight_Item;

    [Header("아이템 스폰 위치")]
    public Transform ItemSpawnPoint;


    private void Awake()
    {
        Instance = this;

        typingDisplay = GetComponentInChildren<TypingDisplay>();


    }

    // Start is called before the first frame update
    private void Start()
    {
        Money_text.text = $"소지금:${Player.Stat.MoneyCount}";
        Terminal_text1.enabled = false;
        Terminal_text2.enabled = false;
        Notify_text.enabled = false;

        IsUsingTerminal = false;
    }

    // Update is called once per frame
    private void Update()
    {
        //UseKeyboard();
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 playerToTerminal = transform.position - other.transform.position; // 터미널을 향한 벡터
        playerToTerminal.y = 0; // 높이 차이는 무시
        float angle = Vector3.Angle(other.transform.forward, playerToTerminal); // 각도 계산


        if (other.CompareTag("Player") )
        {
            if(angle < 30.0f)
            {
                Interaction_UI.SetActive(true);
                Interaction_Text.text = "터미널사용: [E]";
            }
            else
            {
                Interaction_UI.SetActive(true);
            }

        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 playerToTerminal = transform.position - other.transform.position; // 터미널을 향한 벡터
            playerToTerminal.y = 0; // 높이 차이는 무시
            float angle = Vector3.Angle(other.transform.forward, playerToTerminal); // 각도 계산

            if (Input.GetKeyDown(KeyCode.E) && angle < 40.0f)
            {
                Interaction_UI.SetActive(false);

                Terminal_text1.enabled = true;

                StartCoroutine(IsUsingTerminal_Coroutine());
                

            }
            if (Input.GetKeyDown(KeyCode.Escape) && IsUsingTerminal)
            {
                Terminal_text1.enabled = false;
                Terminal_text2.enabled = false;
                Notify_text.enabled = false;

                IsUsingTerminal = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Interaction_UI.SetActive(false);

        }


    }

    private void UseKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.E) && !IsUsingTerminal)
        {
            //Interaction_UI.SetActive(false);

            Terminal_text1.enabled = true;
            
            StartCoroutine(IsUsingTerminal_Coroutine());

        }

    }

    private IEnumerator IsUsingTerminal_Coroutine()
    {
        yield return new WaitForSeconds(0.2f);
        IsUsingTerminal = true;
    }

    public void HandleStoreCommand()
    {
        Terminal_text1.enabled = false;
        Terminal_text2.enabled = true;
    }

    public void HandleAxeCommand()
    {
        Player.Stat.MoneyCount -= 30;
        Instantiate(Axe_Item, ItemSpawnPoint.position, ItemSpawnPoint.rotation);

    }

    public void HandleFlashlightCommand()
    {
        Player.Stat.MoneyCount -= 25;
        Instantiate(Flashlight_Item, ItemSpawnPoint.position, ItemSpawnPoint.rotation);
    }

    public IEnumerator Notification_Coroutine_text1()
    {
        Notify_text.enabled = true;
        Terminal_text1.enabled = false;
        Terminal_text2.enabled = false;
        Notify_text.text = "※ 일치하는 명령어가 없습니다. \n명령어를 다시 입력해주세요.";

        yield return new WaitForSeconds(0.5f);

        Notify_text.enabled = false;
        Terminal_text1.enabled = true;

    }

    public IEnumerator Notification_Coroutine_text2()
    {
        Notify_text.enabled = true;
        Terminal_text1.enabled = false;
        Terminal_text2.enabled = false;
        Notify_text.text = "※ 일치하는 명령어가 없습니다. \n명령어를 다시 입력해주세요.";

        yield return new WaitForSeconds(1f);

        Notify_text.enabled = false;
        Terminal_text2.enabled = true;
    }


    public IEnumerator BoughtItem_Coroutine()
    {
        Notify_text.enabled = true;
        Terminal_text1.enabled = false;
        Terminal_text2.enabled = false;
        Notify_text.text = "아이템 구입 완료!";

        Money_text.text = $"소지금:${Player.Stat.MoneyCount}";


        yield return new WaitForSeconds(1f);

        Notify_text.enabled = false;
        IsUsingTerminal = false;

    }

    public IEnumerator CantBuy_Coroutine()
    {
        Notify_text.enabled = true;
        Terminal_text1.enabled = false;
        Terminal_text2.enabled = false;
        Notify_text.text = "구매 실패:잔액 부족";

        yield return new WaitForSeconds(1f);

        Notify_text.enabled = false;
        IsUsingTerminal = false;

    }



}
