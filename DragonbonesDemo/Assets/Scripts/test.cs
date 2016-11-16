using UnityEngine;
using System.Collections;
using DragonBones;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		UnityFactory.factory.LoadDragonBonesData ("huochairen");
		UnityFactory.factory.LoadTextureAtlasData ("texture");

		var armatureComponent = UnityFactory.factory.BuildArmatureComponent ("armatureName");
		armatureComponent.animation.Play ("gongji01",0);

		armatureComponent.transform.localPosition = new Vector3 (-5, -3, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
