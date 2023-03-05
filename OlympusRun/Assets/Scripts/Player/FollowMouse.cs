using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public GameObject trailPrefab;

    List<GameObject> trails = new List<GameObject>();

    private void Start()
    {
        if (this.GetComponent<Canvas>().worldCamera == null)
        {
            this.GetComponent<Canvas>().worldCamera = Camera.main;
        }

    }

    private void FixedUpdate()
    {
        if (Input.touchCount >= 1)
        {
            while (Input.touchCount != trails.Count)
            {
                if (Input.touchCount > trails.Count)
                {
                    SpawnTrail();
                }

                else
                {
                    trails.RemoveAt(trails.Count - 1);
                }
            }


            for (int i = 0; i < trails.Count; i++)
            {
                var touch = Input.GetTouch(i);

                Debug.Log(touch.position);

                var newPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 100));

                trails[i].transform.position = newPos;
            }
        }

        else
        {
            if (trails.Count > 0)
            {
                foreach (GameObject t in trails)
                {
                    Destroy(t);
                }
                trails.Clear();
            }
        }
    }

    public void SpawnTrail()
    {
        GameObject j = Instantiate(trailPrefab);
        j.transform.SetParent(this.transform);
        trails.Add(j);
    }
}
        
