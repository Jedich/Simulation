using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CameraSensor : MonoBehaviour {
	public Texture2D outputFrame;
	public RenderTexture camTexture;
	private Color32[] rawFrame;
	public Image testSprite;
	private Rect spriteRect;
	private Vector2 pivot;
	public bool isComputed = false;
	private int camWidth, camHeight;
	
	Texture2D ToTexture2D(RenderTexture rTex) {
		Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);
		RenderTexture.active = rTex;
		tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
		tex.Apply();
		return tex;
	}
	
	[DllImport("CVConnection")]
	private static extern void ProcessImage(ref Color32[] rawImage, int width, int height);

	void Start() {
		camWidth = camTexture.width;
		camHeight = camTexture.height;
		outputFrame = new Texture2D(camTexture.width, camTexture.height); 
		spriteRect = new Rect(0, 0, camTexture.width, camTexture.height); 
		pivot = new Vector2(0.5f, 0.5f);
		rawFrame = ToTexture2D(camTexture).GetPixels32();
		//StartCoroutine(CVCall());
		Thread a = new Thread(VisionCall);
		a.Start();
	}
	

	void VisionCall() {
		for (;;){
			if (!isComputed) {
				ProcessImage(ref rawFrame, camWidth, camHeight);
				Thread.Sleep((int) (CarPreferences.map["cvCallDelay"] * 100));
				isComputed = true;
			}
			//yield return new WaitForSeconds(CarPreferences.map["cvCallDelay"]);
			//System.IO.File.WriteAllBytes (Application.dataPath + "/test.png", outputFrame.EncodeToPNG());
		}
	}

	void Update() {
		if (isComputed) {
			outputFrame.SetPixels32(rawFrame);
			outputFrame.Apply();
			testSprite.sprite = Sprite.Create(outputFrame, spriteRect, pivot);
			rawFrame = ToTexture2D(camTexture).GetPixels32();
			isComputed = false;
		}
	}
}