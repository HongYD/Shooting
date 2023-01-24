using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBall : MonoBehaviour
{
    public Transform pivot;
    List<Transform> pivots= new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        pivot = GameObject.Find("BallPivot").transform;
        if (this.transform.childCount > 0)
        {
            for(int i=0;i<this.transform.childCount;i++)
            {
                pivots.Add(this.transform.GetChild(i));
            }

            for(int i = 0; i < pivots.Count - 1; i++)
            {
                GameObject.Instantiate(GameAsstes.Instance.basketball, pivots[i].transform.position,Quaternion.identity, pivot);
            }
            GameObject.Instantiate(GameAsstes.Instance.football, pivots[pivots.Count - 1].transform.position, Quaternion.identity, pivot);
        }
    }
}
