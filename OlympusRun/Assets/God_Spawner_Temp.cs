using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class God_Spawner_Temp : MonoBehaviour
{
    [SerializeField] private GameObject tempGod;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(tempGod,transform.parent);
    }

    // Update is called once per frame
    void Update()
    {
        tempGod.transform.position += new Vector3(0, 10, 0);
    }
}
