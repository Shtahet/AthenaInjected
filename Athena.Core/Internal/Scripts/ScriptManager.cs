using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Athena.Core.Internal.DirectX;

namespace Athena.Core.Internal.Scripts
{
    public class ScriptManager : IPulsable
    {
        public ScriptManager()
        {
            Scripts = new List<Script>();

            CompileInternal();
        }

        public List<Script> Scripts
        {
            get;
            private set;
        }

        private Script CurrentScript
        {
            get;
            set;
        }

        public void OnPulse()
        {
            foreach (var script in Scripts)
            {
                CurrentScript = script;
                script.Tick();
            }

            CurrentScript = null;
        }

        private void CompileInternal()
        {
            OnCompilerStarted();

            Scripts.AddRange(
                Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(Script)))
                    .Select(s => Register(s))
                    .Where(s => s != null)
                    );

            OnCompilerFinished();
        }

        private Script Register(Type type)
        {
            try
            {
                var ctor = type.GetConstructor(new Type[] { });
                if (ctor == null)
                    throw new Exception("Constructor not found");

                var script = (Script)ctor.Invoke(new object[] { });
                if (script == null)
                    throw new Exception("Unable to instantiate script");

                OnScriptRegistered(script);
                return script;
            }
            catch (Exception ex)
            {
                //Log.WriteLine("Failed to compile: {0}", ex.Message);
            }
            return null;
        }

        private void OnCompilerStarted()
        {
            if (CompilerStarted != null)
                CompilerStarted(null, new EventArgs());
        }

        private void OnCompilerFinished()
        {
            if (CompilerFinished != null)
                CompilerFinished(null, new EventArgs());
        }

        private void OnScriptRegistered(Script script)
        {
            if (ScriptRegistered != null)
                ScriptRegistered(script, new EventArgs());
        }

        public event EventHandler CompilerStarted;
        public event EventHandler CompilerFinished;
        public event EventHandler ScriptRegistered;
    }
}
