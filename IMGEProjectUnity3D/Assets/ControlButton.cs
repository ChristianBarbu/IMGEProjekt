using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlButton : MonoBehaviour
{
    [SerializeField]
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        button.onClick.AddListener(loadControls);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void loadControls()
    {
        SceneManager.LoadScene("Controls");
    }
}
