using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject MouseController;
    public GameObject PlayerShip;
    public GameObject VariablesRoom; //VariablesRoom游戏对象，存储需要在场景切换时传递的数据
    public GameObject EndCamera;
    public GameObject MiniMapCamera;
    public Image Mouse;
    public Text GameOverText;

    private MouseController _MouseController; //MouseController类
    private PlayerController _PlayerController;
    private VariablesRoom _VariablesRoom; //VariablesRoom类

    private bool IfGameOver = false;
    void Start()
    {
        EndCamera.SetActive(false);
        _VariablesRoom = VariablesRoom.GetComponent<VariablesRoom>();
        _MouseController = MouseController.GetComponent<MouseController>();
        _PlayerController = PlayerShip.GetComponent<PlayerController>();
    }
    void FixedUpdate()
    {
        if (_MouseController.IfReady)
        {
            if(PlayerShip != null)
            {
                MiniMapCamera.transform.position = new Vector3(PlayerShip.transform.position.x, PlayerShip.transform.position.y + 5, PlayerShip.transform.position.z);
            }
            if(IfGameOver)
            {
                if (_MouseController.ModeSwitch)
                {
                    IfGameOver = false;
                    GameOverText.text = "";
                    SceneManager.LoadScene("Loading");
                }
            }
            else
            {
                Mouse.rectTransform.localPosition = _MouseController.GetMousePosition("RightHand");
                //按钮点击事件
                if (_MouseController.GetHandState("RightHand") == 1)
                {
                    if (_MouseController.RightY > 465 && _MouseController.RightY < 530)
                    {
                        if (_MouseController.RightX > -950 && _MouseController.RightX < -770)
                        {
                            //返回主菜单按钮点击事件
                            Debug.Log("Menu");
                            SceneManager.LoadScene("Menu");
                        }
                    }
                    else
                    {
                        _PlayerController.Launch();
                    }
                }
                
                if (_PlayerController.GetCurrentPosition() != "Space")
                {
                    if (_MouseController.ModeSwitch)
                    {
                        _PlayerController.SetMode(1);
                    }
                    else
                    {
                        _PlayerController.SetMode(2);
                    }
                }
            }
        }   
    }
    public void GameOver()
    {
        IfGameOver = true;
        _MouseController.ModeSwitch = false;
        GameOverText.text = "Game Over! 左手握拳重新开始游戏！";
    }
}
