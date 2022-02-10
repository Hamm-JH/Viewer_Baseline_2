using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bearroll.UltimateDecals;
using Definition;

namespace Snippets
{
	public class TestClick : MonoBehaviour
	{
		public Material mat;

		// Update is called once per frame
		void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				int maskIndex = -1 & ~(1 << 6); //LayerMask.NameToLayer(LayerNames.DecalPoint.ToString()));

				if (Physics.Raycast(ray, out hit, 100, maskIndex))
				{
					Debug.Log(hit.collider.name);

					GameObject _obj;
					if (Prefabs.Set(PrefabType.Decal, out _obj))
					{
						UltimateDecal _decal = _obj.GetComponent<UltimateDecal>();

						_decal.material = mat;
						_obj.transform.position = hit.point;
						_obj.transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * _obj.transform.rotation;
						UD_Manager.UpdateDecal(_decal);
					}
				}
			}
		}

		private void SetDecal(RaycastHit _hit)
		{
			Debug.Log(_hit.collider.name);

			GameObject _obj;
			if (Prefabs.Set(PrefabType.Decal, out _obj))
			{
				UltimateDecal _decal = _obj.GetComponent<UltimateDecal>();

				_decal.material = mat;
				_obj.transform.position = _hit.point;
				_obj.transform.rotation = Quaternion.FromToRotation(transform.up, _hit.normal) * _obj.transform.rotation;
				UD_Manager.UpdateDecal(_decal);
			}
		}
	}
}
