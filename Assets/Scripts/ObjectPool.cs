using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefabObject;
    [SerializeField] private int objectsNumberOnStart;

    private List<GameObject> objectsPool = new List<GameObject>();


    private void Start()
    {
        CreateObjects(objectsNumberOnStart);
    }

    /// <summary>
    ///    create the objects needed at the begining of the game
    /// </summary>
    /// <param name="numberOfObjects"></param>
    private void CreateObjects(int numberOfObjects)
    {
        for (int i = 0; i < objectsNumberOnStart; i++)
        {
            //TODO createnewObject()
            CreateNewObject();
        }
    }


    /// <summary>
    /// Instantiate new object and add to the list
    /// </summary>
    /// <returns></returns>
    private GameObject CreateNewObject()
    {
        //instantiate anywhere
        GameObject newObject = Instantiate(prefabObject);

        //deactivate
        newObject.SetActive(false);

        //add to the list
        objectsPool.Add(newObject);
        return newObject;
    }

    /// <summary>
    /// take from the list available obj/ if not create & active obj
    /// </summary>
    /// <returns></returns>
    public GameObject GetGameObject()
    {
        //find in objpool inactive obj in game hierarchy
        GameObject theObject = objectsPool.Find(x => x.activeInHierarchy == false);

        //if not exist, create one
        if (gameObject == null)
        {
            theObject = CreateNewObject();
        }

        theObject.SetActive(true);

        return theObject;
    }


}
