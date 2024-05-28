using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBoltV2 : MonoBehaviour
{
	public Vector3 rot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		gameObject.transform.Rotate(rot, 1f);
	}
}
