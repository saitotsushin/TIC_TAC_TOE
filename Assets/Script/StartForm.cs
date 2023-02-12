using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartForm : MonoBehaviour
{
    //オブジェクトと結びつける
    public GameObject FormArea;
    public InputField inputField;
  public static StartForm instance;
  public Text text;
     void Awake ()
    {
        if (instance == null) {
        
            instance = this;     
        }
        else {
            Destroy (gameObject);
        }
    }
    void Start () {
        //Componentを扱えるようにする
        inputField = inputField.GetComponent<InputField> ();
    }
    public void HideForm(){
        FormArea.SetActive(false);
    }
}
