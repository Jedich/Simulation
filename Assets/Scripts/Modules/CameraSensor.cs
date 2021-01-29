using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Modules {
	public class CameraSensor : MonoBehaviour {
		private Texture2D outputFrame;
		public RenderTexture camTexture;
		private Color32[] rawFrame; 
		public Image visionImage;
		private Rect spriteRect;
		private Vector2 pivot;
		public bool isComputed = false;
		private int camWidth, camHeight;
		internal GameObject physical;
	
		Texture2D ToTexture2D(RenderTexture rTex) {
			Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);
			RenderTexture.active = rTex;
			tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
			tex.Apply();
			return tex;
		}
	
		[DllImport("CVConnection")]
		private static extern void ProcessImage(ref Color32[] rawImage, int width, int height);

		internal void Start() {
			visionImage = Load.instance.vision;
			camTexture = Load.instance.camTexture;
			visionImage.gameObject.SetActive(true);
			camWidth = camTexture.width;
			camHeight = camTexture.height;
			outputFrame = new Texture2D(camTexture.width, camTexture.height); 
			spriteRect = new Rect(0, 0, camTexture.width, camTexture.height); 
			pivot = new Vector2(0.5f, 0.5f);
			rawFrame = ToTexture2D(camTexture).GetPixels32();
			Thread a = new Thread(VisionCall);
			a.Start();
		}
	

		void VisionCall() {
			for (;;){
				if (!isComputed) {
					ProcessImage(ref rawFrame, camWidth, camHeight);
					Thread.Sleep((int) (CarPreferences.current.cvCallDelay * 100));
					isComputed = true;
				}
			}
		}

		void Update() {
			if (isComputed) {
				outputFrame.SetPixels32(rawFrame);
				outputFrame.Apply();
				visionImage.sprite = Sprite.Create(outputFrame, spriteRect, pivot);
				rawFrame = ToTexture2D(camTexture).GetPixels32();
				if (Load.instance.sensor) {
					Load.instance.sensor.Behaviour();
				}
				isComputed = false;
			}
		}
	}
}