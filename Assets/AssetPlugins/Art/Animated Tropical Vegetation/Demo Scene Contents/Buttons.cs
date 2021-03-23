using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
	public Camera _camera;
	public GameObject bamboo;
	public GameObject bananaTree;
	public GameObject bigPalmTree;
	public GameObject bushyPalm;
	public GameObject deadTree;
	public GameObject doublePalmTree;
	public GameObject jungleBushes;
	public GameObject jungleTree;
	public GameObject largePalmTree;
	public GameObject palmTree;
	public GameObject smallPalmTree;
	public GameObject smallTree;
	public GameObject tallTree;
	public GameObject tropicalPlant;
	
	public void Bamboos()
	{
		
		_camera.transform.position = new Vector3(88.98f, 33.7f, -56.0f);
		bamboo.transform.rotation = Quaternion.identity;
		bamboo.SetActive(true);
		bananaTree.SetActive(false);
		bigPalmTree.SetActive(false);
		bushyPalm.SetActive(false);
		doublePalmTree.SetActive(false);
		deadTree.SetActive(false);
		jungleBushes.SetActive(false);
		jungleTree.SetActive(false);
		largePalmTree.SetActive(false);
		palmTree.SetActive(false);
		smallPalmTree.SetActive(false);
		smallTree.SetActive(false);
		tallTree.SetActive(false);
		tropicalPlant.SetActive(false);
	}
		public void BananaTrees()
	{
		_camera.transform.position = new Vector3(90.3f, 26.9f, -22.3f);
		bananaTree.transform.rotation = Quaternion.identity;
		bamboo.SetActive(false);
		bananaTree.SetActive(true);
		bigPalmTree.SetActive(false);
		bushyPalm.SetActive(false);
		doublePalmTree.SetActive(false);
		deadTree.SetActive(false);
		jungleBushes.SetActive(false);
		jungleTree.SetActive(false);
		largePalmTree.SetActive(false);
		palmTree.SetActive(false);
		smallPalmTree.SetActive(false);
		smallTree.SetActive(false);
		tallTree.SetActive(false);
		tropicalPlant.SetActive(false);
	}
	
		public void BigPalmTree()
	{
		_camera.transform.position = new Vector3(96.4f, 78.2f, -114.5f);
		bigPalmTree.transform.rotation = Quaternion.identity;
		bamboo.SetActive(false);
		bananaTree.SetActive(false);
		bigPalmTree.SetActive(true);
		bushyPalm.SetActive(false);
		doublePalmTree.SetActive(false);
		deadTree.SetActive(false);
		jungleBushes.SetActive(false);
		jungleTree.SetActive(false);
		largePalmTree.SetActive(false);
		palmTree.SetActive(false);
		smallPalmTree.SetActive(false);
		smallTree.SetActive(false);
		tallTree.SetActive(false);
		tropicalPlant.SetActive(false);
	}
	
		public void BushyPalms()
	{
		_camera.transform.position = new Vector3(108.9f, 16.3f, -35f);
		bushyPalm.transform.rotation = Quaternion.identity;
		bamboo.SetActive(false);
		bananaTree.SetActive(false);
		bigPalmTree.SetActive(false);
		bushyPalm.SetActive(true);
		doublePalmTree.SetActive(false);
		deadTree.SetActive(false);
		jungleBushes.SetActive(false);
		jungleTree.SetActive(false);
		largePalmTree.SetActive(false);
		palmTree.SetActive(false);
		smallPalmTree.SetActive(false);
		smallTree.SetActive(false);
		tallTree.SetActive(false);
		tropicalPlant.SetActive(false);
	}
		public void DeadTrees()
	{
		_camera.transform.position = new Vector3(98.1f, 32.82f, -30.7f);
		deadTree.transform.rotation = Quaternion.identity;
		bamboo.SetActive(false);
		bananaTree.SetActive(false);
		bigPalmTree.SetActive(false);
		bushyPalm.SetActive(false);
		doublePalmTree.SetActive(false);
		deadTree.SetActive(true);
		jungleBushes.SetActive(false);
		jungleTree.SetActive(false);
		largePalmTree.SetActive(false);
		palmTree.SetActive(false);
		smallPalmTree.SetActive(false);
		smallTree.SetActive(false);
		tallTree.SetActive(false);
		tropicalPlant.SetActive(false);
	}
		
		public void DoublePalmTree()
	{
		_camera.transform.position = new Vector3(82.0f, 71.2f, -115.0f);
		doublePalmTree.transform.rotation = Quaternion.identity;
		bamboo.SetActive(false);
		bananaTree.SetActive(false);
		bigPalmTree.SetActive(false);
		bushyPalm.SetActive(false);
		doublePalmTree.SetActive(true);
		deadTree.SetActive(false);
		jungleBushes.SetActive(false);
		jungleTree.SetActive(false);
		largePalmTree.SetActive(false);
		palmTree.SetActive(false);
		smallPalmTree.SetActive(false);
		smallTree.SetActive(false);
		tallTree.SetActive(false);
		tropicalPlant.SetActive(false);
	}
	
		public void JungleBushes()
	{
		_camera.transform.position = new Vector3(77.25f, 12.91f, -4.8f);
		jungleBushes.transform.rotation = Quaternion.identity;
		bamboo.SetActive(false);
		bananaTree.SetActive(false);
		bigPalmTree.SetActive(false);
		bushyPalm.SetActive(false);
		doublePalmTree.SetActive(false);
		deadTree.SetActive(false);
		jungleBushes.SetActive(true);
		jungleTree.SetActive(false);
		largePalmTree.SetActive(false);
		palmTree.SetActive(false);
		smallPalmTree.SetActive(false);
		smallTree.SetActive(false);
		tallTree.SetActive(false);
		tropicalPlant.SetActive(false);
	}
	
		public void JungleTree()
	{
		_camera.transform.position = new Vector3(93.26f, 70.0f, -114.8f);
		jungleTree.transform.rotation = Quaternion.identity;
		bamboo.SetActive(false);
		bananaTree.SetActive(false);
		bigPalmTree.SetActive(false);
		bushyPalm.SetActive(false);
		doublePalmTree.SetActive(false);
		deadTree.SetActive(false);
		jungleBushes.SetActive(false);
		jungleTree.SetActive(true);
		largePalmTree.SetActive(false);
		palmTree.SetActive(false);
		smallPalmTree.SetActive(false);
		smallTree.SetActive(false);
		tallTree.SetActive(false);
		tropicalPlant.SetActive(false);
	}
	
		public void LargePalmTrees()
	{
		_camera.transform.position = new Vector3(82.18f, 99.1f, -186.8f);
		largePalmTree.transform.rotation = Quaternion.identity;
		bamboo.SetActive(false);
		bananaTree.SetActive(false);
		bigPalmTree.SetActive(false);
		bushyPalm.SetActive(false);
		doublePalmTree.SetActive(false);
		deadTree.SetActive(false);
		jungleBushes.SetActive(false);
		jungleTree.SetActive(false);
		largePalmTree.SetActive(true);
		palmTree.SetActive(false);
		smallPalmTree.SetActive(false);
		smallTree.SetActive(false);
		tallTree.SetActive(false);
		tropicalPlant.SetActive(false);
	}
	
		public void PalmTrees()
	{
		_camera.transform.position = new Vector3(48.3f, 55.6f, -105.7f);
		palmTree.transform.rotation = Quaternion.identity;
		bamboo.SetActive(false);
		bananaTree.SetActive(false);
		bigPalmTree.SetActive(false);
		bushyPalm.SetActive(false);
		doublePalmTree.SetActive(false);
		deadTree.SetActive(false);
		jungleBushes.SetActive(false);
		jungleTree.SetActive(false);
		largePalmTree.SetActive(false);
		palmTree.SetActive(true);
		smallPalmTree.SetActive(false);
		smallTree.SetActive(false);
		tallTree.SetActive(false);
		tropicalPlant.SetActive(false);
	}
	
		public void SmallPalmTrees()
	{
		_camera.transform.position = new Vector3(47.83f, 31.1f, -41.6f);
		smallPalmTree.transform.rotation = Quaternion.identity;
		bamboo.SetActive(false);
		bananaTree.SetActive(false);
		bigPalmTree.SetActive(false);
		bushyPalm.SetActive(false);
		doublePalmTree.SetActive(false);
		deadTree.SetActive(false);
		jungleBushes.SetActive(false);
		jungleTree.SetActive(false);
		largePalmTree.SetActive(false);
		palmTree.SetActive(false);
		smallPalmTree.SetActive(true);
		smallTree.SetActive(false);
		tallTree.SetActive(false);
		tropicalPlant.SetActive(false);
	}
		public void SmallTrees()
	{
		_camera.transform.position = new Vector3(43.6f, 31.7f, -28.3f);
		smallTree.transform.rotation = Quaternion.identity;
		bamboo.SetActive(false);
		bananaTree.SetActive(false);
		bigPalmTree.SetActive(false);
		bushyPalm.SetActive(false);
		doublePalmTree.SetActive(false);
		deadTree.SetActive(false);
		jungleBushes.SetActive(false);
		jungleTree.SetActive(false);
		largePalmTree.SetActive(false);
		palmTree.SetActive(false);
		smallPalmTree.SetActive(false);
		smallTree.SetActive(true);
		tallTree.SetActive(false);
		tropicalPlant.SetActive(false);
	}
		public void TallTrees()
	{
		_camera.transform.position = new Vector3(48.07f, 75.77f, -126f);
		tallTree.transform.rotation = Quaternion.identity;
		bamboo.SetActive(false);
		bananaTree.SetActive(false);
		bigPalmTree.SetActive(false);
		bushyPalm.SetActive(false);
		doublePalmTree.SetActive(false);
		deadTree.SetActive(false);
		jungleBushes.SetActive(false);
		jungleTree.SetActive(false);
		largePalmTree.SetActive(false);
		palmTree.SetActive(false);
		smallPalmTree.SetActive(false);
		smallTree.SetActive(false);
		tallTree.SetActive(true);
		tropicalPlant.SetActive(false);
	}
		public void TropicalPlant()
	{
		_camera.transform.position = new Vector3(89.0f, 7.1f, 19.2f);
		tropicalPlant.transform.rotation = Quaternion.identity;
		bamboo.SetActive(false);
		bananaTree.SetActive(false);
		bigPalmTree.SetActive(false);
		bushyPalm.SetActive(false);
		doublePalmTree.SetActive(false);
		deadTree.SetActive(false);
		jungleBushes.SetActive(false);
		jungleTree.SetActive(false);
		largePalmTree.SetActive(false);
		palmTree.SetActive(false);
		smallPalmTree.SetActive(false);
		smallTree.SetActive(false);
		tallTree.SetActive(false);
		tropicalPlant.SetActive(true);
	}
	
}
