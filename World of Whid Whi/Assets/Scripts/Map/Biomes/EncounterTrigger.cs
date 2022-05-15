using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterTrigger : MonoBehaviour
{
    EncounterTerrain Parent;
    // Start is called before the first frame update
    void Start()
    {
        Parent = transform.parent.GetComponent<EncounterTerrain>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Parent.GotCollission(collision);
    }
}