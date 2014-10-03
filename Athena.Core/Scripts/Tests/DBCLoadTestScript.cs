using Athena.Core.Internal.GameManager;
using Athena.Core.Internal.GameManager.DBC;
using Athena.Core.Internal.GameManager.IngameObjects;
using Athena.Core.Internal.Scripts;

namespace Athena.Core.Scripts.Tests
{
    public class DBCLoadTestScript : Script
    {
        public DBCLoadTestScript()
            : base("DBC -> Load", "Tests")
        { }

        public override void OnStart()
        {
            if (!ObjectManager.IsInGame)
                return;

            DBCManager dbc = new DBCManager();
            dbc.Initialize();
            Stop();
        }
    }
}
