using UnityEngine;
using System.Collections;
using SgLib;

public class PlayerController : MonoBehaviour
{
    public PlayerAnim anim;

    public static event System.Action PlayerDied;

    void OnEnable()
    {
        GameManager.GameStateChanged += OnGameStateChanged;
    }

    void OnDisable()
    {
        GameManager.GameStateChanged -= OnGameStateChanged;
    }

    void Start()
    {
        Character selectedCharacter = CharacterManager.Instance.characters[CharacterManager.Instance.CurrentCharacterIndex];
        GetComponentInChildren<SpriteRenderer>().sprite = selectedCharacter.sprite;
    }



    // Listens to changes in game state
    void OnGameStateChanged(GameState newState, GameState oldState)
    {
        if (newState == GameState.Playing)
        {
            GetComponentInChildren<SpriteRenderer>().enabled = true;
            GetComponentInChildren<Rigidbody2D>().gravityScale = GameManager.Instance.playerGravityScale;
        }
    }

    public Vector3 recordPos;
    public GameObject AdPanel;
    public bool isCancelAd;
    public bool isRevive;

    public void SetAdPanel(bool a)
    {
        AdPanel.SetActive(a);
        Time.timeScale = a ? 0 : 1;
    }

    public void CancelAd()
    {
        isCancelAd = true;
        SetAdPanel(false);
    }

    public void PreDie()
    {
        SetAdPanel(true);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        GetComponent<Rigidbody2D>().simulated = false;
        StartCoroutine("Die");
    }

    public void Revive()
    {
        AdManager.ShowVideoAd("192if3b93qo6991ed0",
    (bol) => {
        if (bol)
        {
            GameManager.Instance.rotationDirection = 0;
            transform.position = recordPos;
            CameraController.Instance.FollowImmediately();
            SetAdPanel(false);
            StopCoroutine("Die");
            GetComponent<Rigidbody2D>().simulated = true;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Spawner.Instance.BunkSpawn(1);

            AdManager.clickid = "";
            AdManager.getClickid();
            AdManager.apiSend("game_addiction", AdManager.clickid);
            AdManager.apiSend("lt_roi", AdManager.clickid);


        }
        else
        {
            StarkSDKSpace.AndroidUIManager.ShowToast("观看完整视频才能获取奖励哦！");
        }
    },
    (it, str) => {
        Debug.LogError("Error->" + str);
        //AndroidUIManager.ShowToast("广告加载异常，请重新看广告！");
    });

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Cloud")
        {
            Vector3 pos= new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y + 1f, 0);
            recordPos = pos;
        }
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Cloud")
    //    {
    //        recordPos = collision.gameObject.transform.position;
    //    }
    //}


    // Calls this when the player dies and game over
    public IEnumerator Die()
    {
        yield return new WaitForSeconds(0.2f);
        // Fire event
        PlayerDied();
        GetComponent<Rigidbody2D>().simulated = true;
    }

    public void DieImmediately()
    {
        PlayerDied();
    }
}
