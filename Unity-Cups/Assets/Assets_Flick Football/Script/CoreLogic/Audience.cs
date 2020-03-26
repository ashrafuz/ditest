using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Audience : MonoBehaviour {

	public static Audience share;

	public Transform _bodies1;
	public Transform _bodies2;
	public Transform _bodies3;
	public Transform _bodies4;
	public Transform _bodies5;
	public Transform _bodies6;
	public Transform _bodies7;
	public Transform _bodies8;

	public Transform _sbodies1;
	public Transform _sbodies2;
	public Transform _sbodies3;
	public Transform _sbodies4;
	public Transform _sbodies5;
	public Transform _sbodies6;
	public Transform _sbodies7;
	public Transform _sbodies8;

	public int peopleDensity;
	public int animatedPeople;

	private List<Transform> _staticBodiesAudience;
	private List<Transform> _bodiesAudience;

	public Transform _reference;

	void Awake() {
		share = this;
		_bodiesAudience = new List<Transform>();
		_staticBodiesAudience = new List<Transform>();
	}

	// Use this for initialization
	void Start () {

		#if UNITY_IOS
		// If 0 do auto calc
		if (animatedPeople == 0) {

			// animated audience density 
			if (UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6 ||
			    UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6Plus) {
				animatedPeople = 50;
			} else {
				if (UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6S ||
				   UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6SPlus 
				   ) {
					animatedPeople = 10;//10
				} else {
					animatedPeople = 1;
				}
			}

		}

		// If 0 do auto calc
		if (peopleDensity == 0) {
			if (UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6 ||
			    UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6Plus) {
				peopleDensity = 95;
			} else {
				if (UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6S ||
				   UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6SPlus 
				   ) {
					peopleDensity = 90;//90
				} else {
					peopleDensity = 86;
				}
			}
		}
		#endif

		#if UNITY_ANDROID
		// If 0 do auto calc
		if (animatedPeople == 0) {
			animatedPeople = 2;
		}

		// If 0 do auto calc
		if (peopleDensity == 0) {
			peopleDensity = 86;
		}
		#endif

		DrawFans ();

		#if UNITY_IOS
			InvokeRepeating ("AnimatePeople", 2.0f, 2.0f);
		#endif
	}

	void AnimatePeople () {


		foreach (Transform b in _bodiesAudience) {
		
			int animar = (int)(Random.value * 80);
			if (animar <= 5) {
				var animation = b.GetComponent<Animation> ();
				animation.Play ("applause");
			}
			if (animar >= 6 && animar <= 10) {
				var animation = b.GetComponent<Animation> ();
				animation.Play ("applause2");
			}
			if (animar == 11) {
				var animation = b.GetComponent<Animation> ();
				animation.Play ("applause2");
			}
			if (animar == 12) {
				var animation = b.GetComponent<Animation> ();
				animation.Play ("celebration2");
			}
			if (animar == 13) {
				var animation = b.GetComponent<Animation> ();
				animation.Play ("celebration3");
			}
			if (animar == 10) {
				var animation = b.GetComponent<Animation> ();
				animation.Play ("celebration");
			}

		}

	}



	// Update is called once per frame
	void Update () {

	}


	private void DrawFans()
	{
		int rows = 20;
		float ystep = 8.0f / rows;
		float zstep = 18.0f / rows;
		int columns = 50;
		float xstep = 20.0f / columns;


		for (int row = 0; row < rows; row++) {

			for (int col = 0; col < columns; col++) {

				int audienceType = (int)(Random.value * 100);

				if (audienceType < animatedPeople) {
					Vector3 newpos = new Vector3 (-10 + (xstep * col), -4 + (ystep * row), -9 + (zstep * row));
					DrawFan (newpos);
				} else {
					Vector3 newpos = new Vector3 (-10 + (xstep * col), -3.5f+ (ystep * row), -9 + (zstep * row));
					DrawStaticFan (newpos);
				}
			
			}
		
		}


	}
		


	private void DrawFan(Vector3 position)
	{
		int densidad = (peopleDensity <= 100) ? peopleDensity : 50;

		int color = (int)(Random.value * (110-densidad));
		Transform bodyprefab = _bodies1;
		if (color == 0) 
			bodyprefab = _bodies1;
		if (color == 1)
			bodyprefab = _bodies2;
		if (color == 2)
			bodyprefab = _bodies3;
		if (color == 3) 
			bodyprefab = _bodies4;
		if (color == 4) 
			bodyprefab = _bodies5;
		if (color == 5)
			bodyprefab = _bodies6;
		if (color == 6) 
			bodyprefab = _bodies7;
		if (color == 7) 
			bodyprefab = _bodies8;
		
		if (color < 8) { 
			// Instancia el objeto gráfico
			Transform body = (Transform)Instantiate (bodyprefab, new Vector3 (0, 0, 0), Quaternion.Euler (0, 180, 0));
			NGUITools.SetLayer (body.gameObject, 9);
			body.parent = _reference;
			body.localPosition = position;

			_bodiesAudience.Add (body);
		}
	}

	private void DrawStaticFan(Vector3 position)
	{
		int densidad = (peopleDensity <= 100) ? peopleDensity : 50;

		int color = (int)(Random.value * (110-densidad));
		Transform bodyprefab = _sbodies1;
		if (color == 0) 
			bodyprefab = _sbodies1;
		if (color == 1)
			bodyprefab = _sbodies2;
		if (color == 2)
			bodyprefab = _sbodies3;
		if (color == 3) 
			bodyprefab = _sbodies4;
		if (color == 4) 
			bodyprefab = _sbodies5;
		if (color == 5)
			bodyprefab = _sbodies6;
		if (color == 6) 
			bodyprefab = _sbodies7;
		if (color == 7) 
			bodyprefab = _sbodies8;

		if (color < 8) { 
			// Instancia el objeto gráfico
			Transform body = (Transform)Instantiate (bodyprefab, new Vector3 (0, 0, 0), Quaternion.Euler (0, -360, 0));
			NGUITools.SetLayer (body.gameObject, 9);
			body.parent = _reference;
			body.localPosition = position;


			_staticBodiesAudience.Add (body);
		}
	}

}
