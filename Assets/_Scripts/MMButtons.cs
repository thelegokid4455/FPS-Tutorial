using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MMButtons : MonoBehaviour
{

    public Animator animator;

    public string levelToLoad;

    public bool isUIMenu;

    public bool isQuit;

    public AudioClip hoverAudio;
    public AudioClip clickAudio;

    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<Animator>())
            animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isUIMenu)
        {

        }
    }

    

    private void OnMouseEnter()
    {
        if (!isUIMenu)
        {
            GetComponent<AudioSource>().PlayOneShot(hoverAudio);
        }
    }

    private void OnMouseExit()
    {
        if (!isUIMenu)
        {

        }
    }

    private void OnMouseDown()
    {
        if (!isUIMenu)
        {
            GetComponent<AudioSource>().PlayOneShot(clickAudio);
            animator.gameObject.GetComponent<MMButtons>().levelToLoad = levelToLoad;
            animator.gameObject.GetComponent<MMButtons>().isQuit = isQuit;
            Fade();
        }
    }

    public void Fade()
    {
        animator.SetTrigger("Fade");
    }

    public void FadeComplete ()
    {
        if (isQuit)
            Quit();
        else
            SceneManager.LoadScene(levelToLoad);
    }

    public void LoadLevel(string level)
    {
            levelToLoad = level;
            Fade();
    }

    public void QuitUI()
    {
        animator.gameObject.GetComponent<MMButtons>().isQuit = true;
        Fade();
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
