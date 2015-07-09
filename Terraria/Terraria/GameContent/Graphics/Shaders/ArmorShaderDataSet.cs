/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;

namespace Terraria.Graphics.Shaders
{
    public class ArmorShaderDataSet
    {
        protected List<ArmorShaderData> _shaderData = new List<ArmorShaderData>();
        protected Dictionary<int, int> _shaderLookupDictionary = new Dictionary<int, int>();
        protected int _shaderDataCount;

        public T BindShader<T>(int itemId, T shaderData) where T : ArmorShaderData
        {
            this._shaderLookupDictionary[itemId] = ++this._shaderDataCount;
            this._shaderData.Add((ArmorShaderData)shaderData);
            return shaderData;
        }

        public void Apply(int shaderId, Entity entity, DrawData? drawData = null)
        {
            if (shaderId != 0 && shaderId <= this._shaderDataCount)
                this._shaderData[shaderId - 1].Apply(entity, drawData);
            else
                Main.pixelShader.CurrentTechnique.Passes[0].Apply();
        }

        public void ApplySecondary(int shaderId, Entity entity, DrawData? drawData = null)
        {
            if (shaderId != 0 && shaderId <= this._shaderDataCount)
                this._shaderData[shaderId - 1].GetSecondaryShader(entity).Apply(entity, drawData);
            else
                Main.pixelShader.CurrentTechnique.Passes[0].Apply();
        }

        public ArmorShaderData GetShaderFromItemId(int type)
        {
            if (this._shaderLookupDictionary.ContainsKey(type))
                return this._shaderData[this._shaderLookupDictionary[type] - 1];
            return (ArmorShaderData)null;
        }

        public int GetShaderIdFromItemId(int type)
        {
            if (this._shaderLookupDictionary.ContainsKey(type))
                return this._shaderLookupDictionary[type];
            return 0;
        }

        public ArmorShaderData GetSecondaryShader(int id, Player player)
        {
            if (id != 0 && id <= this._shaderDataCount && this._shaderData[id - 1] != null)
                return this._shaderData[id - 1].GetSecondaryShader((Entity)player);
            return (ArmorShaderData)null;
        }
    }
}
