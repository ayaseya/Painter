using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintController : MonoBehaviour
{

	public Texture2D texture2D; // 描き込み先のTexture
	public GameObject pointer;  // カーソル
	private Brush brush;        // ブラシサイズ、色情報を保持するクラス

	void Start()
	{
		// 初期化処理(Inspectorでpublic変数が紐付けられていない時はタグから取得する)
		if (texture2D == null)
		{
			texture2D = GameObject.FindWithTag("Canvas")
								  .GetComponent<SpriteRenderer>()
								  .sprite
								  .texture;
		}

		if (pointer == null)
		{
			Debug.Log("pointer");
			pointer = GameObject.FindWithTag("Pointer");
		}

		brush = new Brush();
	}

	void Update()
	{
		// マウス座標をワール座標からスクリーン座標に変換する
		Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pointer.transform.position = new Vector3(
			mouse.x,
			mouse.y,
			this.transform.position.z /* マウスのz座標は-10となってしまうため、
			スクリプトがアタッチされているオブジェクトのz座標で補正する */
		);
      
		// マウスクリック
		if (Input.GetMouseButton(0))
		{
			Draw(Input.mousePosition);
		}
	}

	// 描き込み(Textureに描き込むだけで、元になったpngファイルには反映されない)
	private void Draw(Vector2 position)
	{
		// Textureにピクセルカラーを設定する
		texture2D.SetPixels((int)position.x, (int)position.y,
							brush.blockWidth,
							brush.blockHeight,
							brush.colors);

		// 反映
		texture2D.Apply();
	}

	// ブラシ
	private class Brush
	{
		public int blockWidth = 4;
		public int blockHeight = 4;
		public Color color = Color.gray;
		public Color[] colors;

		public Brush()
		{
			colors = new Color[blockWidth * blockHeight];
			for (int i = 0; i < colors.Length; i++)
			{
				colors[i] = color;
			}
		}
	}
}
