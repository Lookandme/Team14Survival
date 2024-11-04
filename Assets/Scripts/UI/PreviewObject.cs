using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    // Start is called before the first frame update
    List<Collider> colliderList = new List<Collider>();

    [SerializeField] int layerGround;
    const int IGNORE_RAYCST_LAYER = 2;

    [SerializeField] Material green;
    [SerializeField] Material red;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }

    void ChangeColor()
    {
        if (colliderList.Count > 0)
            SetColor(red);
        else
            SetColor(green);
    }

    void SetColor(Material mat)
    {
        foreach(Transform transformChild in this.transform)
        {
            var newMaterials = new Material[transformChild.GetComponent<Renderer>().materials.Length];

            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = mat;
            }
            transformChild.GetComponent<Renderer>().materials = newMaterials;
        }

        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCST_LAYER) 
        colliderList.Add(other);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCST_LAYER)
            colliderList.Remove(other);
    }

    public bool isBuildable()
    {
        return colliderList.Count == 0;
    }
}
