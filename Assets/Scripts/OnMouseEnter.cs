using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.IO;

public class OnMouseEnter : MonoBehaviour
{
    private GameObject MouseController;
    private MouseController _MouseController;

    private bool IfCanContinue;

    private void Start()
    {
        MouseController = GameObject.Find("MouseController");
        _MouseController = MouseController.GetComponent<MouseController>();
        //为按钮增加音效
        gameObject.AddComponent<AudioSource>();
        gameObject.GetComponent<AudioSource>().clip = (AudioClip)Resources.Load("Sounds/weapon_enemy");
        //检查是否存在存档文件
        string path = Application.streamingAssetsPath + "/Record.xml";
        if (!File.Exists(path))
        {
            IfCanContinue = false;
        }
        else
        {
            IfCanContinue = true;
        }
        if(!IfCanContinue && tag == "ContinueButton")
        {
            GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Mouse")
        {
            gameObject.GetComponent<AudioSource>().Play();
            if(tag == "Weapon")
            {
                GetComponent<RawImage>().color = new Color(0.5f, 1.0f, 1.0f, 1.0f);
            }
            else if (tag != "ContinueButton" || (IfCanContinue && tag == "ContinueButton"))
            {
                GetComponent<Image>().color = new Color(0, 0.5f, 0, 0.5f);
            }
            switch(tag)
            {
                case "StartButton":
                {
                    _MouseController.ButtonNum = 1;
                } break;
                case "ContinueButton":
                {
                    _MouseController.ButtonNum = 2;
                } break;
                case "ConfigButton":
                {
                    _MouseController.ButtonNum = 3;
                }
                break;
                case "RecordButton":
                {
                    _MouseController.ButtonNum = 4;
                }
                break;
                case "CloseButton":
                {
                    _MouseController.ButtonNum = 5;
                }
                break;
                case "ExitButton":
                {
                    _MouseController.ButtonNum = 6;
                }
                break;
                case "MenuButton":
                {
                    _MouseController.ButtonNum = 7;
                }
                break;
                case "StatusButton":
                {
                    _MouseController.ButtonNum = 8;
                }
                break;
                case "Weapon":
                {
                    _MouseController.ButtonNum = 9;
                } break;
                default: break;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Mouse")
        {
            _MouseController.ButtonNum = 0;
            if(tag == "Weapon")
            {
                GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            else if (tag != "ContinueButton" || (IfCanContinue && tag == "ContinueButton"))
            {
                GetComponent<Image>().color = new Color(1.0f, 0.5f, 0, 0.5f);
            }
        }
    }
}
