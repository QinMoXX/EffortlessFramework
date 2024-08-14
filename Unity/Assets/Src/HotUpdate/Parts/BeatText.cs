using System;
using TMPro;
using UnityEngine;

namespace HotUpdate
{
    public class BeatText:TextMeshProUGUI
    {
        
        private void Update()
        {
            this.ForceMeshUpdate();

            var text = this.textInfo;
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible)
                {
                    continue;   
                }

                var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
                for (int j = 0; j < 4; j++)
                {
                    var orig = verts[charInfo.vertexIndex + j];
                    verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Max(Mathf.Sin((Time.time + i) * 5) * 20,0), 0);
                }
                
            }
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                var meshInfo = textInfo.meshInfo[i];
                meshInfo.mesh.vertices = meshInfo.vertices;
                UpdateGeometry(meshInfo.mesh , i);
            }
        }
    }
}