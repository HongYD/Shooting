//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class LoaderCallback : MonoBehaviour
//{
//    private bool isFirstUpdate = true;
//    // Update is called once per frame

//    private void Start()
//    {
//        CursorManager.instance.SetCursorType(CursorType.CursorNormal, true);
//    }
//    void Update()
//    {
//        if(isFirstUpdate)
//        {
//            isFirstUpdate= false;
//            StartCoroutine(LoaderCallBackAsyc());
//        }
//    }

//    public IEnumerator LoaderCallBackAsyc()
//    {
//        yield return null;
//        SceneLoader.LoaderCallback();
//    }
//}
