using UnityEngine;
using UnityEditor;

// @kurtdekker - easy cheesy add sphere colliders in a circle (for a torus)
// see notes below for how to make it go around a different axis

public class AddRingOfSphereColliders : EditorWindow
{
	int count = 24;
	float largeRadius = 2.0f;
	float smallRadius = 0.5f;

	Vector3 positionOffset = Vector3.zero;
	Vector3 rotationOffset = Vector3.zero;

	bool isTrigger = true; // Add this line

	[MenuItem("GameObject/Add Ring Of Sphere Colliders")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		AddRingOfSphereColliders window = (AddRingOfSphereColliders)EditorWindow.GetWindow(typeof(AddRingOfSphereColliders));
		window.Show();
	}

	void OnGUI()
	{
		GUILayout.Label("Toroidal SphereCollider Settings", EditorStyles.boldLabel);
		GUILayout.Space(10);
		GUILayout.Label("@kurtdekker");
		GUILayout.Space(10);
		GUILayout.Label("Select the GameObject(s) you want to operate on first!");
		GUILayout.Space(10);

		count = (int)EditorGUILayout.Slider("Sphere Count", count, 10, 50);

		largeRadius = EditorGUILayout.Slider("Large Radius", largeRadius, 1.0f, 5.0f);
		smallRadius = EditorGUILayout.Slider("Small Radius", smallRadius, 0.1f, 2.0f);

		positionOffset = EditorGUILayout.Vector3Field("Position Offset", positionOffset);
		rotationOffset = EditorGUILayout.Vector3Field("Rotation Offset", rotationOffset);

		isTrigger = EditorGUILayout.Toggle("Is Trigger", isTrigger); // Add this line

		if (GUILayout.Button("REPLACE COLLIDERS"))
		{
			if (EditorUtility.DisplayDialog("CONFIRM!",
				"Wipe out all previous SphereColliders, add a fresh ring of SphereColliders.",
				"REPLACE COLLIDERS", "Cancel"))
			{
				foreach (var go in Selection.gameObjects)
				{
					// remove old
					var allc = go.GetComponents<SphereCollider>();
					foreach (var col in allc)
					{
						DestroyImmediate(col);
					}

					// add new
					for (int i = 0; i < count; i++)
					{
						float angle = (Mathf.PI * 2 * i) / count;

						float sin = Mathf.Sin(angle) * largeRadius;
						float cos = Mathf.Cos(angle) * largeRadius;

						var sp = go.AddComponent<SphereCollider>();

						sp.radius = smallRadius;
						sp.isTrigger = isTrigger; // Add this line

						Vector3 localPosition = new Vector3(sin, cos, 0);
						sp.center = Quaternion.Euler(rotationOffset) * localPosition + positionOffset;
					}
				}
			}
		}
	}
}
