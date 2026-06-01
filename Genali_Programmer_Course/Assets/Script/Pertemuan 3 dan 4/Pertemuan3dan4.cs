using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pertemuan3dan4 : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI HUDText;
    [SerializeField] Rigidbody _cubeRigid;
    [SerializeField] Transform _cubeTransform;

    // Start is called before the first frame update
    void Start()
    {
        HUDText.text = "Tugas Pertemuan 3 dan 4";
        _cubeRigid.isKinematic = true;
        _cubeTransform.position = new Vector3(10, 0, 15);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
