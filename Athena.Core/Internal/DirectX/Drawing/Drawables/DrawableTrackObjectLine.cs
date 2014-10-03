using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Athena.Core.Internal.GameManager;
using Athena.Core.Internal.GameManager.IngameObjects;
using Athena.Core.Internal.Objects;

namespace Athena.Core.Internal.DirectX.Drawing.Drawables
{
    public class DrawableTrackObjectLine : IResource
    {
        public int EntryId { get; set; }


        public DrawableTrackObjectLine(int entryId)
        {
            EntryId = entryId;
        }
        public void Draw()
        {
            Location me = ObjectManager.LocalPlayer.Location;
            List<WoWObject> objects = GetItems().OrderByDescending(x => x.Distance).ToList();
            if (objects.Count > 0)
            {
                float minDitance = objects.Last().Distance;
                float maxDistance = objects.First().Distance;
                foreach (WoWObject obj in objects)
                {
                    Rendering.DrawLine(me, obj.Location, GetLineColor(minDitance, maxDistance, obj.Distance));
                }
            }
        }

        private Color GetLineColor(float mindistance, float maxdistance, float distance)
        {
            try
            {
                float pct = ((distance / maxdistance) * 100);
                int val = (int)((pct / 100) * 255);
                return Color.FromArgb(val, 255, 0);
            }
            catch { }
            return Color.Black;
        }

        public virtual IEnumerable<WoWObject> GetItems()
        {

            return ObjectManager.Objects.Where(x => x.Entry == EntryId);
        }

        public bool Remove { get; set; }
        public void OnBeforeRemove()
        {
            
        }
    }
}
