using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WipeTransition : MonoBehaviour
{
    public static void SceneTransition(string sceneName)
    {
        GameObject prefab = Resources.Load("WipeTransition") as GameObject;
        GameObject instance = GameObject.Instantiate(prefab);
        DontDestroyOnLoad(instance);
        instance.transform.GetChild(0).GetComponent<WipeTransition>().ChangeScene(sceneName);
    }


    public void ChangeScene(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }

    private IEnumerator Transition(string sceneName)
    {
        yield return new WaitForEndOfFrame();
        Animator animator = GetComponent<Animator>();
        animator.SetBool("WipeIn", false);
        animator.SetTrigger("Go");

        yield return new WaitForEndOfFrame();

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Decide"))
        {
            yield return null;
        }

        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOp.isDone)
        {
            yield return new WaitForEndOfFrame();
        }

        animator.SetBool("WipeIn", true);
        animator.SetTrigger("Go");

        yield return new WaitForEndOfFrame();
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Decide"))
        {
            yield return null;
        }
        Destroy(transform.parent.gameObject);
    }
}
