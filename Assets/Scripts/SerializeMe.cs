using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class SerializeMe : MonoBehaviour {

	[Serializable]
	public struct TEST_EXPORT{
		public float number;
	}
	
	public TEST_EXPORT TestExport;
	
	// Use this for initialization
	void Start () {
	
		TestExport = new TEST_EXPORT();
        TestExport.number = 0f;

	}
	
	// Update is called once per frame
	void Update () {
        //SAVING
        if (Input.GetKeyDown("k"))
        {
            TestExport.number += Time.time;
            Debug.Log("K WAS PRESSED");
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("TEST_EXPORT.bin", FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, TestExport);
            stream.Close();
            TestExport.number = 0f;
        }

        //LOADING
        if (Input.GetKeyDown("l"))
        {
            Debug.Log("L WAS PRESSED");
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("TEST_EXPORT.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
            TestExport = (TEST_EXPORT)formatter.Deserialize(stream);

            stream.Close();
            Debug.Log("NEW NUMBER IS: " + TestExport.number);
        }
	}
}
