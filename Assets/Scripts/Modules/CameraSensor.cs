using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class CameraSensor : MonoBehaviour {
	public Texture2D outputFrame;
	public RenderTexture camTexture;
	private Color32[] rawFrame;
	public Image testSprite;
	private Rect spriteRect;
	private Vector2 pivot;
	
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
		outputFrame = new Texture2D(camTexture.width, camTexture.height); 
		spriteRect = new Rect(0, 0, camTexture.width, camTexture.height); 
		pivot = new Vector2(0.5f, 0.5f);
	}

	void Update() {
		rawFrame = ToTexture2D(camTexture).GetPixels32();
		ProcessImage(ref rawFrame, camTexture.width, camTexture.height);
		outputFrame.SetPixels32(rawFrame);
		outputFrame.Apply();
		testSprite.sprite = Sprite.Create(outputFrame, spriteRect, pivot);
		//System.IO.File.WriteAllBytes (Application.dataPath + "/test.png", outputFrame.EncodeToPNG());
	}
}