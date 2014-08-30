using System.Collections.Generic;
using System.Linq;
using Athena.Core.Internal.GameManager;
using Athena.Core.Internal.GameManager.IngameObjects;

namespace Athena.Core.Internal.Drawing.Drawables
{
    public class DrawableTrackUnitLine : DrawableTrackObjectLine
    {

        public DrawableTrackUnitLine(int entryId)
            : base(entryId)
        {
        }

        public override IEnumerable<WoWObject> GetItems()
        {
            return ObjectManager.Objects.Where(x => x.Entry == EntryId && x.IsUnit).Cast<WoWUnit>().Where(l => !l.IsDead);
        }
    }
}
