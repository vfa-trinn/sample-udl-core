using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class GameObjectExtensions
{
	public static GameObject FindDeep( 
		this GameObject self, 
		string name, 
		bool includeInactive = false )
	{
		var children = self.GetComponentsInChildren<Transform>( includeInactive );
		foreach ( var transform in children )
		{
			if ( transform.name == name )
			{
				return transform.gameObject;
			}
		}
		return null;
	}

	public static List<Material> FindMaterials( 
		this GameObject self, 
		bool includeInactive = false, bool sharingMaterial = false )
	{
		var materials = new List<Material> ();

		var children = self.GetComponentsInChildren<Transform>( includeInactive );
		foreach ( var transform in children )
		{
			var renderer = transform.GetComponent<Renderer> ();
			if (renderer == null)
				continue;

            if (sharingMaterial)
            {
                foreach (var material in renderer.sharedMaterials)
                {
                    if (material != null)
                    {
                        materials.Add(material);
                    }
                }
            }
            else
            {
                foreach (var material in renderer.materials)
                {
                    if (material != null)
                    {
                        materials.Add(material);
                    }
                }
            }


		}
		return materials;
	}
}