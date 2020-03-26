using UnityEngine;
using System.Collections;

public class Displaysled : MonoBehaviour {


	public Transform _led1;
	public Transform _led2;
	public Transform _led3;

	private float textureoffset = 0;
	private float texturescale = 0;
	private bool ledsecuence = false;

	// Use this for initialization
	void Start () {

		#if UNITY_IOS
//		if (UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6 ||
//			UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6Plus ||
//			UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6S||
//			UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6SPlus 
//		) {
			InvokeRepeating ("AnimateLeds", 0.5f, 0.1f);
//		}
		#endif
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void AnimateLeds () {

		if (!ledsecuence) {

			textureoffset = textureoffset + 0.005f;
			if (textureoffset > 0.2) {
				textureoffset = 0;
				ledsecuence = true;
			}

			Renderer renderer1 = _led1.GetComponent<Renderer> ();
			renderer1.material.mainTextureOffset = new Vector2 (textureoffset, 0);

			Renderer renderer2 = _led2.GetComponent<Renderer> ();
			renderer2.material.mainTextureOffset = new Vector2 (textureoffset, 0);

			Renderer renderer3 = _led3.GetComponent<Renderer> ();
			renderer3.material.mainTextureOffset = new Vector2 (textureoffset, 0);

		} else {

			texturescale = texturescale + 0.1f;
			if (texturescale > 0.85f) {
				texturescale = 0f;
				ledsecuence = false;
			}

			Renderer renderer1 = _led1.GetComponent<Renderer> ();
			renderer1.material.mainTextureScale = new Vector2 (-1f + texturescale, -1.4f + texturescale);
			Renderer renderer2 = _led2.GetComponent<Renderer> ();
			renderer2.material.mainTextureScale = new Vector2 (-1f + texturescale, -1.4f + texturescale);			
			Renderer renderer3 = _led3.GetComponent<Renderer> ();
			renderer3.material.mainTextureScale = new Vector2 (-1f + texturescale, -1.4f + texturescale);


		}

	}
}
