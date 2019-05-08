using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sun Position Controller For State 1
public class SunPositionController : MonoBehaviour
{
    // Update is called once per frame
    public float startPosition = 0f;
    public float endPosition = 4.23f;

    public Material edgeRaysMaterial = null;
    GameSystem GS;

    void Start()
    {
        Initialize();
        GS = FindObjectOfType<GameSystem>();
    }

    public void Initialize()
    {
    	gameObject.transform.position = new Vector3(gameObject.transform.position.x, startPosition, gameObject.transform.position.z);
    }

    public void StartAllCoroutine()
    {
        StartCoroutine(MovingCoroutine());
    }

    IEnumerator MovingCoroutine()
    {
        while (GS.state == 1)
        {
            float position = startPosition + (endPosition - startPosition) * GS.s1SmoothProgress;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, position, gameObject.transform.position.z);
            yield return 0;
        }
    }

    void OnDestroy()
    {
        edgeRaysMaterial.SetFloat("_StartPointV", 0.5f);
    }
}
