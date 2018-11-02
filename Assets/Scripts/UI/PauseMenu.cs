using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    delegate void VoidAction();

    [SerializeField] GameObject panel;
    [SerializeField] Vector2 resumedPosition = new Vector2(0, 400);
    [SerializeField] Vector2 pausedPosition = new Vector2(0, 0);
    [SerializeField] float speed = 20;
    GamePauser gamePauser;
    
    public void Pause()
    {
        gamePauser.Pause();
        MovePanelPause();
    }

    public void MovePanelPause()
    {
        StopAllCoroutines();
        panel.gameObject.SetActive(true);
        StartCoroutine(MovePanel(pausedPosition, null));
    }

    public void Resume()
    {
        gamePauser.Resume();
        MovePanelResume();
    }

    public void MovePanelResume()
    {
        StopAllCoroutines();
        StartCoroutine(MovePanel(resumedPosition, () => panel.gameObject.SetActive(false)));
    }

    IEnumerator MovePanel(Vector2 endPos, VoidAction action)
    {
        Vector3 direction = ((Vector3)endPos - panel.transform.position).normalized;

        while (Vector2.Distance(panel.transform.position, endPos) > speed * Time.deltaTime)
        {
            panel.transform.position += direction * speed * Time.deltaTime;
            yield return null;
        }

        if (action != null)
        {
            action();
        }
    }

    private void Start()
    {
        Transform canvasTransform = FindObjectOfType<Canvas>().transform;
        pausedPosition = canvasTransform.position;
        Vector3 deltaPos = new Vector3(0, ((RectTransform)canvasTransform).sizeDelta.y, 0);
        deltaPos += new Vector3(0, ((RectTransform)panel.transform).sizeDelta.y + 300, 0);
        resumedPosition = canvasTransform.position + deltaPos;
        panel.transform.position = resumedPosition;
        panel.gameObject.SetActive(false);
        gamePauser = FindObjectOfType<GamePauser>();
    }
}
