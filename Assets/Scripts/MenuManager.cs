using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI emptyText;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += PlayerHelper;
    }

    public void Exit(){
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }

    public void StartGame(){
        if(string.IsNullOrEmpty(PlayerManager.Instance.PlayerName)){
            emptyText.gameObject.SetActive(true);
        }
        else{
            SceneManager.LoadScene(1);
        }
    }

    private void PlayerHelper(Scene scene, LoadSceneMode mode){
        if(scene.isLoaded && scene.name == "menu"){
            PlayerManager.Instance.GetNameInput();
        }
    }
}
