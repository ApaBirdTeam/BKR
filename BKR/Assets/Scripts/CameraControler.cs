using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    [SerializeField] GameObject selecter;
    [SerializeField] private List <GameObject> target = new List<GameObject>();
    [SerializeField] private float pauseTime = 0.1f;
    private Camera camera;
    private float timeClick;
    private GameObject nowSelect;


    void Start()
    {
        camera = this.GetComponent<Camera>();
    }

    void Update()
    {
        Vector3 cameraSee = camera.ScreenPointToRay(Input.mousePosition).GetPoint(Mathf.Abs(this.transform.position.z));

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            timeClick = Time.time;
            RaycastHit2D hit = Physics2D.Raycast(cameraSee, Vector3.back);
            if (target.Count > 1)
            {
                bool ican = true;
                foreach (GameObject i in target)
                    if (!i.GetComponent<Component>().CheckUnselect())
                    {
                        ican = false;
                        break;
                    }
                if (ican)
                {
                    foreach (GameObject i in target)
                        i.GetComponent<Component>().Unselect();
                    target = new List<GameObject>();
                }
            }
            else if (target.Count == 1)
            {
                foreach (GameObject i in target)
                    if (i != null)
                        i.GetComponent<IClickComponent>().Unselect();
                target = new List<GameObject>();
            }
            else if (hit.collider != null && hit.collider.GetComponent<IClickComponent>())
            {
                target.Add(hit.collider.GetComponent<IClickComponent>().Select(camera));   
            }

        }

        if (Input.GetKey(KeyCode.Mouse0) && Time.time - timeClick > pauseTime && target.Count == 0 && nowSelect == null)
        {
            foreach (GameObject i in target)
                i.GetComponent<IClickComponent>().Unselect();
            nowSelect = Instantiate(selecter, new Vector3(cameraSee.x, cameraSee.y, 0), Quaternion.identity);
            nowSelect.GetComponent<Select>().selectCamera(camera);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && nowSelect != null)
        {
            Debug.Log("I");
            GameObject[] x = nowSelect.GetComponent<Select>().GetObjects();
            Destroy(nowSelect);
            foreach (GameObject i in x)
            {
                target.Add(i.GetComponent<IClickComponent>().Select(camera)); 
            }
            nowSelect = null;
        }

    }
}
