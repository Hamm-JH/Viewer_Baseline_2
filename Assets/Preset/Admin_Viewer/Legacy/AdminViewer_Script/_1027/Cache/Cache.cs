using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Viewer
{
	public partial class Cache : MonoBehaviour
	{
		#region Instance

		private static Cache instance;

		public static Cache Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<Cache>() as Cache;
					if(instance == null)
					{
						GameObject obj = new GameObject("Cache", typeof(Cache));
						instance = obj.GetComponent<Cache>();
					}
				}
				return instance;
			}
		}

		#endregion

		private void Awake()
		{
			DontDestroyOnLoad(gameObject);

			models.Segments = new List<GameObject>();
			models.Lines = new List<GameObject>();
			models.Objects = new List<GameObject>();
		}
	}
}