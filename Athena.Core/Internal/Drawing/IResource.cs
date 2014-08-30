namespace Athena.Core.Internal.Drawing
{
    public interface IResource
    {
        void Draw();

        bool Remove { get; set; }
        void OnBeforeRemove();
    }
}
