using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athena.Core.Patchables;

namespace Athena.Core.Internal.GameManager.IngameObjects.Extensions
{
    class WoWFishingBobber : WoWGameObject
    {
        public WoWFishingBobber(uint pointer)
            : base(pointer)
        {

        }

        public uint IsBitten
        {
            get
            {
                return (int)GeneralHelper.Memory.Read<byte>(Pointer + Offsets.GameObjectFields.Animation) != 0;
            }
        }
    }
}
